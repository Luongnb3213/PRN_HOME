// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var con = new signalR.HubConnectionBuilder().withUrl("/hub").build();
con.start().then().catch(function (arr) {
    return console.log(err.toString())
})

class threadMain extends HTMLElement {
    constructor() {
        super();
        this.id = parseInt(this.dataset?.id)
        this.heart = this.querySelector(".heart");
        this.number = this.querySelector(".number")
        this.react = parseInt(this.dataset.react);
        this.init();
        this.saveScroll();
        this.number.appendChild(this.createMainNumber(this.react, "new"));
        this.doReact();
    }
    init() {
        var _this = this
        this.heart.addEventListener("click", _this.likeSlideUp.bind(_this))
        con?.on("ReceiveMessage", function (currentReactAfter, threadID) {
            if (_this.id == threadID) {
                _this.realTimeHeart(currentReactAfter)
            }

        })
    }
    realTimeHeart(currentReactAfter) {
        var _this = this
        var heart_realtime = this.createMainNumber(currentReactAfter, "heart-reatltime")
        this.number.appendChild(heart_realtime);
        this.setAttribute("data-react", currentReactAfter)
        this.number.classList.remove("active")
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
            if (this.heart.classList.contains('reacted')) {
                this.number.appendChild(this.createMainNumber(parseInt(this.dataset.react) - 1, "new"));
            } else {
                this.number.appendChild(this.createMainNumber(parseInt(this.dataset.react) + 1, "new"));
            }
           
        }
        if (_this.number.classList.contains("active")) {
            _this.number.querySelectorAll(".main_number").forEach((item) => {
                item.classList.remove("translate-y--50")
            })
         

            _this.number.classList.remove("active");
        } else {
            _this.number.classList.add("active");
            // call ajax
            _this.number.querySelectorAll(".main_number").forEach((item) => {
                item.classList.add("translate-y--50")
            })

        }
    }
    createMainNumber(number, classes) {
        var mainNumber = document.createElement("div")
        mainNumber.classList.add("transition", "fs-15", "main_number", classes)
        mainNumber.innerHTML = number;
        return mainNumber
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
                    url: `?handler=Reacted&typeReact=up&threadId=${_this.querySelector('.thread-id').innerHTML}`,
                    method: 'POST',
                    processData: false,
                    contentType: false,
                    success: async function (data) {

                        _this.setAttribute("data-react", data.currentReactAfter)
                        try {
                            await con?.invoke("SendMessage", data.currentReactAfter, parseInt(_this.id));
                        } catch (err) {
                            console.error(err);
                        }
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
                    url: `?handler=Reacted&typeReact=down&threadId=${_this.querySelector('.thread-id').innerHTML}`,
                    method: 'POST',
                    processData: false,
                    contentType: false,
                    success: async function (data) {
                        _this.setAttribute("data-react", data.currentReactAfter)
                  
                        try {
                            await con?.invoke("SendMessage", data.currentReactAfter, parseInt(_this.id));
                        } catch (err) {
                            console.error(err);
                        }
                    },
                    error: function (error) {
                        console.log('Error:', error);
                    }
                });
            }
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
            slideSection.innerHTML = ''
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



class buttonFollow extends HTMLElement {
    constructor() {
        super()
        this.id = this.dataset.followdid

        this.init()
    }
    init() {
     
    }
}
customElements.define("button-follow", buttonFollow)

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

class timeConvert extends HTMLElement {
    constructor() {
        super()
        this.time = this.dataset?.time

        this.init()
    }
    init() {
        if (this.time) {
            this.innerHTML = timeSince(this.time)

        }
    }
}
customElements.define("time-convert", timeConvert)



con.on("receiveNofication", function (userId,nofi, account, newThread, count, threadId) {
    var html = ''

    if (nofi == 1) {
        html = ` <noti>
                        <div  class="a-link-noti">
                            <div class="noti-wrapper">
                                <div class="noti-avt-wrapper">
                                    <img src="${account.image}" width="36" height="36" class="noti-avt" />
                                </div>
                                <div class="noti-content-wrapper">
                                    <div class="noti-content-header">
                                        <a class="show-specific-profile" href="/profile?userId=${userId}"> ${account.userName} </a>

                                        <time-convert class="noti-time">now</time-convert>
                                    </div>
                                    <a href="/detailpost?threadId=${count}" class="noti-content-body">
                                        <div class="noti-content-message">Liked a post</div>
                                        <div class="post_content">
                                           ${newThread}
                                        </div>
                                    </a>
                                </div>
                            </div>
                        </div>
                        <div class="line"></div>
                    </noti>`
    } else if (nofi == 2) {
        html =`  <nofi>
                        <div class="flex flex-column py-15">
                            <div class="flex flex-cols gap-10  flex-wrap">
                                <div class="thread_info col-w-custom" style="width: 48px">
                                    <div class="flex h-full flex-column">
                                        <img src="${account.image}" width=" 36" height="36" class="noti-avt" />
                                        <div class="line_main mx-auto flex-1 hidden">
                                        </div>
                                    </div>


                                </div>
                                <div class="thread_main col-w-custom" style="--col-width: calc(100% - 48px - 30px);">
                                    <div class="flex mb-2 align-center justify-between">
                                        <div class="flex flex-column">
                                            <h3 class="author flex align-center fs-15">
                                                <a class="show-specific-profile"  href="/profile?userId=${userId}">${account.userName}</a>
                                                <time-convert class="noti-time">now</time-convert>
                                            </h3>
                                            <h4 class="author_nickname">${account.name}</h4>
                                        </div>
                                        <button-follow data-followdid="${userId}" style="margin-left: auto;background-color: rgb(24, 24, 24); color: white" class="button_create px-15  py-2">
                                           ${newThread ? 'Đang theo dõi' : "Theo dõi "}
                                        </button-follow>
                                    </div>
                                    <span class="follow_text">
                                        
                                        ${count} người theo dõi 
                                    </span>

                                </div>
                            </div>



                        </div>
                        <div class="line"></div>
                    </nofi>`
    } else {
        html = ` <noti>
                        <div  class="a-link-noti">
                            <div class="noti-wrapper">
                                <div class="noti-avt-wrapper">
                                    <img src=" ${account}" width="36" height="36" class="noti-avt" />
                                </div>
                                <div class="noti-content-wrapper">
                                    <div class="noti-content-header">
                                        <a class="show-specific-profile" href="/profile?userId=${userId}"> ${newThread}</a>

                                        <time-convert class="noti-time">now</time-convert>
                                    </div>
                                    <a href="/detailpost?threadId=${threadId}" class="noti-content-body">
                                        <div class="noti-content-message">Created a post</div>
                                        <div class="post_content">
                                           ${count}
                                        </div>
                                    </a>
                                </div>
                            </div>
                        </div>
                        <div class="line"></div>
                    </noti>`
    }
    var fisrtNofi = document.querySelectorAll("noti")[0]
    if (fisrtNofi) {
        $(html).insertBefore(fisrtNofi);
    } else {
        $(".main_noti").append(html)
    }
   
})
