using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessModel;

namespace DataModel.Contracts
{
    public interface IUserService
    {
        Task<UserModel> IsValidUser(LoginModel credentials);
    }
}
