; (function ($,fns) {
    fns.ExceptionProcess = {
        Process : function (error, sourcename) {
            if (error != null) {
                fns.Message.Dialog({
                    msg: "An error occured in \"" + sourcename + "\"!\n" + error.message
                });
            }
        },
        ShowErrorMsg: function (msg) {
            if (msg != null) {
                fns.Message.Dialog({
                    msg: "This is a system error：\n" + msg
                });
            }
        }
    };
}(jQuery,FrameNameSpace));

