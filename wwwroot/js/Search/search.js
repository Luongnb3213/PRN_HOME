
var time;   
function Debounce(value) {

    if (time) {
        clearTimeout(time)
    }
    return new Promise((resolve, reject) => {
        time = setTimeout(() => {
            resolve(value)
        }, 10000)
    })
}


var BlsSearch = (function () {
    return {
        init: function () {
            var text = document.querySelector("input[name='text']");
            var loader_search = document.querySelector("input[name='text'] ~ .loader_search")
           
            if (text) {
                text.addEventListener('input', (e) => {
                    loader_search.classList.remove("hidden")
                    if (e.target.value.trim() === '') {
                        
                        loader_search.classList.add("hidden")
                        clearTimeout(time)
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
                                loader_search.classList.add("hidden")
                                console.log(data)
                            }
                        })
                    })
                })
            }
        }
    }
})()
BlsSearch.init()