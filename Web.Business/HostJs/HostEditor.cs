using Builder.Web.Logic;
using EFFC.Frame.Net.Base.Common;
using EFFC.Frame.Net.Base.Constants;
using EFFC.Frame.Net.Base.Data.Base;
using EFFC.Frame.Net.Base.ResouceManage.Files;
using EFFC.Frame.Net.Business.Logic;
using EFFC.Frame.Net.Data.LogicData;
using EFFC.Frame.Net.Global;
using EFFC.Frame.Net.Web.ViewEngine;
using Noesis.Javascript;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Business.HostJs
{
    public class HostEditor : GoLogic
    {
        protected override Func<LogicData, object> GetFunction(string actionName)
        {
            switch (actionName.ToLower())
            {
                case "compileview":
                    return DoCompileView;
                case "compilelogic":
                    return DoCompileLogic;
                case "allfiles":
                    return AllFiles;
                case "alllogicfiles":
                    return AllLogicFiles;
                case "save":
                    return DoSave;
                case "getcode":
                    return GetCode;
                case "help":
                    return GetHelper;
                case "del":
                    return DoDel;
                case "compileall":
                    return DoCompileAll;
                case "publish":
                    return DoPublish;
                case "allproject":
                    return AllProject;
                case "saveproject":
                    return SaveProject;
                case "delproject":
                    return DelProject;
                default:
                    return null;
            }
        }

        private object DelProject(LogicData arg)
        {
            var rtn = FrameDLRObject.CreateInstance(@"
{
issuccess:true,
msg:''
}
");
            var projectname = ComFunc.nvl(arg["projectname"]);
            var path = GlobalCommon.HostCommon.RootPath + "/Project/";
            if (Directory.Exists(path))
            {
                if (Directory.Exists(path + projectname))
                {
                    Directory.Delete(path + projectname,true);
                    rtn.msg = "删除成功";
                }
                else
                {
                    rtn.issuccess = false;
                    rtn.msg = "该工程不存在";
                }
            }
           
            return rtn;
        }

        private object SaveProject(LogicData arg)
        {
            var rtn = FrameDLRObject.CreateInstance(@"
{
issuccess:true,
msg:''
}
");
            var projectname = ComFunc.nvl(arg["projectname"]);
            if (projectname == "")
            {
                rtn.issuccess = false;
                rtn.msg = "请输入项目名称";
                return rtn;
            }

            var path = GlobalCommon.HostCommon.RootPath + "/Project/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (Directory.Exists(path + projectname))
            {
                rtn.issuccess = false;
                rtn.msg = "该工程已经存在";
            }
            else
            {
                Directory.CreateDirectory(path + projectname);
                rtn.msg = "保存成功";
            }
            return rtn;
        }



        private object AllProject(LogicData arg)
        {
            var rtn = FrameDLRObject.CreateInstance();
            var path = GlobalCommon.HostCommon.RootPath + "/Project/";
            if (Directory.Exists(path))
            {
                var directs = Directory.GetDirectories(path);
                var dlist = new List<string>();
                foreach (var f in directs)
                {
                    dlist.Add(f.Substring(f.LastIndexOf("/")+1));

                }
                rtn.directories = dlist.ToArray();
            }
            return rtn;
        }

        private object DoCompileAll(LogicData arg)
        {
            var rtn = FrameDLRObject.CreateInstance(@"{
issuccess:true,
msg:''
}");
            var projectname = ComFunc.nvl(arg["projectname"]);
            var path = GlobalCommon.HostCommon.RootPath + "/Project/" + projectname + "/Logics/";
            if (IsWebSocket)
            {
                WS.Send("开始执行Logic编译");
            }
            if(Directory.Exists(path))
            {
                var fs = Directory.GetFiles(path, "*.js", SearchOption.AllDirectories);
                var hle = new HostLogicEngine(this);
                hle.CurrentContext.RootPath = GlobalCommon.HostCommon.RootPath;
                hle.CurrentContext.RunTimeLibPath = GlobalCommon.HostCommon.RootPath + "/Project/" + projectname + "/" + HostJsConstants.COMPILED_ROOT_PATH;
                hle.CurrentContext.CommonLibPath = GlobalCommon.HostCommon.RootPath;
                if (IsWebSocket)
                {
                    WS.Send("删除所有已编译的Logic文件");
                }
                hle.DeleteAllCompiledFile();
                if (IsWebSocket)
                {
                    WS.Send("开始执行编译操作，共计" + fs.Length + "份文件");
                }
                foreach (var f in fs)
                {
                    var text = File.ReadAllText(f, Encoding.UTF8);
                    hle.Compile(Path.GetFileNameWithoutExtension(f), text, true);
                    if (IsWebSocket)
                    {
                        WS.Send(string.Format("编译{0}文件完成", Path.GetFileNameWithoutExtension(f)));
                    }
                }
            }
            if (IsWebSocket)
            {
                WS.Send("结束Logic编译");
            }

            path = GlobalCommon.HostCommon.RootPath + "/Project/" + projectname + "/Views/";
            if (IsWebSocket)
            {
                WS.Send("开始执行View编译");
            }
            if (Directory.Exists(path))
            {
                var fs = Directory.GetFiles(path, "*.html", SearchOption.AllDirectories);
                var hve = new HostJsView();
                hve.CurrentContext.RootPath = GlobalCommon.HostCommon.RootPath;
                hve.CurrentContext.RunTimeLibPath = GlobalCommon.HostCommon.RootPath + "/Project/" + projectname + "/" + HostJsConstants.COMPILED_ROOT_PATH;
                hve.CurrentContext.CommonLibPath = GlobalCommon.HostCommon.RootPath;
                if (IsWebSocket)
                {
                    WS.Send("删除所有已编译的View文件");
                }
                hve.DeleteAllCompiledFile();
                if (IsWebSocket)
                {
                    WS.Send("开始执行编译操作，共计" + fs.Length + "份文件");
                }
                foreach (var f in fs)
                {
                    var text = File.ReadAllText(f, Encoding.UTF8);
                    hve.Compile(Path.GetFileNameWithoutExtension(f), text, true);
                    if (IsWebSocket)
                    {
                        WS.Send(string.Format("编译{0}文件完成", Path.GetFileNameWithoutExtension(f)));
                    }
                }
            }
            if (IsWebSocket)
            {
                WS.Send("结束View编译");
            }

            return rtn;
        }

        private object DoPublish(LogicData arg)
        {
            var rtn = DoCompileAll(arg);
            var projectname = ComFunc.nvl(arg["projectname"]);
            var path = GlobalCommon.HostCommon.RootPath + "/Project/" + projectname + "/Publish/";
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                Directory.CreateDirectory(path);
            }
            var compiledpath = GlobalCommon.HostCommon.RootPath + "/Project/" + projectname + "/" + HostJsConstants.COMPILED_ROOT_PATH;
            ComFunc.CopyDirectory(compiledpath, path + HostJsConstants.COMPILED_ROOT_PATH,".hjs");
            //拷贝公共js文件
            var fs = Directory.GetFiles(GlobalCommon.HostCommon.RootPath);
            foreach (var f in fs)
            {
                if (Path.GetExtension(f) == ".js")
                {
                    File.Copy(f, path + Path.GetFileName(f));
                }
            }
            var zip = CreateResource<CompressFile>();
            zip.ZipParameter = new ZipParameter();
            zip.ZipParameter.Zip_Name = GlobalCommon.HostCommon.RootPath + "/Project/" + projectname + "/" + "publish.zip";
            zip.ZipParameter.DirectoryName = path;
            var s = zip.CompressReturnStream();
            s.Close();
            var zipstream = new FileStream(zip.ZipParameter.Zip_Name, FileMode.Open);
            var ms = new MemoryStream();
            zipstream.CopyTo(ms);
            zipstream.Flush();
            zipstream.Close();
            WS.Send(ms);
            return rtn;
        }

        private object DoDel(LogicData arg)
        {
            var rtn = FrameDLRObject.CreateInstance(@"{
issuccess:true,
msg:''
}");

            string filename = ComFunc.nvl(arg["filename"]);
            string jstype = ComFunc.nvl(arg["jstype"]);
            var projectname = ComFunc.nvl(arg["projectname"]);
            if (filename == "")
            {
                rtn.msg = "请输入文件名称";
                rtn.issuccess = false;
                return rtn;
            }
            if (projectname == "")
            {
                rtn.msg = "请选择工程";
                rtn.issuccess = false;
                return rtn;
            }

            var filepath = "";
            var path = "";
            if (jstype == "view")
            {
                path = GlobalCommon.HostCommon.RootPath + "/Project/" + projectname + "/Views/";
                filepath = path + filename + ".html";
            }
            else
            {
                path = GlobalCommon.HostCommon.RootPath + "/Project/" + projectname + "/Logics/";
                filepath = path + filename + ".js";
            }
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            else
            {
                rtn.msg = "文件不存在";
                rtn.issuccess = false;
            }

            return rtn;
        }

        private object GetHelper(LogicData arg)
        {
            var jstype = ComFunc.nvl(arg["jstype"]);
            var type = ComFunc.nvl(arg["type"]);
            if (jstype == "view")
            {
                var hve = new HostJsView();
                if (type == "serverobj")
                {
                    return hve.ServerReserverObjectKey;
                }
                else
                {
                    return hve.ReserverTags;
                }
            }
            else
            {
                var hle = FrameHostJsLogic.GetCurrentLogicEngine(this);
                if (type == "serverobj")
                {
                    return hle.ServerReserverObjectKey;
                }
                else
                {
                    return hle.ReserverTags;
                }
            }
        }

        private object GetCode(LogicData arg)
        {
            var rtn = FrameDLRObject.CreateInstance(@"{
issuccess:true,
msg:''
}");
            var file = ComFunc.nvl(arg["file"]);
            var jstype = ComFunc.nvl(arg["jstype"]);
            var projectname = ComFunc.nvl(arg["projectname"]);
            var path = "";
            if (jstype == "view")
            {
                path = GlobalCommon.HostCommon.RootPath + "/Project/" + projectname + "/Views/" + file;
            }
            else
            {
                path = GlobalCommon.HostCommon.RootPath + "/Project/" + projectname + "/Logics/" + file;
            }
            if (File.Exists(path))
            {
                var content = File.ReadAllText(path, Encoding.UTF8);
                rtn.content = ComFunc.UrlEncode(content);
            }
            else
            {
                rtn.issuccess = false;
                rtn.msg = "文件不存在";
            }

            return rtn;
        }

        private object DoSave(LogicData arg)
        {
            var rtn = FrameDLRObject.CreateInstance(@"{
issuccess:true,
msg:''
}");
            string filename = ComFunc.nvl(arg["filename"]);
            string code = ComFunc.nvl(arg["code"]);
            string jstype = ComFunc.nvl(arg["jstype"]);
            var projectname = ComFunc.nvl(arg["projectname"]);
            if (filename == "")
            {
                rtn.msg = "请输入文件名称";
                rtn.issuccess = false;
                return rtn;
            } 
            if (projectname == "")
            {
                rtn.msg = "请选择工程";
                rtn.issuccess = false;
                return rtn;
            }
            code = ComFunc.UrlDecode(code);
            var filepath = "";
            var path = "";
            if (jstype == "view")
            {
                path = GlobalCommon.HostCommon.RootPath + "/Project/" + projectname + "/Views/";
                filepath = path + filename + ".html";
            }
            else
            {
                path = GlobalCommon.HostCommon.RootPath + "/Project/" + projectname + "/Logics/";
                filepath = path + filename + ".js";
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.WriteAllText(filepath, code, Encoding.UTF8);

            rtn.issuccess = true;


            return rtn;

        }
        private object DoCompileLogic(LogicData arg)
        {
            var rtn = FrameDLRObject.CreateInstance(@"{
issuccess:true,
msg:''
}");
            var file = ComFunc.nvl(arg["file"]);
            var projectname = ComFunc.nvl(arg["projectname"]);
            var path = GlobalCommon.HostCommon.RootPath + "/Project/" + projectname + "/Logics/" + file;
            if (File.Exists(path))
            {
                var content = File.ReadAllText(path, Encoding.UTF8);
                var hle = new HostLogicEngine(this);
                hle.CurrentContext.AddHostJsObject(new OuterInterfaceObject(this));
                hle.CurrentContext.AddHostJsObject(new WeixinObject(this));
                hle.CurrentContext.RootPath = GlobalCommon.HostCommon.RootPath;
                hle.CurrentContext.CommonLibPath = GlobalCommon.HostCommon.RootPath;
                hle.CurrentContext.RunTimeLibPath = GlobalCommon.HostCommon.RootPath + "/Project/" + projectname + "/" + HostJsConstants.COMPILED_ROOT_PATH;
                var filename = Path.GetFileNameWithoutExtension(path);
                var d = hle.Compile(filename, content, true);

                StringBuilder sbstr = new StringBuilder();
                sbstr.AppendLine("执行代码......");
                bool issuccess = true;
                foreach (var item in d)
                {
                    sbstr.AppendLine("执行代码:"+item.Key);
                    try
                    {
                        //每个js文件都是独立的上下文结构
                        var context = new HostLogicContext();
                        context.RootPath = GlobalCommon.HostCommon.RootPath;
                        context.CommonLibPath = GlobalCommon.HostCommon.RootPath;
                        context.RunTimeLibPath = GlobalCommon.HostCommon.RootPath + "/Project/" + projectname + "/" + HostJsConstants.COMPILED_ROOT_PATH;
                        HostLogicEngine.InitContext(context, this);
                        context.AddHostJsObject(new OuterInterfaceObject(this));
                        context.AddHostJsObject(new WeixinObject(this));
                        
                        //执行的根目录环境
                        var obj = hle.RunJs(item.Value, context);
                        
                        sbstr.AppendLine("执行成功！");
                    }
                    catch (JavascriptException ex)
                    {
                        issuccess = false;
                        sbstr.AppendLine("执行代码:" + item.Key + "失败！");
                        sbstr.AppendLine("错误信息:" +ex.Message);
                        sbstr.AppendLine("出错代码行数:" + ex.Line);
                        sbstr.AppendLine("出错代码列数:" + ex.StartColumn + "~" + ex.EndColumn);
                        sbstr.AppendLine("出错代码位置:" + ex.V8SourceLine);
                        string[] sarr = item.Value.Split('\n');
                        if (ex.Line < sarr.Length)
                        {
                            sbstr.AppendLine("出错源代码:" + sarr[ex.Line]);
                        }
                    }
                    sbstr.AppendLine("执行代码:" + item.Key+"完成！");
                }
                sbstr.AppendLine("执行代码完成");
                sbstr.AppendLine("控制台输出信息:" + hle.GetConsoleMsg());
                rtn.issuccess = issuccess;
                rtn.msg = sbstr.ToString().Replace("\"",@"'");
            }
            else
            {
                rtn.issuccess = false;
                rtn.msg = "文件不存在";
            }
            //防止脚本在运行时对本logic的contenttype做修改
            SetContentType(GoResponseDataType.Json);
            return rtn;
        }

        private object AllLogicFiles(LogicData arg)
        {
            var rtn = FrameDLRObject.CreateInstance();
            var projectname = ComFunc.nvl(arg["projectname"]);
            var path = GlobalCommon.HostCommon.RootPath + "/Project/" + projectname + "/Logics/";
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);
                var filelist = new List<string>();
                foreach (var f in files)
                {
                    filelist.Add(Path.GetFileName(f));

                }
                rtn.files = filelist.ToArray();
            }
            return rtn;
        }

        private object AllFiles(LogicData arg)
        {
            var rtn = FrameDLRObject.CreateInstance();
            var projectname = ComFunc.nvl(arg["projectname"]);
            var path = GlobalCommon.HostCommon.RootPath + "/Project/" + projectname + "/Views/";
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);
                var filelist = new List<string>();
                foreach (var f in files)
                {
                    filelist.Add(Path.GetFileName(f));

                }
                rtn.files = filelist.ToArray();
            }
            return rtn;
        }

        private object DoCompileView(LogicData arg)
        {
            var rtn = FrameDLRObject.CreateInstance(@"{
issuccess:true,
msg:''
}");
            var file = ComFunc.nvl(arg["file"]);
            var projectname = ComFunc.nvl(arg["projectname"]);
            var path = GlobalCommon.HostCommon.RootPath + "/Project/" + projectname + "/Views/" + file;
            if (File.Exists(path))
            {
                var content = File.ReadAllText(path, Encoding.UTF8);
                var hve = new HostJsView();
                var filename = Path.GetFileNameWithoutExtension(path);
                hve.CurrentContext.RootPath = GlobalCommon.HostCommon.RootPath;
                hve.CurrentContext.CommonLibPath = GlobalCommon.HostCommon.RootPath;
                hve.CurrentContext.RunTimeLibPath = GlobalCommon.HostCommon.RootPath + "/Project/" + projectname + "/" + HostJsConstants.COMPILED_ROOT_PATH;
                var js = hve.Compile(filename, content, true);
            }
            else
            {
                rtn.issuccess = false;
                rtn.msg = "文件不存在";
            }

            return rtn;
        }

        public override string Name
        {
            get { return "hosteditor"; }
        }
    }
}
