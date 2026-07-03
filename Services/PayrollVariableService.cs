using AutoMapper;
using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;
using Pymex.Payroll.Services.Interfaces;

namespace Pymex.Payroll.Services
{
    public class PayrollVariableService : IPayrollVariableService
    {
        private readonly IPayrollVariableRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<PayrollVariableService> _logger;

        public PayrollVariableService(IPayrollVariableRepository repository, IMapper mapper, ILogger<PayrollVariableService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<PayrollVariableDto>> GetById(int id)
        {
            var result = await _repository.GetById(id);
            if (result.IsSuccess && result.Value != null)
                return Result<PayrollVariableDto>.Success(_mapper.Map<PayrollVariableDto>(result.Value));
            return Result<PayrollVariableDto>.Failure(result.ErrorMessage ?? "Variable no encontrada.");
        }

        public async Task<Result<PayrollVariableDto>> GetNavBarById(int navPositionId, int idReference)
        {
            var result = await _repository.GetNavBarById(navPositionId, idReference);
            if (result.IsSuccess && result.Value != null)
                return Result<PayrollVariableDto>.Success(_mapper.Map<PayrollVariableDto>(result.Value));
            return Result<PayrollVariableDto>.Failure(result.ErrorMessage ?? "No hay registros disponibles.");
        }

        public async Task<Result<List<PayrollVariableDto>>> GetList(string? name)
        {
            var result = await _repository.GetList(name);
            if (result.IsSuccess && result.Value != null)
                return Result<List<PayrollVariableDto>>.Success(_mapper.Map<List<PayrollVariableDto>>(result.Value));
            return Result<List<PayrollVariableDto>>.Failure(result.ErrorMessage ?? "No se pudo cargar la lista de variables.");
        }

        public async Task<Result<bool>> Insert(PayrollVariableDto payrollVariableDto)
        {
            try
            {
                var entity = _mapper.Map<PayrollVariable>(payrollVariableDto);
                return await _repository.Insert(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al insertar variable.");
                return Result<bool>.Failure("Error al procesar los datos de la variable.");
            }
        }

        public async Task<Result<bool>> Update(PayrollVariableDto payrollVariableDto)
        {
            try
            {
                var entity = _mapper.Map<PayrollVariable>(payrollVariableDto);
                return await _repository.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al actualizar variable ID: {Id}", payrollVariableDto.Id);
                return Result<bool>.Failure("Error al procesar la actualización.");
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            return await _repository.Delete(id);
        }
    }
}
