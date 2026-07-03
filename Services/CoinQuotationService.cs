using AutoMapper;
using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;
using Pymex.Payroll.Services.Interfaces;

namespace Pymex.Payroll.Services
{
    public class CoinQuotationService : ICoinQuotationService
    {
        private readonly ICoinQuotationRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CoinQuotationService> _logger;

        public CoinQuotationService(ICoinQuotationRepository repository, IMapper mapper, ILogger<CoinQuotationService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<CoinQuotationDto>> GetById(int id)
        {
            var result = await _repository.GetById(id);
            if (result.IsSuccess && result.Value != null)
                return Result<CoinQuotationDto>.Success(_mapper.Map<CoinQuotationDto>(result.Value));
            return Result<CoinQuotationDto>.Failure(result.ErrorMessage ?? "Cotización no encontrada.");
        }

        public async Task<Result<List<CoinQuotationDto>>> GetList(int page, int pageSize, int coinId)
        {
            var result = await _repository.GetList(page, pageSize, coinId);
            if (result.IsSuccess && result.Value != null)
                return Result<List<CoinQuotationDto>>.Success(_mapper.Map<List<CoinQuotationDto>>(result.Value));
            return Result<List<CoinQuotationDto>>.Failure(result.ErrorMessage ?? "No se pudo cargar la lista de cotizaciones.");
        }

        public async Task<Result<bool>> Insert(CoinQuotationDto quotationDto)
        {
            try
            {
                var entity = _mapper.Map<CoinQuotations>(quotationDto);
                return await _repository.Insert(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al insertar cotización.");
                return Result<bool>.Failure("Error al procesar los datos de la cotización.");
            }
        }

        public async Task<Result<bool>> Update(CoinQuotationDto quotationDto)
        {
            try
            {
                var entity = _mapper.Map<CoinQuotations>(quotationDto);
                return await _repository.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al actualizar cotización ID: {Id}", quotationDto.Id);
                return Result<bool>.Failure("Error al procesar la actualización.");
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<Result<bool>> InsertList(List<CoinQuotationDto> quotationsDto)
        {
            try
            {
                var entities = _mapper.Map<List<CoinQuotations>>(quotationsDto);
                return await _repository.InsertList(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al insertar lista de cotizaciones.");
                return Result<bool>.Failure("Error al procesar los datos.");
            }
        }

        public async Task<Result<bool>> UpdateList(List<CoinQuotationDto> quotationsDto)
        {
            try
            {
                var entities = _mapper.Map<List<CoinQuotations>>(quotationsDto);
                return await _repository.UpdateList(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al actualizar lista de cotizaciones.");
                return Result<bool>.Failure("Error al procesar los datos.");
            }
        }

        public async Task<Result<bool>> UpdateBCVCoinQuotationListScraper(string webSite)
        {
            _logger.LogInformation("Iniciando scraping BCV desde: {Url}", webSite);

            if (string.IsNullOrWhiteSpace(webSite))
                return Result<bool>.Failure("La URL del sitio web es requerida.");

            return await _repository.UpdateBCVCoinQuotationListScraper(webSite);
        }

        public async Task<Result<bool>> ImportBCVExcelHistoricalRates()
        {
            _logger.LogInformation("Iniciando importación de cotizaciones históricas desde Excel.");
            return await _repository.ImportBCVExcelHistoricalRates();
        }
    }
}
