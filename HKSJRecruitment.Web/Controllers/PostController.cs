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
    public class PostController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Buttons = this.LoadButton();
            return View();
        }

        public ActionResult List(PostQueryParam queryParam)
        {
            Expression<Func<TErp_Post, bool>> predicate = c => c.IsDelete == 0;
            if (queryParam.DeptId > 0)
            {
                predicate = predicate.And(c => c.DeptId == queryParam.DeptId);
            }
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            IPageList<PostDto> models = ctx.TErp_Post
                .Where(predicate)
                .OrderUsingSortExpression(queryParam.Sort, queryParam.Order)
                .Select(c => new PostDto
                {
                    DeptId = c.DeptId,
                    DeptName = c.TErp_Department.DeptName,
                    PositionId = c.PositionId,
                    PositionLevel = c.TErp_Position.PositionLevel,
                    PostDesc = c.PostDesc,
                    PostName = c.PostName,
                    Id = c.Id,
                    PositionName = c.TErp_Position.PositionName
                })
                .ToPagedList(queryParam.PageIndex, queryParam.PageSize);
            List<PostDto> dtos = models.ToList();
            return Content(this.GetJSON(new { total = models.PageInfo.TotalCount, rows = dtos }), this.JsonContentType());
        }
        public ActionResult Detail(int Id)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            TErp_Post dto = ctx.TErp_Post.FirstOrDefault(c => c.Id == Id);
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
        public ActionResult Add(TErp_Post dto)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            int deptId = dto.DeptId;
            int positionId = dto.PositionId;
            if (ctx.TErp_Department.Count(c => c.Id == deptId && c.IsDelete == 0) == 0)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "请选择部门" }), this.JsonContentType());
            }
            if (ctx.TErp_Position.Count(c => c.Id == positionId && c.IsDelete == 0) == 0)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "职位不存在,请在职位管理中维护" }), this.JsonContentType());
            }
            Tapp_User user = this.AppUser();
            dto.CreateBy = user.UserName;
            dto.CreateTime = DateTime.Now;
            ctx.TErp_Post.Add(dto);
            if (ctx.SaveChanges() >= 0)
            {
                var model = ctx.TErp_Post.Where(c => c.Id == dto.Id).Select(c => new PostDto
                {
                    DeptId = c.DeptId,
                    DeptName = c.TErp_Department.DeptName,
                    PositionId = c.PositionId,
                    PositionLevel = c.TErp_Position.PositionLevel,
                    PostDesc = c.PostDesc,
                    PostName = c.PostName,
                    Id = c.Id,
                    PositionName = c.TErp_Position.PositionName
                }).FirstOrDefault();
                return Content(this.GetJSON(new { Result = true, Msg = "成功", Dto = model }), this.JsonContentType());
            }
            return Content(this.GetJSON(new { Result = false, Msg = "失败", Dto = dto }), this.JsonContentType());
        }

        [HttpPost]
        public ActionResult Edit(TErp_Post dto)
        {
            if (dto.Id > 0)
            {
                HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
                int deptId = dto.DeptId;
                int positionId = dto.PositionId;
                if (ctx.TErp_Department.Count(c => c.Id == deptId && c.IsDelete == 0) == 0)
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "请选择部门" }), this.JsonContentType());
                }
                if (ctx.TErp_Position.Count(c => c.Id == positionId && c.IsDelete == 0) == 0)
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "职位不存在,请在职位管理中维护" }), this.JsonContentType());
                }
                TErp_Post model = ctx.TErp_Post.FirstOrDefault(c => c.Id == dto.Id);
                this.CopyObject<TErp_Post>(model, dto);
                Tapp_User user = this.AppUser();
                model.EditBy = user.UserName;
                model.EditTime = DateTime.Now;
                if (ctx.SaveChanges() >= 0)
                {
                    var m = ctx.TErp_Post.Where(c => c.Id == dto.Id).Select(c => new PostDto
                    {
                        DeptId = c.DeptId,
                        DeptName = c.TErp_Department.DeptName,
                        PositionId = c.PositionId,
                        PositionLevel = c.TErp_Position.PositionLevel,
                        PostDesc = c.PostDesc,
                        PostName = c.PostName,
                        Id = c.Id,
                        PositionName = c.TErp_Position.PositionName
                    }).FirstOrDefault();
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
                TErp_Post model = ctx.TErp_Post.FirstOrDefault(c => c.Id == tid);
                if (model != null)
                {
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
        public ActionResult ListPosition()
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            List<TErp_Position> dtos = ctx.TErp_Position.Where(c => c.IsDelete == 0).OrderBy(c => c.SeqNo).ToList();
            return Content(this.GetJSON(new { total = dtos.Count, rows = dtos }), this.JsonContentType());
        }
        [Anonymous]
        public ActionResult ListPost(int id)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            var query = ctx.TErp_Post.Where(c => c.IsDelete == 0 && c.DeptId == id)
                .OrderBy(c => c.CreateTime)
                .Select(c => new PostDto { Id = c.Id, PostName = c.PostName, PositionId = c.PositionId });
            List<PostDto> dtos = query.ToList();
            return Content(this.GetJSON(new { total = dtos.Count, rows = dtos }), this.JsonContentType());
        }
    }
}
