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
using System.IO;
using Aspose.Cells;
using System.Text;

namespace HKSJRecruitment.Web.Controllers
{
    public class Hr_NeedController : Controller
    {
        public ActionResult Index()
        {

            ViewBag.Buttons = this.LoadButton();

            return View();
        }

        public void SaveHaveBeenQuantity(Hr_NeedQueryParam queryParam)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            Expression<Func<THR_Needs, bool>> predicate = SearchPredicate(queryParam);
            IPageList<THR_Needs> models = ctx.THR_Needs
          .Where(predicate)
          .OrderUsingSortExpression(queryParam.Sort.Replace("deptname", "deptid").Replace("postname", "postid"), queryParam.Order)
          .ToPagedList(queryParam.PageIndex, queryParam.PageSize);
            List<THR_Needs> dtos = models.ToList();
            //       List<THR_Needs> list = ctx.THR_Needs
            //.Where(c => c.IsDelete == 0 && c.NeedQuantity > 0).ToList();
            foreach (THR_Needs needmodel in dtos)
            {
                int Quantity = ctx.THR_Recruit
      .Where(c => c.IsDelete == 0 && c.NeedsId == needmodel.Id && c.Status == 82).ToList().Count;//已录用
                needmodel.HaveBeenQuantity = Quantity;
                if (needmodel.NeedQuantity == needmodel.HaveBeenQuantity)
                    needmodel.IsHaveBeen = 1;
                else
                    needmodel.IsHaveBeen = 0;
            }
            ctx.SaveChanges();
        }

