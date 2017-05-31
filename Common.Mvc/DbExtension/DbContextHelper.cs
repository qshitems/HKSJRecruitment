using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Entity;

namespace Common.Mvc
{
    public static class DbContextHelper
    {
        private static string HostDbContext = "HostDbContext";

        /// <summary>
        /// 获取DbContext
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static TDbContext GetDbContext<TDbContext>(this System.Web.HttpContextBase httpContext)
            where TDbContext : System.Data.Entity.DbContext, new()
        {
            return InnerGetDbContext<TDbContext>();
        }

        /// <summary>
        /// 获取DbContext
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static TDbContext GetDbContext<TDbContext>(this System.Web.HttpContext httpContext)
            where TDbContext : System.Data.Entity.DbContext, new()
        {
            return InnerGetDbContext<TDbContext>();
        }

        private static TDbContext InnerGetDbContext<TDbContext>() where TDbContext : DbContext, new()
        {
            if (System.Web.HttpContext.Current == null)
                return null;
            IDictionary items = System.Web.HttpContext.Current.Items;
            string keyName = typeof(TDbContext).FullName;
            if (items[HostDbContext] == null)
            {
                TDbContext db = CreateDbContext<TDbContext>(items, new Dictionary<string, DbContext>(), keyName);
                return db;
            }
            IDictionary dicDbContext = items[HostDbContext] as Dictionary<string, DbContext>;
            if (!dicDbContext.Contains(keyName))
            {
                TDbContext db = CreateDbContext<TDbContext>(items, dicDbContext, keyName);
                return db;
            }
            return dicDbContext[keyName] as TDbContext;
        }

        /// <summary>
        /// 创建DbContext
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="items">代表HttpContext.Items,DbContextSet放在此字典里</param>
        /// <param name="dicDbContextSet">创建的DbContext放在此字典里</param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        private static TDbContext CreateDbContext<TDbContext>(IDictionary items, IDictionary dicDbContextSet, string keyName) where TDbContext : DbContext, new()
        {
            TDbContext db = new TDbContext();
            db.Configuration.LazyLoadingEnabled = false;
            dicDbContextSet.Add(keyName, db);
            items[HostDbContext] = dicDbContextSet;
            return db;
        }

        /// <summary>
        /// 回收DbContext
        /// </summary>
        public static void DisposeDbContext(this System.Web.HttpContext httpContext)
        {
            if (System.Web.HttpContext.Current == null)
                return;
            IDictionary items = System.Web.HttpContext.Current.Items;
            if (items[HostDbContext] == null)
                return;
            IDictionary dicDbContext = items[HostDbContext] as Dictionary<string, DbContext>;
            foreach (DbContext db in dicDbContext.Values)
                db.Dispose();
        }
    }
}
