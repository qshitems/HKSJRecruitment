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
using System.Web;
using Aspose.Cells;
using System.IO;
using System.Text.RegularExpressions;

namespace HKSJRecruitment.Web.Controllers
{
    public class RecruitController : Controller
    {
        /// <summary>
        /// 招聘信息控制器
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Buttons = this.LoadButton();
            return View();
        }
        /// <summary>
        /// 状态下拉
        /// </summary>
        /// <returns></returns>
        [Anonymous]
        public ActionResult ListStatus()
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            List<Tapp_Param> models = ctx.Tapp_Param
        .Where(c => c.IsDelete == 0 && c.Parentid == 10).OrderUsingSortExpression("parentid asc, sort asc").ToList();
            return Content(this.GetJSON(new { total = models.Count, rows = models }), this.JsonContentType());
        }
        /// <summary>
        /// 修改管理员下拉
        /// </summary>
        /// <returns></returns>
        [Anonymous]
        public ActionResult ListCreateBy()
        {

            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            var query = from a in ctx.Tapp_User
                        join b in ctx.HR_Employee on a.UserName equals b.uid
                        where (a.State == 1 && b.dptid == 6)
                        orderby a.Id ascending
                        select new UserEmployee
                 {
                     UserName = a.UserName,
                     Name = b.name
                 };
            List<UserEmployee> models = query.ToList();


            return Content(this.GetJSON(new { total = models.Count, rows = models }), this.JsonContentType());
        }
        /// <summary>
        /// 搜索管理员下拉
        /// </summary>
        /// <returns></returns>
        [Anonymous]
        public ActionResult ListSearchCreateBy()
        {

            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            var query = from a in ctx.Tapp_User
                        join b in ctx.HR_Employee on a.UserName equals b.uid
                        where (a.State == 1)
                        orderby a.Id ascending
                        select new UserEmployee
                        {
                            UserName = a.UserName,
                            Name = b.name
                        };
            List<UserEmployee> models = query.ToList();


            return Content(this.GetJSON(new { total = models.Count, rows = models }), this.JsonContentType());
        }

        [Anonymous]

        public ActionResult List(RecruitQueryParam queryParam)
        {
            Expression<Func<THR_Recruit, bool>> predicate = c => c.IsDelete == 0;
            if (!string.IsNullOrEmpty(queryParam.Name))
            {
                predicate = predicate.And(c => c.Name.StartsWith(queryParam.Name));
            }
            if (!string.IsNullOrEmpty(queryParam.Tel))
            {
                predicate = predicate.And(c => c.Tel.StartsWith(queryParam.Tel));
            }
            if (queryParam.Status > 0)
            {
                predicate = predicate.And(c => c.Status == queryParam.Status);
            }
            if (queryParam.DptId > 0)
            {
                predicate = predicate.And(c => c.DptId == queryParam.DptId);
            }
            if (queryParam.PostId > 0)
            {
                predicate = predicate.And(c => c.PostId == queryParam.PostId);
            }
            if (!string.IsNullOrEmpty(queryParam.CreateBy))
            {
                predicate = predicate.And(c => c.CreateBy == queryParam.CreateBy);
            }
            if (queryParam.IntervieweStart.HasValue && queryParam.IntervieweEnd.HasValue && queryParam.IntervieweStart.Value < queryParam.IntervieweEnd.Value)
            {
                predicate = predicate.And(c => c.Interview >= queryParam.IntervieweStart.Value && c.Interview <= queryParam.IntervieweEnd.Value);
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
                    StatusName = c.Tapp_Param.ParamsName,
                    Interview = c.Interview,
                    Email = c.Email,
                    CreateBy = c.CreateBy,
                    CreateTime = c.CreateTime,
                    HireTime = c.HireTime,
                    NeedsId = c.NeedsId,
                    NeedsName = c.THR_Needs.TErp_Department.DeptName + "  " + c.THR_Needs.TErp_Position.PositionName,
                    Interviewer = c.Interviewer

                })
                .ToPagedList(queryParam.PageIndex, queryParam.PageSize);
            List<RecruitModel> dtos = models.ToList();
            foreach (var item in dtos)
            {
                HR_Employee emp = ctx.HR_Employee.FirstOrDefault(c => c.uid == item.CreateBy);
                if (emp != null)
                {
                    item.CreateBy = emp.name;
                }
            }
            return Content(this.GetJSON(new { total = models.PageInfo.TotalCount, rows = dtos }), this.JsonContentType());



        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Detail(int Id)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            RecruitModel dto = GetRecruitModel(Id, ctx);

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
        public ActionResult NeedsAdd(THR_Recruit dto)
        {

            return Content(this.GetJSON(new { Result = false, Msg = "失败", Dto = dto }), this.JsonContentType());
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(THR_Recruit dto)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            if (!dto.DptId.HasValue)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "部门不存在,请在组织架构中维护" }), this.JsonContentType());
            }
            int dptId = dto.DptId.Value;
            if (ctx.TErp_Department.Count(c => c.Id == dptId) == 0)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "部门不存在,请在组织架构中维护" }), this.JsonContentType());
            }
            if (!dto.PostId.HasValue)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "职位不存在,请在职位管理中维护" }), this.JsonContentType());
            }
            int postId = dto.PostId.Value;
            if (ctx.TErp_Position.Count(c => c.Id == postId) == 0)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "职位不存在,请在职位管理中维护" }), this.JsonContentType());
            }
            if (!dto.Status.HasValue)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "状态不存在,请在参数配置中维护" }), this.JsonContentType());
            }
            int status = dto.Status.Value;
            if (ctx.Tapp_Param.Count(c => c.Id == status) == 0)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "状态不存在,请在参数配置中维护" }), this.JsonContentType());
            }
            if (string.IsNullOrEmpty(dto.Interview.ToString()))
            {
                return Content(this.GetJSON(new { Result = false, Msg = "面试时间不能为空" }), this.JsonContentType());
            }
            DateTime interview;
            if (!DateTime.TryParse(dto.Interview.ToString(), out interview) && !string.IsNullOrEmpty(dto.Interview.ToString()))
            {
                return Content(this.GetJSON(new { Result = false, Msg = "面试时间格式不正确" }), this.JsonContentType());
            }
            DateTime hireTime;
            if (!DateTime.TryParse(dto.HireTime.ToString(), out hireTime) && !string.IsNullOrEmpty(dto.HireTime.ToString()))
            {
                return Content(this.GetJSON(new { Result = false, Msg = "报道时间格式不正确" }), this.JsonContentType());
            }
            if (dto.NeedsId == 0 || dto.NeedsId == null)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "招聘需求不能为空" }), this.JsonContentType());
            }
            if (dto.Status == 82)//已录用
            {
                THR_Needs needmodel = ctx.THR_Needs.FirstOrDefault(c => c.Id == dto.NeedsId);
                int Quantity = ctx.THR_Recruit
         .Where(c => c.IsDelete == 0 && c.NeedsId == dto.NeedsId && c.Status == dto.Status).ToList().Count;//已录用
                if (needmodel.NeedQuantity <= Quantity)
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "招聘人数已达到招聘需求数,请新增或变更招聘需求!" }), this.JsonContentType());
                }
                //needmodel.HaveBeenQuantity = Quantity + 1;
                //if (needmodel.HaveBeenQuantity == needmodel.NeedQuantity)
                //{ needmodel.IsHaveBeen = 1; }
            }
            TErp_Department dept = ctx.TErp_Department.FirstOrDefault(c => c.Id == dto.DptId.Value);
            dept.SeqNo = dept.SeqNo + 1;
            if (ctx.THR_Recruit.Count(c => c.Tel == dto.Tel && c.IsDelete == 0) > 0)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "电话已存在" }), this.JsonContentType());
            }
            dto.CreateTime = DateTime.Now;
            dto.CreateBy = this.AppUser().UserName;
            dto.IsDelete = 0;
            dto.EntryType = 1;
            ctx.THR_Recruit.Add(dto);
            if (ctx.SaveChanges() > 0)
            {
                RecruitModel model = GetRecruitModel(dto.Id, ctx);

                return Content(this.GetJSON(new { Result = true, Msg = "成功", Dto = model }), this.JsonContentType());
            }
            return Content(this.GetJSON(new { Result = false, Msg = "失败", Dto = dto }), this.JsonContentType());
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(THR_Recruit dto)
        {
            if (dto.Id > 0)
            {
                HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
                if (ctx.THR_Recruit.Count(c => c.Tel == dto.Tel && c.IsDelete == 0 && c.Id != dto.Id) > 0)
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "电话已存在" }), this.JsonContentType());
                }
                if (!dto.DptId.HasValue)
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "部门不存在,请在组织架构中维护" }), this.JsonContentType());
                }
                int dptId = dto.DptId.Value;
                if (ctx.TErp_Department.Count(c => c.Id == dptId) == 0)
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "部门不存在,请在组织架构中维护" }), this.JsonContentType());
                }
                if (!dto.PostId.HasValue)
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "职位不存在,请在职位管理中维护" }), this.JsonContentType());
                }
                int postId = dto.PostId.Value;
                if (ctx.TErp_Position.Count(c => c.Id == postId) == 0)
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "职位不存在,请在职位管理中维护" }), this.JsonContentType());
                }
                if (!dto.Status.HasValue)
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "状态不存在,请在参数配置中维护" }), this.JsonContentType());
                }
                int status = dto.Status.Value;
                if (ctx.Tapp_Param.Count(c => c.Id == status) == 0)
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "状态不存在,请在参数配置中维护" }), this.JsonContentType());
                }
                if (string.IsNullOrEmpty(dto.Interview.ToString()))
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "面试时间不能为空" }), this.JsonContentType());
                }
                DateTime interview;
                if (!DateTime.TryParse(dto.Interview.ToString(), out interview) && !string.IsNullOrEmpty(dto.Interview.ToString()))
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "面试时间格式不正确" }), this.JsonContentType());
                }
                DateTime hireTime;
                if (!DateTime.TryParse(dto.HireTime.ToString(), out hireTime) && !string.IsNullOrEmpty(dto.HireTime.ToString()))
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "报道时间格式不正确" }), this.JsonContentType());
                }
                if (dto.NeedsId == 0 || dto.NeedsId == null)
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "招聘需求不能为空" }), this.JsonContentType());
                }

                //THR_Recruit model = ctx.THR_Recruit.Include("THR_Needs").FirstOrDefault(c => c.Id == dto.Id);
                THR_Recruit model = ctx.THR_Recruit.FirstOrDefault(c => c.Id == dto.Id);
                //if (model.Status == 82)
                //{
                //    return Content(this.GetJSON(new { Result = false, Msg = "已录用的招聘信息不能修改" }), this.JsonContentType());
                //}
                if (dto.Status == 82 && dto.Status != model.Status)//已录用  
                {
                    THR_Needs needsmodel = ctx.THR_Needs.FirstOrDefault(c => c.Id == dto.NeedsId);
                    int Quantity = ctx.THR_Recruit
      .Where(c => c.IsDelete == 0 && c.NeedsId == dto.NeedsId && c.Status == dto.Status).ToList().Count;//已录用
                    if (needsmodel.NeedQuantity <= Quantity)
                    {
                        return Content(this.GetJSON(new { Result = false, Msg = "招聘人数已达到招聘需求数,请新增或变更招聘需求!" }), this.JsonContentType());
                    }
                }
                this.CopyObject<THR_Recruit>(model, dto);
                model.EditTime = DateTime.Now;
                model.EditBy = this.AppUser().UserName;
                if (ctx.SaveChanges() >= 0)
                {
                    RecruitModel m = GetRecruitModel(dto.Id, ctx);

                    return Content(this.GetJSON(new { Result = true, Msg = "成功", Dto = m }), this.JsonContentType());
                }
                return Content(this.GetJSON(new { Result = false, Msg = "失败", Dto = dto }), this.JsonContentType());
            }
            else
            {
                return Content(this.GetJSON(new { Result = false, Msg = "失败，未找到要修改的数据", Dto = dto }), this.JsonContentType());
            }
        }

        [HttpPost]
        public ActionResult EditCreateBy(THR_Recruit dto)
        {
            if (dto.Id > 0)
            {
                HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
                THR_Recruit model = ctx.THR_Recruit.FirstOrDefault(c => c.Id == dto.Id);
                model.CreateBy = dto.CreateBy;
                if (ctx.SaveChanges() >= 0)
                {
                    RecruitModel m = GetRecruitModel(dto.Id, ctx);

                    return Content(this.GetJSON(new { Result = true, Msg = "成功", Dto = m }), this.JsonContentType());
                }
                return Content(this.GetJSON(new { Result = false, Msg = "失败", Dto = dto }), this.JsonContentType());
            }
            else
            {
                return Content(this.GetJSON(new { Result = false, Msg = "失败，未找到要修改的数据", Dto = dto }), this.JsonContentType());
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteMulti(string Id)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            string[] arrId = Id.Split(new char[] { ',', '-' }, StringSplitOptions.RemoveEmptyEntries);
            int counter = 0;
            foreach (var item in arrId)
            {
                int tid = Convert.ToInt32(item);
                THR_Recruit model = ctx.THR_Recruit.FirstOrDefault(c => c.Id == tid);
                if (model != null)
                {
                    model.IsDelete = 1;
                    model.DeleteBy = this.AppUser().UserName;
                    model.DeleteTime = DateTime.Now;
                    // ctx.THR_Recruit.Remove(model);
                    counter++;
                }
            }
            if (ctx.SaveChanges() > 0)
            {
                return Content(this.GetJSON(new { Result = true, Msg = "成功", Id = counter }), this.JsonContentType());
            }
            return Content(this.GetJSON(new { Result = false, Msg = "失败", id = counter }), this.JsonContentType());
        }

        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportData()
        {
            HttpPostedFileBase postFile = HttpContext.Request.Files[0];
            string extension = Path.GetExtension(postFile.FileName).ToLower();
            if (extension != ".xls" && extension != ".xlsx")
            {
                return Content(this.GetJSON(new { Result = false, Msg = "文件格式错误" }), this.JsonContentType());
            }
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            Tapp_User user = this.AppUser();
            Workbook workbook = new Workbook(postFile.InputStream);
            Cells cells = workbook.Worksheets[0].Cells;
            int rows = cells.MaxDataRow;
            int cols = cells.MaxDataColumn;
            if (rows > 0)
            {

                int dptId = 0;
                int postId = 0;
                int status = 0;
                DateTime interview;
                DateTime hireTime;

                for (int i = 1; i <= rows; i++)
                {
                    THR_Recruit model = new THR_Recruit();
                    model.CreateBy = user.UserName;
                    model.CreateTime = DateTime.Now;
                    model.IsDelete = 0;
                    model.EntryType = 1;

                    for (int j = 0; j <= cols; j++)
                    {
                        string itemValue = cells[i, j].StringValue;
                        #region InitModel
                        switch (j)
                        {
                            case 0:
                                if (string.IsNullOrEmpty(itemValue))
                                {
                                    return Content(this.GetJSON(new { Result = false, Msg = "第" + i + "行,姓名不能为空" }), this.JsonContentType());
                                }
                                model.Name = itemValue;
                                break;
                            case 1:
                                if (string.IsNullOrEmpty(itemValue))
                                {
                                    return Content(this.GetJSON(new { Result = false, Msg = "第" + i + "行,电话不能为空" }), this.JsonContentType());
                                }
                                if (!Regex.IsMatch(itemValue, @"^-?\d+$"))
                                {
                                    return Content(this.GetJSON(new { Result = false, Msg = "第" + i + "行,电话格式不合法" }), this.JsonContentType());
                                }
                                if (IsTel(itemValue) > 0)
                                { return Content(this.GetJSON(new { Result = false, Msg = "第" + i + "行,电话已存在" }), this.JsonContentType()); }
                                model.Tel = itemValue;
                                break;

                            case 2:
                                dptId = GetDeptId(itemValue);
                                if (dptId == 0)
                                {
                                    return Content(this.GetJSON(new { Result = false, Msg = "第" + i + "行,部门名称[" + itemValue + "]不存在" }), this.JsonContentType());
                                }
                                model.DptId = dptId;
                                break;
                            case 3:
                                postId = GetPostId(itemValue);
                                if (postId == 0)
                                {
                                    return Content(this.GetJSON(new { Result = false, Msg = "第" + i + "行,职位名称[" + itemValue + "]不存在" }), this.JsonContentType());
                                }
                                model.PostId = postId;
                                break;
                            case 4:
                                status = GetStatus(itemValue);
                                if (status == 0)
                                {
                                    return Content(this.GetJSON(new { Result = false, Msg = "第" + i + "行,状态名称[" + itemValue + "]不存在" }), this.JsonContentType());
                                }
                                model.Status = status;
                                break;

                            case 5:
                                if (!DateTime.TryParse(itemValue, out interview) && !string.IsNullOrEmpty(itemValue))
                                {
                                    return Content(this.GetJSON(new { Result = false, Msg = "第" + i + "行,面试时间格式不正确" }), this.JsonContentType());
                                }
                                if (string.IsNullOrEmpty(itemValue))
                                {
                                    return Content(this.GetJSON(new { Result = false, Msg = "第" + i + "行,面试时间格式不能为空" }), this.JsonContentType());
                                }
                                model.Interview = interview;
                                break;
                            case 6:
                                model.Interviewer = itemValue;
                                break;
                            case 7:
                                model.Remark = itemValue;
                                break;
                            case 8:
                                model.Userurl = itemValue;
                                break;
                            case 9:
                                if (!DateTime.TryParse(itemValue, out hireTime) && !string.IsNullOrEmpty(itemValue))
                                {
                                    return Content(this.GetJSON(new { Result = false, Msg = "第" + i + "行,报到时间格式不正确" }), this.JsonContentType());
                                }
                                if (string.IsNullOrEmpty(itemValue))
                                { model.HireTime = null; }
                                else
                                {
                                    model.HireTime = hireTime;
                                }
                                break;
                            case 10:
                                model.Email = itemValue;
                                break;
                            default:
                                break;

                        }
                        #endregion
                    }
                    ctx.THR_Recruit.Add(model);
                }
                if (ctx.SaveChanges() > 0)
                {
                    //RecruitModel m = GetRecruitModel(model.Id, ctx);
                    return Content(this.GetJSON(new { Result = true, Msg = "导入数据成功" }), this.JsonContentType());

                }
            }
            return Content(this.GetJSON(new { Result = false, Msg = "数据文件中无数据" }), this.JsonContentType());
        }
        /// <summary>
        /// 根据部门名称获得部门ID
        /// </summary>
        /// <param name="dptName"></param>
        /// <returns></returns>
        private int GetDeptId(string dptName)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            List<TErp_Department> models = ctx.TErp_Department
.Where(c => c.IsDelete == 0).OrderUsingSortExpression("id asc").ToList();
            TErp_Department tDepartment = models.FirstOrDefault(c => c.DeptName.Equals(dptName, StringComparison.OrdinalIgnoreCase));
            if (tDepartment != null) return tDepartment.Id;
            else return 0;
        }
        /// <summary>
        /// 根据职位名称获得职位ID
        /// </summary>
        /// <param name="postName"></param>
        /// <returns></returns>
        private int GetPostId(string postName)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            List<TErp_Position> models = ctx.TErp_Position
