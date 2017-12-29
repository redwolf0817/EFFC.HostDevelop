using Builder.Web.Constant;
using EFFC.Frame.Net.Base.Constants;
using EFFC.Frame.Net.Base.Data;
using EFFC.Frame.Net.Business.Logic;
using EFFC.Frame.Net.Data.Parameters;
using EFFC.Frame.Net.Data.UnitData;
using EFFC.Frame.Net.Data.WebData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Builder.Web.Logic
{
    public abstract partial class ViewLogic: WebBaseLogic<WebParameter, WMvcData>
    {
        JoJoDBHelper _db;
        /// <summary>
        /// db操作相关
        /// </summary>
        public override DBHelper DB
        {
            get
            {
                if (_db == null)
                    _db = new JoJoDBHelper(this);

                return _db;
            }
        } 

        public class JoJoDBHelper : DBHelper
        {
            ViewLogic _logic = null;

            public JoJoDBHelper(ViewLogic logic):base(logic)
            {
                _logic = logic;
            }

            public override UnitDataCollection QueryByPage<T>(UnitParameter p, string actionflag)
            {
                if (_logic.CallContext_Parameter[DomainKey.POST_DATA, KeyDics.QueryByPage.ToPage] != null)
                {
                    p.ToPage = IntStd.ParseStd(_logic.CallContext_Parameter[DomainKey.POST_DATA, KeyDics.QueryByPage.ToPage]);
                }
                else
                {
                    p.ToPage = 1;
                }
                if (_logic.CallContext_Parameter[DomainKey.POST_DATA, KeyDics.QueryByPage.Count_per_Page] != null)
                {
                    p.Count_Of_OnePage = IntStd.ParseStd(_logic.CallContext_Parameter[DomainKey.POST_DATA, KeyDics.QueryByPage.Count_per_Page]);
                }
                else
                {
                    p.Count_Of_OnePage = _logic.CallContext_Parameter[DomainKey.CONFIG, KeyDics.QueryByPage.Count_per_Page] != null ? IntStd.ParseStd(_logic.CallContext_Parameter[DomainKey.CONFIG, KeyDics.QueryByPage.Count_per_Page]).Value : 10;
                }
                UnitDataCollection rtn = base.QueryByPage<T>(p, actionflag);

                _logic.SetViewData(KeyDics.QueryByPage.Count_per_Page, rtn.Count_Of_OnePage);
                _logic.SetViewData(KeyDics.QueryByPage.CurrentPage, rtn.CurrentPage);
                _logic.SetViewData(KeyDics.QueryByPage.Total_Page, rtn.TotalPage);
                _logic.SetViewData(KeyDics.QueryByPage.Total_Row, rtn.TotalRow);
                return rtn;
            }
        }
    }
}
