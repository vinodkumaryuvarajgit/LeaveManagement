using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessModel
{
    public class UserModel
    {
        public int Id { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public GenderModel Gender { get; set; }
        [Display(Name = "Gender Type")]
        public string GenderName { get; set; }
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        [Display(Name = "Access Type")]
        public AccessTypeModel AccessType { get; set; }
        public int AccessTypeId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> PaternityLeave { get; set; }
        public Nullable<int> MaternityLeave { get; set; }
        public Nullable<int> SickLeave { get; set; }
        public Nullable<int> EarnedLeave { get; set; }
        public Nullable<int> CasualLeave { get; set; }
    }

    public class GenderModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class AccessTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class LoginModel
    {
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "Password")]
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public string ReturnURL { get; set; }
    }

    public enum AccessTypeEnum
    { 
        Admin=1,
        Employee=2
    }
}
