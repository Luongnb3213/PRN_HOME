var linkPrevious = document.querySelector('.a-back-page');
linkPrevious.setAttribute('href', localStorage.getItem('previous'));

//Comment-detail-like
class commentAuthor extends PopupBase {
    constructor() {
        super();
        this.init();
    }

    init() {
        var _this = this;
        var link = _this.querySelector('.comment-detail-like-link');
        link.addEventListener('click', function () {
            _this.initPopup(_this.querySelector('comment-detail-like').innerHTML);
        })
    }
}
customElements.define('comment-author', commentAuthor)


//Like-detail-box
class threadDetail extends PopupBase {
    constructor() {
        super();
        this.init();
    }

    init() {
        var _this = this;
        var numLike = _this.querySelector('.num-like.detail-like.detail');
        //console.log(numLike);
        numLike.addEventListener('click', function () {
            _this.initPopup(_this.querySelector('thread-detail-like .detail-like-box').innerHTML);
        })
    }
}
customElements.define('thread-detail', threadDetail);

document.addEventListener('DOMContentLoaded', function () {
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

    const timeElement = document.querySelector('.time');
    const submitDate = timeElement.getAttribute('data-submit-date');
    timeElement.textContent = timeSince(submitDate);

    var timeComments = document.querySelectorAll('.author-comment-time');
    timeComments.forEach((element) => {
        const submitDate = element.getAttribute('data-submit-date');
        element.textContent = timeSince(submitDate);
    })
});

document.querySelector('.write-comment-btn-submit').addEventListener('click', function () {
    let currentThreadId = document.querySelector('.currentThreadId');
    let commentContent = document.querySelector('.write-comment-box-content').value;
    $.ajax({
        headers:
        {
            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        url: `/detailpost?threadId=${currentThreadId.dataset.threadid}`,
        method: 'POST',
        data: JSON.stringify(commentContent),
        success: function (data) {
            let commentBox = document.querySelector('.comment-box');
            let noComment = document.querySelector('.no-comment-wrapper');
            noComment.classList.add('hidden');
            commentBox.insertAdjacentHTML('beforeend', `
<div class="comment-wrapper">
    <comment-author data-custom-class="comment-detail-like-box" class="comment-author">
        <comment-detail-like class="wrapper-detail-like-box hidden">
            <div class="detail-like-box ">
                <div class="detail-like-header">
                    <div class="detail-like--title">Likes</div>
                    <div class="detail-like--line"></div>
                </div>
                <div class="detail-like-content">
                    <div class="detail-like-person">
                        <div class="person-info">
                            <img src="../pics/profile/9edcbf73-6987-4b40-b0d5-1354d43e6bd5.jfif" class="person-avt" />
                            <div class="person-name">
                                <div class="person-username">giu_giot_le_sau</div>
                                <div class="person-fullname">Sát thủ đa tình</div>
                            </div>
                        </div>
                        <div class="person-follow-status">
                            <div class="btn-follow hide">Follow</div>
                            <div class="btn-unfollow ">Unfollow</div>
                        </div>
                    </div>
                    <div class="detail-like-person">
                        <div class="person-info">
                            <img src="../pics/profile/9edcbf73-6987-4b40-b0d5-1354d43e6bd5.jfif" class="person-avt" />
                            <div class="person-name">
                                <div class="person-username">giu_giot_le_sau</div>
                                <div class="person-fullname">Tối thứ 7</div>
                            </div>
                        </div>
                        <div class="person-follow-status">
                            <div class="btn-follow hide">Follow</div>
                            <div class="btn-unfollow">Unfollow</div>
                        </div>
                    </div>
                    <div class="detail-like-person">
                        <div class="person-info">
                            <img src="../pics/profile/9edcbf73-6987-4b40-b0d5-1354d43e6bd5.jfif" class="person-avt" />
                            <div class="person-name">
                                <div class="person-username">giu_giot_le_sau</div>
                                <div class="person-fullname">Tối chủ nhật</div>
                            </div>
                        </div>
                        <div class="person-follow-status">
                            <div class="btn-follow">Follow</div>
                            <div class="btn-unfollow hide">Unfollow</div>
                        </div>
                    </div>
                </div>
            </div>
        </comment-detail-like>
        <div class="comment-left-wrapper">
            <img src="${data.data.Comment.Account.Info.Image}" class="author-comment-avt" />
            <div class="author-comment-content">
                <div class="author-comment-header">
                    <a href="" class="author-name">${data.data.Comment.Account.Info.userName}</a>
                    <div data-submit-date="${data.data.Comment.CreatedAt}" class="author-comment-time"></div>
                </div>
                <div class="author-comment-body">
                    <div class="comment-content">${data.data.Comment.Content}</div>
                </div>
                <div class="author-comment-footer">
                    <div class="react-wrapper author-comment-react more-reply">
                        <div class="wrapper-react-num">
                            <div class="heart">
                                <svg width="20" height="19" aria-label="Thích" role="img" viewBox="0 0 24 22"
                                     style="--fill: transparent; --height: 19px; --width: 20px;">
                                    <title>Thích</title>
                                    <path d="M1 7.66c0 4.575 3.899 9.086 9.987 12.934.338.203.74.406 1.013.406.283 0 .686-.203 1.013-.406C19.1 16.746 23 12.234 23 7.66 23 3.736 20.245 1 16.672 1 14.603 1 12.98 1.94 12 3.352 11.042 1.952 9.408 1 7.328 1 3.766 1 1 3.736 1 7.66Z">
                                    </path>
                                </svg>
                            </div>
                            <div class="num-like detail-like detail comment-detail-like-link">${data.data.Comment.React}</div>
                        </div>
                        <div class="wrapper-react-num">
                            <div class="comment">
                                <svg width="20" height="19" aria-label="Trả lời" role="img" viewBox="0 0 24 24"
                                     style="--fill: currentColor; --height: 20px; --width: 20px;">
                                    <title>Trả lời</title>
                                    <path d="M20.656 17.008a9.993 9.993 0 1 0-3.59 3.615L22 22Z"></path>
                                </svg>
                            </div>
                            <div class="num-comment detail-comment">${data.data.Conversations.$values.length}</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="comment-right-wrapper">
            <div class="option-icon">
                <icon class="option-icon pointer more_options flex align-center justify-content-center rounded-50">
                    <svg width="20" height="20" aria-label="Xem thêm" role="img" viewBox="0 0 24 24"
                         class="x1lliihq xffa9am x1jwls1v x1n2onr6 x17fnjtu x1gaogpn"
                         style="fill: #777777; --height: 20px; --width: 20px;">
                        <title>Xem thêm</title>
                        <circle cx="12" cy="12" r="1.5"></circle>
                        <circle cx="6" cy="12" r="1.5"></circle>
                        <circle cx="18" cy="12" r="1.5"></circle>
                    </svg>
                </icon>
            </div>
        </div>
        <div class="view-reply-wrapper">
            <div class="view-reply-line "></div>
            <div class="view-reply ">View (1) more reply</div>
        </div>
        <div class="view-reply-wrapper">
            <div class="view-reply-line hidden"></div>
            <div class="hide-reply hidden">Hide all replies</div>
        </div>
    </comment-author>
    <div class="detail-separate-line"></div>
</div>

            `)
        },
        dataType: "json",
        contentType: "application/json",

    });
});