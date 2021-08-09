using AutoMapper;
using BusinessModel;

namespace DataModel.Mappings
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<AccessType, AccessTypeModel>().ReverseMap();
            CreateMap<Gender, GenderModel>().ReverseMap();
            CreateMap<LeaveType, LeaveTypeModel>().ReverseMap();
            CreateMap<LeaveRequest, LeaveRequestModel>().ReverseMap();
            CreateMap<LeaveRequest, CreateLeaveRequestModel>().ReverseMap();
            CreateMap<Users, UserModel>().ReverseMap();
        }
    }
}
