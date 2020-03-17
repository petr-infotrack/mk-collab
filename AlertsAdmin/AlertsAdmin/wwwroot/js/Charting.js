const maxChartScreens = 2
var currentChartScreen = 1
$(function () {
    var chartistBarOptions = {
        //seriesBarDistance: 10,
        reverseData: true,
        horizontalBars: true,
        axisY: {
            offset: 70
        },
        axisX: {
            onlyInteger: true
        }
    }
    new Chartist.Bar('#queueChart', {}, chartistBarOptions)

    UpdateChart()
    dataFunctions.push(UpdateChart)
})

function ParseChartData(data){
    //data = JSON.parse(data);
    var labels = []
    var series = []

    data.forEach((item) => {
        labels.push(item.Key)
        series.push(item.Value)
    })

    var chartData = {
        labels: labels
        , series: [series]
    }

    return chartData
}

function UpdateChart() {
    let route = '/api/v1/Queues/'
    switch (currentChartScreen) {
        case 1:
            route += 'LdmQueues'
            break;
        case 2:
            route += 'PencilQueues';
            break;
        default:
            route = 'LdmQueues';
            break;
    }

    ajax_get_promise(route).then((response) => {
        var chartData = ParseChartData(response)
        document.getElementById('queueChart').__chartist__.update(chartData);
    })

    currentChartScreen++;
    if (currentChartScreen > maxChartScreens){
        currentChartScreen = 1
    }
}