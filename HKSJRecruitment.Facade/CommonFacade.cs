using Microsoft.International.Converters.PinYinConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Common.Mvc;
using HKSJRecruitment.Models.Models;

namespace HKSJRecruitment.Facade
{
    public class CommonFacade
    {
        public string GetUid(string fullName)
        {
            fullName = fullName.Trim();
            string pinYins = string.Empty;
            foreach (char obj in fullName)
            {
                try
                {
                    ChineseChar chineseChar = new ChineseChar(obj);
                    string t = chineseChar.Pinyins[0];
                    pinYins += t.Substring(0, t.Length - 1);
                }
                catch
                {
                    pinYins += obj;
                }
            }
            pinYins = pinYins.ToLower();
            List<string> uids = new List<string>();
            string username = string.Empty;
            HKSJRecruitmentContext ctx = HttpContext.Current.GetDbContext<HKSJRecruitmentContext>();
            if (ctx.Tapp_User.Count(c => c.UserName == pinYins) == 0)
            {
                username = pinYins;
            }
            else
            {
                Regex regex = new Regex(@"^\d+$");
                foreach (var item in ctx.Tapp_User.Where(c => c.UserName.Contains(pinYins)).Select(c => c.UserName))
                {
                    string input = item.Substring(pinYins.Length);
                    if (regex.IsMatch(input))
                    {
                        uids.Add(item);
                    }
                }
                username = pinYins + (uids.Count + 1);
            }
            return username;
        }

    }
}
