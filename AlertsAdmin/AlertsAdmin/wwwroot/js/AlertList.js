
$(function () {
    UpdateAlerts()
})

function UpdateAlerts() {
    var alertId = document.getElementById("alertId").innerText
    ajax_get_promise(`https://localhost:44397/api/v1/Alerts/${alertId}`).then((response) => {
        loadListOfAlerts(response)
    })
}

let loadListOfAlerts = (alertPayload) => {
    var templateText = $("#tableTemplate").html();
    var tableTemplate = Handlebars.compile(templateText);
    $("#alertList").html(tableTemplate({array:alertPayload}))
};
