﻿var g = {
    get remoteurl() {
        return localStorage.getItem("remote_server_ip");
    },
    set remoteurl(value) {
        localStorage.setItem("remote_server_ip", value);
    },
    get isconfig() {
        var remoteserver = g.remoteurl;
        if (remoteserver == null || remoteserver == "") {
            return false;
        } else {
            return true;
        }
    },
    get isconfig_show() {
        if (!this.isconfig) {
            $.Frame.Message.ShowMsg("请先进行远程服务器配置！");
            return false;
        } else {
            return true;
        }
    },
    get editor_theme() {
        if (localStorage.getItem("ace_editor_theme")) {
            return localStorage.getItem("ace_editor_theme");
        } else {
            return "twilight";
        }
        
    },
    set editor_theme(value){
        localStorage.setItem("ace_editor_theme", value);
    }
}


; (function ($, fns) {
 
})(jQuery, FrameNameSpace)
