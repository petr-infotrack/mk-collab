
function ajax_get_promise(target, data) {
    return new window.Promise((resolve, reject) => {
        $.get({ url: target, data: data })
            .done((response) => {
                if (typeof response.success !== "undefined") {
                    resolve(response.success);
                } else {
                    reject(response.error);
                }
            })
            .fail((jqXhr, textStatus, errorThrown) => reject(errorThrown));
    });
}
$(function () {
    var detailUrl = '/Message/Edit';
    $('.editbtn').on('click', function (e) {
        e.preventDefault();
        var $buttonClicked = $(this);
        var id = $buttonClicked.attr('data-id');
        var data = { "Id": id };
        ajax_get_promise(detailUrl, data).then((response) => {
            console.log(response);
            var options = { "backdrop": "static", keyboard: true };
            //$('#myModalContent').(response);
            $('#MessageTypeEditPopup').modal(options);
            $('#MessageTypeEditPopup').modal('show');
        }).catch((response) => {
            console.log(response);
            alert(response);
        })
    });
})