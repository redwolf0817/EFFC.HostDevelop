﻿using EFFC.Frame.Net.Base.Common;
using EFFC.Frame.Net.Base.Constants;
using EFFC.Frame.Net.Base.ResouceManage.FTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Builder.Web.Logic
{
    public abstract partial class GoLogic
    {
        FileServer _t = null;
        public FileServer Files
        {
            get
            {
                if (_t == null) _t = new FileServer(this);
                return _t;
            }


        }
        public class FileServer
        {
            GoLogic _logic;

            public FileServer(GoLogic logic)
            {
                _logic = logic;
            }

            public void SetDownLoadFileName(string filename)
            {
                _logic.CallContext_DataCollection["__download_filename__"] = filename;
            }
            /// <summary>
            /// 下載文件
            /// </summary>
            /// <param name="filepath"></param>
            /// <param name="newfilename"></param>
            /// <param name="datatype"></param>
            /// <returns></returns>
            public MemoryStream DownLoadFromFileServer(string filepath, string newfilename, GoResponseDataType datatype)
            {
                var ftp = _logic.DB.NewResourceEntity<FTPAccess>();
                FtpParameter fp = new FtpParameter();
                string ftpurl = "ftp://" + _logic.Configs["FtpServer"] + ":" + _logic.Configs["FtpPort"] + "/" + _logic.Configs["FtpFile"] + filepath;
                fp.FTP_URL = ftpurl;
                fp.Login_UserId = MyConfig.GetConfigurationByEncrypt("FtpLoginID", MyConfig.EncryptionType.AES256);
                fp.Login_Password = MyConfig.GetConfigurationByEncrypt("FtpPass", MyConfig.EncryptionType.AES256);
                fp.UseBinary = true;
                fp.UsePassive = (bool)_logic.Configs["FtpUsePassive"];
                fp.KeepAlive = (bool)_logic.Configs["FtpKeepAlive"];
                fp.TimeOut = (int)_logic.Configs["FtpTimeout"];

                _logic.SetContentType(datatype);
                SetDownLoadFileName(newfilename);
                return ftp.DownLoadStream(fp);
            }
        }
    }
}
