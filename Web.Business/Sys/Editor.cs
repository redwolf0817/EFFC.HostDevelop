using Builder.Web.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Business.Sys
{
    public class Editor:ViewLogic
    {
        public override Action<EFFC.Frame.Net.Data.LogicData.LogicData> GetAction(string actionname)
        {
            return Load;
        }

        private void Load(EFFC.Frame.Net.Data.LogicData.LogicData obj)
        {
            SetViewPath("~/Views/IDE/editor.cshtml");
        }

        public override string Name
        {
            get { return "editor"; }
        }
    }
}
