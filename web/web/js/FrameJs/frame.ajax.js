/*
根據返回的frame json串進行判定
successfunc(contentobj):contentobj返回的json對象，FrameJson中的Content內容；返回值為bool
failedfunc(errorCode, errorMsg):errorCode錯誤代碼，errorMsg錯誤信息
*/
; (function ($,fns) {
    fns.Ajax = {
        Ajax : function(options){
            var defaults = {
                url: "",
                returntype: "json",
                postdata: "",
                before: null, //function(XMLHttpRequest)
                success: null,//function(rtn)
                complete: null,//function (XMLHttpRequest, textStatus)
                fail: null,//function(errorcode,errormsg)
                async: true
            }
            var opts = $.extend(defaults, options);
            //json格式需要单独处理
            var datatype = opts.returntype == "json" ? "text" : opts.returntype;
            if (opts.url != "") {
                $.ajax({
                    type: "Post",
                    url: opts.url,
                    dataType: datatype,
                    cache: false,
                    data: opts.postdata,
                    async: opts.async,
                    beforeSend: function (XMLHttpRequest) {
                        if (opts.before != null) {
                            opts.before(XMLHttpRequest);
                        }
                    },
                    success: function (rtn) {
                        var jobj;
                        if (rtn != null) {
                            //判斷jsonstr是否就是json對象
                            //jobj = $.parseJSON(rtn);
                            if (jobj == null) {
                                jobj = rtn;
                            }
                            if (opts.returntype.toLowerCase() == "json") {
                                var bobj = eval("(" + jobj + ")");
                                if (bobj.ErrorCode == "") {
                                    if (bobj.Content.__isneedlogin__) {
                                        fns.Message.DialogLogin("请先登录！", bobj.Content.__loginurl__);
                                    } else {
                                        if (opts.success != null) {
                                            opts.success(bobj.Content);
                                        }
                                    }
                                } else {
                                    fns.ExceptionProcess.ShowErrorMsg(bobj.ErrorCode + "\n" + bobj.ErrorMessage);
                                    if (opts.fail) {
                                        opts.fail(bobj.ErrorCode, bobj.ErrorMessage);
                                    }
                                }
                            } else {
                                if (opts.success != null) {
                                    opts.success(jobj);
                                }
                            }
                        }
                    },
                    complete: function (XMLHttpRequest, textStatus) {
                        if (opts.complete != null) {
                            opts.complete(XMLHttpRequest, textStatus);
                        }
                    },
                    error: function (jqXHR, errormsg, errorThrown) {
                        if (opts.fail) {
                            opts.fail(errormsg, errormsg);
                        }
                    }
                });
            } else {
                fns.ExceptionProcess.ShowErrorMsg("需要提供url信息！");
            }
        },
        AjaxForm: function (options) {
            var defaults = {
                url: "",
                formid:"",
                returntype: "html",
                befersend: null,//bool function(XMLHttpRequest)，if false abort
                success: null,//function(jsonrtn)
                complete: null,//function(XMLHttpRequest, textStatus)
                fail: null//function(errorcode,errormsg)
            }
            var opts = $.extend(defaults, options);
            if (opts.formid == "") {
                fns.ExceptionProcess.ShowErrorMsg("需要提供formid信息！");
                return false;
            }
            if (opts.url == "") {
                fns.ExceptionProcess.ShowErrorMsg("需要提供url信息！");
                return false;
            }
            var __f = $("#" + opts.formid);
            //取消已经绑定的事件，防止事件重复性注册，并导致重复提交
            __f.unbind();

            var dt = opts.returntype == "" ? "html" : opts.returntype;
            var __op = {
                url: opts.url, type: "post", cache: false, datatype: dt,
                beforeSubmit: function (dataarray,obj) {
                    if (opts.befersend != null) {
                        opts.befersend(dataarray, obj);
                    }
                },
                success: function (rtn) {
                    if (opts.returntype.toLowerCase() == "json") {
                        if (rtn != null) {
                            var jobj = $.parseJSON(rtn);
                            if (jobj == null) {
                                jobj = rtn;
                            }
                            if (jobj.ErrorCode == "") {
                                if (jobj.Content.__isneedlogin__) {
                                    fns.Message.DialogLogin("请先登录！", jobj.Content.__loginurl__);
                                } else {
                                    if (opts.success != null) {
                                        opts.success(jobj.Content);
                                    }
                                }
                            } else {
                                fns.ExceptionProcess.ShowErrorMsg(jobj.ErrorCode + "\n" + jobj.ErrorMessage);
                                if (opts.fail) {
                                    opts.fail(jobj.ErrorCode, jobj.ErrorMessage);
                                }
                            }
                        }
                    } else {
                        if (opts.success != null) {
                            opts.success(rtn);
                        }
                    }
                    
                },
                complete: function (XMLHttpRequest, textStatus) {
                    if (opts.complete != null) {
                        opts.complete(XMLHttpRequest, textStatus);
                    }
                },
                error: function (jqXHR, errormsg, errorThrown) {
                    if (opts.fail) {
                        opts.fail(errormsg, errormsg);
                    }
                }
            };

            __f.submit(function () {
                $(this).ajaxSubmit(__op);
                return false;
            });

            __f.submit();
        }
    }
    fns.Ajax.WebSocket = {
        Open: function (options) {
            var defaults = {
                url: "",
                onopen: null,//function(ws)
                onmessage: null,//function(ws,data)
                onerror: null,//function(ws,status,msg)
                onclose: null//function()
            }
            var me = this;
            var opts = $.extend(defaults, options);
            var ws = new WebSocket('ws://' + window.location.hostname + (window.location.port == "" ? "" : ':' + window.location.port) + '/' + opts.url);
            if (!ws) {
                fns.ExceptionProcess.ShowErrorMsg("当前浏览器并不支持WebSocket");
                return null;
            }
            if (ws) {
                ws.onopen = function () {
                    if (opts.onopen) {
                        opts.onopen(me);
                    }
                }
                ws.onmessage = function (evt) {
                    if (Object.prototype.toString.call(evt.data) === "[object Blob]") {
                        if (opts.onmessage) {
                            opts.onmessage(me, evt.data);
                        }
                    } else {
                        eval("var jobj=" + evt.data);
                        if (jobj) {
                            if (jobj.ErrorCode == "") {
                                if (opts.onmessage) {
                                    opts.onmessage(me, jobj.Content);
                                }
                            } else {
                                fns.ExceptionProcess.ShowErrorMsg(jobj.ErrorCode + "\n" + jobj.ErrorMessage);
                                if (opts.onerror) {
                                    opts.onerror(me, jobj.ErrorCode, jobj.ErrorMessage);
                                }
                            }
                        } else {
                            $('#result').val($('#result').val() + "\n" + evt.data);
                        }
                    }
                }
                ws.onerror = function (evt) {
                    fns.ExceptionProcess.ShowErrorMsg(evt.message);
                }
                ws.onclose = function () {
                    if (opts.onclose) {
                        opts.onclose();
                    }
                }
            }
            $(me).data("ws", ws);
            return me;
        },
        Send: function (obj) {
            var ws = $(this).data("ws");
            if (ws) {
                ws.send(JSON.stringify(obj), 20971520);
            }
        },
        Close: function () {
            var ws = $(this).data("ws");
            if (ws) {
                ws.close();
            }
        },
        IsClose: function () {
            var ws = $(this).data("ws");
            if (ws) {
                return ws.CLOSED;
            }
        }
    }
}(jQuery,FrameNameSpace))

