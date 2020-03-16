var dataFunctions = []

$(function () {
    PollForData()
})

function PollForData(pollingPeriod = 10000, pollingTimeout = 10000) {
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

        dataFunctions.forEach(
            fun => fun()
        )
    })

    poll(() => new Promise((resolve) => {
        resolve(); //you need resolve to jump into .then()
    }), pollingPeriod, pollingTimeout);
}