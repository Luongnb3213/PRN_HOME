const page = document.querySelector(".page");
const up = document.querySelector(".tab-left");
const share = document.querySelector(".tab-right");
const line = document.querySelector(".selected-line");
const overlay = document.querySelector(".overlay");
const subOverlay = document.querySelector(".sub-overlay");
const optionBox = document.querySelector(".option-box");
const newAvt = document.querySelector(".new-avt");
const deleteAvt = document.querySelector(".delete-avt");
const btnEdit = document.querySelector(".btn-edit");
const editBox = document.querySelector(".edit-info-box");
const viewDetailPost = document.querySelector(".view-detail-post");
const detailPost = document.querySelector(".detail-post");
const detailLikeBox = document.querySelector(".detail-like-box");


up.addEventListener("click", function () {
    if (!up.classList.contains("selected")) {
        up.classList.add("selected");
        share.classList.remove("selected");
        line.style.left = this.offsetLeft + "px";
        line.style.width = this.offsetWidth + "px";
    }
});

share.addEventListener("click", function () {
    if (!share.classList.contains("selected")) {
        share.classList.add("selected");
        up.classList.remove("selected");
        line.style.left = this.offsetLeft + "px";
        line.style.width = this.offsetWidth + "px";
    }
});



//detailLikeNum.addEventListener("click", function (e) {
//    subOverlay.classList.remove("hide");
//    detailLikeBox.classList.remove("hide");
//    // page.style.height = "100vh";
//    page.style.overflow = "hidden";
//});

// Close overlay
//overlay.addEventListener("click", function () {
//    overlay.classList.add("hide");
//    optionBox.classList.add("hide");
//    editBox.classList.add("hide");
//    detailPost.classList.add("hide");
//    page.style.height = "";
//    page.style.overflow = "";
//});

//subOverlay.addEventListener("click", function () {
//    subOverlay.classList.add("hide");
//    detailLikeBox.classList.add("hide");
//    page.style.height = "";
//    page.style.overflow = "";
//});

// Stop propagation
//newAvt.addEventListener("click", function (event) {
//    event.stopPropagation();
//});

//deleteAvt.addEventListener("click", function (event) {
//    event.stopPropagation();
//});

//editBox.addEventListener("click", function (event) {
//    event.stopPropagation();
//});

//detailPost.addEventListener("click", function (event) {
//    event.stopPropagation();
//});

//detailLikeBox.addEventListener("click", function (event) {
//    event.stopPropagation();
//});

//detailLikeBox.addEventListener("scroll", function () {
//    if (detailLikeBox.scrollHeight > detailLikeBox.clientHeight) {
//        container.style.overflow = "hidden";
//    } else {
//        container.style.overflow = "auto";
//    }
//});



//View Detail Post
class threadUp extends PopupBase {
    constructor() {
        super();
        this.init();
    }

    init() {
        var _this = this;
        var caption = _this.querySelector('.caption.link-detail');
        caption.addEventListener('click', function () {
            _this.initPopup(_this.querySelector('.detail-post').innerHTML);
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

//Like-detail-box
class threadDetail extends PopupBase {
    constructor() {
        super();
        this.init();
    }

    init() {
        var _this = this;
        var numLike = _this.querySelector('.num-like.detail-like.detail');
        numLike.addEventListener('click', function () {
            _this.initPopup(_this.querySelector('thread-detail-like .detail-like-box').innerHTML);
        })
    }
}
customElements.define('thread-detail', threadDetail);
