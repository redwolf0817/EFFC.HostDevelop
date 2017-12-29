; (function ($,fns) {
    fns.Control = {
        GenImage : function (id, src) {
            var rtn = "<img id='" + id + "' name='" + id + "' src='" + src + "' />";
            return rtn;
        },
        GenValidationLabel : function (id, text) {
            var rtn = "<label id='" + id + "' name='" + id + "' style='color:red'>" + text + "</label>";
            return rtn;
        },
        GenOption : function (value, text) {
            rtn = "<option value='" + value + "'>" + text + "</option>";
            return rtn;
        }
    }
}(jQuery,FrameNameSpace));
