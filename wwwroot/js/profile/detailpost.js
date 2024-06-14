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

