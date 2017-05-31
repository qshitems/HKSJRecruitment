namespace Common.Mvc
{
    /// <summary>
    /// Helper 的摘要说明。
    /// </summary>
    public class SQLHelper
    {
        /// <summary>
        /// 本函数用于数据库操作过程中的传字串参数问题
        /// 比如将一个C#里面的字符串转成可直接入库的SQL String
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>将字符串中的"'"替代为"''",然后在字符串前后加"'"</returns>
        public static string ToSQLParamStr(object str)
        {
            return "'" + str.ToString().Replace("'", "''") + "'";
        }

        public static string ToSQLParamLikeStr(object str)
        {
            return "'%" + str.ToString().Replace("'", "''") + "%'";
        }

        /// <summary>
        /// 将字符串中的"'"换成SQL语句中的表达方式
        /// 即用"''"代替"'"
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>将字符串中的"'"替代为"''"</returns>
        public static string QuotedStr(string str)
        {
            return str.Replace("'", "''");
        }
    }
}
