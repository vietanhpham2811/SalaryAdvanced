using AutoMapper;
using SalaryAdvanced.Application.DTOs;
using SalaryAdvanced.Application.Interfaces;
using SalaryAdvanced.Domain.Entities;
using SalaryAdvanced.Domain.Interfaces;

namespace SalaryAdvanced.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<GetDepartmentDTO?> AddDepartmentAsync(DepartmentDTO dto)
        {
            var dept = _mapper.Map<Department>(dto);
            await _unitOfWork.Departments.AddAsync(dept);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<GetDepartmentDTO>(dept);
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var result = await _unitOfWork.Departments.DeleteAsync(id);
            if (result == false)
                return false;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<GetDepartmentDTO>> GetAllDepartmentsAsync()
        {
            var list = await _unitOfWork.Departments.GetAllAsync();
            return _mapper.Map<List<GetDepartmentDTO>>(list);
        }

        public async Task<GetDepartmentDTO?> GetDepartmentByIdAsync(int id)
        {
            var dept = await _unitOfWork.Departments.GetByIdAsync(id);
            return _mapper.Map<GetDepartmentDTO?>(dept);
        }

        public async Task<GetDepartmentDTO?> UpdateDepartmentAsync(int id, DepartmentDTO dto)
        {
            var dept = _mapper.Map<Department>(dto);
            await _unitOfWork.Departments.UpdateAsync(id, dept);
            await _unitOfWork.SaveChangesAsync();
            var newDept = await _unitOfWork.Departments.GetByIdAsync(id);
            return _mapper.Map<GetDepartmentDTO?>(newDept);
        }

        public async Task<bool> DepartmentExist(int id)
        {
            return await _unitOfWork.Departments.ExistsAsync(id);
        }
    }
}
