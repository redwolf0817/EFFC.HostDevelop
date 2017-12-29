using Builder.Web.Logic;
using EFFC.Frame.Net.Base.Common;
using EFFC.Frame.Net.Base.Constants;
using EFFC.Frame.Net.Base.Data.Base;
using EFFC.Frame.Net.Base.ResouceManage.JsEngine;
using EFFC.Frame.Net.Business.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Web.Business.Sys
{
    public class EditorGo:GoLogic
    {
        
        protected override Func<EFFC.Frame.Net.Data.LogicData.LogicData, object> GetFunction(string actionName)
        {
            switch (actionName.ToLower())
            {
                case "syntax":
                    return LoadSyntax;
                case "allsyntax":
                    return LoadAllSyntax;
                default:
                    return null;
            }
        }

        private object LoadAllSyntax(EFFC.Frame.Net.Data.LogicData.LogicData arg)
        {
            HostLogicContext context = new HostLogicContext();
            context.RootPath = ComFunc.nvl(Configs["HostJs_Path"]);
            HostLogicEngine hle = new HostLogicEngine(context, this);
            var rtn = FrameDLRObject.CreateInstance(FrameDLRFlags.SensitiveCase);
            rtn.ReserveKeys = hle.ReserveKeys;
            rtn.SystemObject = hle.ServerReserverObjectKey;
            return rtn;
        }

        private object LoadSyntax(EFFC.Frame.Net.Data.LogicData.LogicData arg)
        {
           
            WS.Send("处理中，请稍后...");
            var js = HttpUtility.UrlDecode(ComFunc.nvl(arg["js"]));
            var checksyntax = HttpUtility.UrlDecode(ComFunc.nvl(arg["syntax"]));
            var filename = ComFunc.nvl(arg["filename"]);
            if (js.IndexOf(";")>0)
            {
                if (js.LastIndexOf("}") > js.LastIndexOf(";"))
                {
                    js = js.Substring(0, js.LastIndexOf("}") + 1);
                }
                else
                {
                    js = js.Substring(0, js.LastIndexOf(";") + 1);
                }
            }
            //var obj = Js.Call(js, null, checksyntax);
            HostLogicContext context = new HostLogicContext();
            context.RootPath = ComFunc.nvl(Configs["HostJs_Path"]);
            HostLogicEngine hle = new HostLogicEngine(context, this);
            var rtn = FrameDLRObject.CreateInstance();
            
            if (checksyntax.IndexOf(".") < 0)
            {
                if (hle.ReserveKeys.Contains(checksyntax))
                {
                    rtn.result = "保留关键字";
                }
                else
                {
                    var cont = hle.ServerReserverObjectKey;
                    if (cont.Keys.Contains(checksyntax))
                    {
                        rtn.result = cont.GetValue(checksyntax);
                    }
                }
            }
            else
            {
                var cont = hle.ServerReserverObjectKey;
                var arr = checksyntax.Split('.');
                if (arr.Length == 2 && cont.Keys.Contains(arr[0]))
                {
                    FrameDLRObject obj = (FrameDLRObject)cont.GetValue(arr[0]);
                    var functions = (FrameDLRObject)obj.GetValue("functions");
                    var properties = (FrameDLRObject)obj.GetValue("properties");
                    if (functions.GetValue(arr[1]) != null)
                    {
                        rtn.result = functions.GetValue(arr[1]);
                    }
                    else if (properties.GetValue(arr[1]) != null)
                    {
                        rtn.result = properties.GetValue(arr[1]);
                    }
                }
                else
                {
                    rtn.result = "无法解析";
                }
            }

            WS.Send("处理完成！");
            return rtn;
        }

        public override string Name
        {
            get { return "editor"; }
        }
    }
}
