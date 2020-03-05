
function ajax_get_promise(target) {
    return new window.Promise((resolve, reject) => {
        $.get({ url: target})
            .done((response) => {
                resolve(response);
            })
            .fail((jqXhr, textStatus, errorThrown) => reject(errorThrown));
    });
}
$(function () {
    var detailUrl = '/Messages/Edit';
    $('.editbtn').on('click', function (e) {
        e.preventDefault();
        var $buttonClicked = $(this);
        var id = $buttonClicked.attr('data-id');
        ajax_get_promise(detailUrl + "?Id=" + id).then((response) => {
            r = JSON.parse(response)
            var options = { "backdrop": "static", keyboard: true };
            $('.modal-body #Id').val(r.Id);
            $('.modal-body #Template').val(r.Template);
            $('.modal-body #Level').val(r.Level).selectpicker("refresh");
            $('.modal-body #Priority').val(r.Priority).selectpicker("refresh");
            $('.modal-body #Notification').val(r.Notification).selectpicker("refresh");
            $('.modal-body #DefaultStatus').val(r.DefaultStatus).selectpicker("refresh");
            $('.modal-body #ExpiryTime').val(r.ExpiryTime);
            $('.modal-body #ExpiryCount').val(r.ExpiryCount);
            $('#MessageTypeEditPopup').modal(options);
            $('#MessageTypeEditPopup').modal('show');
        }).catch((response) => {
            console.log(response);
            alert(response);
        })
    });
})