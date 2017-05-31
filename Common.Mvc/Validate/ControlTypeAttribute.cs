using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Mvc.Validate
{
    /// <summary>
    /// 控件类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ControlTypeAttribute : Attribute
    {
        public ControlTypeAttribute()
        {
            ControlType = ControlType.Custom;
        }
        public ControlTypeAttribute(ControlType controlType)
        {
            ControlType = controlType;
        }
        public ControlType ControlType { get; set; }
        public string ControlHtml(string modelName, string className, string dataOptions, Dictionary<string, string> selectOptions)
        {
            if (!string.IsNullOrEmpty(className))
            {
                className = string.Format("class=\"{0}\"", className);
            }
            if (!string.IsNullOrEmpty(dataOptions))
            {
                dataOptions = string.Format("data-options=\"{0}\" ", dataOptions);
            }
            switch (ControlType)
            {
                case ControlType.DateTime:
                    return string.Format("<input type=\"text\" id=\"{0}\" name=\"{0}\" class=\"easyui-datetimebox\" data-options=\"required:true,showSeconds:true\" value=\"\" style=\"width:150px\" />", modelName);
                case ControlType.Date:
                    return string.Format("<input type=\"text\" id=\"{0}\" name=\"{0}\" class=\"easyui-datebox\" data-options=\"required:true,showSeconds:false\" value=\"\" style=\"width:150px\" />", modelName);
                case ControlType.Time:
                    return string.Format("<input type=\"text\" id=\"{0}\" name=\"{0}\" class=\"easyui-timespinner\" data-options=\"required:true,showSeconds:true\" value=\"\" style=\"width:80px\" />", modelName);
                case ControlType.MultilineText:
                    return string.Format("<textarea id=\"{0}\" name=\"{0}\" {1} {2} style=\"width:300px;height:100px;\"></textarea> ", modelName, className, dataOptions);
                case ControlType.Html:
                    return string.Format("<textarea id=\"{0}\" name=\"{0}\" style=\"width:800px;\" {1} {2} ></textarea>" +
                           "<script type=\"text/javascript\">UE.getEditor(\"{0}\");</script>", modelName, className, dataOptions);
                case ControlType.Select:
                    return string.Format("<select id=\"{0}\" name=\"{0}\" {1} {2} style=\"width:150px\">" +
                           GetOptions(selectOptions) +
                           "</select>", modelName, className, dataOptions);
                case ControlType.CheckBox:
                    return GetCheckBox(modelName, className, dataOptions, selectOptions);
                case ControlType.RadioBox:
                    return GetRadioBox(modelName, className, dataOptions, selectOptions);
                case ControlType.Hidden:
                    return string.Format("<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"\" />", modelName);
                case ControlType.Password:
                    return string.Format("<input type=\"password\" id=\"{0}\" name=\"{0}\" {1} {2} value=\"\" style=\"width:150px\" />", modelName, className, dataOptions);
                case ControlType.Integer:
                case ControlType.Decimal:
                    return string.Format("<input type=\"text\" id=\"{0}\" name=\"{0}\" class=\"easyui-numberbox\" {1} value=\"\" style=\"width:150px\" />", modelName, dataOptions);
                case ControlType.Upload:
                    return string.Format("<input type=\"file\" id=\"{0}\" name=\"{0}\" class=\"easyui-datetimebox\" style=\"width:150px\" />", modelName);
                case ControlType.UploadImg:
                    return string.Format("<div style=\"float: left; display: none;\">" +
                                        "<input type=\"hidden\" id=\"{0}\" name=\"{0}\" />" +
                                        "<img id=\"img{0}\" width=\"80\" height=\"50\" src=\"\" /><a href=\"#\" class=\"easyui-linkbutton easyui-tooltip remove\" data-options=\"iconCls:'icon-remove',plain:true,position:'left'\" title=\"删除\" onclick=\"delImg(this);\"></a>" +
                                        "</div>" +
                                        "<div style=\"float: left; margin-left: 5px; padding-top: 15px;\"><span id=\"uploadify{0}\"></span></div>" +
                                        "<script type=\"text/javascript\">" + Environment.NewLine +
                                        "    uploadifyFile({{" + Environment.NewLine +
                                        "       fileId: \"uploadify{0}\"," + Environment.NewLine +
                                        "       fileTypeExts: \"*.jpg;*.gif;*.png\"," + Environment.NewLine +
                                        "       fileTypeDesc: \"图片\"," + Environment.NewLine +
                                        "       onUploadSuccess: function (file, data, response) {{" + Environment.NewLine +
                                        "           if (data) {{" + Environment.NewLine +
                                        "               var result = JSON.parse(data);" + Environment.NewLine +
                                        "               if (result.Status == 0) {{" + Environment.NewLine +
                                        "                   $(\"#img{0}\").attr(\"src\", result.FileName);" + Environment.NewLine +
                                        "                   $(\"#img{0}\").parent().show();" +
                                        "                   $(\"#{0}\").val(result.FileName);" + Environment.NewLine +
                                        "               }}" + Environment.NewLine +
                                        "               else {{" + Environment.NewLine +
                                        "                   alert(result.Message);" + Environment.NewLine +
                                        "               }}" + Environment.NewLine +
                                        "           }}" + Environment.NewLine +
                                        "           else {{" + Environment.NewLine +
                                        "               alert(\"未知错误\");" + Environment.NewLine +
                                        "           }}" + Environment.NewLine +
                                        "       }}" + Environment.NewLine +
                                        "   }});" + Environment.NewLine +
                                        "</script>", modelName);
                case ControlType.UploadDoc:
                    return string.Format("<div style=\"float: left;\">" +
                                       "<input type=\"text\" id=\"{0}\"  style=\"width:200px;\" />" +
                                       "</div>" +
                                       "<div style=\"float: left; margin-left: 5px;\">" +
                                       "    <div id=\"uploadify{0}\"></div>" +
                                       "</div>" +
                                       "<script type=\"text/javascript\">" + Environment.NewLine +
                                        "    uploadifyFile({{" + Environment.NewLine +
                                        "       fileId: \"uploadify{0}\"," + Environment.NewLine +
                                        "       fileTypeExts: \"*.rar;*.zip;*.doc;*.docx;*.xls;*.xlsx;*.pdf;*.txt\"," + Environment.NewLine +
                                        "       fileTypeDesc: \"文件\"," + Environment.NewLine +
                                        "       onUploadSuccess: function (file, data, response) {{" + Environment.NewLine +
                                        "           if (data) {{" + Environment.NewLine +
                                        "               var result = JSON.parse(data);" + Environment.NewLine +
                                        "               if (result.Status == 0) {{" + Environment.NewLine +
                                        "                   $(\"#{0}\").val(result.FileName);" + Environment.NewLine +
                                        "               }}" + Environment.NewLine +
                                        "               else {{" + Environment.NewLine +
                                        "                   alert(result.Message);" + Environment.NewLine +
                                        "               }}" + Environment.NewLine +
                                        "           }}" + Environment.NewLine +
                                        "           else {{" + Environment.NewLine +
                                        "               alert(\"未知错误\");" + Environment.NewLine +
                                        "           }}" + Environment.NewLine +
                                        "       }}" +
                                        "   }});" +
                                        "</script>", modelName);
                default:
                    return string.Format("<input type=\"text\" id=\"{0}\" name=\"{0}\" {1} {2} value=\"\" style=\"width:150px\" />", modelName, className, dataOptions);
            }
        }
        private string GetOptions(Dictionary<string, string> options)
        {
            string result = "<option value=\"-1\">请选择</option>";
            if (options != null && options.Count > 0)
            {
                foreach (KeyValuePair<string, string> item in options)
                {
                    result += string.Format("<option value=\"{0}\">{1}</option>", item.Value, item.Key);
                }
            }
            return result;
        }
        private string GetCheckBox(string modelName, string className, string dataOptions, Dictionary<string, string> options)
        {
            string result = string.Empty;
            int index = 0;
            foreach (KeyValuePair<string, string> item in options)
            {
                result += string.Format("<input type=\"checkbox\" id=\"{0}_{1}\" name=\"{0}\" {2} {3} value=\"{4}\" /><label>{5}</label>&nbsp;&nbsp;", 
                    modelName,index++, className, dataOptions, item.Value, item.Key);
            }
            return result;
        }
        private string GetRadioBox(string modelName, string className, string dataOptions, Dictionary<string, string> options)
        {
            string result = string.Empty;
            int index = 0;
            foreach (KeyValuePair<string, string> item in options)
            {
                result += string.Format("<input type=\"radio\" id=\"{0}_{1}\" name=\"{0}\" {2} {3} value=\"{4}\" /><label>{5}</label>&nbsp;&nbsp;",
                    modelName, index++, className, dataOptions, item.Value, item.Key);
            }
            return result;
        }
    }
    public enum ControlType : int
    {
        /// <summary>
        /// 表示自定义
        /// </summary>
        Custom = 0,
        /// <summary>
        /// 表示某个具体时间，以日期和当天的时间表示
        /// </summary>
        DateTime = 1,
        /// <summary>
        /// 表示日期值
        /// </summary>
        Date = 2,
        /// <summary>
        /// 表示时间值
        /// </summary>
        Time = 3,
        /// <summary>
        /// 表示所显示的文本
        /// </summary>
        Text = 7,
        /// <summary>
        /// 表示一个 HTML 文件
        /// </summary>
        Html = 8,
        /// <summary>
        /// 表示多行文本
        /// </summary>
        MultilineText = 9,
        /// <summary>
        /// 表示选择框
        /// </summary>
        Select = 10,
        /// <summary>
        /// 表示复选框
        /// </summary>
        CheckBox = 11,
        /// <summary>
        /// 表示单选框
        /// </summary>
        RadioBox = 12,
        /// <summary>
        /// 表示隐藏域
        /// </summary>
        Hidden = 13,
        /// <summary>
        /// 表示整型数字
        /// </summary>
        Integer = 14,
        /// <summary>
        /// 表示浮点数
        /// </summary>
        Decimal = 15,
        /// <summary>
        /// 表示密码框
        /// </summary>
        Password = 16,
        /// <summary>
        /// 表示文件上传数据类型
        /// </summary>
        Upload = 17,
        /// <summary>
        /// 表示普通文件上传数据类型
        /// </summary>
        UploadDoc = 18,
        /// <summary>
        /// 表示图片文件上传数据类型
        /// </summary>
        UploadImg = 19,
    }
}
