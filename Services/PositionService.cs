using AutoMapper;
using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;
using Pymex.Payroll.Services.Interfaces;

namespace Pymex.Payroll.Services
{
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<PositionService> _logger;

        public PositionService(IPositionRepository repository, IMapper mapper, ILogger<PositionService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<PositionDto>> GetById(int id)
        {
            var result = await _repository.GetById(id);
            if (result.IsSuccess && result.Value != null)
                return Result<PositionDto>.Success(_mapper.Map<PositionDto>(result.Value));
            return Result<PositionDto>.Failure(result.ErrorMessage ?? "Cargo no encontrado.");
        }

        public async Task<Result<PositionDto>> GetNavBarById(int navPositionId, int idReference)
        {
            var result = await _repository.GetNavBarById(navPositionId, idReference);
            if (result.IsSuccess && result.Value != null)
                return Result<PositionDto>.Success(_mapper.Map<PositionDto>(result.Value));
            return Result<PositionDto>.Failure(result.ErrorMessage ?? "No hay registros disponibles.");
        }

        public async Task<Result<List<PositionDto>>> GetList(string? name)
        {
            var result = await _repository.GetList(name);
            if (result.IsSuccess && result.Value != null)
                return Result<List<PositionDto>>.Success(_mapper.Map<List<PositionDto>>(result.Value));
            return Result<List<PositionDto>>.Failure(result.ErrorMessage ?? "No se pudo cargar la lista de cargos.");
        }

        public async Task<Result<bool>> Insert(PositionDto positionDto)
        {
            try
            {
                var entity = _mapper.Map<Position>(positionDto);
                return await _repository.Insert(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al insertar cargo.");
                return Result<bool>.Failure("Error al procesar los datos del cargo.");
            }
        }

        public async Task<Result<bool>> Update(PositionDto positionDto)
        {
            try
            {
                var entity = _mapper.Map<Position>(positionDto);
                return await _repository.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al actualizar cargo ID: {Id}", positionDto.Id);
                return Result<bool>.Failure("Error al procesar la actualización.");
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            return await _repository.Delete(id);
        }
    }
}
