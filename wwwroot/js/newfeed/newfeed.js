let msgStatus = document.querySelector('.msg-status');
console.log(msgStatus.dataset.msg)
if (msgStatus.dataset.msg === "CreatedThread") {
    Swal.fire({
        title: "Posted",
        text: "Your post has just been posted!",
        icon: "success",
        confirmButtonColor: '#0a0a0a',
        customClass: {
            confirmButton: 'custom-confirm-button'
        },
        confirmButtonAriaLabel: '',
        confirmButtonText: 'Yay'
    });
}


