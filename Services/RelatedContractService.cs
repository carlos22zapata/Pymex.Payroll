using AutoMapper;
using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;
using Pymex.Payroll.Services.Interfaces;

namespace Pymex.Payroll.Services
{
    public class RelatedContractService : IRelatedContractService
    {
        private readonly IRelatedContractRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<RelatedContractService> _logger;

        public RelatedContractService(IRelatedContractRepository repository, IMapper mapper, ILogger<RelatedContractService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<RelatedContractDto>> GetById(int id)
        {
            var result = await _repository.GetById(id);
            if (result.IsSuccess && result.Value != null)
                return Result<RelatedContractDto>.Success(_mapper.Map<RelatedContractDto>(result.Value));
            return Result<RelatedContractDto>.Failure(result.ErrorMessage ?? "Contrato relacionado no encontrado.");
        }

        public async Task<Result<RelatedContractDto>> GetNavBarById(int navPositionId, int idReference)
        {
            var result = await _repository.GetNavBarById(navPositionId, idReference);
            if (result.IsSuccess && result.Value != null)
                return Result<RelatedContractDto>.Success(_mapper.Map<RelatedContractDto>(result.Value));
            return Result<RelatedContractDto>.Failure(result.ErrorMessage ?? "No hay registros disponibles.");
        }

        public async Task<Result<List<RelatedContractDto>>> GetByContractId(int contractId)
        {
            var result = await _repository.GetByContractId(contractId);
            if (result.IsSuccess && result.Value != null)
                return Result<List<RelatedContractDto>>.Success(_mapper.Map<List<RelatedContractDto>>(result.Value));
            return Result<List<RelatedContractDto>>.Failure(result.ErrorMessage ?? "No se pudieron cargar los contratos relacionados.");
        }

        public async Task<Result<bool>> Insert(RelatedContractDto relatedContractDto)
        {
            try
            {
                var entity = _mapper.Map<RelatedContract>(relatedContractDto);
                return await _repository.Insert(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al insertar contrato relacionado.");
                return Result<bool>.Failure("Error al procesar los datos del contrato relacionado.");
            }
        }

        public async Task<Result<bool>> Update(RelatedContractDto relatedContractDto)
        {
            try
            {
                var entity = _mapper.Map<RelatedContract>(relatedContractDto);
                return await _repository.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al actualizar contrato relacionado ID: {Id}", relatedContractDto.Id);
                return Result<bool>.Failure("Error al procesar la actualización.");
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            return await _repository.Delete(id);
        }
    }
}
