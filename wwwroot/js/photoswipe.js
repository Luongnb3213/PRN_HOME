import PhotoSwipeLightbox from '/js/lib/photoswipe-lightbox.esm.js';


class customImage extends HTMLElement {
    constructor() {
        super();
        this.element = this.querySelector("img") || this.querySelector("video");
        this.a = this.querySelector("a")
        this.init();
    }
    init() {

        var naturalHeight = parseFloat(this.element.naturalHeight) || parseFloat(this.element.videoHeight);
        var naturalWidth = parseFloat(this.element.naturalWidth) || parseFloat(this.element.videoWidth);
        console.log(naturalHeight)
        console.log(naturalWidth)
        var aspect_ratio = naturalWidth / naturalHeight
        this.style = `--aspect-ratio :  ${aspect_ratio}`
        var fixedHeight = 1500; 
        var calculatedWidth = fixedHeight * aspect_ratio;
        if (this.a) {
            this.a.setAttribute("data-pswp-width", calculatedWidth);
            this.a.setAttribute("data-pswp-height", fixedHeight);
        }
     
    }
}
customElements.define("custom-media", customImage);
class SlideSection extends HTMLElement {
    constructor() {
        super();
        this.globalSlide = null;
        this.thumbnailSlide = null;
        this.lightbox = null
        this.id = this.getAttribute("id")
        this.init();
    }
    init() {
        this.initSlide();
        this.initLightBox();

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
    initLightBox() {
        this.lightbox = new PhotoSwipeLightbox({
            // may select multiple "galleries"
            gallery: `#gallery--getting-started-${this.id}`,

            // Elements within gallery (slides)
            children: 'a',

            // setup PhotoSwipe Core dynamic import
            pswpModule: () => import('/js/lib/photoswipe.esm.js'),
            wheelToZoom: true,
            errorMsg: 'The photo cannot be loaded',
            closeTitle: 'Close the dialog',
            zoomTitle: 'Zoom the photo',
            arrowPrevTitle: 'Go to the previous photo',
            arrowNextTitle: 'Go to the next photo',
            indexIndicatorSep: ' of '
        });
        this.lightbox.init();
    }

}
customElements.define("slide-section", SlideSection);
