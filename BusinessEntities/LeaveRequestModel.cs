using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BusinessModel
{
    public class LeaveTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> DefaultDays { get; set; }
    }

    public class LeaveRequestModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Display(Name = "Leave Type")]
        public string LeaveTypeName { get; set; }
        [Display(Name = "From Date")]
        [Required]
        public DateTime FromDate { get; set; }
        [Display(Name = "To Date")]
        [Required]
        public DateTime ToDate { get; set; }
        [Display(Name = "No Of Days")]
        [Required]
        public int TotalDays { get; set; }
        [Display(Name = "Reason")]
        [MaxLength(100)]
        public string Reason { get; set; }
        [Display(Name = "File")]
        [MaxLength(50)]
        public string DocumentName { get; set; }
        [Display(Name = "Status")]
        public string ApprovalStatusName { get; set; }
        public string Users2FirstName { get; set; }
        public string Users2LastName { get; set; }
    }

    public class ViewLeaveRequestModel
    {
        [Display(Name = "Paternity Leave")]
        public int PaternityLeave { get; set; }
        [Display(Name = "Maternity Leave")]
        public int MaternityLeave { get; set; }
        [Display(Name = "Sick Leave")]
        public int SickLeave { get; set; }
        [Display(Name = "Earned Leave")]
        public int EarnedLeave { get; set; }
        [Display(Name = "Casual Leave")]
        public int CasualLeave { get; set; }
        public List<LeaveTypeModel> LeaveTypes { get; set; }
        public List<LeaveRequestModel> LeaveRequests { get; set; }
    }

    public class CreateLeaveRequestModel
    {
        public IEnumerable<SelectListItem> LeaveTypes { get; set; }
        public int UserId { get; set; }
        [Display(Name = "Leave Type")]
        public int LeaveTypeId { get; set; }
        [Display(Name = "From Date")]
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public string ToDate { get; set; }
        [Display(Name = "No Of Days")]
        [Required]
        public int TotalDays { get; set; }
        [Display(Name = "Reason")]
        [MaxLength(100)]
        public string Reason { get; set; }
        [Display(Name = "Document Name")]
        [MaxLength(50)]
        public string DocumentName { get; set; }
        public string CreatedOn { get; set; }
        public int CreatedBy { get; set; }
    }
}
