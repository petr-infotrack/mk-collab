let cardContainer;

$(function () {
    UpdateAlerts();
    dataFunctions.push(UpdateAlerts);
})

function UpdateAlerts() {
    ajax_get_promise('api/v1/Alerts').then((response) => {
        //cardPayload = JSON.parse(response)
        loadListOfCards(response)
    })
}

let createTaskCard = (cardResponse) => {
    let row = document.createElement('div');
    row.className = 'row-12';
    row.style = 'padding-bottom: 10px;'

    let card = document.createElement('div');
    card.className = 'card cursor-pointer';
    card.className = `${card.className} ${cardResponse.class}`
    card.style = 'min-height: 100px';

    let cardLink = document.createElement('a');
    cardLink.className = 'card-block stretched-link text-decoration-none';
    cardLink.href = `/Alerts/View/${cardResponse.id}`;

    let cardBody = document.createElement('div');
    cardBody.className = 'card-body text-white';

    let title = document.createElement('h5');
    title.innerText = cardResponse.title;
    title.className = 'card-title';

    let cardText = document.createElement('p');
    cardText.className = 'card-text';
    cardText.innerText = cardResponse.message;

    cardBody.appendChild(title);
    cardBody.appendChild(cardText);
    cardLink.appendChild(cardBody)
    card.appendChild(cardLink);
    row.appendChild(card)
    cardContainer.appendChild(row);
}

let loadListOfCards = (cardPayload) => {
    if (cardContainer) {
        document.getElementById('alertDeck').replaceWith(cardContainer);
        return;
    }

    cardContainer = document.getElementById('alertDeck');

    cardPayload.forEach((card) => {
        createTaskCard(card);
    });
};
