; (function ($) {
    $.Frame.Config.Message =
        {
            Dialog: function (options) {
                if (options) {
                    Alert(options.msg, options.buttons);
                }
            },
            ConfirmMsg: function (options) {
                if (options) {
                    dialog(options.msg, "确认", options.buttons);
                }
            },
            ShowMsg: function (options) {
                if (options) {
                    Alert(options.msg, options.buttons);
                }
            }
        }
}(jQuery));
