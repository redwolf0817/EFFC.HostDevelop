using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFFC.Frame.Net.Base.Common;
using EFFC.Frame.Net.Base.Constants;
using EFFC.Frame.Net.Base.Data;
using EFFC.Frame.Net.Base.Data.Base;

namespace Builder.Web.Logic
{
    public abstract partial class GoLogic
    {
        WeixinHelper _weixin = null;
        public WeixinHelper Weixin
        {
            get
            {
                if (_weixin == null) _weixin = new WeixinHelper(this);
                return _weixin;
            }


        }
        public class WeixinHelper
        {
            GoLogic _logic;

            public WeixinHelper(GoLogic logic)
            {
                _logic = logic;
            }
            /// <summary>
            /// 微信的APPID
            /// </summary>
            public string AppID
            {
                get
                {
                    return ComFunc.nvl(_logic.CallContext_Parameter.ExtentionObj.weixin.appid);
                }
            }
            /// <summary>
            /// 微信的APPSecret
            /// </summary>
            public string AppSecret
            {
                get
                {
                    return ComFunc.nvl(_logic.CallContext_Parameter.ExtentionObj.weixin.appsecret);
                }
            }
            /// <summary>
            /// 请求的FromUserName
            /// </summary>
            public string FromUserName
            {
                get
                {
                    return ComFunc.nvl(_logic.CallContext_Parameter[DomainKey.POST_DATA, "FromUserName"]);
                }
            }
            /// <summary>
            /// 请求的ToUserName
            /// </summary>
            public string ToUserName
            {
                get
                {
                    return ComFunc.nvl(_logic.CallContext_Parameter[DomainKey.POST_DATA, "ToUserName"]);
                }
            }
            /// <summary>
            /// 请求的CreateTime
            /// </summary>
            public DateTime CreateTime
            {
                get
                {
                    return (DateTime)_logic.CallContext_Parameter[DomainKey.POST_DATA, "CreateTime"];
                }
            }
            /// <summary>
            /// 请求的MsgType
            /// </summary>
            public string MsgType
            {
                get
                {
                    return ComFunc.nvl(_logic.CallContext_Parameter[DomainKey.POST_DATA, "MsgType"]);
                }
            }

            /// <summary>
            /// 请求的MsgType
            /// </summary>
            public string Content
            {
                get
                {
                    return ComFunc.nvl(_logic.CallContext_Parameter[DomainKey.POST_DATA, "Content"]);
                }
            }

