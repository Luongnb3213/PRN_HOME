//Comment-detail-like
class commentBox extends PopupBase {
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
customElements.define('comment-box', commentBox)


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

