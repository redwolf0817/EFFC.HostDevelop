(function ($,fns) {
    var _sleeptimer;
    $.sleep = function (ms2sleep, callback) {
        $.sleep._sleeptimer = ms2sleep;
        $.sleep._cback = callback;
        $.sleep.timer = setTimeout('$.sleep.count()', ms2sleep);//setInterval('$.sleep.count()', 1000);
    }
    $.extend($.sleep, {
        current_i: 1,
        _sleeptimer: 0,
        _cback: null,
        timer: null,
        count: function () {
            //if ($.sleep.current_i >= $.sleep._sleeptimer) {
            //    clearInterval($.sleep.timer);
            //    $.sleep._cback.call(this);
            //}
            //$.sleep.current_i++;

            clearInterval($.sleep.timer);
            $.sleep._cback.call(this);
        }
    });
})(jQuery,FrameNameSpace);