$(function () {
    var chartistBarOptions = {
        seriesBarDistance: 10,
        reverseData: true,
        horizontalBars: true,
        axisY: {
            offset: 70
        }
    }
    new Chartist.Bar('#queueChart', {}, chartistBarOptions)
    new Chartist.Bar('#pencilChart', {}, chartistBarOptions)

    UpdateCharts();
    PollForData();
})

function ParseChartData(data){
    data = JSON.parse(data);
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

function UpdateCharts() {
    ajax_get_promise('/Data/Queues').then((response) => {
        var chartData = ParseChartData(response)
        document.getElementById('queueChart').__chartist__.update(chartData);
    })

    ajax_get_promise('/Data/PencilQueues').then((response) => {
        var chartData = ParseChartData(response)
        document.getElementById('pencilChart').__chartist__.update(chartData);
    })
}

function PollForData() {
    let cancelCallback = () => { };

    var sleep = (period) => {
        return new Promise((resolve) => {
            cancelCallback = () => {
                console.log("Canceling...");
                // send cancel message...
                return resolve('Canceled');
            }
            setTimeout(() => {
                resolve("tick");
            }, period)
        })
    }

    var poll = (promiseFn, period, timeout) => promiseFn().then(() => {
        let asleep = async (period) => {
            let respond = await sleep(period);
            return respond;
        }


        if (cancelCallback.toString() === "() => {}") {
            setTimeout(() => {
                cancelCallback()
            }, timeout);
        }

        asleep(period).then((respond) => {
            if (respond !== 'Canceled') {
                poll(promiseFn, period);
            } else {
                console.log(respond);
            }
        })

        UpdateCharts();

    })

    poll(() => new Promise((resolve) => {
        resolve(); //you need resolve to jump into .then()
    }), 10000, 10000);
}