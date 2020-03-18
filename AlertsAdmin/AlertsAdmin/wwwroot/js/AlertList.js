
$(function () {
    UpdateAlerts()
    dataFunctions.push(UpdateAlerts)
})

function UpdateAlerts() {
    var alertId = document.getElementById("alertId").nodeValue
    ajax_get_promise(`api/v1/Alerts/${alertId}`).then((response) => {
        loadListOfAlerts(response)
    })
}

let loadListOfAlerts = (alertPayload) => {
    var templateText = $("#tableTemplate").html();
    var tableTemplate = Handlebars.compile(templateText);
    $("#alertList").html(tableTemplate({alertPayload}))
};