        public ActionResult List(Hr_NeedQueryParam queryParam)
        {
            SaveHaveBeenQuantity(queryParam);
            IPageList<Hr_NeedDto> models = GetNeedData(queryParam);
            List<Hr_NeedDto> dtos = models.ToList();
            return Content(this.GetJSON(new { total = models.PageInfo.TotalCount, rows = dtos }), this.JsonContentType());
        }
        public ActionResult Detail(int Id)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            Hr_NeedDto dto = ctx.THR_Needs.Select(c => new Hr_NeedDto
            {
                Id = c.Id,
                DeptId = c.DeptId,
                DeptName = c.TErp_Department.DeptName,
                PostId = c.PostId,
                PostName = c.TErp_Position.PositionName,
                CutTime = c.CutTime,
                Demander = c.Demander,
                NeedQuantity = c.NeedQuantity,
                Remarks = c.Remarks,
                CreateBy = c.CreateBy,
                CreateTime = c.CreateTime,
                IsDelete = c.IsDelete,
                DeleteBy = c.DeleteBy,
                DeleteTime = c.DeleteTime,
                EditBy = c.EditBy,
                EditTime = c.EditTime,
                HaveBeenQuantity = c.HaveBeenQuantity,
                FileWord = c.FileWord,
                JobResponsibility = c.JobResponsibility,
                PostRequest = c.PostRequest,
                IsHaveBeen = c.IsHaveBeen.Value,
                InterviewAddress = c.InterviewAddress,
                Principal = c.Principal
            }).FirstOrDefault(c => c.Id == Id);
            HR_Employee emp = ctx.HR_Employee.FirstOrDefault(c => c.uid == dto.CreateBy);
            if (emp != null)
            {
                dto.CreateBy = emp.name;
            }
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
        public ActionResult Add(THR_Needs dto)
        {

            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            int deptId = dto.DeptId;
            int postId = dto.PostId;
            if (ctx.TErp_Department.Count(c => c.Id == deptId) == 0)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "部门不存在,请在组织架构中维护" }), this.JsonContentType());
            }
            if (dto.NeedsPostId > 0)
            {
                if (ctx.TErp_Position.Count(c => c.Id == dto.NeedsPostId) == 0)
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "职位不存在,请在职位管理中维护" }), this.JsonContentType());
                }
                dto.PostId = dto.NeedsPostId;
            }
            else
            {
                if (ctx.TErp_Position.Count(c => c.Id == postId) == 0)
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "职位不存在,请在职位管理中维护" }), this.JsonContentType());
                }
                dto.NeedsPostId = 0;
            }
            if (string.IsNullOrEmpty(dto.CutTime.ToString()))
            {
                return Content(this.GetJSON(new { Result = false, Msg = "截止时间不能为空" }), this.JsonContentType());
            }
            DateTime cuttime;
            if (!DateTime.TryParse(dto.CutTime.ToString(), out cuttime) && !string.IsNullOrEmpty(dto.CutTime.ToString()))
            {
                return Content(this.GetJSON(new { Result = false, Msg = "截止时间格式不正确" }), this.JsonContentType());

            }
            //THR_Needs needmodel = ctx.THR_Needs.FirstOrDefault(c => c.DeptId == dto.DeptId && c.PostId == dto.PostId && c.IsDelete == 0 && c.IsHaveBeen == 0);
            //if (needmodel != null)
            //{ return Content(this.GetJSON(new { Result = false, Msg = "部门和职位已存在,不能重复添加" }), this.JsonContentType()); }
            Tapp_User user = this.AppUser();
            dto.CreateBy = user.UserName;
            dto.CreateTime = DateTime.Now;
            dto.IsHaveBeen = 0;
            ctx.THR_Needs.Add(dto);
            if (ctx.SaveChanges() >= 0)
            {
                var model = ctx.THR_Needs.Where(c => c.Id == dto.Id).Select(c => new Hr_NeedDto
                {
                    Id = c.Id,
                    DeptId = c.DeptId,
                    DeptName = c.TErp_Department.DeptName,
                    PostId = c.PostId,
                    PostName = c.TErp_Position.PositionName,
                    CutTime = c.CutTime,
                    Demander = c.Demander,
                    NeedQuantity = c.NeedQuantity,
                    Remarks = c.Remarks,
                    CreateBy = c.CreateBy,
                    CreateTime = c.CreateTime,
                    IsDelete = c.IsDelete,
                    DeleteBy = c.DeleteBy,
                    DeleteTime = c.DeleteTime,
                    EditBy = c.EditBy,
                    EditTime = c.EditTime,
                    HaveBeenQuantity = c.HaveBeenQuantity,
                    FileWord = c.FileWord,
                    JobResponsibility = c.JobResponsibility,
                    PostRequest = c.PostRequest,
                    IsHaveBeen = c.IsHaveBeen.Value,
                    InterviewAddress = c.InterviewAddress,
                    Principal = c.Principal
                }).FirstOrDefault();
                HR_Employee emp = ctx.HR_Employee.FirstOrDefault(c => c.uid == model.CreateBy);
                if (emp != null)
                {
                    model.CreateBy = emp.name;
                }
                return Content(this.GetJSON(new { Result = true, Msg = "成功", Dto = model }), this.JsonContentType());
            }
            return Content(this.GetJSON(new { Result = false, Msg = "失败", Dto = dto }), this.JsonContentType());
        }

        [HttpPost]
        public ActionResult Edit(THR_Needs dto)
        {
            if (dto.Id > 0)
            {
                HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
                //THR_Needs needmodel = ctx.THR_Needs.FirstOrDefault(c => c.DeptId == dto.DeptId && c.PostId == dto.PostId && c.IsDelete == 0 && c.Id != dto.Id);
                //if (needmodel != null)
                //{ return Content(this.GetJSON(new { Result = false, Msg = "部门和职位已存在,不能重复添加" }), this.JsonContentType()); }
                THR_Needs model = ctx.THR_Needs.FirstOrDefault(c => c.Id == dto.Id);
                if (dto.NeedQuantity < model.HaveBeenQuantity)
                { return Content(this.GetJSON(new { Result = false, Msg = "招聘人数不能小于已招聘人数" }), this.JsonContentType()); }
                this.CopyObject<THR_Needs>(model, dto);
                Tapp_User user = this.AppUser();
                model.EditBy = user.UserName;
                model.EditTime = DateTime.Now;
                if (ctx.SaveChanges() >= 0)
                {
                    var m = ctx.THR_Needs.Where(c => c.Id == dto.Id).Select(c => new Hr_NeedDto
                    {
                        Id = c.Id,
                        DeptId = c.DeptId,
                        DeptName = c.TErp_Department.DeptName,
                        PostId = c.PostId,
                        PostName = c.TErp_Position.PositionName,
                        CutTime = c.CutTime,
                        Demander = c.Demander,
                        NeedQuantity = c.NeedQuantity,
                        Remarks = c.Remarks,
                        CreateBy = c.CreateBy,
                        CreateTime = c.CreateTime,
                        IsDelete = c.IsDelete,
                        DeleteBy = c.DeleteBy,
                        DeleteTime = c.DeleteTime,
                        EditBy = c.EditBy,
                        EditTime = c.EditTime,
                        HaveBeenQuantity = c.HaveBeenQuantity,
                        FileWord = c.FileWord,
                        JobResponsibility = c.JobResponsibility,
                        PostRequest = c.PostRequest,
                        IsHaveBeen = c.IsHaveBeen.Value,
                        InterviewAddress = c.InterviewAddress,
                        Principal = c.Principal
                    }).FirstOrDefault();
                    HR_Employee emp = ctx.HR_Employee.FirstOrDefault(c => c.uid == model.CreateBy);
                    if (emp != null)
                    {
                        m.CreateBy = emp.name;
                    }
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
        public ActionResult DeleteMulti(string Id)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            string[] arrId = Id.Split(new char[] { ',', '-' }, StringSplitOptions.RemoveEmptyEntries);
            int counter = 0;
            Tapp_User user = this.AppUser();
            foreach (var item in arrId)
            {
                int tid = Convert.ToInt32(item);
                THR_Needs model = ctx.THR_Needs.FirstOrDefault(c => c.Id == tid);
                if (model != null)
                {
                    if (model.HaveBeenQuantity > 0)
                    {
                        return Content(this.GetJSON(new { Result = false, Msg = "该招聘需求在招聘信息中已被引用不能删除!" }), this.JsonContentType());
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
                List<DepartmentDto> depts = ctx.TErp_Department.Where(c => c.IsDelete == 0).Select(c => new DepartmentDto
                {
                    Id = c.Id,
                    DeptName = c.DeptName,
                    ParentId = c.ParentId,
                    SeqNo = c.SeqNo,
                    DeptIcon = c.DeptIcon
                }).ToList();
                List<TErp_Position> positions = ctx.TErp_Position.Where(c => c.IsDelete == 0).ToList();
                int deptId = 0;
                int postId = 0;
                int needQuantity = 0;
                DateTime cutTime;
                for (int i = 1; i <= rows; i++)
                {
                    THR_Needs model = new THR_Needs();
                    model.CreateBy = user.UserName;
                    model.CreateTime = DateTime.Now;
                    model.IsHaveBeen = 0;
                    model.IsDelete = 0;

                    for (int j = 0; j <= cols; j++)
                    {
                        string itemValue = cells[i, j].StringValue;
                        #region InitModel
                        switch (j)
                        {
                            case 0:
                                deptId = GetDeptId(depts, itemValue);
                                if (deptId == 0)
                                {
                                    return Content(this.GetJSON(new { Result = false, Msg = "导入失败,第" + i + "行,需求部门名称[" + itemValue + "]不存在" }), this.JsonContentType());
                                }
                                model.DeptId = deptId;
                                break;
                            case 1:
                                postId = GetPositionId(positions, itemValue);
                                if (postId == 0)
                                {
                                    return Content(this.GetJSON(new { Result = false, Msg = "导入失败,第" + i + "行,需求职位名称[" + itemValue + "]不存在" }), this.JsonContentType());
                                }
                                model.PostId = postId;
                          
                                break;
                            case 2:
                                if (!int.TryParse(itemValue, out needQuantity))
                                {
                                    return Content(this.GetJSON(new { Result = false, Msg = "导入失败,第" + i + "行,招聘人数[" + itemValue + "]格式错误" }), this.JsonContentType());
                                }
                                model.NeedQuantity = needQuantity;
                                break;
                            case 3:
                                if (string.IsNullOrEmpty(itemValue))
                                {
                                    return Content(this.GetJSON(new { Result = false, Msg = "导入失败,第" + i + "行,需求人不能为空" }), this.JsonContentType());
                                }
                                model.Demander = itemValue;
                                break;
                            case 4:
                                if (!DateTime.TryParse(itemValue, out cutTime))
                                {
                                    return Content(this.GetJSON(new { Result = false, Msg = "导入失败,第" + i + "行,截止时间格式错误" }), this.JsonContentType());
                                }
                                model.CutTime = cutTime;
                                break;
                            case 5:
                                model.Principal = itemValue;
                                break;
                            case 6:
                                model.InterviewAddress = itemValue;
                                break;
                            case 7:
                                model.JobResponsibility = itemValue;
                                break;
                            case 8:
                                model.PostRequest = itemValue;
                                break;
                            case 9:
                                model.Remarks = itemValue;
                                break;
                            default:
                                break;
                        }

                        #endregion
                    }

                    ctx.THR_Needs.Add(model);

                }
                //List<THR_Needs> needslist = ctx.THR_Needs.ToList();
                if (ctx.SaveChanges() >= 0)
                {

                    return Content(this.GetJSON(new { Result = true, Msg = "导入数据成功" }), this.JsonContentType());
                }
            }
            return Content(this.GetJSON(new { Result = false, Msg = "数据文件中无数据" }), this.JsonContentType());
        }



        private int GetDeptId(List<DepartmentDto> depts, string deptName)
        {
            DepartmentDto dept = depts.FirstOrDefault(c => c.DeptName.Equals(deptName, StringComparison.OrdinalIgnoreCase));
            if (dept != null) return dept.Id;
            else return 0;
        }
        private int GetPositionId(List<TErp_Position> positions, string postName)
        {
            TErp_Position position = positions.FirstOrDefault(c => c.PositionName.Equals(postName, StringComparison.OrdinalIgnoreCase));
            if (position != null) return position.Id;
            else return 0;
        }
        public ActionResult ExportData(Hr_NeedQueryParam queryParam)
        {
            queryParam.PageIndex = 1;
            queryParam.PageSize = 999999;
            List<Hr_NeedDto> dtos = GetNeedData(queryParam).ToList();
            MemoryStream ms = OutFileToStream(dtos);
            return File(ms.ToArray(), "application/ms-excel", string.Format("{0}.xls", "招聘需求信息" + DateTime.Now.ToString("yyMMddhhmmss")));
        }
        private IPageList<Hr_NeedDto> GetNeedData(Hr_NeedQueryParam queryParam)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            Expression<Func<THR_Needs, bool>> predicate = SearchPredicate(queryParam);


            //var models = ctx.THR_Needs
            //    .Join(ctx.HR_Employee, left => left.CreateBy, right => right.uid, (left, right) => new Hr_NeedDto
            //    {
            //        Id = left.Id,
            //        DeptId = left.DeptId,
            //        DeptName = left.TErp_Department.DeptName,
            //        PostId = left.PostId,
            //        PostName = left.TErp_Position.PositionName,
            //        CutTime = left.CutTime,
            //        Demander = left.Demander,
            //        NeedQuantity = left.NeedQuantity,
            //        Remarks = left.Remarks,
            //        CreateBy = right.name,
            //        CreateTime = left.CreateTime,
            //        IsDelete = left.IsDelete,
            //        DeleteBy = left.DeleteBy,
            //        DeleteTime = left.DeleteTime,
            //        EditBy = left.EditBy,
            //        EditTime = left.EditTime,
            //        HaveBeenQuantity = left.HaveBeenQuantity,
            //        FileWord = left.FileWord
            //    })
            //    .Where(predicate)
            //    .OrderUsingSortExpression(queryParam.Sort, queryParam.Order)
            //    .ToPagedList(queryParam.PageIndex, queryParam.PageSize);
            IPageList<Hr_NeedDto> models = ctx.THR_Needs
          .Where(predicate)
          .OrderUsingSortExpression(queryParam.Sort.Replace("deptname", "deptid").Replace("postname", "postid"), queryParam.Order)
          .Select(c => new Hr_NeedDto
          {
              Id = c.Id,
              DeptId = c.DeptId,
              DeptName = c.TErp_Department.DeptName,
              PostId = c.PostId,
              PostName = c.TErp_Position.PositionName,
              CutTime = c.CutTime,
              Demander = c.Demander,
              NeedQuantity = c.NeedQuantity,
              Remarks = c.Remarks,
              CreateBy = c.CreateBy,
              CreateTime = c.CreateTime,
              IsDelete = c.IsDelete,
              DeleteBy = c.DeleteBy,
              DeleteTime = c.DeleteTime,
              EditBy = c.EditBy,
              EditTime = c.EditTime,
              HaveBeenQuantity = c.HaveBeenQuantity,
              FileWord = c.FileWord,
              JobResponsibility = c.JobResponsibility,
              PostRequest = c.PostRequest,
              IsHaveBeen = c.IsHaveBeen.Value,
              InterviewAddress = c.InterviewAddress,
              Principal = c.Principal

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

        private Expression<Func<THR_Needs, bool>> SearchPredicate(Hr_NeedQueryParam queryParam)
        {
            Expression<Func<THR_Needs, bool>> predicate = c => c.IsDelete == 0;
            var rolelist = this.LoadUserRole();
            string roleid = string.Empty;
            foreach (UserRoleDto role in rolelist)
            {
                roleid += role.RoleId.ToString() + ",";
            }
            if (!roleid.Contains("1") && !roleid.Contains("26") && !roleid.Contains("25"))
            {

                string user = this.AppUser().UserName;
                predicate = predicate.And(c => c.CreateBy == user);
            }
            if (queryParam.IsHaveBeen != 2)
            {
                predicate = predicate.And(c => c.IsHaveBeen == queryParam.IsHaveBeen);
                //predicate = predicate.And(c => c.CutTime >= DateTime.Now);
            }
            if (queryParam.DeptId > 0)
            {
                predicate = predicate.And(c => c.DeptId == queryParam.DeptId);
            }
            if (queryParam.PostId > 0)
            {
                predicate = predicate.And(c => c.PostId == queryParam.PostId);
            }
            if (!string.IsNullOrEmpty(queryParam.Demander))
            {
                predicate = predicate.And(c => c.Demander.StartsWith(queryParam.Demander));
            }
            if (queryParam.NeedQuantity > 0)
            {
                predicate = predicate.And(c => c.NeedQuantity == queryParam.NeedQuantity);
            }
            if (queryParam.HaveBeenQuantity > 0)
            {
                predicate = predicate.And(c => c.HaveBeenQuantity == queryParam.HaveBeenQuantity);
            }
            if (queryParam.CutTimeStart.HasValue && queryParam.CutTimeEnd.HasValue && queryParam.CutTimeStart.Value < queryParam.CutTimeEnd.Value)
            {
                predicate = predicate.And(c => c.CutTime >= queryParam.CutTimeStart.Value && c.CutTime <= queryParam.CutTimeEnd.Value);
            }

            return predicate;
        }
        private MemoryStream OutFileToStream(List<Hr_NeedDto> dtos)
        {
            Workbook workbook = new Workbook();         //工作簿 
            Worksheet sheet = workbook.Worksheets[0];   //工作表 
            Cells cells = sheet.Cells;                  //单元格 

            int rowNums = dtos.Count;                   //表格行数 

            //生成行1 列名行
            cells[0, 0].PutValue("需求部门");
            cells[0, 1].PutValue("需求职位");
            cells[0, 2].PutValue("招聘人数");
            cells[0, 3].PutValue("已招聘人数");
            cells[0, 4].PutValue("需求状态");
            cells[0, 5].PutValue("需求人");
            cells[0, 6].PutValue("截止时间");
            cells[0, 7].PutValue("负责人");
            cells[0, 8].PutValue("面试地址");
            cells[0, 9].PutValue("工作职责");
            cells[0, 10].PutValue("岗位要求");
            cells[0, 11].PutValue("备注");

            //生成数据行 
            for (int i = 0; i < rowNums; i++)
            {
                cells[1 + i, 0].PutValue(dtos[i].DeptName);
                cells[1 + i, 1].PutValue(dtos[i].PostName);
                cells[1 + i, 2].PutValue(dtos[i].NeedQuantity);
                cells[1 + i, 3].PutValue(dtos[i].HaveBeenQuantity);
                cells[1 + i, 4].PutValue(dtos[i].IsHaveBeen == 0 ? "未完成" : "已完成");
                cells[1 + i, 5].PutValue(dtos[i].Demander);
                cells[1 + i, 6].PutValue(dtos[i].CutTime.HasValue ? dtos[i].CutTime.Value.ToString("yyyy-MM-dd") : "");
                cells[1 + i, 7].PutValue(dtos[i].Principal);
                cells[1 + i, 8].PutValue(dtos[i].InterviewAddress);
                cells[1 + i, 9].PutValue(dtos[i].JobResponsibility);
                cells[1 + i, 10].PutValue(dtos[i].PostRequest);
                cells[1 + i, 11].PutValue(dtos[i].Remarks);
            }
            MemoryStream ms = workbook.SaveToStream();
            return ms;
        }

        public ActionResult FileWordData(string pid)
        {
            int id = 0;
            if (!string.IsNullOrEmpty(pid))
                id = int.Parse(pid);
            if (id > 0)
            {

                HttpPostedFileBase postFile = HttpContext.Request.Files[0];
                string extension = Path.GetExtension(postFile.FileName).ToLower();
                if (extension != ".doc" && extension != ".docx")
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "文件格式错误,支持.doc或.docx" }), this.JsonContentType());
                }
                string filename = postFile.FileName.Substring(postFile.FileName.LastIndexOf("\\") + 1);
                string houzhui = postFile.FileName.Substring(postFile.FileName.LastIndexOf(".") + 1);
                string name = DateTime.Now.ToString("yyyyMMddHHmmss");//重新命名
                postFile.SaveAs(Server.MapPath(@"/UploadFiles/" + name + filename));
                HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
                THR_Needs model = ctx.THR_Needs.FirstOrDefault(c => c.Id == id);
                model.FileWord = name + filename;
                if (ctx.SaveChanges() >= 0)
                {
                    return Content(this.GetJSON(new { Result = true, Msg = "上传试题成功" }), this.JsonContentType());
                }
                return Content(this.GetJSON(new { Result = false, Msg = "失败" }), this.JsonContentType());
            }
            else
            {
                return Content(this.GetJSON(new { Result = false, Msg = "失败，未找到要修改的数据" }), this.JsonContentType());
            }
        }

    }
}
