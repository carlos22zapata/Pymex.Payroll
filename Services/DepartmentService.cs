using AutoMapper;
using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;
using Pymex.Payroll.Services.Interfaces;

namespace Pymex.Payroll.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartmentService> _logger;

        public DepartmentService(IDepartmentRepository repository, IMapper mapper, ILogger<DepartmentService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<DepartmentDto>> GetById(int id)
        {
            var result = await _repository.GetById(id);
            if (result.IsSuccess && result.Value != null)
                return Result<DepartmentDto>.Success(_mapper.Map<DepartmentDto>(result.Value));
            return Result<DepartmentDto>.Failure(result.ErrorMessage ?? "Departamento no encontrado.");
        }

        public async Task<Result<DepartmentDto>> GetNavBarById(int navPositionId, int idReference)
        {
            var result = await _repository.GetNavBarById(navPositionId, idReference);
            if (result.IsSuccess && result.Value != null)
                return Result<DepartmentDto>.Success(_mapper.Map<DepartmentDto>(result.Value));
            return Result<DepartmentDto>.Failure(result.ErrorMessage ?? "No hay registros disponibles.");
        }

        public async Task<Result<List<DepartmentDto>>> GetList(string? name)
        {
            var result = await _repository.GetList(name);
            if (result.IsSuccess && result.Value != null)
                return Result<List<DepartmentDto>>.Success(_mapper.Map<List<DepartmentDto>>(result.Value));
            return Result<List<DepartmentDto>>.Failure(result.ErrorMessage ?? "No se pudo cargar la lista de departamentos.");
        }

        public async Task<Result<bool>> Insert(DepartmentDto departmentDto)
        {
            try
            {
                var entity = _mapper.Map<Department>(departmentDto);
                return await _repository.Insert(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al insertar departamento.");
                return Result<bool>.Failure("Error al procesar los datos del departamento.");
            }
        }

        public async Task<Result<bool>> Update(DepartmentDto departmentDto)
        {
            try
            {
                var entity = _mapper.Map<Department>(departmentDto);
                return await _repository.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al actualizar departamento ID: {Id}", departmentDto.Id);
                return Result<bool>.Failure("Error al procesar la actualización.");
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            return await _repository.Delete(id);
        }
    }
}
