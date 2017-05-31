using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Mvc;
using System.Data.Entity;
using HKSJRecruitment.Models.Models;
using System.Linq.Expressions;
using HKSJRecruitment.Web.Models;
using System.IO;
using Aspose.Cells;

namespace HKSJRecruitment.Web.Controllers
{
    public class Hr_EmployController : Controller
    {
        //
        // GET: /Hr_Employ/

        public ActionResult Index()
        {
            ViewBag.Buttons = this.LoadButton();
            return View();
        }

        public ActionResult Detail(int Id)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            RecruitModel dto = ctx.THR_Recruit.Select(c => new RecruitModel
            {
                DptId = c.DptId.Value,
                DptName = c.TErp_Department.DeptName,
                Id = c.Id,
                PostId = c.PostId.Value,
                PostName = c.TErp_Position.PositionName,
                Userurl = c.Userurl,
                Remark = c.Remark,
                Name = c.Name,
                Tel = c.Tel,
                Status = c.Status.Value,
                Interview = c.Interview,
                Email = c.Email,
                EntryType = c.EntryType.Value,
                NeedsName = c.THR_Needs.TErp_Department.DeptName + "  " + c.THR_Needs.TErp_Position.PositionName,
                CreateBy = c.CreateBy,
                CreateTime = c.CreateTime,
                HireTime = c.HireTime,
                Interviewer = c.Interviewer

            }).FirstOrDefault(c => c.Id == Id);

            if (dto != null)
            {
                HR_Employee emp = ctx.HR_Employee.FirstOrDefault(c => c.uid == dto.CreateBy);
                if (emp != null)
                {
                    dto.CreateBy = emp.name;
                }
                return Content(this.GetJSON(new { Result = true, Dto = dto }), this.JsonContentType());
            }
            else
            {
                return Content(this.GetJSON(new { Result = false, Msg = "未找到数据" }), this.JsonContentType());
            }
        }

