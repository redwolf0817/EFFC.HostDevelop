using Builder.Web.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Business.Sys
{
    public class IndexView:ViewLogic
    {
        public override Action<EFFC.Frame.Net.Data.LogicData.LogicData> GetAction(string actionname)
        {
            switch (actionname.ToLower())
            {
                default:
                    return Load;
            }
        }

        private void Load(EFFC.Frame.Net.Data.LogicData.LogicData obj)
        {
            SetViewPath("~/Views/Index.cshtml");
            Cookie.SetCookie("token", Guid.NewGuid().ToString(),null,DateTime.MaxValue);
            var token = Cookie.GetCookie("token");
            Cookie.SetCookie("name", "ych");
            Cookie.RemoveCookie("token");
        }

        public override string Name
        {
            get { return "index"; }
        }
    }
}
