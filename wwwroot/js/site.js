function foldingInformation() {

    let foldingButtons = document.getElementsByClassName("foldingBtn");

    for (let i = 0; i < foldingButtons.length; i++) {
   
        foldingButtons[i].addEventListener("click", function () {

            let foldingText = this.nextElementSibling;
 
            if (foldingText.style.display === "block") {
                foldingText.style.display = "none";

                foldingText.style.borderBottom = "none";

                this.setAttribute("aria-expanded", "false");

                this.lastElementChild.classList.remove("fa-sort-up");
                this.lastElementChild.classList.add("fa-sort-down");

            } else {
                foldingText.style.display = "block";

                this.setAttribute("aria-expanded", "true");

                this.lastElementChild.classList.remove("fa-sort-down");
                this.lastElementChild.classList.add("fa-sort-up");

            }
        })
    }
}

foldingInformation();