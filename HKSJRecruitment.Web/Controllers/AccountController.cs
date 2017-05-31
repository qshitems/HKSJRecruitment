using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using HKSJRecruitment.Models.Models;
using HKSJRecruitment.Web.Models;
using Common.Mvc;


namespace HKSJRecruitment.Web.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// 用户登录
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [Anonymous]
        public ActionResult Login()
        {
            return View();
        }
        [Anonymous]
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            if (Session["ValidateCode"] == null || !string.Equals(model.VerifyCode, Session["ValidateCode"].ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return Content(this.GetJSON(new { Result = false, Msg = "验证码错误" }));
            }
            Tapp_User appUser = ctx.Tapp_User.FirstOrDefault(c => c.UserName == model.UserName && c.State == 1);
            if (appUser == null)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "用户名不存在" }));
            }
            string pwdDb = Md5.Encrypt(appUser.UserName + model.UserPwd);
            if (pwdDb != appUser.UserPwd)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "密码错误" }));
            }
            UserHelper.Login(appUser.Id,appUser.UserName);
            return Content(this.GetJSON(new { Result = true, Msg = "成功" }));
        }
        public ActionResult UpdatePwd(UpdatePwdModel model)
        {
            if (model.NewPwd != model.ConfigPwd)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "两次新密码不一致" }));
            }
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            int userId = HttpContext.AppUserId();
            Tapp_User appUser = ctx.Tapp_User.FirstOrDefault(c => c.Id == userId && c.State == 1);
            if (appUser == null)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "用户不存在" }));
            }
            string oldPwdDb = Md5.Encrypt(appUser.UserName + model.OldPwd);
            if (oldPwdDb != appUser.UserPwd)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "旧密码错误" }));
            }
            appUser.UserPwd = Md5.Encrypt(appUser.UserName + model.NewPwd);
            if (ctx.SaveChanges() >= 0)
            {
                return Content(this.GetJSON(new { Result = true, Msg = "修改密码成功" }));
            }
            return Content(this.GetJSON(new { Result = false, Msg = "修改密码失败" }));

        }
        /// <summary>
        /// 退出
        /// <returns></returns>
        [Anonymous]
        public ActionResult LogOff()
        {
            UserHelper.LogOff();
            return RedirectToAction("Login");
        }

        [Anonymous]
        public ActionResult VerifyCode()
        {
            ValidateCode vCode = new ValidateCode();
            string code = vCode.CreateValidateCode(5);
            Session["ValidateCode"] = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }
    }
}
