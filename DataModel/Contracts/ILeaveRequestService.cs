using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessModel;

namespace DataModel.Contracts
{
    public interface ILeaveRequestService
    {
        Task<IEnumerable<LeaveTypeModel>> GetLeaveTypes();
        Task<ViewLeaveRequestModel> GetEmployeeLeaveRequests();
        Task<bool> CreateEmployeeLeaveRequest(CreateLeaveRequestModel employeeLeaveRequest);
        Task<bool> ApproveRejectLeaveRequest(int Id, bool IsApprove, int ApprovedBy);
        Task<bool> UpdateLeaveTypeDefaults(IList<LeaveTypeModel> leaveTypeDefaultsModel);
    }
}
