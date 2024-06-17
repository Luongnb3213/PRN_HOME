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
    }
    init() {
        var _this = this
        this.heart.addEventListener("click", (e) => {
            _this.number.innerHTML = `<div style="transition: all 0.2s linear" class="number_1 fs-15">
                767
            </div>
            <div style="transition: all 0.2s linear" class="main-number fs-15">
                763
            </div>`
            setTimeout(() => {
                _this.number.classList.add("active");
            }, 300)

        })
    }
    saveScroll() {
    //    var _this = this;
    //    let aLink = _this.querySelector('.link-detail');
    //    aLink.addEventListener('click', function () {
    //        let scrollX = window.pageXOffset;
    //        let scrollY = window.pageYOffset;
    //        let currentScroll = {
    //            scrollX,
    //            scrollY
    //        }
    //        console.log(currentScroll);
    //        //setTimeout(() => {
    //        //    // Chuyển hướng đến href của thẻ <a>
    //        //    window.location.href = e.target.href;
    //        //}, 100);
    //        sessionStorage.setItem('currentScroll', JSON.stringify(currentScroll));
    //        sessionStorage.setItem('previous', '/');
    //    })
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


