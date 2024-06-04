const up = document.querySelector(".tab-left");
const share = document.querySelector(".tab-right");
const line = document.querySelector(".selected-line");
const overlay = document.querySelector(".overlay");
const optionBox = document.querySelector(".option-box");
const avatar = document.querySelector(".avatar");
const newAvt = document.querySelector(".new-avt");
const deleteAvt = document.querySelector(".delete-avt");
const btnEdit = document.querySelector(".btn-edit");
const editBox = document.querySelector(".edit-info-box");
const btnStatus = document.querySelector(".btn-private");
const ball = document.querySelector(".btn-ball");

up.addEventListener("click", function () {
    if (!up.classList.contains("selected")) {
        up.classList.add("selected");
        share.classList.remove("selected");
        line.style.left = this.offsetLeft + "px";
        line.style.width = this.offsetWidth + "px";
    }
});

share.addEventListener("click", function () {
    if (!share.classList.contains("selected")) {
        share.classList.add("selected");
        up.classList.remove("selected");
        line.style.left = this.offsetLeft + "px";
        line.style.width = this.offsetWidth + "px";
    }
});

avatar.addEventListener("click", function () {
    overlay.classList.remove("hide");
    optionBox.classList.remove("hide");
    editBox.classList.add("hide");
});

overlay.addEventListener("click", function () {
    overlay.classList.add("hide");
    optionBox.classList.add("hide");
    editBox.classList.add("hide");
});

btnEdit.addEventListener("click", function () {
    overlay.classList.remove("hide");
    editBox.classList.remove("hide");
    optionBox.classList.add("hide");
});

btnStatus.addEventListener("click", function (event) {
    if (btnStatus.classList.contains("public")) {
        btnStatus.classList.remove("public");
        ball.style.right = "3px";
        ball.style.left = "auto";
    } else {
        btnStatus.classList.add("public");
        ball.style.left = "3px";
        ball.style.right = "auto";
    }
});

// Stop propagation
newAvt.addEventListener("click", function (event) {
    event.stopPropagation();
});

deleteAvt.addEventListener("click", function (event) {
    event.stopPropagation();
});

editBox.addEventListener("click", function (event) {
    event.stopPropagation();
});
