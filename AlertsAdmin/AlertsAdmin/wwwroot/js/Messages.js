$(function () {
    UpdateAlerts()
})

function UpdateAlerts() {
    var alertId = document.getElementById("alertId").innerText
    ajax_get_promise(`${constants.AlertsAdminAPIHost}/api/v1/Messages/${alertId}`).then((response) => {
        loadListOfAlerts(response)
    })
}

let loadListOfAlerts = (alertPayload) => {
    var templateText = $("#tableTemplate").html();
    var tableTemplate = Handlebars.compile(templateText);
    $("#alertList").html(tableTemplate({ array: alertPayload }))
};
