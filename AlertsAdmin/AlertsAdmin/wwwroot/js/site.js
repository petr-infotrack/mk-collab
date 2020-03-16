// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function ajax_get_promise(target) {
    return new window.Promise((resolve, reject) => {
        $.get({ url: target })
            .done((response) => {
                resolve(response);
            })
            .fail((jqXhr, textStatus, errorThrown) => reject(errorThrown));
    });
}