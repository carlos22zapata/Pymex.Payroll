using AutoMapper;
using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;
using Pymex.Payroll.Services.Interfaces;

namespace Pymex.Payroll.Services
{
    public class ContractVariableService : IContractVariableService
    {
        private readonly IContractVariableRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ContractVariableService> _logger;

        public ContractVariableService(IContractVariableRepository repository, IMapper mapper, ILogger<ContractVariableService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<ContractVariableDto>> GetById(int id)
        {
            var result = await _repository.GetById(id);
            if (result.IsSuccess && result.Value != null)
                return Result<ContractVariableDto>.Success(_mapper.Map<ContractVariableDto>(result.Value));
            return Result<ContractVariableDto>.Failure(result.ErrorMessage ?? "Variable de contrato no encontrada.");
        }

        public async Task<Result<ContractVariableDto>> GetNavBarById(int navPositionId, int idReference)
        {
            var result = await _repository.GetNavBarById(navPositionId, idReference);
            if (result.IsSuccess && result.Value != null)
                return Result<ContractVariableDto>.Success(_mapper.Map<ContractVariableDto>(result.Value));
            return Result<ContractVariableDto>.Failure(result.ErrorMessage ?? "No hay registros disponibles.");
        }

        public async Task<Result<List<ContractVariableDto>>> GetByContractId(int contractId)
        {
            var result = await _repository.GetByContractId(contractId);
            if (result.IsSuccess && result.Value != null)
                return Result<List<ContractVariableDto>>.Success(_mapper.Map<List<ContractVariableDto>>(result.Value));
            return Result<List<ContractVariableDto>>.Failure(result.ErrorMessage ?? "No se pudieron cargar las variables del contrato.");
        }

        public async Task<Result<bool>> Insert(ContractVariableDto contractVariableDto)
        {
            try
            {
                var entity = _mapper.Map<ContractVariable>(contractVariableDto);
                return await _repository.Insert(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al insertar variable de contrato.");
                return Result<bool>.Failure("Error al procesar los datos de la variable de contrato.");
            }
        }

        public async Task<Result<bool>> Update(ContractVariableDto contractVariableDto)
        {
            try
            {
                var entity = _mapper.Map<ContractVariable>(contractVariableDto);
                return await _repository.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al actualizar variable de contrato ID: {Id}", contractVariableDto.Id);
                return Result<bool>.Failure("Error al procesar la actualización.");
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            return await _repository.Delete(id);
        }
    }
}