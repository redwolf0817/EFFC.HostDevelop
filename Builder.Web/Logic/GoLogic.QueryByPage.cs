using Builder.Web.Constant;
using EFFC.Frame.Net.Base.Constants;
using EFFC.Frame.Net.Base.Data;
using EFFC.Frame.Net.Data.Parameters;
using EFFC.Frame.Net.Data.UnitData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Builder.Web.Logic
{
    public abstract partial class GoLogic
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
            GoLogic _logic = null;

            public JoJoDBHelper(GoLogic logic)
                : base(logic)
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

                _logic.CallContext_DataCollection.SetValue(KeyDics.QueryByPage.Count_per_Page, rtn.Count_Of_OnePage);
                _logic.CallContext_DataCollection.SetValue(KeyDics.QueryByPage.CurrentPage, rtn.CurrentPage);
                _logic.CallContext_DataCollection.SetValue(KeyDics.QueryByPage.Total_Page, rtn.TotalPage);
                _logic.CallContext_DataCollection.SetValue(KeyDics.QueryByPage.Total_Row, rtn.TotalRow);
                return rtn;
            }
        }
    }
}