            /// <summary>
            /// 请求的MsgType
            /// </summary>
            public string MsgId
            {
                get
                {
                    return ComFunc.nvl(_logic.CallContext_Parameter[DomainKey.POST_DATA, "MsgId"]);
                }
            }
            /// <summary>
            /// 请求的PicUrl
            /// </summary>
            public string PicUrl
            {
                get
                {
                    return ComFunc.nvl(_logic.CallContext_Parameter[DomainKey.POST_DATA, "PicUrl"]);
                }
            }
            /// <summary>
            /// 请求的MediaId
            /// </summary>
            public string MediaId
            {
                get
                {
                    return ComFunc.nvl(_logic.CallContext_Parameter[DomainKey.POST_DATA, "MediaId"]);
                }
            }
            /// <summary>
            /// 请求的语音Format，语音格式，如amr，speex等
            /// </summary>
            public string Format
            {
                get
                {
                    return ComFunc.nvl(_logic.CallContext_Parameter[DomainKey.POST_DATA, "Format"]);
                }
            }
            /// <summary>
            /// 请求的语音识别结果，使用UTF8编码
            /// </summary>
            public string Recognition
            {
                get
                {
                    return ComFunc.nvl(_logic.CallContext_Parameter[DomainKey.POST_DATA, "Recognition"]);
                }
            }
            /// <summary>
            /// 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据。
            /// </summary>
            public string ThumbMediaId
            {
                get
                {
                    return ComFunc.nvl(_logic.CallContext_Parameter[DomainKey.POST_DATA, "ThumbMediaId"]);
                }
            }
            /// <summary>
            /// 地理位置维度
            /// </summary>
            public double Location_X
            {
                get
                {
                    return DoubleStd.ParseStd(_logic.CallContext_Parameter[DomainKey.POST_DATA, "Location_X"]).Value;
                }
            }
            /// <summary>
            /// 地理位置经度
            /// </summary>
            public double Location_Y
            {
                get
                {
                    return DoubleStd.ParseStd(_logic.CallContext_Parameter[DomainKey.POST_DATA, "Location_Y"]).Value;
                }
            }
            /// <summary>
            /// 地图缩放大小
            /// </summary>
            public double Scale
            {
                get
                {
                    return DoubleStd.ParseStd(_logic.CallContext_Parameter[DomainKey.POST_DATA, "Scale"]).Value;
                }
            }
            /// <summary>
            /// 地理位置信息
            /// </summary>
            public string Label
            {
                get
                {
                    return ComFunc.nvl(_logic.CallContext_Parameter[DomainKey.POST_DATA, "Label"]);
                }
            }
            /// <summary>
            /// 消息标题
            /// </summary>
            public string MessageTitle
            {
                get
                {
                    return ComFunc.nvl(_logic.CallContext_Parameter[DomainKey.POST_DATA, "Title"]);
                }
            }
            /// <summary>
            /// 消息描述
            /// </summary>
            public string MessageDescription
            {
                get
                {
                    return ComFunc.nvl(_logic.CallContext_Parameter[DomainKey.POST_DATA, "Description"]);
                }
            }
            /// <summary>
            /// 消息链接
            /// </summary>
            public string MessageUrl
            {
                get
                {
                    return ComFunc.nvl(_logic.CallContext_Parameter[DomainKey.POST_DATA, "Url"]);
                }
            }
            /// <summary>
            /// 事件类型，
            /// subscribe(订阅)、unsubscribe(取消订阅)、SCAN（取消关注）、LOCATION（上报地理位置）、CLICK（点击菜单）
            /// </summary>
            public string Event
            {
                get
                {
                    return ComFunc.nvl(_logic.CallContext_Parameter[DomainKey.POST_DATA, "Event"]);
                }
            }
            /// <summary>
            /// 事件KEY值，qrscene_为前缀，后面为二维码的参数值
            /// </summary>
            public string EventKey
            {
                get
                {
                    return ComFunc.nvl(_logic.CallContext_Parameter[DomainKey.POST_DATA, "EventKey"]);
                }
            }
            /// <summary>
            /// 二维码的ticket，可用来换取二维码图片
            /// </summary>
            public string Ticket
            {
                get
                {
                    return ComFunc.nvl(_logic.CallContext_Parameter[DomainKey.POST_DATA, "Ticket"]);
                }
            }

