using AutoMapper;
using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;
using Pymex.Payroll.Services.Interfaces;

namespace Pymex.Payroll.Services
{
    public class ContractConceptService : IContractConceptService
    {
        private readonly IContractConceptRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ContractConceptService> _logger;

        public ContractConceptService(IContractConceptRepository repository, IMapper mapper, ILogger<ContractConceptService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<ContractConceptDto>> GetById(int id)
        {
            var result = await _repository.GetById(id);
            if (result.IsSuccess && result.Value != null)
                return Result<ContractConceptDto>.Success(_mapper.Map<ContractConceptDto>(result.Value));
            return Result<ContractConceptDto>.Failure(result.ErrorMessage ?? "Concepto de contrato no encontrado.");
        }

        public async Task<Result<ContractConceptDto>> GetNavBarById(int navPositionId, int idReference)
        {
            var result = await _repository.GetNavBarById(navPositionId, idReference);
            if (result.IsSuccess && result.Value != null)
                return Result<ContractConceptDto>.Success(_mapper.Map<ContractConceptDto>(result.Value));
            return Result<ContractConceptDto>.Failure(result.ErrorMessage ?? "No hay registros disponibles.");
        }

        public async Task<Result<List<ContractConceptDto>>> GetByContractId(int contractId)
        {
            var result = await _repository.GetByContractId(contractId);
            if (result.IsSuccess && result.Value != null)
                return Result<List<ContractConceptDto>>.Success(_mapper.Map<List<ContractConceptDto>>(result.Value));
            return Result<List<ContractConceptDto>>.Failure(result.ErrorMessage ?? "No se pudieron cargar los conceptos del contrato.");
        }

        public async Task<Result<bool>> Insert(ContractConceptDto contractConceptDto)
        {
            try
            {
                var entity = _mapper.Map<ContractConcept>(contractConceptDto);
                return await _repository.Insert(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al insertar concepto de contrato.");
                return Result<bool>.Failure("Error al procesar los datos del concepto de contrato.");
            }
        }

        public async Task<Result<bool>> Update(ContractConceptDto contractConceptDto)
        {
            try
            {
                var entity = _mapper.Map<ContractConcept>(contractConceptDto);
                return await _repository.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al actualizar concepto de contrato ID: {Id}", contractConceptDto.Id);
                return Result<bool>.Failure("Error al procesar la actualización.");
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            return await _repository.Delete(id);
        }
    }
}
