using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using System.IO;

namespace Common.Mvc
{
    public class Log
    {
        private ILog log;

        static Log()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            if (!baseDir.EndsWith("\\"))
                baseDir += "\\";
            FileInfo file = new FileInfo(baseDir + "log4net.config");
            log4net.Config.XmlConfigurator.Configure(file);
        }

        public Log(Type type)
        {
            log = LogManager.GetLogger(type);
        }

        public Log(string name)
        {
            log = LogManager.GetLogger(name);
        }

        #region ILog ≥…‘±

        public void Debug(object message, Exception exception)
        {
            log.Debug(message, exception);
        }

        public void Debug(object message)
        {
            log.Debug(message);
        }

        public void Debug(object message, params object[] args)
        {
            Debug(string.Format(message.ToString(), args));
        }

        public void Error(object message, Exception exception)
        {
            log.Error(message, exception);
        }

        public void Error(object message, params object[] args)
        {
            Error(string.Format(message.ToString(), args));
        }

        public void Error(object message)
        {
            log.Error(message);
        }

        public void Fatal(object message, Exception exception)
        {
            log.Fatal(message, exception);
        }

        public void Fatal(object message)
        {
            log.Fatal(message);
        }

        public void Fatal(object message, params object[] args)
        {
            Fatal(string.Format(message.ToString(), args));
        }

        public void Info(object message, Exception exception)
        {
            log.Info(message, exception);
        }

        public void Info(object message)
        {
            log.Info(message);
        }

        public void Info(object message, params object[] args)
        {
            Info(string.Format(message.ToString(), args));
        }

        public void Warn(object message, Exception exception)
        {
            log.Warn(message, exception);
        }

        public void Warn(object message)
        {
            log.Warn(message);
        }

        public void Warn(object message, params object[] args)
        {
            Warn(string.Format(message.ToString(), args));
        }

        public bool IsDebugEnabled
        {
            get { return log.IsDebugEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return log.IsErrorEnabled; }
        }

        public bool IsFatalEnabled
        {
            get { return log.IsFatalEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return log.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return log.IsWarnEnabled; }
        }

        #endregion
    }
}
