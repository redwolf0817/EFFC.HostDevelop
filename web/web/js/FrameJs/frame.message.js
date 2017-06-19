; (function ($, fns) {
    fns.Message = {}
    //将方法加入到子线程，进入event loop
    var asap = function (fn) {
        setTimeout(fn, 0);
    };
    FrameMessage = (function(){
        function FrameMessage(options) {
            var defaults = {
                Dialog: null,//function (msg) 
                ConfirmMsg: null,//function (msg) 
                ShowMsg: null,//function(msg)
                Close:null//function()
            }
            this.opts = fns.extend(defaults, options);
        }
        FrameMessage.prototype.ShowMsg = function (msg) {
            var self = this;
            var opts = {
                msg:""
            }
            if (typeof msg === "string") {
                opts.msg = msg;
            } else {
                opts = fns.extend(opts, msg);
            }
            self.opts.ShowMsg(opts);
        }
        FrameMessage.prototype.ShowConfirm = function (msg) {
            var self = this;
            var opts = {
                msg: "",
                ok: {
                    text: "确定",
                    click: function (target) {
                        self.Close();
                    }
                },
                cancel: {
                    text: "取消",
                    click: function (target) {
                        self.Close();
                    }
                }
            }
            if (typeof msg === "string") {
                opts.msg = msg;
            } else {
                opts = fns.extend(opts, msg);
            }

            self.ok = function (fn) {
                if (fn) {
                    opts.ok.click = fn;
                }
                return this;
            }
            self.cancel = function (fn) {
                if (fn) {
                    opts.cancel.click = fn;
                }
                return this;
            }

            asap(function () { 
                self.opts.ConfirmMsg(opts);
            })

            return self;
        }


        FrameMessage.prototype.Dialog = function (reoptions) {
            var self = this;
            var opts = {
                msg:"",
                buttons: []//[{text:"",click:function()},{text:"",click:function()},...]
            }
            if (typeof reoptions === "string") {
                opts.msg = reoptions;
            } else {
                opts = fns.extend(opts, reoptions);
            }

            self.addButton = function (option) {
                if (!option)
                    return this;

                if (typeof option === "function")
                    return this;

                opts.buttons.push(option);
                return this;
            }

            asap(function () {
                self.opts.Dialog(opts);
            })
            
            return self;
        }
        FrameMessage.prototype.Close = function () {
            var self = this;
            return self.opts.Close();
        }
        return FrameMessage;
    })();

    //封装
    //_frame_msg_methods = {
    //    init: function (options) {
    //        var op = options;
    //        if (options == null)
    //            op = fns.Config.Message;

    //        var message = new FrameMessage(op);
    //        $(this).data("message", message);
    //        return this;
    //    },
    //    ShowMsg: function (msg) {
    //        $(this).data("message").ShowMsg(msg);
    //    },
    //    ShowConfirm: function (msg) {
    //        return $(this).data("message").ShowConfirm(msg);
    //    },
    //    Dialog: function (reoptions) {
    //        $(this).data("message").Dialog(reoptions);
    //    },
    //    Close: function () {
    //        $(this).data("message").Close();
    //    }
    //}

    fns.Message = new FrameMessage(fns.Config.Message);

    
}(jQuery, FrameNameSpace));

