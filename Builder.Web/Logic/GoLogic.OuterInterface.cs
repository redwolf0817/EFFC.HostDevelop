using Builder.Web.Proxy;
using EFFC.Frame.Net.Base.Constants;
using EFFC.Frame.Net.Base.Data.Base;
using EFFC.Frame.Net.Base.Module;
using EFFC.Frame.Net.Data.Parameters;
using EFFC.Frame.Net.Data.WebData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Builder.Web.Logic
{
    public abstract partial class GoLogic
    {
        OuterInterfaceHelper _oifh = null;
        public new OuterInterfaceHelper OuterInterface
        {
            get
            {
                if (_oifh == null) _oifh = new OuterInterfaceHelper(this);
                return _oifh;
            }


        }
        public class OuterInterfaceHelper
        {
            GoLogic _logic = null;

            public OuterInterfaceHelper(GoLogic logic)
            {
                _logic = logic;
            }

            public object CallWeixinServer(string url, params KeyValuePair<string, object>[] data)
            {
                var copyp = _logic.CallContext_Parameter.Clone<WebParameter>();
                var copyd = _logic.CallContext_DataCollection.Clone<GoData>();
                copyd.ExtentionObj.OuterHttpUrl = url;
                if (data != null)
                {
                    FrameDLRObject dobj = FrameDLRObject.CreateInstance(FrameDLRFlags.SensitiveCase);
                    foreach (var item in data)
                    {
                        dobj.SetValue(item.Key, item.Value);
                    }
                    copyd.ExtentionObj.OuterHttpPostData = dobj;
                }
                ModuleProxyManager.Call<WeixinHttpProxy, WebParameter, GoData>(copyp, copyd);
                return copyd.ExtentionObj.OuterHttpResult;
            }
        }


    }
}
