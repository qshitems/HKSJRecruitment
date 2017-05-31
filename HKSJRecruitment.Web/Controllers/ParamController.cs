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
    public class ParamController : Controller
    {
        /// <summary>
        /// 参数配置控制器
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Buttons = this.LoadButton();
            int id = 0;
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            Tapp_ParamType info = ctx.Tapp_ParamType
    .Where(c => c.IsShow == 0).OrderBy(c => c.Id).FirstOrDefault();
            if (info != null)
            {
                id = info.Id;
            }
            ViewBag.ParentId = id;
            return View();
        }
        /// <summary>
        /// 参数左侧栏目
        /// </summary>
        /// <returns></returns>
        [Anonymous]
        public ActionResult ListParamType()
        {

            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            List<Tapp_ParamType> models = ctx.Tapp_ParamType
        .Where(c => c.IsShow == 0).OrderUsingSortExpression("parentid asc, sort asc").ToList();
            return Content(this.GetJSON(new { total = models.Count, rows = models }), this.JsonContentType());
        }
        [Anonymous]
        public ActionResult List(PanramQueryParam queryParam)
        {
            Expression<Func<Tapp_Param, bool>> predicate = c => true;
            predicate = predicate.And(c => c.Parentid == queryParam.Parentid && c.IsDelete == 0);
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            IPageList<Tapp_Param> models = ctx.Tapp_Param
                .Where(predicate)
                .OrderUsingSortExpression("parentid asc, sort asc")
                .ToPagedList(queryParam.PageIndex, queryParam.PageSize);
            List<Tapp_Param> dtos = models.ToList();
            return Content(this.GetJSON(new { total = models.PageInfo.TotalCount, rows = dtos }), this.JsonContentType());
        }
        public ActionResult Detail(int Id)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            Tapp_Param dto = ctx.Tapp_Param.FirstOrDefault(c => c.Id == Id);
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
        public ActionResult Add(Tapp_Param dto)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            if (ctx.Tapp_Param.Count(c => c.ParamsName == dto.ParamsName && c.IsDelete == 0 ) > 0)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "参数名已存在" }), this.JsonContentType());
            }
            dto.CreateTime = DateTime.Now;
            dto.CreateBy = this.AppUser().UserName;
            dto.IsDelete = 0;
            ctx.Tapp_Param.Add(dto);
            if (ctx.SaveChanges() > 0)
            {
                return Content(this.GetJSON(new { Result = true, Msg = "成功", Dto = dto }), this.JsonContentType());
            }
            return Content(this.GetJSON(new { Result = false, Msg = "失败", Dto = dto }), this.JsonContentType());
        }

        [HttpPost]
        public ActionResult Edit(Tapp_Param dto)
        {
            if (dto.Id > 0)
            {
                HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
                if (ctx.Tapp_Param.Count(c => c.ParamsName == dto.ParamsName && c.IsDelete == 0 && c.Id != dto.Id) > 0)
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "参数名已存在" }), this.JsonContentType());
                }
                Tapp_Param model = ctx.Tapp_Param.FirstOrDefault(c => c.Id == dto.Id);
                this.CopyObject<Tapp_Param>(model, dto);
                model.EditTime = DateTime.Now;
                model.EditBy = this.AppUser().UserName;
                if (ctx.SaveChanges() > 0)
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
            foreach (var item in arrId)
            {
                int tid = Convert.ToInt32(item);
                Tapp_Param model = ctx.Tapp_Param.FirstOrDefault(c => c.Id == tid);
                if (model != null)
                {
                    model.IsDelete = 1;
                    model.DeleteBy = this.AppUser().UserName;
                    model.DeleteTime = DateTime.Now;
                    // ctx.Tapp_Param.Remove(model);
                    counter++;
                }
            }
            if (ctx.SaveChanges() > 0)
            {
                return Content(this.GetJSON(new { Result = true, Msg = "成功", Id = counter }), this.JsonContentType());
            }
            return Content(this.GetJSON(new { Result = false, Msg = "失败", id = counter }), this.JsonContentType());
        }


    }
}
