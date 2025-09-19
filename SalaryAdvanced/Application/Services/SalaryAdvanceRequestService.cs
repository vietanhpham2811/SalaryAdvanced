using AutoMapper;
using SalaryAdvanced.Application.DTOs;
using SalaryAdvanced.Application.Interfaces;
using SalaryAdvanced.Domain.Entities;
using SalaryAdvanced.Domain.Interfaces;

namespace SalaryAdvanced.Application.Services
{
    public class SalaryAdvanceRequestService : ISalaryAdvanceRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SalaryAdvanceRequestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetRequestDTO> CreateSalaryAdvanceRequestAsync(CreateRequestDTO dto)
        {
            var request = _mapper.Map<SalaryAdvanceRequest>(dto);
            request.StatusId = 1;
            await _unitOfWork.SalaryAdvanceRequests.AddAsync(request);
            await _unitOfWork.SaveChangesAsync();
            var result = _mapper.Map<GetRequestDTO>(request);
            return result;
        }

        public async Task<bool> DeleteSalaryAdvanceRequestAsync(int id)
        {
            var request = await _unitOfWork.SalaryAdvanceRequests.GetByIdAsync(id);
            if (request == null)
            {
                return false;  
            }
            _unitOfWork.SalaryAdvanceRequests.Remove(request);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task<GetRequestDTO?> GetSalaryAdvanceRequestsById(int id)
        {
            var request = await _unitOfWork.SalaryAdvanceRequests.GetByIdAsync(id);
            var result = _mapper.Map<GetRequestDTO>(request);
            return result;
        }

        public async Task<IEnumerable<GetRequestDTO>> GetSalaryAdvanceRequestsByEmployeeAsync(int employeeId)
        {
            var list = await _unitOfWork.SalaryAdvanceRequests.GetByEmployeeAsync(employeeId);
            var result = _mapper.Map<IEnumerable<GetRequestDTO>>(list);
            return result;
        }

        public async Task<IEnumerable<GetRequestDTO>> GetSalaryAdvanceRequestsByStatus(int statusId)
        {
            var list = await _unitOfWork.SalaryAdvanceRequests.GetByStatusAsync(statusId);
            var result = _mapper.Map<IEnumerable<GetRequestDTO>>(list);
            return result;
        }

        public async Task<IEnumerable<GetRequestDTO>> GetSalaryAdvanceRequestsForApprovalAsync(int managerId)
        {
            var list = await _unitOfWork.SalaryAdvanceRequests.GetRequestsForApprovalAsync(managerId);
            var result = _mapper.Map<IEnumerable<GetRequestDTO>>(list);
            return result;
        }

        public async Task<GetRequestDTO?> RespondToSalaryAdvanceRequestAsync(RequestResponseDTO dto)
        {
            var response = await _unitOfWork.SalaryAdvanceRequests.GetByIdAsync(dto.Id);
            if (response == null)
            {
                return null;
            }
            response.StatusId = dto.StatusId;
            response.ApprovedById = dto.ApprovedById;
            response.RejectionReason = dto.RejectionReason;
            response.ProcessedAt = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync();
            var result = _mapper.Map<GetRequestDTO>(response);
            return result;
        }
    }
}