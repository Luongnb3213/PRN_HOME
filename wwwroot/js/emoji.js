import { EmojiButton } from '/js/lib/emoji-button.min.js';
class emojiBox extends HTMLElement {
    constructor() {
        super();
        this.picker = null
        this.init()
    }
    init() {
        emojione.emojiSize = 32;
        emojione.ascii = true;
        const button = this.querySelector('.emoji-button');
        const emojiContainer = this.closest("comment-reply")?.querySelector("input[name='comment']");
        const picker = new EmojiButton();
            picker.on('emoji', emoji => {
                if (emojiContainer) {
                    emojiContainer.value += emoji.emoji
                }
            });
        if (button) {
            button.addEventListener('click', () => {
                picker.togglePicker(button);
            });
        }
           
    }
}
customElements.define("emoji-box", emojiBox)


class commetContent extends HTMLElement{
    constructor() {
        super();
        this.init()
    }
    init() {
        this.innerHTML = emojione.toImage(this.innerHTML);
    }
}
customElements.define("commet-content", commetContent)
