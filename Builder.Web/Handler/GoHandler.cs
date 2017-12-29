using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using EFFC.Frame.Net.Web.Core;
using EFFC.Frame.Net.Data.Parameters;
using EFFC.Frame.Net.Data.WebData;
using EFFC.Frame.Net.Global;
using EFFC.Frame.Net.Base.Common;
using EFFC.Frame.Net.Base.Constants;
using EFFC.Frame.Net.Base.Module;
using Builder.Web.Proxy;
using EFFC.Frame.Net.Base.Data.Base;
using Builder.Web.Global;
using Builder.Web.Helper;
using System.Net.WebSockets;
using System.Threading;
using System.Web.SessionState;
using Noesis.Javascript;


namespace Builder.Web.Handler
{
    public class GoHandler : GoBaseHandler<WebParameter, GoData>, IReadOnlySessionState
    {
        protected override void OnError(Exception ex, WebParameter p, GoData d)
        {
            GlobalCommon.ExceptionProcessor.ProcessException(this, ex, p, d);
            string errorCode = "E-" + ComFunc.nvl(p[DomainKey.CONFIG, "Machine_No"]) + "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string errlog = "";
            if (ex is JavascriptException)
            {
                var jex = (JavascriptException)ex;
                if (ex.InnerException != null)
                {
                    if (ex.InnerException is JavascriptException)
                    {
                        var ijex = (JavascriptException)ex.InnerException;
                        errlog = string.Format("错误编号：{0}，\n{1}\n{2}\n出错代码行数{3}\n出错代码列数{4}\n出错代码位置{5}\nInnerException:{6}\n{7}\n出错代码行数{8}\n出错代码列数{9}\n出错代码位置{10}", errorCode, ex.Message, ex.StackTrace,
                            jex.Line, jex.StartColumn + "~" + jex.EndColumn, jex.V8SourceLine.Replace("\"", "'"),
                            ex.InnerException.Message, ex.InnerException.StackTrace,
                            ijex.Line, ijex.StartColumn + "~" + ijex.EndColumn, ijex.V8SourceLine.Replace("\"", "'"));
                    }
                    else
                    {
                        errlog = string.Format("错误编号：{0}，\n{1}\n{2}\n出错代码行数{3}\n出错代码列数{4}\n出错代码位置{5}\nInnerException:{6}\n{7}", errorCode, ex.Message, ex.StackTrace, jex.Line, jex.StartColumn + "~" + jex.EndColumn, jex.V8SourceLine, ex.InnerException.Message, ex.InnerException.StackTrace);
                    }
                }
                else
                {
                    errlog = string.Format("错误编号：{0}，\n{1}\n{2}\n出错代码行数{3}\n出错代码列数{4}\n出错代码位置{5}", errorCode, ex.Message, ex.StackTrace,
                        jex.Line, jex.StartColumn + "~" + jex.EndColumn, jex.V8SourceLine.Replace("\"", "'"));
                }
            }
            else
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException is JavascriptException)
                    {
                        var ijex = (JavascriptException)ex.InnerException;
                        errlog = string.Format("错误编号：{0}，\n{1}\n{2}\nInnerException:{3}\n{4}\n\n出错代码行数{5}\n出错代码列数{6}\n出错代码位置{7}", errorCode, ex.Message, ex.StackTrace,
                            ex.InnerException.Message, ex.InnerException.StackTrace,
                            ijex.Line, ijex.StartColumn + "~" + ijex.EndColumn, ijex.V8SourceLine.Replace("\"", "'"));
                    }
                    else
                    {
                        errlog = string.Format("错误编号：{0}，\n{1}\n{2}\nInnerException:{3}\n{4}", errorCode, ex.Message, ex.StackTrace, ex.InnerException.Message, ex.InnerException.StackTrace);
                    }
                }
                else
                {
                    errlog = string.Format("错误编号：{0}，\n{1}\n{2}", errorCode, ex.Message, ex.StackTrace);
                }
            }

            GlobalCommon.Logger.WriteLog(LoggerLevel.ERROR, errlog);

            var errormsg = "";
            var isdebug = p[DomainKey.CONFIG, "DebugMode"] == null ? false : (bool)p[DomainKey.CONFIG, "DebugMode"];
            if (isdebug)
            {
                errormsg = string.Format("出错了，{0}", errlog); ;
            }
            else
            {
                errormsg = string.Format("系统出错了，请联系相关人员帮助处理，并告知其错误编号。谢谢！（错误编号：{0}）", errorCode);
            }

            p.Resources.RollbackTransaction(p.CurrentTransToken);
            p.Resources.ReleaseAll();

            if (d.ContentType == GoResponseDataType.Json)
            {
                if (IsWebSocket)
                {
                    if (CurrentSocket.State == WebSocketState.Open)
                    {
                        var b = Encoding.UTF8.GetBytes(ComFunc.FormatJSON(errorCode, errlog, "").ToJSONString());
                        var buffer = new ArraySegment<byte>(b);
                        CurrentSocket.SendAsync(buffer, WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
                    }
                }
                else
                {
                    CurrentContext.Response.Write(ComFunc.FormatJSON(errorCode, errlog, "").ToJSONString());
                }
            }
            else
            {
                p[DomainKey.SESSION, "IsAjaxAsync"] = IsAjaxAsync;
                p[DomainKey.SESSION, "ErrorMsg"] = errormsg;
                p[DomainKey.SESSION, "ErrorTitle"] = "系统错误";
                CurrentContext.Response.Redirect("error.view", false);
            }
        }

        protected override bool RunMe(WebParameter p, GoData d)
        {
            try
            {
                bool isSuccess = ModuleProxyManager<WebParameter, GoData>.Call<GoBusinessProxy>(p, d);

                return isSuccess;
            }
            finally
            {
                p.Resources.ReleaseAll();
            }
        }

        protected override void Init(System.Web.HttpContext context, WebParameter p, GoData d)
        {
            base.Init(context, p, d);
            GlobalPrepare.ConfigPrepare(ref p);
        }

        public override string Name
        {
            get { return "Other"; }
        }

        public override string Version
        {
            get { return "0.0.1"; }
        }

        public override string Description
        {
            get { return "Go类型的Request的处理"; }
        }
    }
}
