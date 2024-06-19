var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();


connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});