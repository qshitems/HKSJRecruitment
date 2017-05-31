using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Xml;
using System.IO;

namespace Common.Mvc
{
    public sealed class ProgramTestConfig
    {
        string siteUrl = string.Empty;
        const string appKey = "TestConfigFilePath";     //系统Config中的键名

        public static readonly string ssnIgnoreCheckRight = "IgnoreCheckRight";
        public static readonly string ssnDefaultAutoLoginUserId = "DefaultAutoLoginUserId";
        public static readonly string ssnIsAutoLogin = "IsAutoLogin";

        public ProgramTestConfig(string sitePath)
        {
            this.siteUrl = sitePath;
        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        void ReadConfig()
        {
            ignoreCheckRight = false;
            isAutoLogin = false;

            XmlDocument xmlDoc;

            string fileName = ConfigurationManager.AppSettings[appKey];
            if (string.IsNullOrEmpty(fileName))
                fileName = @"..\ProgramTest.Config";
            if (File.Exists(siteUrl + fileName))
            {
                xmlDoc = new XmlDocument();
                xmlDoc.Load(siteUrl + fileName);
            }
            else
                return;

            XmlNode Node = xmlDoc.SelectSingleNode("Settings/" + ssnIgnoreCheckRight);
            if (Node != null && bool.TryParse(Node.Attributes["value"].Value, out ignoreCheckRight))
            {
            }
            Node = xmlDoc.SelectSingleNode("Settings/" + ssnDefaultAutoLoginUserId);
            if (Node != null && int.TryParse(Node.Attributes["value"].Value, out defaultAutoLoginUserId))
            {
            }
            Node = xmlDoc.SelectSingleNode("Settings/" + ssnIsAutoLogin);
            if (Node != null && bool.TryParse(Node.Attributes["value"].Value, out isAutoLogin))
            {
            }
        }

        public bool GetConfigBoolValue(string str)
        {
            bool theValue = false;
            XmlDocument xmlDoc;

            string fileName = ConfigurationManager.AppSettings[appKey];
            if (string.IsNullOrEmpty(fileName))
                fileName = @"..\ProgramTest.Config";
            if (File.Exists(siteUrl + fileName))
            {
                xmlDoc = new XmlDocument();
                xmlDoc.Load(siteUrl + fileName);
            }
            else
                return theValue;

            XmlNode Node = xmlDoc.SelectSingleNode("Settings/" + str);
            if (Node != null && bool.TryParse(Node.Attributes["value"].Value, out theValue))
            {
            }
            return theValue;
        }

        bool ignoreCheckRight;
        /// <summary>
        /// 是否不检查权限
        /// </summary>
        public bool IgnoreCheckRight
        {
            get
            {
                ReadConfig();
                return ignoreCheckRight;
            }
        }

        int defaultAutoLoginUserId;
        /// <summary>
        /// 自动登录用户ID
        /// </summary>
        public int DefaultAutoLoginUserId
        {
            get
            {
                ReadConfig();
                return defaultAutoLoginUserId;
            }
        }

        bool isAutoLogin = false;
        /// <summary>
        /// 是否自动登录
        /// </summary>
        public bool IsAutoLogin
        {
            get
            {
                ReadConfig();
                return isAutoLogin;
            }
        }
    }
}
