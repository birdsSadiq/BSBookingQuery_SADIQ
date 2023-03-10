$(function () {
    $("#loaderbody").addClass('hide');
    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });
});

showInPopup = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#modalForm .modal-body').html(res);
            $('#modalForm .modal-title').html(title);
            $('#modalForm').modal('show');
        }
    })
}

jQueryAjaxPost = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#dvViewAll').html(res.html)
                    $('#modalForm .modal-body').html('');
                    $('#modalForm .modal-title').html('');
                    $('#modalForm').modal('hide');

                    $.notify(res.message, {
                        globalPosition: "top center",
                        className: "success"
                    })
                } else {
                    $('#modalForm .modal-body').html(res.html);
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
        return false;//to prevent default form submit event
    } catch (ex) {
        console.log(ex)
    }
}

jQueryAjaxDelete = form => {
    if (confirm('Are you sure to delete this record ?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#dvViewAll').html(res.html);

                    $.notify(res.message, {
                        globalPosition: "top center",
                        className: "success"
                    })
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }
    return false;//prevent default form submit event
}

$(document).on('click', '#btnSearchHotelList', function () {

    var rf = 0;
    var rt = 0;
    var city = $('#sCity').val();
    var country = $('#sCountry').val();
    if ($.isNumeric($('#rf').val())) { rf = parseInt($('#rf').val()); }
    if ($.isNumeric($('#rt').val())) { rt = parseInt($('#rt').val()); }
    $.ajax({
        type: 'POST',
        url: '@url.Action("Hotel", "GetSearchList")', /*url: '../Hotel/GetSearchList',*/
        data: { city: city, country: country, rf: rf, rt: rt },
        success: function (res) {
            $('#dvViewAll').html(res.html);
        },
        error: function (err) {
            console.log(err)
        }
    })

});