using Microsoft.EntityFrameworkCore;
using Pymex.Payroll.Data.Contexts;
using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Enums;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;
using System.Text.RegularExpressions;

namespace Pymex.Payroll.Repositories
{
    public partial class PayrollCalculationRepository : IPayrollCalculationRepository
    {
        private readonly PayrollDbContext _context;
        private readonly ILogger<PayrollCalculationRepository> _logger;

        public PayrollCalculationRepository(PayrollDbContext context, ILogger<PayrollCalculationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        [GeneratedRegex(@"\[(.*?)\]")]
        private static partial Regex VariablePattern();

        public async Task<Result<List<PayrollResultDto>>> CalculateEmployeePayroll(int employeeId)
        {
            try
            {
                var employee = await _context.Employees
                    .AsNoTracking()
                    .Include(e => e.Contract)
                    .ThenInclude(c => c.ContractConcepts)
                    .ThenInclude(cc => cc.PayrollConcept)
                    .FirstOrDefaultAsync(e => e.Id == employeeId);

                if (employee == null)
                    return Result<List<PayrollResultDto>>.Failure("Empleado no encontrado.");

                var contract = employee.Contract;

                if (contract == null)
                    return Result<List<PayrollResultDto>>.Failure("El empleado no tiene un contrato asignado.");

                var variables = await _context.ContractVariables
                    .AsNoTracking()
                    .Include(cv => cv.PayrollVariable)
                    .Where(cv => cv.ContractId == contract.Id)
                    .ToDictionaryAsync(cv => cv.PayrollVariable.Code, cv => cv.Value.ToString());

                var result = new PayrollResultDto
                {
                    EmployeeId = employee.Id,
                    EmployeeName = $"{employee.Name} {employee.LastName}",
                    Concepts = new List<ConceptResultDto>()
                };

                foreach (var cc in contract.ContractConcepts.Where(cc => cc.IsActive))
                {
                    var concept = cc.PayrollConcept;
                    var formula = concept.Formula;

                    var matches = VariablePattern().Matches(formula);
                    foreach (Match match in matches)
                    {
                        var varCode = match.Groups[1].Value;
                        if (variables.TryGetValue(varCode, out var varValue))
                        {
                            formula = formula.Replace($"[{varCode}]", varValue);
                        }
                    }

                    decimal amount = 0;
                    try
                    {
                        var expr = new System.Data.DataTable().Compute(formula, null);
                        amount = Convert.ToDecimal(expr);
                    }
                    catch
                    {
                        _logger.LogWarning("Error evaluando fórmula para concepto {Code}: {Formula}", concept.Code, formula);
                    }

                    result.Concepts.Add(new ConceptResultDto
                    {
                        PayrollConceptId = concept.Id,
                        ConceptCode = concept.Code,
                        ConceptName = concept.Name,
                        ConceptType = (int)concept.ConceptType,
                        Amount = amount,
                        Formula = concept.Formula
                    });
                }

                result.TotalEarnings = result.Concepts
                    .Where(c => c.ConceptType == (int)ConceptType.Earning)
                    .Sum(c => c.Amount);

                result.TotalDeductions = result.Concepts
                    .Where(c => c.ConceptType == (int)ConceptType.Deduction || c.ConceptType == (int)ConceptType.Withholding)
                    .Sum(c => c.Amount);

                result.NetPay = result.TotalEarnings - result.TotalDeductions;

                return Result<List<PayrollResultDto>>.Success(new List<PayrollResultDto> { result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al calcular nómina del empleado ID: {Id}", employeeId);
                return Result<List<PayrollResultDto>>.Failure("Error al calcular la nómina.");
            }
        }

        public async Task<Result<List<PayrollResultDto>>> CalculateAllPayroll()
        {
            try
            {
                var employees = await _context.Employees
                    .AsNoTracking()
                    .Where(e => e.IsActive)
                    .Select(e => e.Id)
                    .ToListAsync();

                var results = new List<PayrollResultDto>();
                foreach (var empId in employees)
                {
                    var empResult = await CalculateEmployeePayroll(empId);
                    if (empResult.IsSuccess && empResult.Value != null)
                        results.AddRange(empResult.Value);
                }

                return Result<List<PayrollResultDto>>.Success(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al calcular nómina completa.");
                return Result<List<PayrollResultDto>>.Failure("Error al calcular la nómina completa.");
            }
        }

        public async Task<Result<bool>> ClosePayroll()
        {
            try
            {
                var volatileVars = await _context.ContractVariables
                    .Include(cv => cv.PayrollVariable)
                    .Where(cv => cv.PayrollVariable.Behavior == PersistenceBehavior.Volatile)
                    .ToListAsync();

                foreach (var v in volatileVars)
                {
                    v.Value = 0;
                }

                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cerrar nómina.");
                return Result<bool>.Failure("Error al cerrar la nómina.");
            }
        }
    }
}