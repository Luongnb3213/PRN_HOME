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
        this.fnClose = null;
        this.fnOpen = null;
    }

    initPopup(content, text) {
        const _this = this;
        this.modal = new tingle.modal({
            footer: false,
            stickyFooter: false,
            closeMethods: ["overlay", "button", "escape"],
            cssClass: [this.customClass],
            onOpen: function () { },
            onClose: function () {
                if (this.fnClose) {
                    this.fnClose()
                }
            },
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

var blsInputAppendImage = (function () {
    return {
        init: function () {
            var inputImages = document.querySelector('.tingle-modal-box input[name="UploadedFiles"]');
            var slideSection = document.querySelector('.tingle-modal-box slide-section .swiper-wrapper');
            
            if (inputImages) {
                inputImages.addEventListener('change', (e) => {
                    slideSection.innerHTML = '';
                    if (e.target.files) { 
                        Array.from(e.target.files).forEach((item) => {
                            var fileURL = URL.createObjectURL(item);
                            var swiperSlide = document.createElement("div")
                            swiperSlide.classList.add("swiper-slide")
                            if (item.type.includes("mp4")) {
                                swiperSlide.innerHTML = `<custom-media class="h-full pointer d-block">
                                                   <video src="${fileURL}" controls></video>
                                               </custom-media>`;
                            } else {
                                swiperSlide.innerHTML = `<custom-media class="h-full pointer d-block">
                                                   <a data-pswp-src="${fileURL}">
                                                       <img src="${fileURL}" class="w-full" />
                                                   </a>
                                               </custom-media>`;
                            }
                            slideSection.appendChild(swiperSlide)


                        });
                    }
                  
                });
            }
        },
        deleteSlide: function () {
            var slideSection = document.querySelector('.tingle-modal-box slide-section .swiper-wrapper');
            slideSection.innerHTML= ''
        }
    }

})()

class popupCreate extends PopupBase {
    constructor() {
        super()
        this.init()
    }
    init() {
        var _this = this
        this.addEventListener("click", () => {
            _this.fnClose = blsInputAppendImage.deleteSlide;
            _this.initPopup(_this.querySelector(".create_thread").innerHTML)
            var slide = document.querySelector(".swiper-wrapper")
            blsInputAppendImage.init()
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



class commentReply extends HTMLElement {
    constructor() {
        super()
        this.inputImage = this.querySelector('input[name="UploadedFiles"]')
        
        this.init()
    }
    init() { 
        var _this = this
        if (_this.inputImage) {
            _this.inputImage.addEventListener("change", (e) => {
                var custom_media = _this.querySelector(".custom_media")
                if (custom_media) {
                    custom_media.remove();
                }
                if (e.target.files[0]) { 
                    var fileURL = URL.createObjectURL(e.target.files[0]);
                    var custom_media_1 = document.createElement("div")
                    custom_media_1.classList.add("mt-10", "w-50", "mb-10", "relative", 'custom_media')
                    custom_media_1.innerHTML += `  <custom-media>
                                                <img src="${fileURL}"  />
                                                <span class="shutdonw_icon absolute transition pointer top-0 right-0">
                                                    <svg width="20" height="20" viewBox="0 0 24 24" fill="white" xmlns="http://www.w3.org/2000/svg">
                                                        <path d="M19 5L5 19" stroke="#333333" stroke-width="4" stroke-linecap="round" />
                                                        <path d="M5 5L19 19" stroke="#333333" stroke-width="4" stroke-linecap="round" />
                                                    </svg>
                                                </span>
                                                </custom-media>
                                           `
                    _this.appendChild(custom_media_1)
                }
                
            })
        }
    }
}
customElements.define("comment-reply", commentReply)
