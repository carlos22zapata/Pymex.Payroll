using AutoMapper;
using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;
using Pymex.Payroll.Services.Interfaces;

namespace Pymex.Payroll.Services
{
    public class PayrollConceptService : IPayrollConceptService
    {
        private readonly IPayrollConceptRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<PayrollConceptService> _logger;

        public PayrollConceptService(IPayrollConceptRepository repository, IMapper mapper, ILogger<PayrollConceptService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<PayrollConceptDto>> GetById(int id)
        {
            var result = await _repository.GetById(id);
            if (result.IsSuccess && result.Value != null)
                return Result<PayrollConceptDto>.Success(_mapper.Map<PayrollConceptDto>(result.Value));
            return Result<PayrollConceptDto>.Failure(result.ErrorMessage ?? "Concepto de nómina no encontrado.");
        }

        public async Task<Result<PayrollConceptDto>> GetNavBarById(int navPositionId, int idReference)
        {
            var result = await _repository.GetNavBarById(navPositionId, idReference);
            if (result.IsSuccess && result.Value != null)
                return Result<PayrollConceptDto>.Success(_mapper.Map<PayrollConceptDto>(result.Value));
            return Result<PayrollConceptDto>.Failure(result.ErrorMessage ?? "No hay registros disponibles.");
        }

        public async Task<Result<List<PayrollConceptDto>>> GetList(string? name)
        {
            var result = await _repository.GetList(name);
            if (result.IsSuccess && result.Value != null)
                return Result<List<PayrollConceptDto>>.Success(_mapper.Map<List<PayrollConceptDto>>(result.Value));
            return Result<List<PayrollConceptDto>>.Failure(result.ErrorMessage ?? "No se pudo cargar la lista de conceptos.");
        }

        public async Task<Result<bool>> Insert(PayrollConceptDto payrollConceptDto)
        {
            try
            {
                var entity = _mapper.Map<PayrollConcept>(payrollConceptDto);
                return await _repository.Insert(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al insertar concepto.");
                return Result<bool>.Failure("Error al procesar los datos del concepto.");
            }
        }

        public async Task<Result<bool>> Update(PayrollConceptDto payrollConceptDto)
        {
            try
            {
                var entity = _mapper.Map<PayrollConcept>(payrollConceptDto);
                return await _repository.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al actualizar concepto ID: {Id}", payrollConceptDto.Id);
                return Result<bool>.Failure("Error al procesar la actualización.");
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            return await _repository.Delete(id);
        }
    }
}