            /// <summary>
            /// 上报事件中的地理位置纬度
            /// </summary>
            public double Latitude
            {
                get
                {
                    return DoubleStd.ParseStd(_logic.CallContext_Parameter[DomainKey.POST_DATA, "Latitude"]).Value;
                }
            }
            /// <summary>
            /// 上报事件中的地理位置经度
            /// </summary>
            public double Longitude
            {
                get
                {
                    return DoubleStd.ParseStd(_logic.CallContext_Parameter[DomainKey.POST_DATA, "Longitude"]).Value;
                }
            }
            /// <summary>
            /// 上报事件中的地理位置精度
            /// </summary>
            public double Precision
            {
                get
                {
                    return DoubleStd.ParseStd(_logic.CallContext_Parameter[DomainKey.POST_DATA, "Precision"]).Value;
                }
            }
            /// <summary>
            /// 获取微信的Access Token，用于与微信服务器进行信息交互
            /// </summary>
            public string Access_Token
            {
                get
                {
                    if (_logic.CacheHelper.GetCache("weixin_access_token") == null)
                    {
                        var result = _logic.OuterInterface.CallWeixinServer(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", AppID, AppSecret));
                        dynamic dobj = (FrameDLRObject)result;
                        var token = ComFunc.nvl(dobj.access_token);
                        var expireseconds = ComFunc.nvl(dobj.expires_in);
                        if (token != "")
                        {
                            //获取之后将超时时间缩短10秒，微信默认超时时间为7200秒，每获取一次就会重置该token
                            _logic.CacheHelper.SetCache("weixin_access_token", token, DateTime.Now.AddSeconds(IntStd.ParseStd(expireseconds).Value - 10));
                        }
                    }

                    return ComFunc.nvl(_logic.CacheHelper.GetCache("weixin_access_token"));
                }
            }
            private dynamic GenResponseObject()
            {
                var rtn = FrameDLRObject.CreateInstance(FrameDLRFlags.SensitiveCase);
                rtn.ToUserName = FromUserName;
                rtn.FromUserName = ToUserName;
                rtn.CreateTime = DateTime.Now;
                return rtn;
            }
            /// <summary>
            /// 生成一个Text类型的回复数据
            /// </summary>
            /// <param name="content"></param>
            /// <returns></returns>
            public FrameDLRObject GenResponseText(string content)
            {
                var rtn = GenResponseObject();
                rtn.MsgType = "text";
                rtn.Content = content;
                return rtn;
            }
            /// <summary>
            /// 生成一个image类型的回复数据
            /// </summary>
            /// <param name="mediaid">通过素材管理接口上传多媒体文件，得到的id。</param>
            /// <returns></returns>
            public FrameDLRObject GenResponseImage(string mediaid)
            {
                var rtn = GenResponseObject();
                rtn.MsgType = "image";
                rtn.Image = FrameDLRObject.CreateInstance(FrameDLRFlags.SensitiveCase);
                rtn.Image.MediaId = mediaid;
                return rtn;
            }
            /// <summary>
            /// 生成一个image类型的回复数据
            /// </summary>
            /// <param name="mediaid">通过素材管理接口上传多媒体文件，得到的id。</param>
            /// <returns></returns>
            public FrameDLRObject GenResponseVoice(string mediaid)
            {
                var rtn = GenResponseObject();
                rtn.MsgType = "voice";
                rtn.Voice = FrameDLRObject.CreateInstance(FrameDLRFlags.SensitiveCase);
                rtn.Voice.MediaId = mediaid;
                return rtn;
            }
            /// <summary>
            /// 生成一个video类型的回复数据
            /// </summary>
            /// <param name="mediaid">通过素材管理接口上传多媒体文件，得到的id。</param>
            /// <param name="title">视频消息的标题</param>
            /// <param name="description">视频消息的描述</param>
            /// <returns></returns>
            public FrameDLRObject GenResponseVideo(string mediaid, string title, string description)
            {
                var rtn = GenResponseObject();
                rtn.MsgType = "video";
                rtn.Video = FrameDLRObject.CreateInstance(FrameDLRFlags.SensitiveCase);
                rtn.Video.MediaId = mediaid;
                rtn.Video.Title = title;
                rtn.Video.Description = description;
                return rtn;
            }
            /// <summary>
            /// 生成一个音乐消息类型的回复数据
            /// </summary>
            /// <param name="mediaid">缩略图的媒体id，通过素材管理接口上传多媒体文件，得到的id</param>
            /// <param name="title">音乐标题</param>
            /// <param name="description">音乐描述</param>
            /// <param name="MusicUrl">音乐链接</param>
            /// <param name="HQMusicUrl">高质量音乐链接，WIFI环境优先使用该链接播放音乐</param>
            /// <returns></returns>
            public FrameDLRObject GenResponseMusic(string mediaid, string title, string description, string MusicUrl, string HQMusicUrl)
            {
                var rtn = GenResponseObject();
                rtn.MsgType = "music";
                rtn.Music = FrameDLRObject.CreateInstance(FrameDLRFlags.SensitiveCase);
                rtn.Music.Title = title;
                rtn.Music.Description = description;
                rtn.Music.MusicUrl = MusicUrl;
                rtn.Music.HQMusicUrl = HQMusicUrl;
                rtn.Music.ThumbMediaId = mediaid;
                return rtn;
            }
            /// <summary>
            /// 生成一个图文消息类型的回复数据
            /// </summary>
            /// <param name="items">数组，动态对象为敏感大小写
            /// 构成如下：
            /// Title：图文消息标题
            /// Description：图文消息描述
            /// PicUrl：图片链接，支持JPG、PNG格式，较好的效果为大图360*200，小图200*200
            /// Url：点击图文消息跳转链接
            /// </param>
            /// <returns></returns>
            public FrameDLRObject GenResponseNews(params FrameDLRObject[] items)
            {
                var rtn = GenResponseObject();
                rtn.MsgType = "news";
                rtn.ArticleCount = items.Length;
                rtn.Articles = FrameDLRObject.CreateInstance(FrameDLRFlags.SensitiveCase);
                rtn.Articles.item = items;
                return rtn;
            }
        }
    }
}
