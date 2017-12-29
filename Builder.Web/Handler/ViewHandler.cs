using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Data.SqlClient;
using EFFC.Frame.Net.Data.Parameters;
using EFFC.Frame.Net.Data.WebData;
using EFFC.Frame.Net.Web.Core;
using EFFC.Frame.Net.Base.Common;
using EFFC.Frame.Net.Base.Constants;
using EFFC.Frame.Net.Global;
using EFFC.Frame.Net.Web.ViewEngine;
using Builder.Web.Global;
using EFFC.Frame.Net.Base.Module;
using Builder.Web.Proxy;
using System.Net.WebSockets;

namespace Builder.Web.Handler
{
    public class ViewHandler : WMcvBaseHandler<WebParameter, WMvcData>
    {

        protected override void OnError(Exception ex, WebParameter p, WMvcData d)
        {
            if (ex is ThreadAbortException) return;

            if (IsAjaxAsync)
            {
                d.ViewPath = "~/Views/Shared/Error_Frame_NoLayout.cshtml";
            }
            else
            {
                d.ViewPath = "~/Views/Shared/Error_Frame.cshtml";
            }
            string errorCode = "E-" + ComFunc.nvl(p[DomainKey.CONFIG, "Machine_No"]) + "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string errlog = "";
            if (ex.InnerException != null)
            {
                errlog = string.Format("错误编号：{0}，\n{1}\n{2}\nInnerException:{3}\n{4}", errorCode, ex.Message, ex.StackTrace, ex.InnerException.Message, ex.InnerException.StackTrace);
            }
            else
            {
                errlog = string.Format("错误编号：{0}，\n{1}\n{2}", errorCode, ex.Message, ex.StackTrace);
            }
            //此處error頁面的跳轉處理不用轉向error.view，以免發生死循環
            d[DomainKey.VIEW_LIST, "ErrorTitle"] = "系统出错了";
            var isdebug = p[DomainKey.CONFIG, "DebugMode"] == null ? false : (bool)p[DomainKey.CONFIG, "DebugMode"];
            if (isdebug)
            {
                d[DomainKey.VIEW_LIST, "ErrorMsg"] = string.Format("出錯了，{0}", errlog);
            }
            else
            {
                d[DomainKey.VIEW_LIST, "ErrorMsg"] = string.Format("系统出错了，请联系相关人员帮助处理，并告知其错误编号。谢谢！（错误编号：{0}）", errorCode);
            }
            p.Resources.RollbackTransaction(p.CurrentTransToken);
            p.Resources.ReleaseAll();
            GlobalCommon.Logger.WriteLog(LoggerLevel.ERROR, errlog);
            if (!IsWebSocket)
            {
                WMvcView.RenderView(p, d, CurrentContext, CurrentContext.Response.Output);
            }
            else
            {
                if (CurrentSocket.State == WebSocketState.Open)
                {
                    var b = Encoding.UTF8.GetBytes(ComFunc.FormatJSON(errorCode, errlog, "").ToJSONString());
                    var buffer = new ArraySegment<byte>(b);
                    CurrentSocket.SendAsync(buffer, WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
                }
            }
        }

        protected override void Init(System.Web.HttpContext context, WebParameter p, WMvcData d)
        {
            base.Init(context, p, d);
            GlobalPrepare.ConfigPrepare(ref p);
        }
        protected override bool RunMe(WebParameter p, WMvcData d)
        {
            try
            {
                //1.进行预处理
                p.CanContinue = true;
                bool isSuccess = ModuleProxyManager<WebParameter, WMvcData>.Call<PreProcessProxy>(p, d);
                //2.业务逻辑处理
                if (isSuccess && p.CanContinue)
                    isSuccess = isSuccess & ModuleProxyManager<WebParameter, WMvcData>.Call<BusinessProxy>(p, d);
                return isSuccess;
            }
            finally
            {
                p.Resources.ReleaseAll();
            }
        }

        protected override void AfterProcess(HttpContext context, WebParameter p, WMvcData d)
        {
            base.AfterProcess(context, p, d);
        }

        public override string Name
        {
            get { return "WMvcHandler"; }
        }

        public override string Version
        {
            get { return "0.0.1"; }
        }

        public override string Description
        {
            get { return "WMvc资源请求处理器"; }
        }

        
    }
}
