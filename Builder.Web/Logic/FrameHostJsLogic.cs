using EFFC.Frame.Net.Base.Constants;
using EFFC.Frame.Net.Business.Logic;
using EFFC.Frame.Net.Data.Parameters;
using EFFC.Frame.Net.Data.WebData;
using EFFC.Frame.Net.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Builder.Web.Logic
{
    public class FrameHostJsLogic : HostJsLogic
    {
        public FrameHostJsLogic()
            : base()
        {

        }
        protected override string GetLogicRootPath()
        {
            return GlobalCommon.HostCommon.RootPath + HostJsConstants.COMPILED_LOGIC_PATH;
        }
        protected override HostLogicEngine GetHostLogicEngine()
        {
            var hle = base.GetHostLogicEngine();
            hle.CurrentContext.AddHostJsObject(new OuterInterfaceObject(this));
            hle.CurrentContext.AddHostJsObject(new WeixinObject(this));
            return hle;
        }
        /// <summary>
        /// 获取FrameLogic用到的HostLogicEngine
        /// </summary>
        /// <returns></returns>
        public static HostLogicEngine GetCurrentLogicEngine(WebBaseLogic<WebParameter,GoData> logic)
        {
            FrameHostJsLogic l = new FrameHostJsLogic();
            CallContext.SetData(l.Name + CALL_CONTEXT_PARAMETER, logic.CallContext_Parameter);
            CallContext.SetData(l.Name + CALL_CONTEXT_DATACOLLECTION, logic.CallContext_DataCollection);
            CallContext.SetData(l.Name + CALL_CONTEXT_RESOURCEMANAGER, logic.CallContext_ResourceManage);
            CallContext.SetData(l.Name + CALL_CONTEXT_TRANSTOKEN, logic.CallContext_CurrentToken);
            return l.GetHostLogicEngine();
        }
    }
}
