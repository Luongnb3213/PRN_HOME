var partnerId = 0;
var groupId = 0;
var typeChat = '';
function showBoxChatSingle(e) {
    typeChat = 'single';
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
    getBoxChat(e.querySelector('.follower-id').dataset.followerid);
}
function showBoxChatGroup(e) {
    typeChat = 'group';
    groupId = e.querySelector('.group-id').dataset.groupid;
    console.log(groupId);
    $.ajax({
        headers:
        {
            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        url: `/mess?handler=GetGroupChat&groupId=${groupId}`,
        method: 'GET',
        processData: false,
        contentType: false,
        success: function (data) {
            con.invoke("JoinGroup", data.nameGroup).catch(err => console.error(err.toString()));
            document.querySelector('.right-panel').classList.remove('hidden');

            document.querySelectorAll('.avt-partner').forEach((el) => {
                el.querySelector('img').src = e.querySelector('img').src
            })
            document.querySelectorAll('.username-partner').forEach((el) => {
                el.innerHTML = e.querySelector('.group-name span').innerHTML;
            })
            document.querySelectorAll('.username-partner.group-chat').forEach((el) => {
                el.innerHTML = `${data.num} members`;
            })
            document.querySelectorAll('.fullname-partner').forEach((el) => {
                el.innerHTML = e.querySelector('.group-name span').innerHTML;
            })

            let mainMess = document.querySelector('.main_mess');
            mainMess.dataset.type = `groupChat-${groupId}`;
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
            mainMess.dataset.type = `singleChat-${partnerId}`;
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

function getFlexibleChatBar() {
    $.ajax({
        headers:
        {
            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        url: `/mess?handler=GetFlexibleChatBar`,
        method: 'GET',
        processData: false,
        contentType: false,
        success: function (data) {
            let inbox = document.querySelector('inbox');
            let arrayData = data.data.$values;
            let innerPart = '';
            arrayData.forEach((el) => {
                if (el.type == false) {
                    innerPart += `<user-chat class="py-4 d-block user-chat-${el.IdToClick}" onclick="clickShow(${el.IdToClick})">
                        <div class="user_chat_wrapper w-full flex gap-20 align-center">
                            <div class="user_image relative" style="width: 56px;">
                                <img src="${el.displayAvt}" class="rounded-50 w-full" style="object-fit: cover; height: 56px;" />
                                <span class="absolute online"></span>
                            </div>
                            <div class="flex-1 user_mess flex gap-3 flex-column" style="max-width: calc(100% - 56px - 20px)">
                                <span class="font-body w-full lh-1 d-block fs-18" style="color:rgb(245,245,245)">${el.displayUsername}</span>
                                <p class="font-body w-full d-block ellipsis lh-1  fs-15" style="color:rgb(245,245,245); opacity: 0.8; margin:0">
                                    ${el.whose == "other" ? "" : "You: "}${el.Content}
                                </p>
                            </div>
                        </div>
                    </user-chat>`
                } else {
                    innerPart += `<user-chat class="py-4 d-block user-chat-${el.IdToClick}-group" onclick="showBoxChatGroup(this)">
                        <div class="group-id" data-groupid="${el.IdToClick}"></div>
                        <div class="user_chat_wrapper w-full flex gap-20 align-center">
                            <div class="user_image relative" style="width: 56px;">
                                <img src="${el.displayAvt}" class="rounded-50 w-full" style="object-fit: cover; height: 56px;" />
                                <span class="absolute online"></span>
                            </div>
                            <div class="flex-1 user_mess flex gap-3 flex-column group-name" style="max-width: calc(100% - 56px - 20px)">
                                <span class="font-body w-full lh-1 d-block fs-18" style="color:rgb(245,245,245)">${el.displayUsername}</span>
                                <p class="font-body w-full d-block ellipsis lh-1  fs-15" style="color:rgb(245,245,245); opacity: 0.8; margin:0">
                                    ${el.whose == "other" ? "" : "You: "}${el.Content}
                                </p>
                            </div>
                        </div>
                    </user-chat>`
                }
            })
            inbox.innerHTML = innerPart;
        }
    })
}

//Code ajax
document.getElementById('myTextarea').addEventListener('keydown', function (event) {
    if (event.key === 'Enter' && !event.shiftKey) {
        event.preventDefault();
        let messContent = document.getElementById('myTextarea').value;
        if (messContent !== '') {
            if (typeChat === 'single') {
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

                        getFlexibleChatBar();

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
            } else {
                let dataSendGroup = {
                    messContent,
                    groupId
                }
                $.ajax({
                    headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
                    url: '/mess?handler=Group',
                    method: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify(dataSendGroup),
                    success: function (data) {
                        let currentMess = data.data;
                        let mainMess = document.querySelector('.main_mess');
                        let mainChat = document.querySelector('.main-chat');
                        con.invoke("SendMessageToGroup", currentMess.groupName, currentMess.AuthorId, messContent, currentMess.avtAuthor, currentMess.AuthorUsername, currentMess.groupImg, groupId);

                        let messageBox = `
                            <div class="image py-3 flex flex-wrap gap-10 wrapper-message ${currentMess.whose}">
                                <img src="${currentMess.avtAuthor}" class="rounded-50 w-full" style="object-fit: cover; height: 48px; width:48px" />
                                <div class="message-box">
                                    <span class="message-box--content">${currentMess.Content}</span>
                                </div>
                            </div>`;
                        mainMess.insertAdjacentHTML('beforeend', messageBox);
                        mainChat.scrollTop = mainChat.scrollHeight;
                        getFlexibleChatBar();

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
            }
            document.getElementById('myTextarea').value = '';
        }
    }
});
con.on("ReceiveGroupMessage", function (AuthorId, messContent, avtAuthor, AuthorUsername, groupImg, groupName, groupId) {
    let currentuserid = document.querySelector('.currentuserid').dataset.currentuserid;
    let mainMess = document.querySelector('.main_mess');
    if (mainMess.dataset.type === `groupChat-${groupId}`) {
        let mainChat = document.querySelector('.main-chat');
        let whose = '';
        if (parseInt(AuthorId) === parseInt(currentuserid)) {
            whose = 'me'
        } else {
            whose = 'other'
        }

        let messageBox = `
                <div class="image py-3 flex flex-wrap gap-10 wrapper-message ${whose}">
                    <img src="${avtAuthor}" class="rounded-50 w-full" style="object-fit: cover; height: 48px; width:48px" />
                    <div class="message-box">
                        <span class="message-box--content">${messContent}</span>
                    </div>
                </div>`;
        mainMess.insertAdjacentHTML('beforeend', messageBox);
        mainChat.scrollTop = mainChat.scrollHeight;
    }
    getFlexibleChatBar()

})
con.on("ReceiveMessageOneUser", function (partnerIdSignalR, messContent, avtAuthor, AuthorUsername) {
    let currentuserid = document.querySelector('.currentuserid').dataset.currentuserid;
    let mainMess = document.querySelector('.main_mess');
    if (mainMess.dataset.type === `singleChat-${partnerIdSignalR}`) {
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
        mainChat.scrollTop = mainChat.scrollHeight;
    }
    getFlexibleChatBar()

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