
var time;   
function Debounce(value) {

    if (time) {
        clearTimeout(time)
    }
    return new Promise((resolve, reject) => {
        time = setTimeout(() => {
            resolve(value)
        }, 1500)
    })
}


var BlsSearch = (function () {
    return {
        init: function () {
            var text = document.querySelector("input[name='text']");
            if (text) {
                text.addEventListener('input', (e) => {
                    var value = Debounce(e.target.value).then((text_value) => {
                        console.log(JSON.stringify(text_value))
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