.Where(c => c.IsDelete == 0).OrderUsingSortExpression("id asc").ToList();
            TErp_Position tPost = models.FirstOrDefault(c => c.PositionName.Equals(postName, StringComparison.OrdinalIgnoreCase));
            if (tPost != null) return tPost.Id;
            else return 0;
        }
        /// <summary>
        /// 根据状态名称获得状态ID
        /// </summary>
        /// <param name="pParamsName"></param>
        /// <returns></returns>
        private int GetStatus(string pParamsName)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            List<Tapp_Param> models = ctx.Tapp_Param
.Where(c => c.IsDelete == 0 && c.Parentid == 10).OrderUsingSortExpression("parentid asc, sort asc").ToList();
            Tapp_Param tParam = models.FirstOrDefault(c => c.ParamsName.Equals(pParamsName, StringComparison.OrdinalIgnoreCase));
            if (tParam != null) return tParam.Id;
            else return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pTel"></param>
        /// <returns></returns>
        private int IsTel(string pTel)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            List<THR_Recruit> models = ctx.THR_Recruit
    .Where(c => c.IsDelete == 0 && c.Tel == pTel).OrderUsingSortExpression("id asc").ToList();
            return models.Count;
        }

        /// <summary>
        /// model
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        private static RecruitModel GetRecruitModel(int pId, HKSJRecruitmentContext pctx)
        {
            RecruitModel dto = pctx.THR_Recruit.Select(c => new RecruitModel
            {
                Id = c.Id,
                DptId = c.DptId.Value,
                DptName = c.TErp_Department.DeptName,
                PostId = c.PostId.Value,
                PostName = c.TErp_Position.PositionName,
                Userurl = c.Userurl,
                Remark = c.Remark,
                Name = c.Name,
                Tel = c.Tel,
                NeedsId = c.NeedsId,
                NeedsName = c.THR_Needs.TErp_Department.DeptName + "  " + c.THR_Needs.TErp_Position.PositionName,
                Status = c.Status.Value,
                StatusName = c.Tapp_Param.ParamsName,
                Interview = c.Interview,
                Email = c.Email,
                CreateBy = c.CreateBy,
                CreateTime = c.CreateTime,
                HireTime = c.HireTime,
                DeleteBy = c.DeleteBy,
                DeleteTime = c.DeleteTime,
                EditBy = c.EditBy,
                EditTime = c.EditTime,
                Interviewer = c.Interviewer,
                IsHaveBeen = c.THR_Needs.IsHaveBeen.Value
            }).FirstOrDefault(c => c.Id == pId);
            HR_Employee emp = pctx.HR_Employee.FirstOrDefault(c => c.uid == dto.CreateBy);
            if (emp != null)
            {
                dto.CreateBy = emp.name;
            }
            return dto;
        }



    }
}
