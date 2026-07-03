using AutoMapper;
using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;
using Pymex.Payroll.Services.Interfaces;

namespace Pymex.Payroll.Services
{
    public class PayrollNoveltyService : IPayrollNoveltyService
    {
        private readonly IPayrollNoveltyRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<PayrollNoveltyService> _logger;

        public PayrollNoveltyService(IPayrollNoveltyRepository repository, IMapper mapper, ILogger<PayrollNoveltyService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<PayrollNoveltyDto>> GetById(int id)
        {
            var result = await _repository.GetById(id);
            if (result.IsSuccess && result.Value != null)
                return Result<PayrollNoveltyDto>.Success(_mapper.Map<PayrollNoveltyDto>(result.Value));
            return Result<PayrollNoveltyDto>.Failure(result.ErrorMessage ?? "Novedad no encontrada.");
        }

        public async Task<Result<PayrollNoveltyDto>> GetNavBarById(int navPositionId, int idReference)
        {
            var result = await _repository.GetNavBarById(navPositionId, idReference);
            if (result.IsSuccess && result.Value != null)
                return Result<PayrollNoveltyDto>.Success(_mapper.Map<PayrollNoveltyDto>(result.Value));
            return Result<PayrollNoveltyDto>.Failure(result.ErrorMessage ?? "No hay registros disponibles.");
        }

        public async Task<Result<List<PayrollNoveltyDto>>> GetByContractId(int contractId)
        {
            var result = await _repository.GetByContractId(contractId);
            if (result.IsSuccess && result.Value != null)
                return Result<List<PayrollNoveltyDto>>.Success(_mapper.Map<List<PayrollNoveltyDto>>(result.Value));
            return Result<List<PayrollNoveltyDto>>.Failure(result.ErrorMessage ?? "No se pudieron cargar las novedades del contrato.");
        }

        public async Task<Result<bool>> Insert(PayrollNoveltyDto payrollNoveltyDto)
        {
            try
            {
                var entity = _mapper.Map<PayrollNovelty>(payrollNoveltyDto);
                return await _repository.Insert(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al insertar novedad.");
                return Result<bool>.Failure("Error al procesar los datos de la novedad.");
            }
        }

        public async Task<Result<bool>> Update(PayrollNoveltyDto payrollNoveltyDto)
        {
            try
            {
                var entity = _mapper.Map<PayrollNovelty>(payrollNoveltyDto);
                return await _repository.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al actualizar novedad ID: {Id}", payrollNoveltyDto.Id);
                return Result<bool>.Failure("Error al procesar la actualización.");
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            return await _repository.Delete(id);
        }
    }
}