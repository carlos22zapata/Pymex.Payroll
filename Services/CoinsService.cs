using AutoMapper;
using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;
using Pymex.Payroll.Services.Interfaces;

namespace Pymex.Payroll.Services
{
    public class CoinsService : ICoinsService
    {
        private readonly ICoinsRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CoinsService> _logger;

        public CoinsService(ICoinsRepository repository, IMapper mapper, ILogger<CoinsService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<CoinsDto>> GetById(int id)
        {
            var result = await _repository.GetById(id);
            if (result.IsSuccess && result.Value != null)
                return Result<CoinsDto>.Success(_mapper.Map<CoinsDto>(result.Value));
            return Result<CoinsDto>.Failure(result.ErrorMessage ?? "Moneda no encontrada.");
        }

        public async Task<Result<CoinsDto>> GetNavBarById(int navPositionId, int idReference)
        {
            var result = await _repository.GetNavBarById(navPositionId, idReference);
            if (result.IsSuccess && result.Value != null)
                return Result<CoinsDto>.Success(_mapper.Map<CoinsDto>(result.Value));
            return Result<CoinsDto>.Failure(result.ErrorMessage ?? "No hay registros disponibles.");
        }

        public async Task<Result<List<CoinsDto>>> GetList(int page, int pageSize, string? name)
        {
            var result = await _repository.GetList(page, pageSize, name);
            if (result.IsSuccess && result.Value != null)
                return Result<List<CoinsDto>>.Success(_mapper.Map<List<CoinsDto>>(result.Value));
            return Result<List<CoinsDto>>.Failure(result.ErrorMessage ?? "No se pudo cargar la lista de monedas.");
        }

        public async Task<Result<bool>> Insert(CoinsDto coinDto)
        {
            try
            {
                var entity = _mapper.Map<Coins>(coinDto);
                return await _repository.Insert(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al insertar moneda.");
                return Result<bool>.Failure("Error al procesar los datos de la moneda.");
            }
        }

        public async Task<Result<bool>> Update(CoinsDto coinDto)
        {
            try
            {
                var entity = _mapper.Map<Coins>(coinDto);
                return await _repository.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al actualizar moneda ID: {Id}", coinDto.Id);
                return Result<bool>.Failure("Error al procesar la actualización.");
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            return await _repository.Delete(id);
        }
    }
}
