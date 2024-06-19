
let msgStatus = document.querySelector('.msg-status');
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

document.addEventListener('DOMContentLoaded', function () {
    function timeSince(date) {
        const now = new Date();
        const seconds = Math.floor((now - new Date(date)) / 1000);
        const minutes = Math.floor(seconds / 60);
        const hours = Math.floor(minutes / 60);
        const days = Math.floor(hours / 24);
        const months = Math.floor(days / 30);
        const years = Math.floor(days / 365);

        if (years >= 1) return years + (years > 1 ? " years" : " year");
        if (months >= 1) return months + (months > 1 ? " months" : " month");
        if (days >= 1) return days + (days > 1 ? " days" : " day");
        if (hours >= 1) return hours + (hours > 1 ? " hours" : " hour");
        if (minutes >= 1) return minutes + (minutes > 1 ? " minutes" : " minute");
        return seconds + (seconds > 1 ? " seconds" : " second");
    }

    const authorElements = document.querySelectorAll('.author');
    authorElements.forEach(function (element) {
        const submitDate = element.getAttribute('data-submit-date');
        const timeElapsedElement = element.querySelector('.time-elapsed');
        if (submitDate && timeElapsedElement) {
            timeElapsedElement.textContent = timeSince(submitDate);
        }
    });
});

