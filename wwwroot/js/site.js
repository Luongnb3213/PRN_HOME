// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
class threadMain extends HTMLElement {
    constructor() {
        super();
        this.heart = this.querySelector(".heart");
        this.number = this.querySelector(".number")
        this.init();
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
            _this.initPopup(`<h1>Firrst Popup</h1>`)
        })
    }
}
customElements.define("popup-create", popupCreate)