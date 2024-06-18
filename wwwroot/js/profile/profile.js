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
    }

    saveScroll() {
        var _this = this;
        let aLink = _this.querySelector('.link-detail');
        aLink.addEventListener('click', function () {
            console.log(aLink)
            let scrollX = window.pageXOffset;
            let scrollY = window.pageYOffset;
            let currentScroll = {
                scrollX,
                scrollY
            }
            console.log(currentScroll);
            //setTimeout(() => {
            //    // Chuyển hướng đến href của thẻ <a>
            //    window.location.href = e.target.href;
            //}, 100);
            sessionStorage.setItem('currentScroll', JSON.stringify(currentScroll));
            sessionStorage.setItem('previous', '/profile');
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


