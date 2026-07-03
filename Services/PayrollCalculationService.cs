using AutoMapper;
using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;
using Pymex.Payroll.Services.Interfaces;

namespace Pymex.Payroll.Services
{
    public class PayrollCalculationService : IPayrollCalculationService
    {
        private readonly IPayrollCalculationRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<PayrollCalculationService> _logger;

        public PayrollCalculationService(IPayrollCalculationRepository repository, IMapper mapper, ILogger<PayrollCalculationService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<List<PayrollResultDto>>> CalculateEmployeePayroll(int employeeId)
        {
            return await _repository.CalculateEmployeePayroll(employeeId);
        }

        public async Task<Result<List<PayrollResultDto>>> CalculateAllPayroll()
        {
            return await _repository.CalculateAllPayroll();
        }

        public async Task<Result<bool>> ClosePayroll()
        {
            return await _repository.ClosePayroll();
        }
    }
}
