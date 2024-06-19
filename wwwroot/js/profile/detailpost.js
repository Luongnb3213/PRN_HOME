var linkPrevious = document.querySelector('.a-back-page');
linkPrevious.setAttribute('href', sessionStorage.getItem('previous'));

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

//AJAX write comment
document.querySelector('.write-comment-btn-submit').addEventListener('click', function () {
    let currentThreadId = document.querySelector('.currentThreadId-origin');
    let typeComment = document.querySelector('.type-comment-origin');
    let commentContent = document.querySelector('.write-comment-box-content').value;
    let commentPic = document.querySelector('.write-comment-pics').files;

    let formData = new FormData();
    formData.append('content', commentContent);
    formData.append('type', typeComment.dataset.type)

    for (let i = 0; i < commentPic.length; i++) {
        formData.append('pictures', commentPic[i]);
    }

    $.ajax({
        headers:
        {
            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        url: `/detailpost?threadId=${currentThreadId.dataset.threadid}`,
        method: 'POST',
        processData: false,
        contentType: false,
        data: formData,
        success: function (data) {
            let commentBox = document.querySelector('.comment-box');
            //let noComment = document.querySelector('.no-comment-wrapper');
            //noComment.classList.add('hidden');
            console.log(data)
            let commentImageHTML = '';
            if (data.data.Comment.CommentImages.$values.length !== 0) {
                commentImageHTML = `<img src="${data.data.Comment.CommentImages.$values[0].Media}" class="comment-picture">`;
            }
            commentBox.insertAdjacentHTML('afterbegin', `
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
                    <div data-submit-date="${data.data.Comment.CreatedAt}" class="author-comment-time">Just now</div>
                </div>
                <div class="author-comment-body">
                    <div class="comment-content">${data.data.Comment.Content}</div>
                    ${commentImageHTML}
                </div>
                <div class="author-comment-footer">
                    <div class="react-wrapper author-comment-react">
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
                        <div class="wrapper-react-num" onclick="clickReply(event)">
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
    </comment-author>
    <div class="reply-area hidden"></div>
    <div class="reply-box hidden">
                                <form class="replyForm" method="post">
                                    <div class="write-reply-wrapper">
                                        <div class="write-reply-account">
                                            <img src="../pics/profile/shh.jpg" class="write-reply-account-avt" />
                                        </div>
                                        <div class="write-reply-box-wrapper">
                                            <div class="currentThreadId-reply" data-threadId="${data.data.ThreadId}" hidden></div>
                                            <div class="type-comment-reply" data-type="reply" hidden></div>
                                            <div class="threadCommentIdInsert" data-threadCommentId="${data.data.ThreadCommentId}" hidden></div>
                                            <input placeholder="Reply to ${data.data.Comment.Account.Info.userName}" type="text" class="write-reply-box-content" />
                                        </div>
                                        <div class="create_image relative thread_main_icon heart">
                                            <svg width="20" height="20" aria-label="Đính kèm file phương tiện" role="img" viewBox="0 0 24 24" class="x1lliihq xffa9am x1jwls1v x1n2onr6 x17fnjtu x1gaogpn" style="--fill: currentColor; --height: 20px; --width: 20px;"><title>Đính kèm file phương tiện</title><g><path clip-rule="evenodd" d="M2.00207 9.4959C1.65132 7.00019 1.47595 5.75234 1.82768 4.73084C2.13707 3.83231 2.72297 3.05479 3.50142 2.50971C4.38639 1.89005 5.63425 1.71467 8.12996 1.36392L10.7047 1.00207C13.2004 0.651325 14.4482 0.47595 15.4697 0.827679C16.3682 1.13707 17.1458 1.72297 17.6908 2.50142C17.9171 2.82454 18.0841 3.19605 18.2221 3.65901C17.7476 3.64611 17.2197 3.64192 16.6269 3.64055C16.5775 3.5411 16.5231 3.44881 16.4621 3.36178C16.0987 2.84282 15.5804 2.45222 14.9814 2.24596C14.3004 2.01147 13.4685 2.12839 11.8047 2.36222L7.44748 2.97458C5.78367 3.20841 4.95177 3.32533 4.36178 3.73844C3.84282 4.10182 3.45222 4.62017 3.24596 5.21919C3.01147 5.90019 3.12839 6.73209 3.36222 8.3959L3.97458 12.7531C4.15588 14.0431 4.26689 14.833 4.50015 15.3978C4.50083 16.3151 4.50509 17.0849 4.53201 17.7448C4.13891 17.4561 3.79293 17.1036 3.50971 16.6991C2.89005 15.8142 2.71467 14.5663 2.36392 12.0706L2.00207 9.4959Z" fill="currentColor" fill-rule="evenodd"></path><g><g clip-path="url(#:r2:)"><rect fill="none" height="15.5" rx="3.75" stroke="currentColor" stroke-width="1.5" width="15.5" x="6.75" y="5.8894"></rect><path d="M6.6546 17.8894L8.59043 15.9536C9.1583 15.3857 10.0727 15.3658 10.6647 15.9085L12.5062 17.5966C12.9009 17.9584 13.5105 17.9451 13.8891 17.5665L17.8181 13.6376C18.4038 13.0518 19.3536 13.0518 19.9394 13.6375L22.0663 15.7644" fill="none" stroke="currentColor" stroke-linejoin="round" stroke-width="1.5"></path><circle cx="10.75" cy="9.8894" fill="currentColor" r="1.25"></circle></g></g></g><defs><clipPath id=":r2:"><rect fill="white" height="17" rx="4.5" width="17" x="6" y="5.1394"></rect></clipPath></defs></svg>
                                            <input class="inset-0 write-reply-pics" type="file" accept="image/*,video/*" name="UploadedFiles">
                                        </div>
                                        <div class="write-reply-btn-emoji">
                                            <i class="fa-solid fa-face-smile"></i>
                                        </div>
                                        <div class="write-reply-btn-submit" onclick="clickReplyBtn(this)">
                                            <i class="fa-solid fa-paper-plane icon-submit"></i>
                                        </div>
                                    </div>
                                </form>
                            </div>
    <div class="detail-separate-line"></div>
</div>

            `)
            document.querySelector('.write-comment-box-content').value = '';
            document.querySelector('#commentForm').reset();
        }
    });
});

//AJAX reply comment
//document.querySelector('.write-reply-btn-submit').addEventListener('click',
function clickReplyBtn(current) {
    let element = current.closest('.comment-wrapper');
    let currentThreadId = element.querySelector('.currentThreadId-reply');
    let typeComment = element.querySelector('.type-comment-reply');
    let threadCommentId = element.querySelector('.threadCommentIdInsert');
    let commentContent = element.querySelector('.write-reply-box-content').value;
    let commentPic = element.querySelector('.write-reply-pics').files;

    console.log(commentContent)

    let formData = new FormData();
    formData.append('content', commentContent);
    formData.append('type', typeComment.dataset.type)
    formData.append('threadCommentId', threadCommentId.dataset.threadcommentid)

    for (let i = 0; i < commentPic.length; i++) {
        formData.append('pictures', commentPic[i]);
    }

    $.ajax({
        headers:
        {
            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        url: `/detailpost?threadId=${currentThreadId.dataset.threadid}`,
        method: 'POST',
        processData: false,
        contentType: false,
        data: formData,
        success: function (data) {
            console.log(data);
            let commentImageHTML = '';
            if (data.data.CommentImages.$values.length !== 0) {
                commentImageHTML = `<img src="${data.data.CommentImages.$values[0].Media}" class="comment-picture">`;
            }
            let replyArea = element.querySelector('.reply-area');
            replyArea.insertAdjacentHTML('beforeend', `
            <comment-author data-custom-class="comment-detail-like-box" class="comment-author comment-reply">
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
                                <img src="${data.data.Account.Info.Image}" class="author-comment-avt" />
                                <div class="author-comment-content">
                                    <div class="author-comment-header">
                                        <a href="" class="author-name">${data.data.Account.Info.userName}</a>
                                        <div data-submit-date="${data.data.CreatedAt}" class="author-comment-time">Just now</div>
                                    </div>
                                    <div class="author-comment-body">
                                        <div class="comment-content">${data.data.Content}</div>
                                        ${commentImageHTML}
                                    </div>
                                    <div class="author-comment-footer">
                                        <div class="react-wrapper author-comment-react">
                                            <div class="wrapper-react-num">
                                                <div class="heart">
                                                    <svg width="20" height="19" aria-label="Thích" role="img" viewBox="0 0 24 22"
                                                         style="--fill: transparent; --height: 19px; --width: 20px;">
                                                        <title>Thích</title>
                                                        <path d="M1 7.66c0 4.575 3.899 9.086 9.987 12.934.338.203.74.406 1.013.406.283 0 .686-.203 1.013-.406C19.1 16.746 23 12.234 23 7.66 23 3.736 20.245 1 16.672 1 14.603 1 12.98 1.94 12 3.352 11.042 1.952 9.408 1 7.328 1 3.766 1 1 3.736 1 7.66Z">
                                                        </path>
                                                    </svg>
                                                </div>
                                                <div class="num-like detail-like detail comment-detail-like-link">${data.data.React}</div>
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
                        </comment-author>
                        <div class="detail-separate-line"></div>
            `)
            element.querySelector('.write-reply-box-content').value = '';
            element.querySelector('.replyForm').reset();
            replyArea.classList.remove('hidden');
            let currentCount = element.querySelector('.num-comment.detail-comment').innerHTML
            element.querySelector('.num-comment.detail-comment').innerHTML = currentCount * 1 + 1;
        }
    });
}

//Form Comment
var formComment = document.querySelector('#commentForm');
formComment.addEventListener('keydown', function (e) {
    if (e.key === 'Enter') {
        e.preventDefault();
        document.querySelector('.write-comment-btn-submit').click();
    }
});


document.addEventListener('keydown', function (e) {
    if (e.target.matches('.replyForm .write-reply-box-content') && e.key === 'Enter') {
        e.preventDefault();
        e.target.closest('.replyForm').querySelector('.write-reply-btn-submit').click();
    }
});

//Reply
function clickReply(event) {
    let father = event.target.closest('.comment-wrapper');
    let replyBox = father.querySelector('.reply-box');
    if (replyBox.classList.contains('hidden')) {
        replyBox.classList.remove('hidden');
        if (father.querySelectorAll('.view-reply-wrapper').length > 0) {
            showMoreReply(event.target);
        }
    } else {
        replyBox.classList.add('hidden');
        hideReply(event.target);
    }
}

//View more
function showMoreReply(element) {
    let fatherWrapper = element.closest('.comment-wrapper');
    let replyArea = fatherWrapper.querySelector('.reply-area');
    replyArea.classList.remove('hidden');
}

function hideReply(element) {
    let fatherWrapper = element.closest('.comment-wrapper');
    let replyArea = fatherWrapper.querySelector('.reply-area');
    replyArea.classList.add('hidden');
}
