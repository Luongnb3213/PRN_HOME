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
        var _this = this;
        let aLink = _this.querySelector('.link-detail');
        aLink.addEventListener('click', function () {
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
class SlideSection extends HTMLElement {
    constructor() {
        super();
        this.globalSlide = null;
        this.thumbnailSlide = null;
        this.init();
    }
    init() {
        this.initSlide();
    }

    initSlide() {
        const _this = this;
        var autoplaying = _this?.dataset.autoplay === "true";
        const loop = _this?.dataset.loop === "true";
        const itemDesktop = _this?.dataset.desktop ? _this?.dataset.desktop : 4;
        const itemTablet = _this?.dataset.tablet ? _this?.dataset.tablet : 2;
        const itemMobile = _this?.dataset.mobile ? _this?.dataset.mobile : 1;
        const direction = _this?.dataset.direction
            ? _this?.dataset.direction
            : "horizontal";
        var autoplaySpeed = _this?.dataset.autoplaySpeed
            ? _this?.dataset.autoplaySpeed * 1000
            : 3000;
        var speed = _this?.dataset.speed ? _this?.dataset.speed : 400;
        const effect = _this?.dataset.effect ? _this?.dataset.effect : "slide";
        const row = _this?.dataset.row ? _this?.dataset.row : 1;
        var spacing = _this?.dataset.spacing ? _this?.dataset.spacing : 30;
        const progressbar = _this?.dataset.paginationProgressbar === "true";
        const autoItem = _this?.dataset.itemMobile === "true";
        const arrowCenterimage = _this?.dataset.arrowCenterimage
            ? _this?.dataset.arrowCenterimage
            : 0;
        spacing = Number(spacing);
        autoplaySpeed = Number(autoplaySpeed);
        speed = Number(speed);
        if (autoplaying) {
            autoplaying = { delay: autoplaySpeed };
        }
        if (direction == "vertical") {
            _this.style.maxHeight = _this.offsetHeight + "px";
        }
        console.log(itemDesktop)
        this.globalSlide = new Swiper(_this, {
            slidesPerView: autoItem ? "auto" : itemMobile,
            spaceBetween: spacing >= 15 ? 15 : spacing,
            autoplay: autoplaying,
            direction: direction,
            loop: loop,
            effect: effect,
            speed: speed,
            watchSlidesProgress: true,
            watchSlidesVisibility: true,
            grid: {
                rows: row,
                fill: "row",
            },
            navigation: {
                nextEl: _this.querySelector(".swiper-button-next"),
                prevEl: _this.querySelector(".swiper-button-prev"),
            },
            pagination: {
                clickable: true,
                el: _this.querySelector(".swiper-pagination"),
                type: progressbar ? "progressbar" : "bullets",
            },
            breakpoints: {
                768: {
                    slidesPerView: itemTablet,
                    spaceBetween: spacing >= 30 ? 30 : spacing,
                },
                1025: {
                    slidesPerView: itemDesktop,
                    spaceBetween: spacing,
                },
            },
            thumbs: {
                swiper: this.thumbnailSlide ? this.thumbnailSlide : null,
            },

            on: {
               
            },
        });
    }

    
}
customElements.define("slide-section", SlideSection);

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


const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub")
    .withAutomaticReconnect([0, 0, 10000])
    .build();
connection.start().then(() => {
    console.log(connection.connection.connectionId)
}).catch(err => console.log(err.toString()));

