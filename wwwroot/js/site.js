// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

class threadMain extends HTMLElement {
    constructor() {
        super();
        this.heart = this.querySelector(".heart");
        this.number = this.querySelector(".number")
        this.init();
        this.saveScroll();
        this.number.appendChild(this.createMainNumber(767, "new"));
        this.realTimeHeart();
        setTimeout(() => {
            this.realTimeHeart();
        }, 3000)
        setTimeout(() => {
            this.realTimeHeart();
        }, 6000)
    }
    init() {
        var _this = this
        this.heart.addEventListener("click", _this.likeSlideUp.bind(_this))
    }
    realTimeHeart() {
        var _this = this
        var heart_realtime = this.createMainNumber(989, "heart-reatltime")
        this.number.appendChild(heart_realtime);
       
        if (_this.number.querySelector(".main_number.new")) {
            _this.number.querySelector(".main_number.new").remove(); 
        }
        heart_realtime.classList.add("slide-upp")
        _this.number.querySelector(".main_number:not(.slide-upp)").classList.add("opacity-0")
        heart_realtime.addEventListener("animationend", (e) => {
            _this.number.querySelector(".main_number:not(.slide-upp)").remove();
            e.target.classList.remove("slide-upp");
        })
        
    }
    likeSlideUp() {
        var _this = this
        if (!_this.number.querySelector(".new")) {
            this.number.appendChild(this.createMainNumber(767, "new"));
        }
        if (_this.number.classList.contains("active")) {
            _this.number.querySelectorAll(".main_number").forEach((item) => {
                item.classList.remove("translate-y--50")
            })
            _this.number.classList.remove("active");
        } else {
            _this.number.classList.add("active");
            _this.number.querySelectorAll(".main_number").forEach((item) => {
                item.classList.add("translate-y--50")
            })

        }
    }
    createMainNumber(number, classes) {
        var mainNumber = document.createElement("div")
        mainNumber.classList.add("transition", "fs-15", "main_number", classes )
        mainNumber.innerHTML = number;
        return mainNumber
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
            sessionStorage.setItem('currentScroll', JSON.stringify(currentScroll));
            sessionStorage.setItem('previous', '/');
        })
    }
}  
customElements.define("thread-main", threadMain)


class PopupBase extends HTMLElement {
    constructor() {
        super();
        this.customClass = this.dataset.customClass;
        this.modal = null;
    }

    initPopup(content, text) {
        const _this = this;
        this.modal = new tingle.modal({
            footer: false,
            stickyFooter: false,
            closeMethods: ["overlay", "button", "escape"],
            cssClass: [this.customClass],
            onOpen: function () { },
            onClose: function () { },
            beforeClose: function () {
                _this.onCloseEvent();
                return true;
            },
        });
        this.modal.setHeader = function (content) {
            let popup_content = document.querySelector(".tingle-modal-box__content");
            let popup_header = document.createElement("div");
            popup_header.classList.add("tingle-modal-box__header");
            popup_header.innerHTML = content;
            let parentElement = popup_content.parentNode;
            parentElement.insertBefore(popup_header, popup_content);
        };
        if (text) {
            this.modal.setHeader(text);
        }

        this.modal.setContent(content);
        this.modal.open();
    }

    onClose() {
        this.modal.close();
    }
    onCloseEvent() { }
}

class popupCreate extends PopupBase {
    constructor() {
        super()
        this.init()
    }
    init() {
        var _this = this
        this.addEventListener("click", () => {
        
            _this.initPopup(_this.querySelector(".create_thread").innerHTML)
            var slide = document.querySelector(".swiper-wrapper")
           
        })
    }

}
customElements.define("popup-create", popupCreate)


class CustomTextArea extends HTMLTextAreaElement {
    constructor() {
        super();
        this.addEventListener('input', this.resize.bind(this));
    }


    resize() {
        this.style.height = 'auto';
        this.style.height = Math.min(this.scrollHeight, 200) + 'px';
    }
}

customElements.define('custom-text', CustomTextArea, { extends: 'textarea' });

class commentPopup extends PopupBase {
    constructor() {
        super()
        this.init()
    }
    init() {
        var _this = this
        this.addEventListener("click", () => {

            _this.initPopup(_this.querySelector(".create_comment").innerHTML)

        })
    }
}

customElements.define("comment-popup", commentPopup)