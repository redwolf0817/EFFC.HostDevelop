; (function ($, fns) {
    fns.Message = {}

    FrameMessage = (function(){
        function FrameMessage(options) {
            var defaults = {
                Dialog: null,//function (msg) 
                ConfirmMsg: null,//function (msg) 
                ShowMsg:null//function(msg)
            }
            this.opts = $.extend(defaults, options);
        }
        FrameMessage.prototype.ShowMsg = function (msg) {
            var self = this;
            var opts = {
                msg:""
            }
            if (typeof msg === "string") {
                opts.msg = msg;
            } else {
                opts = $.extend(opts, msg);
            }
            self.opts.ShowMsg(opts);
        }
        FrameMessage.prototype.ShowConfirm = function (msg) {
            var self = this;
            var opts = {
                msg: "",
                ok: {
                    text: "",
                    click: function () {
                        $(this).dialog("close");
                    }
                },
                cancel: {
                    text: "",
                    click: function () {
                        $(this).dialog("close");
                    }
                }
            }
            if (typeof msg === "string") {
                opts.msg = msg;
            } else {
                opts = $.extend(opts, msg);
            }
            return self.opts.ConfirmMsg(opts);
        }
        FrameMessage.prototype.Dialog = function (reoptions) {
            var defaultoptions={
                msg:"",
                buttons: null//[{text:"",click:function()},{text:"",click:function()},...]
            }
            var opts = $.extend(defaultoptions, reoptions);
            var self = this;
            return self.opts.Dialog(opts);
        }
        return FrameMessage;
    })();

    //封装
    _frame_msg_methods = {
        init: function (options) {
            var op = options;
            if (options == null)
                op = fns.Config.Message;

            var message = new FrameMessage(op);
            $(this).data("message", message);
            return this;
        },
        ShowMsg: function (msg) {
            $(this).data("message").ShowMsg(msg);
        },
        ShowConfirm: function (msg) {
            return $(this).data("message").ConfirmMsg(msg);
        },
        Dialog: function (reoptions) {
            $(this).data("message").Dialog(reoptions);
        },
        DialogLogin: function (msg,tourl) {
            $(this).data("message").Dialog({
                msg: msg,
                buttons: [{
                    text: "确定",
                    click: function () {
                        if (parent) {
                            parent.location.href = tourl == null || tourl=="" ? "/Account/Login?returnurl=" + parent.location.href : tourl;
                        } else {
                            location.href = tourl == null || tourl == "" ? "/Account/Login?returnurl=" + location.href : tourl;
                        }
                    }
                }]
            });
        }
    }

    fns.Message = _frame_msg_methods.init();

    
}(jQuery,FrameNameSpace));

