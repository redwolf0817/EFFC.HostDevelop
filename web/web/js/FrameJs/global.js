

; (function ($) {
    $.Frame = function () {   
    }
    $.Frame.Config = {};
    FrameNameSpace = $.Frame;
    if (typeof js == "undefined") {
        document.write("<script language=javascript src='/js/FrameJs/js.Util/js.lang.Class.js'></script>");
        document.write("<script language=javascript src='/js/FrameJs/js.Util/js.events.EventDispatcher.js'></script>");
        document.write("<script language=javascript src='/js/FrameJs/js.Util/js.util.ArrayList.js'></script>");
        document.write("<script language=javascript src='/js/FrameJs/js.Util/js.util.Dictionary.js'></script>");
    }

    if (typeof $.fn.ajaxSubmit == "undefined") {
        document.write("<script language=javascript src='/js/FrameJs/jquery.form.js'></script>");
    }

    if (typeof FrameNameSpace.ExceptionProcess == "undefined") {
        document.write("<script language=javascript src='/js/FrameJs/frame.exception.js'></script>");
    }
    if (typeof FrameNameSpace.Control == "undefined") {
        document.write("<script language=javascript src='/js/FrameJs/frame.control.js'></script>");
    }
    if (typeof FrameNameSpace.Validation == "undefined") {
        document.write("<script language=javascript src='/js/FrameJs/configs/validation.config.js'></script>");
        document.write("<script language=javascript src='/js/FrameJs/frame.validation.js'></script>");
    }
    if (typeof FrameNameSpace.Ajax == "undefined") {
        document.write("<script language=javascript src='/js/FrameJs/frame.ajax.js'></script>");
    }

    if (typeof FrameNameSpace.Message == "undefined") {
        document.write("<script language=javascript src='/js/FrameJs/configs/message.config.js'></script>");
        document.write("<script language=javascript src='/js/FrameJs/frame.message.js'></script>");
    }
    //jquery ui
    if (typeof $.ui == "undefined") {
        document.write("<script type='text/javascript' src='/js/jquery-ui-1.11.2.min.js'></script>");
        document.write("<link type='text/css' rel='stylesheet' href='/css/jquery-ui-1.11.2.min.css' />")
    }
    //dialog
    if (typeof dialog == "undefined") {
        document.write("<script language=javascript src='/js/dialog.js'></script>");
    }

    if (typeof $.attrchange == "undefined") {
        document.write("<script language=javascript src='/js/FrameJs/attrchange.js'></script>");
    }

    
}(jQuery));

String.format = function () {
    if (arguments.length == 0)
        return null;
    var str = arguments[0];
    for (var i = 1; i < arguments.length; i++) {
        var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        str = str.replace(re, arguments[i]);
    }
    return str;
};

String.prototype.replaceAll = function (s1, s2) {
    return this.replace(new RegExp(s1, "gm"), s2);
}





