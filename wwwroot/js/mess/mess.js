
var partnerId = 0;
function showBoxChatSingle(e) {
    document.querySelector('.right-panel').classList.remove('hidden');

    let followerNamelinkAvt = e.querySelector('.avt-follower-up').src
    let followerName = e.querySelector('.username-follower-up').innerHTML
    let followerFullName = e.querySelector('.fullname-follower-up').innerHTML;

    document.querySelectorAll('.avt-partner').forEach((el) => {
        el.querySelector('img').src = followerNamelinkAvt
    })
    document.querySelectorAll('.username-partner').forEach((el) => {
        el.innerHTML = followerName
    })
    document.querySelectorAll('.fullname-partner').forEach((el) => {
        el.innerHTML = followerFullName
    })
    partnerId = e.querySelector('.follower-id').dataset.followerid;
    con.invoke("RegisterConnection", partnerId, con.connection.connectionId);
    getBoxChat(e.querySelector('.follower-id').dataset.followerid);
}

function getBoxChat(followerId) {
    console.log(followerId)
    $.ajax({
        headers:
        {
            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        url: `/mess?selected=true&partnerId=${followerId}`,
        method: 'GET',
        processData: false,
        contentType: false,
        success: function (data) {
            let mainMess = document.querySelector('.main_mess');
            let mainChat = document.querySelector('.main-chat');
            let arrayMess = data.data.$values
            mainMess.innerHTML = '';
            arrayMess.forEach((el) => {
                let messageBox = `
                <div class="image py-3 flex flex-wrap gap-10 wrapper-message ${el.whose}">
                    <img src="${el.avtAuthor}" class="rounded-50 w-full" style="object-fit: cover; height: 48px; width:48px" />
                    <div class="message-box">
                        <span class="message-box--content">${el.Content}</span>
                    </div>
                </div>`;
                mainMess.insertAdjacentHTML('beforeend', messageBox);
            })
            mainChat.scrollTop = mainChat.scrollHeight;
        }
    })
}
//Code ajax
document.getElementById('myTextarea').addEventListener('keydown', function (event) {
    if (event.key === 'Enter' && !event.shiftKey) {
        event.preventDefault();
        let messContent = document.getElementById('myTextarea').value;
        let dataSend = {
            messContent,
            partnerId
        }
        $.ajax({
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            url: '/mess',
            method: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(dataSend),
            success: function (data) {
                let mainMess = document.querySelector('.main_mess');
                let mainChat = document.querySelector('.main-chat');
                let arrayMess = data.data.$values
                mainMess.innerHTML = '';
                con.invoke("SendMessageOneUser", parseInt(partnerId), messContent);
                arrayMess.forEach((el) => {
                    let messageBox = `
                <div class="image py-3 flex flex-wrap gap-10 wrapper-message ${el.whose}">
                    <img src="${el.avtAuthor}" class="rounded-50 w-full" style="object-fit: cover; height: 48px; width:48px" />
                    <div class="message-box">
                        <span class="message-box--content">${el.Content}</span>
                    </div>
                </div>`;
                   
                    mainMess.insertAdjacentHTML('beforeend', messageBox);
                })
                mainChat.scrollTop = mainChat.scrollHeight;
            },
            error: function (jqXHR, exception) {
                var msg = '';
                if (jqXHR.status === 0) {
                    msg = 'Not connect.\n Verify Network.';
                } else if (jqXHR.status == 404) {
                    msg = 'Requested page not found. [404]';
                } else if (jqXHR.status == 500) {
                    msg = 'Internal Server Error [500].';
                } else if (exception === 'parsererror') {
                    msg = 'Requested JSON parse failed.';
                } else if (exception === 'timeout') {
                    msg = 'Time out error.';
                } else if (exception === 'abort') {
                    msg = 'Ajax request aborted.';
                } else {
                    msg = 'Uncaught Error.\n' + jqXHR.responseText;
                }
                console.log(msg)
            }
        })
        document.getElementById('myTextarea').value = '';
    }
});
con.on("ReceiveMessageOneUser", function (partnerId, messContent) {
    console.log(messContent)
});
