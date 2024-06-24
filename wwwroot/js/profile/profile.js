const page = document.querySelector(".page");
const up = document.querySelector(".tab-left");
const share = document.querySelector(".tab-right");
const line = document.querySelector(".selected-line");



//Get coordinates
document.addEventListener('DOMContentLoaded', function () {
    var currentScrollObj = JSON.parse(sessionStorage.getItem('currentScroll'));
    if (currentScrollObj) {
        window.scrollTo({
            top: currentScrollObj.scrollY,
            left: currentScrollObj.scrollX,
            behavior: "instant"
        });
        setTimeout(function () {
            sessionStorage.removeItem('currentScroll');
            sessionStorage.removeItem('previous');
        }, 100);
    }
})

//Thread-up
class threadUp extends PopupBase {
    constructor() {
        super();
        this.saveScroll();
        this.doReact();
    }

    saveScroll() {
        var _this = this;
        let aLink = _this.querySelector('.link-detail');

        aLink.addEventListener('click', function () {
            let scrollX = window.pageXOffset;
            let scrollY = window.pageYOffset;
            let currentScroll = {
                scrollX,
                scrollY
            }
            //setTimeout(() => {
            //    // Chuyển hướng đến href của thẻ <a>
            //    window.location.href = e.target.href;
            //}, 100);
            let userId = document.querySelector('.current-thread-id').dataset.currentthreadid;
            sessionStorage.setItem('currentScroll', JSON.stringify(currentScroll));
            sessionStorage.setItem('previous', `/profile?userId=${userId}`);
        })
    }

    doReact() {
        var _this = this;
        let tym = _this.querySelector('.heart')

        tym.addEventListener('click', () => {
            if (!tym.classList.contains('reacted')) {
                tym.classList.add('reacted')
                $.ajax({
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    },
                    url: `/?handler=Reacted&typeReact=up&threadId=${_this.querySelector('.thread-id').innerHTML}`,
                    method: 'POST',
                    processData: false,
                    contentType: false,
                    success: function (data) {
                        console.log('Success');
                    },
                    error: function (error) {
                        console.log('Error:', error);
                    }
                });
            } else {
                tym.classList.remove('reacted')
                $.ajax({
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    },
                    url: `/?handler=Reacted&typeReact=down&threadId=${_this.querySelector('.thread-id').innerHTML}`,
                    method: 'POST',
                    processData: false,
                    contentType: false,
                    success: function (data) {
                        console.log('Success');
                    },
                    error: function (error) {
                        console.log('Error:', error);
                    }
                });
            }
        })


    }

}
customElements.define('thread-up', threadUp);


//Edit Info Box
class editInfoBox extends PopupBase {
    constructor() {
        super();
        this.init();
    }

    init() {
        var _this = this
        _this.addEventListener('click', function () {
            _this.initPopup(_this.querySelector(".edit-info-box").innerHTML)
        })
    }
}
customElements.define("edit-info-box", editInfoBox)

//Avatar
class avatar extends PopupBase {
    constructor() {
        super();
        this.init()
    }

    init() {
        var _this = this;
        _this.addEventListener('click', function () {
            _this.initPopup(`<div class="option-box">
                        ${_this.querySelector('.option-box').innerHTML}
                    </div>`)
        })
    }
}
customElements.define("avatar-user", avatar);

//PrivateBtn
class btnPrivateStatus extends PopupBase {
    constructor() {
        super();
        this.btnPrivate();
    }

    btnPrivate() {
        var _this = this;
        _this.addEventListener("click", function (event) {
            const ball = _this.querySelector(".btn-ball");
            if (_this.classList.contains("public")) {
                _this.classList.remove("public");
                ball.style.right = "3px";
                ball.style.left = "auto";
            } else {
                _this.classList.add("public");
                ball.style.left = "3px";
                ball.style.right = "auto";
            }
        });
    }
}
customElements.define('btn-private', btnPrivateStatus);

//Time
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

//popstate
//document.addEventListener('mousedown', function (event) {
//    if (event.button === 3) {
//        consosle.log(window.history)
//    }
//});
