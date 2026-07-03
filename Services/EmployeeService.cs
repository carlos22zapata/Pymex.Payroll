using AutoMapper;
using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;
using Pymex.Payroll.Services.Interfaces;

namespace Pymex.Payroll.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IEmployeeRepository repository, IMapper mapper, ILogger<EmployeeService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<EmployeeDto>> GetById(int id)
        {
            var result = await _repository.GetById(id);
            if (result.IsSuccess && result.Value != null)
                return Result<EmployeeDto>.Success(_mapper.Map<EmployeeDto>(result.Value));
            return Result<EmployeeDto>.Failure(result.ErrorMessage ?? "Empleado no encontrado.");
        }

        public async Task<Result<EmployeeDto>> GetNavBarById(int navPositionId, int idReference)
        {
            var result = await _repository.GetNavBarById(navPositionId, idReference);
            if (result.IsSuccess && result.Value != null)
                return Result<EmployeeDto>.Success(_mapper.Map<EmployeeDto>(result.Value));
            return Result<EmployeeDto>.Failure(result.ErrorMessage ?? "No hay registros disponibles.");
        }

        public async Task<Result<List<EmployeeDto>>> GetList(int page, int pageSize, string? name)
        {
            var result = await _repository.GetList(page, pageSize, name);
            if (result.IsSuccess && result.Value != null)
                return Result<List<EmployeeDto>>.Success(_mapper.Map<List<EmployeeDto>>(result.Value));
            return Result<List<EmployeeDto>>.Failure(result.ErrorMessage ?? "No se pudo cargar la lista de empleados.");
        }

        public async Task<Result<List<EmployeeDto>>> GetByDepartmentId(int departmentId)
        {
            var result = await _repository.GetByDepartmentId(departmentId);
            if (result.IsSuccess && result.Value != null)
                return Result<List<EmployeeDto>>.Success(_mapper.Map<List<EmployeeDto>>(result.Value));
            return Result<List<EmployeeDto>>.Failure(result.ErrorMessage ?? "Error al obtener empleados del departamento.");
        }

        public async Task<Result<EmployeeDto>> GetByCode(string code)
        {
            var result = await _repository.GetByCode(code);
            if (result.IsSuccess && result.Value != null)
                return Result<EmployeeDto>.Success(_mapper.Map<EmployeeDto>(result.Value));
            return Result<EmployeeDto>.Failure(result.ErrorMessage ?? "Empleado no encontrado.");
        }

        public async Task<Result<bool>> Insert(EmployeeDto employeeDto)
        {
            try
            {
                var entity = _mapper.Map<Employee>(employeeDto);
                return await _repository.Insert(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al insertar empleado.");
                return Result<bool>.Failure("Error al procesar los datos del empleado.");
            }
        }

        public async Task<Result<bool>> Update(EmployeeDto employeeDto)
        {
            try
            {
                var entity = _mapper.Map<Employee>(employeeDto);
                return await _repository.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de mapeo al actualizar empleado ID: {Id}", employeeDto.Id);
                return Result<bool>.Failure("Error al procesar la actualización.");
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            return await _repository.Delete(id);
        }
    }
}
