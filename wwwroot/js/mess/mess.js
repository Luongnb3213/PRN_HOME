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
}