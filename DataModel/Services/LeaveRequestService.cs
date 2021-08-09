using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataModel.Contracts;
using DataModel.Repositories;
using DataModel.UnitOfWork;
using BusinessModel;
using DataModel.Mappings;

namespace DataModel.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly IUnitOfWork<LMSDbEntities> unitOfWork = null;
        private IGenericRepository<LeaveRequest> repository = null;
        private IGenericRepository<LeaveType> repoLeaveType = null;
        public LeaveRequestService()
        {
            this.unitOfWork = new UnitOfWork<LMSDbEntities>();
            this.repository = new GenericRepository<LeaveRequest>(unitOfWork);
            this.repoLeaveType = new GenericRepository<LeaveType>(unitOfWork);
        }

        public async Task<bool> ApproveRejectLeaveRequest(int Id, bool IsApprove, int ApprovedBy)
        {
            try
            {
                var updateLeaveRequest = await repository.Get(q => q.Id == Id);
                unitOfWork.CreateTransaction();
                if (IsApprove)
                    updateLeaveRequest.ApprovalStatusId = 2;
                else
                    updateLeaveRequest.ApprovalStatusId = 3;
                updateLeaveRequest.ApprovedOn = DateTime.Now;
                updateLeaveRequest.ApprovedBy = ApprovedBy;
                repository.Update(updateLeaveRequest);
                if (updateLeaveRequest.ApprovalStatusId == 2)
                {
                    var userRepo = new GenericRepository<Users>(unitOfWork);
                    var requestedUser = await userRepo.Get(q => q.Id == updateLeaveRequest.UserId);
                    switch (updateLeaveRequest.LeaveTypeId)
                    {
                        case 1:
                            requestedUser.EarnedLeave = requestedUser.EarnedLeave + 1;
                            break;
                        case 2:
                            requestedUser.SickLeave = requestedUser.SickLeave + 1;
                            break;
                        case 3:
                            requestedUser.CasualLeave = requestedUser.CasualLeave + 1;
                            break;
                        case 4:
                            requestedUser.PaternityLeave = requestedUser.PaternityLeave + 1;
                            break;
                        case 5:
                            requestedUser.MaternityLeave = requestedUser.MaternityLeave + 1;
                            break;
                    }
                    userRepo.Update(requestedUser);
                }
                await unitOfWork.Save();
                unitOfWork.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CreateEmployeeLeaveRequest(CreateLeaveRequestModel employeeLeaveRequest)
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                        cfg.AddMaps(typeof(Maps).Assembly));
                var mapper = new Mapper(config);
                var leaveRequestMapped = mapper.Map<LeaveRequest>(employeeLeaveRequest);
                leaveRequestMapped.ApprovalStatusId = 1;
                unitOfWork.CreateTransaction();
                repository.Insert(leaveRequestMapped);
                await unitOfWork.Save();
                unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<ViewLeaveRequestModel> GetEmployeeLeaveRequests()
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                        cfg.AddMaps(typeof(Maps).Assembly));
                var mapper = new Mapper(config);

                var leaveRequests = await repository.GetAll();
                var leaveRequestsMapped = mapper.Map<List<LeaveRequestModel>>(leaveRequests);

                var requestedUserId = leaveRequests.Select(s => s.UserId).FirstOrDefault();
                var requestedUser = await (new GenericRepository<Users>(unitOfWork)).Get(q => q.Id == requestedUserId);
                var requestedUserMapped = mapper.Map<UserModel>(requestedUser);

                var leaveTypes = await repoLeaveType.GetAll();
                var leaveTypesMapped = mapper.Map<List<LeaveTypeModel>>(leaveTypes);

                var viewLeaveRequest = new ViewLeaveRequestModel
                {
                    PaternityLeave = requestedUserMapped.PaternityLeave ?? 0,
                    MaternityLeave = requestedUserMapped.MaternityLeave ?? 0,
                    SickLeave = requestedUserMapped.SickLeave ?? 0,
                    EarnedLeave = requestedUserMapped.EarnedLeave ?? 0,
                    CasualLeave = requestedUserMapped.CasualLeave ?? 0,
                    LeaveTypes = leaveTypesMapped,
                    LeaveRequests = leaveRequestsMapped
                };
                return viewLeaveRequest;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<LeaveTypeModel>> GetLeaveTypes()
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                        cfg.AddMaps(typeof(Maps).Assembly));
                var mapper = new Mapper(config);

                var leaveTypes = await repoLeaveType.GetAll();
                var leaveTypesMapped = mapper.Map<List<LeaveTypeModel>>(leaveTypes);

                return leaveTypesMapped;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateLeaveTypeDefaults(IList<LeaveTypeModel> leaveTypeDefaultsModel)
        {
            try
            {
                LeaveType ltUpdate;
                unitOfWork.CreateTransaction();
                foreach (LeaveTypeModel lt in leaveTypeDefaultsModel)
                {
                    ltUpdate = await repoLeaveType.Get(q => q.Id == lt.Id);
                    ltUpdate.DefaultDays = lt.DefaultDays;
                    repoLeaveType.Update(ltUpdate);
                }
                await unitOfWork.Save();
                unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
