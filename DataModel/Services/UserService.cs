using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataModel.Contracts;
using DataModel.Repositories;
using BusinessModel;
using DataModel.UnitOfWork;
using DataModel.Mappings;

namespace DataModel.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork<LMSDbEntities> unitOfWork = null;
        private IGenericRepository<Users> repository = null;
        public UserService()
        {
            this.unitOfWork = new UnitOfWork<LMSDbEntities>();
            this.repository = new GenericRepository<Users>(unitOfWork);
        }

        public async Task<UserModel> IsValidUser(LoginModel credentials)
        {
            var config = new MapperConfiguration(cfg =>
                    cfg.AddMaps(typeof(Maps).Assembly));
            var mapper = new Mapper(config);

            var userInfo = await repository.Get(u => u.Email == credentials.UserName && u.PasswordHash == credentials.PasswordHash);

            var userDetails = mapper.Map<UserModel>(userInfo);
            return userDetails;
        }

    //    var config = new MapperConfiguration(cfg =>
    //                cfg.CreateMap<LeaveTypeModel, LeaveType>());
    //    var mapper = new Mapper(config);
    //    var leaveTypesMapped = mapper.Map<IEnumerable<LeaveType>>(leaveTypes);

    //    bool ltModelExists = false;
    //    unitOfWork.CreateTransaction();
    //                foreach (LeaveType lt in leaveTypesMapped)
    //                {
    //                    ltModelExists = (repository.IsExists(r => r.LeaveTypeID == lt.LeaveTypeID));
    //                    if (ltModelExists)
    //                        repository.Update(lt);
    //                    else
    //                        repository.Insert(lt);
    //                }
    //unitOfWork.Save();
    //unitOfWork.Commit();
    }
}
