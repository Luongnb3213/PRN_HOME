
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
    console.log(partnerId)
    //con.invoke("RegisterConnection", partnerId, con.connection.connectionId);
    getBoxChat(e.querySelector('.follower-id').dataset.followerid);
}

function getBoxChat(followerId) {
    $.ajax({
        headers:
        {
            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        url: `/mess?handler=GetBoxChat&selected=true&partnerId=${followerId}`,
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

function clickShow(userId) {
    let circle = document.querySelector(`.circle-${userId}`)
    circle.click();
}

//Code ajax
document.getElementById('myTextarea').addEventListener('keydown', function (event) {
    if (event.key === 'Enter' && !event.shiftKey) {
        event.preventDefault();
        let messContent = document.getElementById('myTextarea').value;
        if (messContent !== '') {
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
                    let currentMess = data.data;
                    let mainMess = document.querySelector('.main_mess');
                    let mainChat = document.querySelector('.main-chat');
                    con.invoke("SendMessageOneUser", parseInt(partnerId), messContent, currentMess.avtAuthor, currentMess.AuthorUsername);
                    let messageBox = `
                <div class="image py-3 flex flex-wrap gap-10 wrapper-message ${currentMess.whose}">
                    <img src="${currentMess.avtAuthor}" class="rounded-50 w-full" style="object-fit: cover; height: 48px; width:48px" />
                    <div class="message-box">
                        <span class="message-box--content">${currentMess.Content}</span>
                    </div>
                </div>`;
                    mainMess.insertAdjacentHTML('beforeend', messageBox);
                    mainChat.scrollTop = mainChat.scrollHeight;

                    let oldUserchat = document.querySelector(`.user-chat-${partnerId}`);
                    oldUserchat.remove();
                    let newUserchat = `
                                <user-chat class="py-4 d-block user-chat-${partnerId}" onclick="clickShow(${partnerId})">
                                    <div class="user_chat_wrapper w-full flex gap-20 align-center">
                                        <div class="user_image relative" style="width: 56px;">
                                            <img src="${currentMess.avtPartner}" class="rounded-50 w-full" style="object-fit: cover; height: 56px;" />
                                            <span class="absolute online"></span>
                                        </div>
                                        <div class="flex-1 user_mess flex gap-3 flex-column" style="max-width: calc(100% - 56px - 20px)">
                                            <span class="font-body w-full lh-1 d-block fs-18" style="color:rgb(245,245,245)">${currentMess.ReceiveName}</span>
                                            <p class="font-body w-full d-block ellipsis lh-1  fs-15" style="color:rgb(245,245,245); opacity: 0.8; margin:0">
                                                ${currentMess.whose === 'me' ? 'You: ' : ''}${currentMess.Content}
                                            </p>
                                        </div>
                                    </div>
                                </user-chat>
                    `
                    let inbox = document.querySelector('inbox')
                    inbox.insertAdjacentHTML('beforebegin', newUserchat)
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
    }
});
con.on("ReceiveMessageOneUser", function (partnerIdSignalR, messContent, avtAuthor, AuthorUsername) {
    let currentuserid = document.querySelector('.currentuserid').dataset.currentuserid;
    let mainMess = document.querySelector('.main_mess');
    let mainChat = document.querySelector('.main-chat');
    let whose = "";
    if (parseInt(currentuserid) !== parseInt(partnerIdSignalR)) {
        whose = "me"
    } else {
        whose = "other"
    }
    let messageBox = `
                <div class="image py-3 flex flex-wrap gap-10 wrapper-message ${whose}">
                    <img src="${avtAuthor}" class="rounded-50 w-full" style="object-fit: cover; height: 48px; width:48px" />
                    <div class="message-box">
                        <span class="message-box--content">${messContent}</span>
                    </div>
                </div>`;
    mainMess.insertAdjacentHTML('beforeend', messageBox);

    let oldUserchat = document.querySelector(`.user-chat-${partnerId}`);
    oldUserchat.remove();
    let newUserchat = `
                                <user-chat class="py-4 d-block user-chat-${partnerId}" onclick="clickShow(${partnerId})">
                                    <div class="user_chat_wrapper w-full flex gap-20 align-center">
                                        <div class="user_image relative" style="width: 56px;">
                                            <img src="${avtAuthor}" class="rounded-50 w-full" style="object-fit: cover; height: 56px;" />
                                            <span class="absolute online"></span>
                                        </div>
                                        <div class="flex-1 user_mess flex gap-3 flex-column" style="max-width: calc(100% - 56px - 20px)">
                                            <span class="font-body w-full lh-1 d-block fs-18" style="color:rgb(245,245,245)">${AuthorUsername}</span>
                                            <p class="font-body w-full d-block ellipsis lh-1  fs-15" style="color:rgb(245,245,245); opacity: 0.8; margin:0">
                                                ${whose === 'me' ? 'You: ' : ''}${messContent}
                                            </p>
                                        </div>
                                    </div>
                                </user-chat>
                    `
    let inbox = document.querySelector('inbox')
    inbox.insertAdjacentHTML('beforebegin', newUserchat)
    //console.log(partnerId)
    mainChat.scrollTop = mainChat.scrollHeight;

});

let btnGroup = document.querySelector('.btn-group');
let overlay = document.querySelector('.overlay-group');
let selectFollower = document.querySelector('popup-follower');
btnGroup.addEventListener('click', function () {
    overlay.classList.remove('hidden');
    selectFollower.classList.remove('hidden');
})

overlay.addEventListener('click', function () {
    overlay.classList.add('hidden');
    selectFollower.classList.add('hidden');
})

function selectAccount(acc) {
    let checkbox = acc.querySelector('.custom-checkbox');
    checkbox.checked = !checkbox.checked;
}

let btnCreateGroup = document.querySelector('.btn-create-group');
btnCreateGroup.addEventListener('click', function () {
    let formCreateGroup = document.querySelector('#form-create-group');
    formCreateGroup.submit();
})