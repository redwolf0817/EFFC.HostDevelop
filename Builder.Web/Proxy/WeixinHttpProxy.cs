using EFFC.Frame.Net.Base.Common;
using EFFC.Frame.Net.Base.Constants;
using EFFC.Frame.Net.Base.Data.Base;
using EFFC.Frame.Net.Base.Module;
using EFFC.Frame.Net.Data.Parameters;
using EFFC.Frame.Net.Data.WebData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Builder.Web.Proxy
{
    public class WeixinHttpProxy : HttpRemoteProxy<WebParameter, GoData>
    {
        protected override void ProcessAfterRequest(FrameDLRObject responseobj, WebParameter p, GoData d)
        {
            var responsestring = responseobj.GetValue("content");

            if (FrameDLRObject.IsJson(ComFunc.nvl(responsestring)))
            {
                d.ExtentionObj.OuterHttpResult = FrameDLRObject.CreateInstance(responsestring, FrameDLRFlags.SensitiveCase);
            }
            else
            {
                d.ExtentionObj.OuterHttpResult = responsestring;
            }
        }

        protected override void ProcessBeforeRequest(WebParameter p, GoData d)
        {
            string url = ComFunc.nvl(d.ExtentionObj.OuterHttpUrl);
            SetRequestURL(url);
            if (d.ExtentionObj.OuterHttpPostData != null && d.ExtentionObj.OuterHttpPostData is FrameDLRObject)
            {
                var dobj = (FrameDLRObject)d.ExtentionObj.OuterHttpPostData;
                foreach (var k in dobj.Keys)
                {
                    AddPostData(k, dobj.GetValue(k));
                }
            }
        }
    }
}
