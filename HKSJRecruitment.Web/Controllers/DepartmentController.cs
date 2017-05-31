using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using HKSJRecruitment.Models.Models;
using HKSJRecruitment.Web.Models;
using Common.Mvc;
using System.Data;
using System.Data.Entity;
using System.Linq.Expressions;

namespace HKSJRecruitment.Web.Controllers
{
    public class DepartmentController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Buttons = this.LoadButton();
            return View();
        }

        public ActionResult List()
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            List<TErp_Department> dtos = ctx.TErp_Department
                .Where(c => c.IsDelete == 0)
                .OrderUsingSortExpression("parentid asc, seqno asc")
                .ToList();
            return Content(this.GetJSON(new { total = dtos.Count, rows = dtos }), this.JsonContentType());
        }
        public ActionResult Detail(int Id)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            Tapp_Button dto = ctx.Tapp_Button.FirstOrDefault(c => c.Id == Id);
            if (dto != null)
            {
                return Content(this.GetJSON(new { Result = true, Dto = dto }), this.JsonContentType());
            }
            else
            {
                return Content(this.GetJSON(new { Result = false, Msg = "未找到数据" }), this.JsonContentType());
            }
        }
        [HttpPost]
        public ActionResult Add(TErp_Department dto)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            string deptName = dto.DeptName;
            if (ctx.TErp_Department.Count(c => c.DeptName == deptName && c.IsDelete == 0) > 0)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "部门名称[" + deptName + "]重复", Dto = dto }), this.JsonContentType());
            }
            Tapp_User user = this.AppUser();
            dto.CreateBy = user.UserName;
            dto.CreateTime = DateTime.Now;
            ctx.TErp_Department.Add(dto);
            if (ctx.SaveChanges() >= 0)
            {
                return Content(this.GetJSON(new { Result = true, Msg = "成功", Dto = dto }), this.JsonContentType());
            }
            return Content(this.GetJSON(new { Result = false, Msg = "失败", Dto = dto }), this.JsonContentType());
        }

        [HttpPost]
        public ActionResult Edit(TErp_Department dto)
        {
            if (dto.Id > 0)
            {
                HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
                TErp_Department model = ctx.TErp_Department.FirstOrDefault(c => c.Id == dto.Id);
                string deptName = dto.DeptName;
                int deptId = dto.Id;
                if (ctx.TErp_Department.Count(c => c.IsDelete == 0 && c.DeptName == deptName && c.Id != deptId) > 0)
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "部门名称[" + deptName + "]重复", Dto = dto }), this.JsonContentType());
                }
                this.CopyObject<TErp_Department>(model, dto);
                Tapp_User user = this.AppUser();
                model.EditBy = user.UserName;
                model.EditTime = DateTime.Now;
                if (ctx.SaveChanges() >= 0)
                {
                    return Content(this.GetJSON(new { Result = true, Msg = "成功", Dto = dto }), this.JsonContentType());
                }
                return Content(this.GetJSON(new { Result = false, Msg = "失败", Dto = dto }), this.JsonContentType());
            }
            else
            {
                return Content(this.GetJSON(new { Result = false, Msg = "失败，未找到要修改的数据", Dto = dto }), this.JsonContentType());
            }
        }




        [HttpPost]
        public ActionResult DeleteMulti(string Id)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            string[] arrId = Id.Split(new char[] { ',', '-' }, StringSplitOptions.RemoveEmptyEntries);
            int counter = 0;
            Tapp_User user = this.AppUser();
            foreach (var item in arrId)
            {
                int tid = Convert.ToInt32(item);
                TErp_Department model = ctx.TErp_Department.FirstOrDefault(c => c.Id == tid);
                if (model != null)
                {
                    int deptId = model.Id;
                    if (ctx.THR_Needs.Count(c => c.DeptId == deptId && c.IsDelete == 0) > 0
                        || ctx.THR_Recruit.Count(c => c.DptId == deptId && c.IsDelete != 1) > 0)
                    {
                        return Content(this.GetJSON(new { Result = false, Msg = "部门[" + model.DeptName + "]已被引用,不能删除", Id = counter }), this.JsonContentType());
                    }
                    model.IsDelete = 1;
                    model.DeleteBy = user.UserName;
                    model.DeleteTime = DateTime.Now;
                    counter++;
                }
            }
            if (ctx.SaveChanges() >= 0)
            {
                return Content(this.GetJSON(new { Result = true, Msg = "成功", Id = counter }), this.JsonContentType());
            }
            return Content(this.GetJSON(new { Result = false, Msg = "失败", id = counter }), this.JsonContentType());
        }

        [Anonymous]
        public ActionResult ListDepartment()
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            var query = ctx.TErp_Department
                .Where(c => c.IsDelete == 0).
                OrderUsingSortExpression("seqno desc")
                .Select(c => new DepartmentDto
                {
                    Id = c.Id,
                    DeptName = c.DeptName,
                    ParentId = c.ParentId,
                    SeqNo = c.SeqNo,
                    DeptIcon = c.DeptIcon
                });
            List<DepartmentDto> dtos = query.ToList();
            return Content(this.GetJSON(new { total = dtos.Count, rows = dtos }), this.JsonContentType());
        }
    }
}
