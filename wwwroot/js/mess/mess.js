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
    getBoxChat(e.querySelector('.follower-id').dataset.followerid);
}

function getBoxChat(followerId) {
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
            console.log(data)
        }
    })
}
//Code ajax
document.getElementById('myTextarea').addEventListener('keydown', function (event) {
    if (event.key === 'Enter' && !event.shiftKey) {
        event.preventDefault();
        let contentMessage = document.getElementById('myTextarea').value;
        let dataSend = {
            contentMessage,
            partnerId
        }
        $.ajax({
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            
            url: '/mess',
            method: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            cache: false,
            data: JSON.stringify(dataSend),
            success: function (data) {
                console.log(data)
            },
            error: function (error) {
                console.error("Error: " + error.msg); 
            }
        })
        document.getElementById('myTextarea').value = '';
    }
});

