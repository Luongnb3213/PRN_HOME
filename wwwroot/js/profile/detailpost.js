var linkPrevious = document.querySelector('.a-back-page');
linkPrevious.setAttribute('href', localStorage.getItem('previous'));

//Comment-detail-like
class commentAuthor extends PopupBase {
    constructor() {
        super();
        this.init();
    }

    init() {
        var _this = this;
        var link = _this.querySelector('.comment-detail-like-link');
        link.addEventListener('click', function () {
            _this.initPopup(_this.querySelector('comment-detail-like').innerHTML);
        })
    }
}
customElements.define('comment-author', commentAuthor)


//Like-detail-box
class threadDetail extends PopupBase {
    constructor() {
        super();
        this.init();
    }

    init() {
        var _this = this;
        var numLike = _this.querySelector('.num-like.detail-like.detail');
        //console.log(numLike);
        numLike.addEventListener('click', function () {
            _this.initPopup(_this.querySelector('thread-detail-like .detail-like-box').innerHTML);
        })
    }
}
customElements.define('thread-detail', threadDetail);

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

    const timeElement = document.querySelector('.time');
    const submitDate = timeElement.getAttribute('data-submit-date');
    timeElement.textContent = timeSince(submitDate);

    var timeComments = document.querySelectorAll('.author-comment-time');
    timeComments.forEach((element) => {
        const submitDate = element.getAttribute('data-submit-date');
        element.textContent = timeSince(submitDate);
    })
});

document.querySelector('.write-comment-btn-submit').addEventListener('click', function () {
    document.querySelector('#commentForm').submit();
});