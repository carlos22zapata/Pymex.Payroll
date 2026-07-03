using AutoMapper;
using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;
using Pymex.Payroll.Services.Interfaces;

namespace Pymex.Payroll.Services
{
    public class ContractService : IContractService
    {
        private readonly IContractRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ContractService> _logger;

        public ContractService(IContractRepository repository, IMapper mapper, ILogger<ContractService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<ContractDto>> GetById(int id)
        {
            var result = await _repository.GetById(id);
            if (result.IsSuccess && result.Value != null)
                return Result<ContractDto>.Success(_mapper.Map<ContractDto>(result.Value));
            return Result<ContractDto>.Failure(result.ErrorMessage ?? "Contrato no encontrado.");
        }

        public async Task<Result<ContractDto>> GetNavBarById(int navPositionId, int idReference)
        {
            var result = await _repository.GetNavBarById(navPositionId, idReference);
            if (result.IsSuccess && result.Value != null)
                return Result<ContractDto>.Success(_mapper.Map<ContractDto>(result.Value));
            return Result<ContractDto>.Failure(result.ErrorMessage ?? "No hay registros disponibles.");
        }

        public async Task<Result<List<ContractDto>>> GetList(string? name)
        {
            var result = await _repository.GetList(name);
            if (result.IsSuccess && result.Value != null)
                return Result<List<ContractDto>>.Success(_mapper.Map<List<ContractDto>>(result.Value));
            return Result<List<ContractDto>>.Failure(result.ErrorMessage ?? "No se pudo cargar la lista de contratos.");
        }

        public async Task<Result<bool>> Insert(ContractDto contractDto)
        {
            try
            {
                var entity = _mapper.Map<Contract>(contractDto);
                return await _repository.Insert(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al insertar contrato.");
                return Result<bool>.Failure("Error al procesar los datos del contrato.");
            }
        }

        public async Task<Result<bool>> Update(ContractDto contractDto)
        {
            try
            {
                var entity = _mapper.Map<Contract>(contractDto);
                return await _repository.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al actualizar contrato ID: {Id}", contractDto.Id);
                return Result<bool>.Failure("Error al procesar la actualización.");
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            return await _repository.Delete(id);
        }
    }
}
