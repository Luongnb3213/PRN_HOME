
var time;   
function Debounce(value) {

    if (time) {
        clearTimeout(time)
    }
    return new Promise((resolve, reject) => {
        time = setTimeout(() => {
            resolve(value)
        }, 5000)
    })
}


var BlsSearch = (function () {
    return {
        init: function () {
            var text = document.querySelector("input[name='text']");
            var loader_search = document.querySelector("input[name='text'] ~ .loader_search")
            var thread_body_search_loader = document.querySelector(".thread_body_search_loader")
            if (text) {
                text.addEventListener('input', (e) => {
                    loader_search.classList.remove("hidden")
                    thread_body_search_loader.classList.remove("hidden")
                    if (e.target.value.trim() === '') {
                        
                        loader_search.classList.add("hidden")
                        clearTimeout(time)
                        $.ajax({
                            headers:
                            {
                                "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                            },
                            url: `/Search?handler=SearchValue`,
                            method: 'POST',
                            processData: true,
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            data: JSON.stringify(e.target.value),
                            success: function (data) {
                                loader_search.classList.add("hidden");
                                thread_body_search_loader.classList.add("hidden")
                                var thread_body_search_content = document.querySelector(".thread_body_search_content");
                                let html = ''
                                console.log(data.text)
                                data.data.forEach((item) => {

                                    html += `
                                         <div class="flex flex-cols gap-20 flex-wrap">
                        <div class="thread_info col-w-custom" style="width: 48px">
                            <div class="flex h-full flex-column">
                                <img width="48" height="48" class="avt-author" src="${item.info.image}" />
                                <div class="line_main mx-auto flex-1 hidden">
                                </div>
                            </div>


                        </div>
                        <div class="thread_main pb-3 col-w-custom" style="--col-width: calc(100% - 48px - 30px);border-bottom: 1px solid rgba(243, 245, 247, 0.15);">
                            <div class="flex mb-2 align-center justify-between">
                                <div class="flex flex-column">
                                    <h3 class="author flex align-center fs-16">
                                        <a class="show-specific-profile" href="/profile?userId=${item.info.userID}">${item.info.name}</a>
                                        <span class="time-elapsed"></span>
                                    </h3>
                                    <h4 class="author_nickname">${item.info.userName}</h4>
                                </div>

                            </div>
                            <span class="follow_text">
                               ${item.countUser} nguoi theo doi
                            </span>

                        </div>
                    </div>

                                    `

                                })
                                thread_body_search_content.innerHTML = html;
                            }
                        })
                        return;
                    }
                    var value = Debounce(e.target.value).then((text_value) => {
                        
                        $.ajax({
                            headers:
                            {
                                "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                            },
                            url: `/Search?handler=SearchValue`,
                            method: 'POST',
                            processData: true,
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            data: JSON.stringify(text_value),
                            success: function (data) {
                                loader_search.classList.add("hidden");
                                thread_body_search_loader.classList.add("hidden")

                                var thread_body_search_content = document.querySelector(".thread_body_search_content");
                                let html = ''
                                console.log(data.text)
                                data.data.forEach((item) => {

                                    html += `
                                         <div class="flex flex-cols gap-20 flex-wrap">
                        <div class="thread_info col-w-custom" style="width: 48px">
                            <div class="flex h-full flex-column">
                                <img width="48" height="48" class="avt-author" src="${item.info.image}" />
                                <div class="line_main mx-auto flex-1 hidden">
                                </div>
                            </div>


                        </div>
                        <div class="thread_main pb-3 col-w-custom" style="--col-width: calc(100% - 48px - 30px);border-bottom: 1px solid rgba(243, 245, 247, 0.15);">
                            <div class="flex mb-2 align-center justify-between">
                                <div class="flex flex-column">
                                    <h3 class="author flex align-center fs-16">
                                        <a class="show-specific-profile" href="/profile?userId=${item.info.userID}">${item.info.name}</a>
                                        <span class="time-elapsed"></span>
                                    </h3>
                                    <h4 class="author_nickname">${item.info.userName}</h4>
                                </div>

                            </div>
                            <span class="follow_text">
                               ${item.countUser} nguoi theo doi
                            </span>

                        </div>
                    </div>

                                    `

                                })
                                thread_body_search_content.innerHTML = html;
                            }
                        })
                    })
                })
            }
        }
    }
})()
BlsSearch.init()