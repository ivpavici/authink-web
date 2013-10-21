var authink = {
    getDataFromUrl: function (url, finisher, id) {
        if (id) {
            url = url +"/" + id;
        }
        $.ajax({
            url: url,
            type:"GET",
            success: function(data) {
                finisher(data);
            }
        });
    },
    
    switchLoginRegister: function () {
        $('#login-form-login').slideUp(400);
    },

    dialogShow: function (url, modal) {
        var popup = $('#popup-root'),
        modalSettings = {
            opacity: 30,
            close: true,
            overlayClose: true,
            onOpen: function (dialog) {
                dialog.overlay.fadeIn('fast', function () {
                    dialog.container.slideDown('fast', function () {
                        dialog.data.fadeIn('fast');
                    });
                });
                $('#simplemodal-container').css('height', 'auto');
                $('#simplemodal-container').css('width', 'auto');
            },
            onClose: function (dialog) {
                dialog.data.fadeOut('slow', function () {
                    dialog.container.slideUp('fast', function () {
                        dialog.overlay.fadeOut('fast', function () {
                            $.modal.close();
                        });
                    });
                });
            }
        };

        popup.load(url, function () {
            popup.modal(modalSettings);
        });
    }
}