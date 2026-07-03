using ClosedXML.Excel;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Pymex.Payroll.Data.Contexts;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;
using System.Globalization;

namespace Pymex.Payroll.Repositories
{
    public class CoinQuotationRepository : ICoinQuotationRepository
    {
        private readonly PayrollDbContext _context;
        private readonly ILogger<CoinQuotationRepository> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public CoinQuotationRepository(
            PayrollDbContext context,
            ILogger<CoinQuotationRepository> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<Result<CoinQuotations>> GetById(int id)
        {
            try
            {
                var entity = await _context.CoinQuotations.AsNoTracking().FirstOrDefaultAsync(q => q.Id == id);
                if (entity == null)
                    return Result<CoinQuotations>.Failure("Cotización no encontrada.");
                return Result<CoinQuotations>.Success(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener cotización ID: {Id}", id);
                return Result<CoinQuotations>.Failure("Error al consultar la cotización.");
            }
        }

        public async Task<Result<List<CoinQuotations>>> GetList(int page, int pageSize, int coinId)
        {
            try
            {
                var skip = (page - 1) * pageSize;
                var list = await _context.CoinQuotations.AsNoTracking()
                    .Where(q => q.CoinId == coinId)
                    .OrderByDescending(q => q.Date)
                    .Skip(skip)
                    .Take(pageSize)
                    .ToListAsync();

                return Result<List<CoinQuotations>>.Success(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar cotizaciones para CoinId: {CoinId}", coinId);
                return Result<List<CoinQuotations>>.Failure("No se pudo obtener la lista de cotizaciones.");
            }
        }

        public async Task<Result<bool>> Insert(CoinQuotations quotation)
        {
            try
            {
                await _context.CoinQuotations.AddAsync(quotation);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar cotización.");
                return Result<bool>.Failure("Error al guardar la cotización.");
            }
        }

        public async Task<Result<bool>> Update(CoinQuotations quotation)
        {
            try
            {
                _context.CoinQuotations.Update(quotation);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar cotización ID: {Id}", quotation.Id);
                return Result<bool>.Failure("Error al actualizar la cotización.");
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            try
            {
                var entity = await _context.CoinQuotations.FindAsync(id);
                if (entity == null)
                    return Result<bool>.Failure("La cotización no existe.");

                _context.CoinQuotations.Remove(entity);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar cotización ID: {Id}", id);
                return Result<bool>.Failure("Error al eliminar la cotización.");
            }
        }

        public async Task<Result<bool>> InsertList(List<CoinQuotations> quotations)
        {
            try
            {
                await _context.CoinQuotations.AddRangeAsync(quotations);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar lista de cotizaciones.");
                return Result<bool>.Failure("No se pudieron insertar los registros en lote.");
            }
        }

        public async Task<Result<bool>> UpdateList(List<CoinQuotations> quotations)
        {
            try
            {
                var toUpdate = quotations.Where(q => q.Id > 0).ToList();
                if (!toUpdate.Any()) return Result<bool>.Success(true);

                var ids = toUpdate.Select(t => t.Id).ToList();
                var existingIds = await _context.CoinQuotations
                    .AsNoTracking()
                    .Where(q => ids.Contains(q.Id))
                    .Select(q => q.Id)
                    .ToListAsync();

                foreach (var quotation in toUpdate)
                {
                    if (!existingIds.Contains(quotation.Id))
                    {
                        _logger.LogWarning("Intento de actualizar cotización inexistente Id: {Id}", quotation.Id);
                        continue;
                    }
                    _context.Attach(quotation);
                    _context.Entry(quotation).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar lista de cotizaciones.");
                return Result<bool>.Failure("Error al procesar la actualización en lote.");
            }
        }

        public async Task<Result<bool>> UpdateBCVCoinQuotationListScraper(string webSite)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var html = await httpClient.GetStringAsync(webSite);

                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var coins = await _context.Coins.ToListAsync();
                var today = DateTime.Today;

                var existingToday = await _context.CoinQuotations
                    .AsNoTracking()
                    .Where(q => q.Date.Date == today && q.Origin == 2)
                    .Select(q => q.CoinId)
                    .ToListAsync();

                var nodes = doc.DocumentNode.SelectNodes("//div[@id='euro' or @id='yuan' or @id='lira' or @id='rublo' or @id='dolar']");

                if (nodes == null)
                    return Result<bool>.Failure("No se encontraron los contenedores de tasa en la web del BCV.");

                foreach (var node in nodes)
                {
                    var coin = coins.FirstOrDefault(c => c.IdWeb == node.Id);

                    if (coin != null && !existingToday.Contains(coin.Id))
                    {
                        var valorTexto = node.SelectSingleNode(".//strong")?.InnerText.Trim();

                        if (decimal.TryParse(valorTexto, NumberStyles.Any, new CultureInfo("es-VE"), out decimal valor))
                        {
                            await _context.CoinQuotations.AddAsync(new CoinQuotations
                            {
                                Date = DateTime.Now,
                                Observation = "Cotización obtenida de la página del BCV",
                                Origin = 2,
                                CoinId = coin.Id,
                                Value = valor
                            });
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error de red al intentar acceder al BCV: {Url}", webSite);
                return Result<bool>.Failure("No se pudo conectar con la página del BCV.");
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Timeout al esperar respuesta del BCV.");
                return Result<bool>.Failure("La página del BCV tardó demasiado en responder.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error general en scraping del BCV.");
                return Result<bool>.Failure($"Error en el proceso: {ex.Message}");
            }
        }

        public async Task<Result<bool>> ImportBCVExcelHistoricalRates()
        {
            string folderPath = _configuration.GetValue<string>("BCVExcelPath:Path");

            if (string.IsNullOrEmpty(folderPath))
            {
                _logger.LogError("BCVExcelPath no configurado en appsettings.json");
                return Result<bool>.Failure("La ruta de importación no está configurada.");
            }

            try
            {
                if (!Directory.Exists(folderPath))
                {
                    _logger.LogError("El directorio no existe: {Path}", folderPath);
                    return Result<bool>.Failure("La carpeta de archivos históricos no fue encontrada.");
                }

                var excelFiles = Directory.GetFiles(folderPath, "*.xlsx");
                if (excelFiles.Length == 0)
                    return Result<bool>.Failure("No hay archivos Excel disponibles para importar.");

                var coins = await _context.Coins.ToListAsync();

                var coinMap = new Dictionary<string, string>
                {
                    { "EUR", "euro" },
                    { "CNY", "yuan" },
                    { "TRY", "lira" },
                    { "RUB", "rublo" },
                    { "USD", "dolar" }
                };

                foreach (var excelFilePath in excelFiles)
                {
                    _logger.LogInformation("Procesando archivo: {FileName}", Path.GetFileName(excelFilePath));

                    using var workbook = new XLWorkbook(excelFilePath);

                    foreach (var worksheet in workbook.Worksheets)
                    {
                        string sheetName = worksheet.Name.Trim();
                        if (sheetName.Length != 8 || !long.TryParse(sheetName, out _)) continue;
                        if (!DateTime.TryParseExact(sheetName, "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime sheetDate)) continue;

                        var existingOnDate = await _context.CoinQuotations
                            .AsNoTracking()
                            .Where(q => q.Date.Date == sheetDate.Date && q.Origin == 2)
                            .Select(q => q.CoinId)
                            .ToListAsync();

                        for (int row = 8; row <= 20; row++)
                        {
                            var coinCodeCell = worksheet.Cell(row, 2).Value;
                            if (coinCodeCell.IsBlank) continue;

                            string coinCode = coinCodeCell.ToString().Trim().ToUpper();

                            if (coinMap.TryGetValue(coinCode, out string idWebAsignado))
                            {
                                var coin = coins.FirstOrDefault(c => c.IdWeb == idWebAsignado);
                                if (coin != null && !existingOnDate.Contains(coin.Id))
                                {
                                    var valorCell = worksheet.Cell(row, 7).Value;
                                    if (valorCell.IsBlank) continue;

                                    if (decimal.TryParse(valorCell.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal valor))
                                    {
                                        await _context.CoinQuotations.AddAsync(new CoinQuotations
                                        {
                                            Date = sheetDate.Date.AddHours(18),
                                            Observation = $"Cotización histórica importada desde Excel oficial: {Path.GetFileName(excelFilePath)} (Hoja {sheetName})",
                                            Origin = 2,
                                            CoinId = coin.Id,
                                            Value = valor
                                        });

                                        existingOnDate.Add(coin.Id);
                                    }
                                }
                            }
                        }
                    }
                }

                if (_context.ChangeTracker.HasChanges())
                    await _context.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar el histórico Excel.");
                return Result<bool>.Failure($"Error en la importación: {ex.Message}");
            }
        }
    }
}
