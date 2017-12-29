using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.IO;
using EFFC.Frame.Net.Data.Parameters;
using EFFC.Frame.Net.Base.Common;
using EFFC.Frame.Net.Base.Constants;
using EFFC.Frame.Net.Base.Data;
using EFFC.Frame.Net.Data.WebData;
using EFFC.Frame.Net.Base.ResouceManage;
using EFFC.Frame.Net.Base.Token;
using EFFC.Frame.Net.Base.Module;
using Builder.Web.Proxy;
using EFFC.Frame.Net.Global;
using EFFC.Frame.Net.Base.Data.Base;

namespace Builder.Web.Global
{
    public class GlobalPrepare
    {
        public static void ConfigPrepare(ref WebParameter p)
        {
            p.DBConnectionString = MyConfig.GetConfiguration("dbconn");
            p[DomainKey.CONFIG, ParameterKey.NONSQL_DBCONNECT_STRING] = MyConfig.GetConfiguration("mongodb");
            bool bvalue = true;
            foreach (var item in MyConfig.GetConfigurationList())
            {
                if (bool.TryParse(ComFunc.nvl(item.Value), out bvalue))
                {
                    p[DomainKey.CONFIG, item.Key] = bool.Parse(ComFunc.nvl(item.Value));
                }
                else if (DateTimeStd.IsDateTime(item.Value))
                {
                    p[DomainKey.CONFIG, item.Key] = DateTimeStd.ParseStd(item.Value).Value;
                }
                else
                {
                    p[DomainKey.CONFIG, item.Key] = ComFunc.nvl(item.Value);
                }
            }
            //微信相关信息
            p.ExtentionObj.weixin = FrameDLRObject.CreateInstance(FrameDLRFlags.SensitiveCase);
            p.ExtentionObj.weixin.signature = ComFunc.nvl(p[DomainKey.QUERY_STRING, "signature"]);
            p.ExtentionObj.weixin.timestamp = ComFunc.nvl(p[DomainKey.QUERY_STRING, "timestamp"]);
            p.ExtentionObj.weixin.nonce = ComFunc.nvl(p[DomainKey.QUERY_STRING, "nonce"]);
            p.ExtentionObj.weixin.token = ComFunc.nvl(p[DomainKey.CONFIG, "weixin_token"]);
            p.ExtentionObj.weixin.encrypt_type = ComFunc.nvl(p[DomainKey.QUERY_STRING, "encrypt_type"]);
            p.ExtentionObj.weixin.encrypt_key = ComFunc.nvl(p[DomainKey.CONFIG, "weixin_encry_key"]);
            p.ExtentionObj.weixin.appid = ComFunc.nvl(p[DomainKey.CONFIG, "weixin_Appid"]);
            p.ExtentionObj.weixin.appsecret = ComFunc.nvl(p[DomainKey.CONFIG, "weixin_Appsecret"]);
            
        }
        static List<string> _ingore_auth_list = null;
        /// <summary>
        /// 排除不需要登录的请求
        /// </summary>
        /// <returns></returns>
        public static List<string> LoginExcept()
        {
            if (_ingore_auth_list == null)
            {
                _ingore_auth_list = new List<string>();
                _ingore_auth_list.Add("login.login.view");
                _ingore_auth_list.Add("bid_assign_center.listhall.view");
                _ingore_auth_list.Add("bid_assign_center.homepart.view");
                _ingore_auth_list.Add("eventfeb_1.*.view");
                _ingore_auth_list.Add("eventfeb_1_friends.*.view");
                _ingore_auth_list.Add("event_package.*.view");
                _ingore_auth_list.Add("fund.*.view");
                _ingore_auth_list.Add("trust.*.view");
                _ingore_auth_list.Add("insurance.*.view");
                _ingore_auth_list.Add("financial.*.view");
                _ingore_auth_list.Add("loginjump.*.view");
            }
            return _ingore_auth_list;
        }
        static Dictionary<string, string> _map_url = null;
        /// <summary>
        /// 跳转映射
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string RedirectMap(string url)
        {
            if (_map_url == null)
            {
                _map_url = new Dictionary<string, string>();
                _map_url.Add("AutoBID_center.*.view".ToLower(), "/Center/AutoBID");
                _map_url.Add("bid_assign_center.assignedrecords.view".ToLower(), "/Center/MyLend");
                _map_url.Add("bid_assign_center.undertabkerecords.view".ToLower(), "/Center/MyLend");
            }
            var index= url.IndexOf('?');
            var la = Path.GetFileName(url.Substring(0, index > 0 ? index : url.Length)).ToLower();
            var lainfo = la.Split('.');
            var l = lainfo[0].ToLower();
            var a = lainfo.Length > 2 ? lainfo[1].ToLower() : "";
            var type = lainfo.Length > 2 ? lainfo[2].ToLower(): lainfo[1].ToLower();
            var query = index > 0 ? url.Substring(url.IndexOf("?")) : "";
            var rtn = "" + query;
            if (_map_url.ContainsKey(la))
            {
                rtn = _map_url[la] + (query) + rtn;
            }
            else if (_map_url.ContainsKey(l + ".*." + type))
            {
                rtn = _map_url[l + ".*." + type] + rtn;
            }
            else if (_map_url.ContainsKey(l + ".*.*"))
            {
                rtn = _map_url[l + ".*.*"] + rtn;
            }
            else
            {
                rtn = url;
            }
            return rtn;
        }
    }
    
}