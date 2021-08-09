using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.Contracts;
using DataModel.Services;
using BusinessModel;
using System.Threading.Tasks;

namespace LeaveManagement.Controllers
{
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveRequestService leaveTypeService;
        public LeaveRequestController()
        {
            leaveTypeService = new LeaveRequestService();
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                var model = await leaveTypeService.GetEmployeeLeaveRequests();
                return View(model);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public async Task<ActionResult> ApproveOrReject(int id, bool isApprove)
        {
            try
            {
                var userId = Session["UserId"] ?? Session["UserId"].ToString();
                var result = await leaveTypeService.ApproveRejectLeaveRequest(id, isApprove, Convert.ToInt32(userId));
                if (!result)
                {
                    ModelState.AddModelError("", "Something Went Wrong...");
                }
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: LeaveRequest/Create
        public async Task<ActionResult> Create()
        {
            try
            {
                var leaveTypes = await leaveTypeService.GetLeaveTypes();
                var leaveTypeItems = leaveTypes.Select(q => new SelectListItem
                {
                    Text = q.Description,
                    Value = q.Id.ToString()
                });
                var model = new CreateLeaveRequestModel
                {
                    LeaveTypes = leaveTypeItems
                };
                return View(model);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateLeaveRequestModel model)
        {
            try
            {
                var userId = Session["UserId"] ?? Session["UserId"].ToString();
                var leaveTypes = await leaveTypeService.GetLeaveTypes();
                var leaveTypeItems = leaveTypes.Select(q => new SelectListItem
                {
                    Text = q.Description,
                    Value = q.Id.ToString()
                });
                model.LeaveTypes = leaveTypeItems;
                model.CreatedOn = DateTime.Now.ToString();
                model.CreatedBy = Convert.ToInt32(userId);
                model.UserId = Convert.ToInt32(userId);
                var result = await leaveTypeService.CreateEmployeeLeaveRequest(model);
                if (!result)
                {
                    ModelState.AddModelError("", "Something Went Wrong, please try again");
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Modify()
        {
            try
            {
                var userId = Session["UserId"] ?? Session["UserId"].ToString();
                var EL = Convert.ToInt32(Request.Form["EL"]);
                var SL = Convert.ToInt32(Request.Form["SL"]);
                var CL = Convert.ToInt32(Request.Form["CL"]);
                var PL = Convert.ToInt32(Request.Form["PL"]);
                var ML = Convert.ToInt32(Request.Form["ML"]);

                var ELId = Convert.ToInt32(Request.Form["ELId"]);
                var SLId = Convert.ToInt32(Request.Form["SLId"]);
                var CLId = Convert.ToInt32(Request.Form["CLId"]);
                var PLId = Convert.ToInt32(Request.Form["PLId"]);
                var MLId = Convert.ToInt32(Request.Form["MLId"]);

                IList<LeaveTypeModel> ltModel = new List<LeaveTypeModel>();
                ltModel.Add(new LeaveTypeModel { Id = ELId, DefaultDays = EL });
                ltModel.Add(new LeaveTypeModel { Id = SLId, DefaultDays = SL });
                ltModel.Add(new LeaveTypeModel { Id = CLId, DefaultDays = CL });
                ltModel.Add(new LeaveTypeModel { Id = PLId, DefaultDays = PL });
                ltModel.Add(new LeaveTypeModel { Id = MLId, DefaultDays = ML });

                var result = await leaveTypeService.UpdateLeaveTypeDefaults(ltModel);
                if (!result)
                {
                    ModelState.AddModelError("", "Something Went Wrong, please try again");
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}