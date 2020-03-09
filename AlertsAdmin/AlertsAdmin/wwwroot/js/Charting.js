const maxScreens = 2
$(function () {
    var chartistBarOptions = {
        seriesBarDistance: 10,
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

    UpdateChart(1);
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

function UpdateChart(state) {
    let route
    switch (state) {
        case 1:
            route = '/Data/Queues';
            break;
        case 2:
            route = '/Data/PencilQueues';
            break;
        default:
            route = '/Data/Queues';
            break;
    }

    ajax_get_promise(route).then((response) => {
        var chartData = ParseChartData(response)
        document.getElementById('queueChart').__chartist__.update(chartData);
    })
}

function PollForData() {
    let state = 2
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

        UpdateChart(state);

        state++;
        if (state > maxScreens)
            state = 1;
    })

    poll(() => new Promise((resolve) => {
        resolve(); //you need resolve to jump into .then()
    }), 10000, 10000);
}