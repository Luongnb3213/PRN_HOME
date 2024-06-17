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
        const button = this.querySelector('#emoji-button');
        const emojiContainer = this.querySelector('#emojiContainer');
        const testEmojin = this.querySelector('#testEmojin');
        const picker = new EmojiButton();

            picker.on('emoji', emoji => {
                const emojiElement = document.createElement('span');
                emojiElement.textContent = emoji.emoji;
                emojiContainer.innerHTML += emoji.emoji + " ";
                testEmojin.innerHTML = emojione.toShort(emojiContainer.innerHTML);
            });

            button.addEventListener('click', () => {
                picker.togglePicker(button);
            });
    }
}
customElements.define("emoji-box", emojiBox)
