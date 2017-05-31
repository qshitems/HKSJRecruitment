using Common.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using HKSJRecruitment.Web.Models;
using Microsoft.International.Converters.PinYinConverter;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using HKSJRecruitment.Models.Models;
using HKSJRecruitment.Facade;

namespace HKSJRecruitment.Web.Controllers
{
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/

        public ActionResult Index()
        {
            ViewBag.Buttons = this.LoadButton();
            return View();
        }
        public ActionResult List(EmployeeQueryParam queryParam)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            string sql = @"SELECT A.ID,A.uid,A.pwd,A.name,A.idcard,A.idcard,A.birthday,A.dptid,B.DeptName as dptname,A.postid,C.PositionName post,A.sex,A.tel,A.status,A.EntryDate,A.address,A.education,A.schools 
from HR_Employee A LEFT join TErp_Department B ON A.dptid=B.Id
left join TErp_Position C ON A.postid=C.Id where (A.isDelete is null or A.isDelete = 0) ";
            if (!string.IsNullOrEmpty(queryParam.name))
            {
                sql += string.Format("and A.name like {0}", SQLHelper.ToSQLParamLikeStr(queryParam.name));
            }
            sql += "order by A.ID desc";
            IPageList<HKSJRecruitment.Models.proModels.HR_Employee> dtoss = ctx.Database.SqlQuery<HKSJRecruitment.Models.proModels.HR_Employee>(sql).ToPagedList(queryParam.PageIndex, queryParam.PageSize);
            return Content(this.GetJSON(new { total = dtoss.PageInfo.TotalCount, rows = dtoss }), this.JsonContentType());
        }
        public ActionResult Detail(int Id)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();

            HKSJRecruitment.Models.proModels.HR_Employee dto = ctx.Database.SqlQuery<HKSJRecruitment.Models.proModels.HR_Employee>(@"SELECT A.ID,A.uid,A.pwd,A.name,A.idcard,A.idcard,A.birthday,A.dptid,B.DeptName as dptname,A.postid,C.PositionName post,A.sex,A.tel,A.status,A.EntryDate,A.address,A.education,A.schools from HR_Employee A LEFT join TErp_Department B ON A.dptid=B.Id
 left join TErp_Position C ON A.postid=C.Id").FirstOrDefault(c => c.ID == Id); ;// ctx.HR_Employee.FirstOrDefault(c => c.ID == Id);
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
        public ActionResult Add(HR_Employee dto)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            string uid = dto.uid;
            if (ctx.Tapp_User.Count(c => c.UserName == uid) > 0)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "账号[" + uid + "]已使用", Dto = dto }), this.JsonContentType());
            }
            if (!dto.dptid.HasValue)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "请选择部门", Dto = dto }), this.JsonContentType());
            }
            if (!dto.postid.HasValue)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "请选择职位", Dto = dto }), this.JsonContentType());
            }
            int deptId = dto.dptid.Value;
            int postId = dto.postid.Value;
            if (ctx.TErp_Department.Count(c => c.Id == deptId && c.IsDelete == 0) == 0)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "部门不存在", Dto = dto }), this.JsonContentType());
            }
            if (ctx.TErp_Position.Count(c => c.Id == postId && c.IsDelete == 0) == 0)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "职位不存在", Dto = dto }), this.JsonContentType());
            }
            ctx.HR_Employee.Add(dto);

            Tapp_User use = new Tapp_User();
            use.UserName = dto.uid;
            use.UserPwd = "123456";

            use.State = 1;
            use.UserPwd = Md5.Encrypt(dto.uid + "123456");
            ctx.Tapp_User.Add(use);

            if (ctx.SaveChanges() >= 0)
            {
                return Content(this.GetJSON(new { Result = true, Msg = "成功", Dto = dto }), this.JsonContentType());
            }
            return Content(this.GetJSON(new { Result = false, Msg = "失败", Dto = dto }), this.JsonContentType());
        }

        [HttpPost]
        public ActionResult Edit(HR_Employee dto)
        {
            if (dto.ID > 0)
            {
                HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
                int id = dto.ID;
                HR_Employee model = ctx.HR_Employee.FirstOrDefault(c => c.ID == id);
                if (model == null)
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "找不到要修改的员工信息" }), this.JsonContentType());
                }
                if (!dto.dptid.HasValue)
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "部门不能为空" }), this.JsonContentType());
                }
                if (!dto.postid.HasValue)
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "职位不能为空" }), this.JsonContentType());
                }
                int deptId = dto.dptid.Value;
                int postId = dto.postid.Value;
                if (ctx.TErp_Department.Count(c => c.Id == deptId && c.IsDelete == 0) == 0)
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "部门不存在" }), this.JsonContentType());
                }
                if (ctx.TErp_Position.Count(c => c.Id == postId && c.IsDelete == 0) == 0)
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "职位不存在" }), this.JsonContentType());
                }
                model.sex = dto.sex;
                model.idcard = dto.idcard;
                model.tel = dto.tel;
                model.EntryDate = dto.EntryDate;
                model.birthday = dto.birthday;
                model.dptid = dto.dptid.Value;
                model.dptname = dto.dptname;
                model.postid = dto.postid.Value;
                model.post = dto.post;
                model.schools = dto.schools;
                model.education = dto.education;
                model.status = dto.status;
                model.address = dto.address;
                if (dto.status == "离职")
                {
                    string uid = model.uid;
                    Tapp_User use = ctx.Tapp_User.FirstOrDefault(c => c.UserName == uid);
                    if (use != null)
                    {
                        use.State = 0;
                    }
                }
                if (ctx.SaveChanges() >= 0)
                {
                    return Content(this.GetJSON(new { Result = true, Msg = "成功", Dto = model }), this.JsonContentType());
                }
                return Content(this.GetJSON(new { Result = false, Msg = "失败" }), this.JsonContentType());
            }
            else
            {
                return Content(this.GetJSON(new { Result = false, Msg = "失败，未找到要修改的数据" }), this.JsonContentType());
            }
        }

        [HttpPost]
        public ActionResult DeleteMulti(string Id)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            string[] arrId = Id.Split(new char[] { ',', '-' }, StringSplitOptions.RemoveEmptyEntries);
            int counter = 0;
            foreach (var item in arrId)
            {
                int tid = Convert.ToInt32(item);
                HR_Employee model = ctx.HR_Employee.FirstOrDefault(c => c.ID == tid);
                Tapp_User appUser = ctx.Tapp_User.FirstOrDefault(c => c.UserName == model.uid);
                if (model != null)
                {
                    model.isDelete = 1;
                    model.Delete_time = DateTime.Now;
                    counter++;
                }
                if (appUser != null)
                {
                    appUser.State = 0;
                }
            }
            if (ctx.SaveChanges() >= 0)
            {
                return Content(this.GetJSON(new { Result = true, Msg = "成功", Id = counter }), this.JsonContentType());
            }
            return Content(this.GetJSON(new { Result = false, Msg = "失败", id = counter }), this.JsonContentType());
        }
        [Anonymous]
        public ActionResult ListMenu()
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            List<Tapp_Menu> Menus = ctx.Tapp_Menu.OrderBy(c => c.SeqNo).ToList();
            return Content(this.GetJSON(new { Result = true, Msg = "成功", Dtos = Menus }), this.JsonContentType());
        }
        [Anonymous]
        public ActionResult UserRole(string uid)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            Tapp_User appUser = ctx.Tapp_User.FirstOrDefault(c => c.UserName == uid);
            if (appUser == null)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "员工不存在" }), this.JsonContentType());
            }
            List<Tapp_Role> Roles = ctx.Tapp_Role.ToList();
            List<Tapp_User_Role> appUserRole = ctx.Tapp_User_Role.Where(c => c.UserId == appUser.Id).ToList();
            List<UserRoleDto> dtos = new List<UserRoleDto>();
            foreach (var item in Roles)
            {
                dtos.Add(new UserRoleDto
                {
                    UserId = appUser.Id,
                    RoleId = item.Id,
                    RoleName = item.RoleName,
                    IsRight = appUserRole.Exists(c => c.UserId == appUser.Id && c.RoleId == item.Id)
                });
            }
            return Content(this.GetJSON(new { Result = true, Msg = "成功", Dtos = dtos }), this.JsonContentType());
        }
        public ActionResult RightEdit(string uid, string roleids)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            Tapp_User appUser = ctx.Tapp_User.FirstOrDefault(c => c.UserName == uid);
            if (appUser == null)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "员工不存在" }), this.JsonContentType());
            }
            string[] arrRoleId = roleids.Split(new char[] { ',', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string[] arrDbRoleId = ctx.Tapp_User_Role.Where(c => c.UserId == appUser.Id).Select(c => c.RoleId.ToString()).ToArray();
            foreach (var item in arrDbRoleId.Except(arrRoleId))
            {
                //删除
                int roleId = Convert.ToInt32(item);
                Tapp_User_Role right = ctx.Tapp_User_Role.FirstOrDefault(c => c.RoleId == roleId && c.UserId == appUser.Id);
                if (right != null)
                {
                    ctx.Tapp_User_Role.Remove(right);
                }
            }
            foreach (var item in arrRoleId.Except(arrDbRoleId))
            {
                //新增
                int roleId = Convert.ToInt32(item);
                Tapp_Role role = ctx.Tapp_Role.FirstOrDefault(c => c.Id == roleId);
                if (role != null)
                {
                    ctx.Tapp_User_Role.Add(new Tapp_User_Role { RoleId = roleId, UserId = appUser.Id });
                }
            }
            if (ctx.SaveChanges() >= 0)
            {
                return Content(this.GetJSON(new { Result = true, Msg = "成功" }), this.JsonContentType());
            }
            else
            {
                return Content(this.GetJSON(new { Result = false, Msg = "失败" }), this.JsonContentType());
            }
        }
        public ActionResult ResetUserPwd(string uid)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            Tapp_User appUser = ctx.Tapp_User.FirstOrDefault(c => c.UserName == uid);
            string defaultPwd = Md5.Encrypt(appUser.UserName + "123456");
            appUser.UserPwd = defaultPwd;
            if (ctx.SaveChanges() >= 0)
            {
                return Content(this.GetJSON(new { Result = true, Msg = "成功" }), this.JsonContentType());
            }
            else
            {
                return Content(this.GetJSON(new { Result = false, Msg = "失败" }), this.JsonContentType());
            }
        }
        [Anonymous]
        public ActionResult GetID(string name)
        {
            CommonFacade facade = new CommonFacade();
            string uid = facade.GetUid(name);
            return Content(this.GetJSON(new { ID = uid }), this.JsonContentType());
        }
        [Anonymous]
        public ActionResult ListEmployee(EmployeeQueryParam queryParam)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            Expression<Func<HR_Employee, bool>> predicate = c => c.isDelete == null || c.isDelete == 0;
            if (queryParam.deptid > 0)
            {
                predicate = predicate.And(c => c.dptid == queryParam.deptid);
            }
            if (!string.IsNullOrEmpty(queryParam.name))
            {
                predicate = predicate.And(c => c.name.Contains(queryParam.name) || c.uid.Contains(queryParam.name));
            }
            IPageList<EmployeeData> dtos = ctx.HR_Employee.Where(predicate).OrderBy(c => c.ID).Select(c => new EmployeeData { Id = c.ID, Name = c.name, Uid = c.uid }).ToPagedList(queryParam.PageIndex, queryParam.PageSize);
            return Content(this.GetJSON(new { total = dtos.PageInfo.TotalCount, rows = dtos }), this.JsonContentType());
        }
        public class EmployeeData
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Uid { get; set; }
        }
    }
}
