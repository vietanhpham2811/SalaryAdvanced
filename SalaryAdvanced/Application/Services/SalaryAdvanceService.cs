using SalaryAdvanced.Application.DTOs;
using SalaryAdvanced.Application.Interfaces;
using SalaryAdvanced.Domain.Entities;
using SalaryAdvanced.Domain.Interfaces;

namespace SalaryAdvanced.Application.Services
{
    public class SalaryAdvanceService : ISalaryAdvanceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SalaryAdvanceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        
    }
}