        public ActionResult Edit(THR_Recruit dto)
        {
            if (dto.Id > 0)
            {
                HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
                THR_Recruit model = ctx.THR_Recruit.FirstOrDefault(c => c.Id == dto.Id);
                //状态为已入职，不能修改
                if (model.EntryType == 3)
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "状态为【已入职】，不能修改！" }), this.JsonContentType());
                }
                if (dto.EntryType == 3 && dto.HireTime == null)
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "已入职时，报到时间不能为空！" }), this.JsonContentType());
                }
                if (dto.HireTime != null && DateTime.Compare(dto.HireTime.Value, DateTime.Now) > 0 && dto.EntryType == 3)
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "报到时间不能超过今天！" }), this.JsonContentType());

                    //this.CopyObject<THR_Recruit>(model, dto);
                }
                model.HireTime = dto.HireTime;
                model.EntryType = dto.EntryType.Value;
                model.Remark = dto.Remark;
                model.EditTime = DateTime.Now;
                model.EditBy = this.AppUser().UserName;

                if (ctx.SaveChanges() >= 0)
                {

                    var m = ctx.THR_Recruit.Where(c => c.Id == dto.Id).Select(c => new RecruitModel
                        {
                            DptId = c.DptId.Value,
                            DptName = c.TErp_Department.DeptName,
                            Id = c.Id,
                            PostId = c.PostId.Value,
                            PostName = c.TErp_Position.PositionName,
                            Userurl = c.Userurl,
                            Remark = c.Remark,
                            Name = c.Name,
                            Tel = c.Tel,
                            Status = c.Status.Value,
                            Interview = c.Interview,
                            Email = c.Email,
                            EntryType = c.EntryType.Value,
                            NeedsName = c.THR_Needs.TErp_Department.DeptName + "  " + c.THR_Needs.TErp_Position.PositionName,
                            CreateBy = c.CreateBy,
                            CreateTime = c.CreateTime,
                            HireTime = c.HireTime,
                            Interviewer = c.Interviewer
                        }).FirstOrDefault();
                    HR_Employee emp = ctx.HR_Employee.FirstOrDefault(c => c.uid == model.CreateBy);
                    if (emp != null)
                    {
                        m.CreateBy = emp.name;
                    }
                    if (model.EntryType == 3)//已入职
                    {
                        WcfRestClient client = new HKSJRecruitment.Web.WcfRestClientAppBase("Employee");
                        EditRecord<string> resultname = client.Get<EditRecord<string>>("Uid/" + m.Name);
                        string uid = resultname.Dto;
                        HKSJRecruitment.Models.proModels.EmployeeDto empdto = new HKSJRecruitment.Models.proModels.EmployeeDto
                        {
                            FullName = model.Name,
                            CreateBy = 0,
                            Uid = uid,
                            CreateTime = DateTime.Now,
                            Sex = "男",
                            Mobile = model.Tel,
                            EntryDate = model.HireTime.Value,
                            Status = 2,
                            IsDelete = 0,
                            Education = "大专",
                            DeptId = 0,
                            PostId = 0,
                            EditBy = 0,
                            DeleteBy = 0,
                            Email = model.Email

                        };
                        EditRecord<HKSJRecruitment.Models.proModels.EmployeeDto> result = client.Post<HKSJRecruitment.Models.proModels.EmployeeDto, EditRecord<HKSJRecruitment.Models.proModels.EmployeeDto>>(empdto);

                    }

                    return Content(this.GetJSON(new { Result = true, Msg = "成功", Dto = m }), this.JsonContentType());

                }
                return Content(this.GetJSON(new { Result = false, Msg = "失败", Dto = dto }), this.JsonContentType());
            }
            else
            {
                return Content(this.GetJSON(new { Result = false, Msg = "失败，报到时间不能超过今天！", Dto = dto }), this.JsonContentType());
            }
        }

        public ActionResult List(RecruitQueryParam queryParam)
        {
            IPageList<RecruitModel> models = GetRecruitList(queryParam);
            List<RecruitModel> dtos = models.ToList();
            return Content(this.GetJSON(new { total = models.PageInfo.TotalCount, rows = dtos }), this.JsonContentType());
        }

        private IPageList<RecruitModel> GetRecruitList(RecruitQueryParam queryParam)
        {
            Expression<Func<THR_Recruit, bool>> predicate = c => c.IsDelete == 0 && c.Status == 82;
            if (!string.IsNullOrEmpty(queryParam.Name))
            {
                predicate = predicate.And(c => c.Name.StartsWith(queryParam.Name));
            }
            if (!string.IsNullOrEmpty(queryParam.Tel))
            {
                predicate = predicate.And(c => c.Tel.StartsWith(queryParam.Tel));
            }
            if (!string.IsNullOrEmpty(queryParam.interviewer))
            {
                predicate = predicate.And(c => c.Interviewer.StartsWith(queryParam.interviewer));
            }
            if (queryParam.DptId > 0)
            {
                predicate = predicate.And(c => c.DptId == queryParam.DptId);
            }
            if (queryParam.PostId > 0)
            {
                predicate = predicate.And(c => c.PostId == queryParam.PostId);
            }
            if (queryParam.QueryType > 0)
            {
                predicate = predicate.And(c => c.EntryType == queryParam.QueryType);
            }
            if (queryParam.IntervieweStart.HasValue && queryParam.IntervieweEnd.HasValue && queryParam.IntervieweStart.Value < queryParam.IntervieweEnd.Value)
            {
                predicate = predicate.And(c => c.Interview >= queryParam.IntervieweStart.Value && c.Interview <= queryParam.IntervieweEnd.Value);
            }
            if (!string.IsNullOrEmpty(queryParam.CreateBy))
            {
                predicate = predicate.And(c => c.CreateBy == queryParam.CreateBy);
            }
            if (queryParam.CreateTimeStart.HasValue && queryParam.CreateTimeEnd.HasValue && queryParam.CreateTimeStart.Value < queryParam.CreateTimeEnd.Value)
            {
                predicate = predicate.And(c => c.CreateTime >= queryParam.CreateTimeStart.Value && c.CreateTime <= queryParam.CreateTimeEnd.Value);
            }
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            IPageList<RecruitModel> models = ctx.THR_Recruit
                .Where(predicate)
                .OrderUsingSortExpression(queryParam.Sort, queryParam.Order)
                .Select(c => new RecruitModel
                {
                    DptId = c.DptId.Value,
                    DptName = c.TErp_Department.DeptName,
                    Id = c.Id,
                    PostId = c.PostId.Value,
                    PostName = c.TErp_Position.PositionName,
                    Userurl = c.Userurl,
                    Remark = c.Remark,
                    Name = c.Name,
                    Tel = c.Tel,
                    Status = c.Status.Value,
                    Interview = c.Interview,
                    Email = c.Email,
                    EntryType = c.EntryType.Value,
                    NeedsName = c.THR_Needs.TErp_Department.DeptName + "  " + c.THR_Needs.TErp_Position.PositionName,
                    CreateBy = c.CreateBy,
                    CreateTime = c.CreateTime,
                    HireTime = c.HireTime,
                    Interviewer = c.Interviewer
                })
                .ToPagedList(queryParam.PageIndex, queryParam.PageSize);

            foreach (var item in models)
            {
                HR_Employee emp = ctx.HR_Employee.FirstOrDefault(c => c.uid == item.CreateBy);
                if (emp != null)
                {
                    item.CreateBy = emp.name;
                }
            }
            return models;
        }

        public ActionResult ExportData(RecruitQueryParam queryParam)
        {
            queryParam.PageIndex = 1;
            queryParam.PageSize = 999999;
            List<RecruitModel> dtos = GetRecruitList(queryParam).ToList();
            MemoryStream ms = OutFileToStream(dtos);
            return File(ms.ToArray(), "application/ms-excel", string.Format("{0}.xls", "招聘录用信息" + DateTime.Now.ToString("yyMMddhhmmss")));
        }

        private MemoryStream OutFileToStream(List<RecruitModel> dtos)
        {
            Workbook workbook = new Workbook();         //工作簿 
            Worksheet sheet = workbook.Worksheets[0];   //工作表 
            Cells cells = sheet.Cells;                  //单元格 

            int rowNums = dtos.Count;                   //表格行数 

            //生成行1 列名行
            cells[0, 0].PutValue("姓名");
            cells[0, 1].PutValue("电话");
            cells[0, 2].PutValue("部门");
            cells[0, 3].PutValue("职位");
            cells[0, 4].PutValue("入职状态");
            cells[0, 5].PutValue("面试时间");
            cells[0, 6].PutValue("面试官");
            cells[0, 7].PutValue("报到时间");
            cells[0, 8].PutValue("邮箱");
            cells[0, 9].PutValue("备注");

            //生成数据行 
            for (int i = 0; i < rowNums; i++)
            {
                cells[1 + i, 0].PutValue(dtos[i].Name);
                cells[1 + i, 1].PutValue(dtos[i].Tel);
                cells[1 + i, 2].PutValue(dtos[i].DptName);
                cells[1 + i, 3].PutValue(dtos[i].PostName);
                cells[1 + i, 4].PutValue(dtos[i].EntryName);
                cells[1 + i, 5].PutValue(dtos[i].Interview.HasValue ? dtos[i].Interview.Value.ToString("yyyy-MM-dd") : "");
                cells[1 + i, 6].PutValue(dtos[i].Interviewer);
                cells[1 + i, 7].PutValue(dtos[i].HireTime.HasValue ? dtos[i].HireTime.Value.ToString("yyyy-MM-dd") : "");
                cells[1 + i, 8].PutValue(dtos[i].Email);
                cells[1 + i, 9].PutValue(dtos[i].Remark);
            }
            MemoryStream ms = workbook.SaveToStream();
            return ms;
        }
    }
}
