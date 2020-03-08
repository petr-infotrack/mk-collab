let chartistBarOptions = {
    seriesBarDistance: 10,
    reverseData: true,
    horizontalBars: true,
    axisY: {
        offset: 70
    }
}

function UpdateChart() {
    return ajax_get_promise('/Data/Queues').then((response) => {
        data = JSON.parse(response);
        var labels = []
        var series = []

        data.forEach((item) => {
            labels.push(item.Key)
            series.push(item.Value)
        })

        var chartData = {
            labels: labels
            , series: series
        }
        
        new Chartist.Bar('.ct-chart', chartData, chartistBarOptions)
    })
}


$(function () {
    UpdateChart();
    PollForData();
})

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

        UpdateChart();

    })


    poll(() => new Promise((resolve) => {
        resolve(); //you need resolve to jump into .then()
    }), 3000, 10000);
}