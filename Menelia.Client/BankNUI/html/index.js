window.addEventListener("load", () => {
    let container = document.getElementById("main");

    function display(bool) {
        container.hidden = !bool;
    }

    display(false);

    window.addEventListener("message", function(event) {
        var item = event.data;
        console.log("message");
        if (item.type == "ui") {
            if (item.status == "True") {
                console.log("show");
                display(true);
            } else {
                console.log("hide");
                display(false);
            }
        }
    });

    async function sendCode(code, data) {
        fetch("https://MeneliaClient/" + code, {
            method: 'POST',
            mode: 'no-cors',
            headers: {
                'Content-Type': 'application/json; charset=UTF-8',
            },
            body: JSON.stringify(data)
        }).catch(error => {});
    }

    /**
     * Pages system
     */
    document.getElementById("withdrawal-button").addEventListener('click', () => {loadPage("page-withdrawal");});
    document.getElementById("deposit-button").addEventListener('click', () => {loadPage("page-deposit");});
    document.getElementById("person-button").addEventListener('click', () => {loadPage("page-person");});
    document.getElementById("history-button").addEventListener('click', () => {loadPage("page-history");});

    let pageContainer = document.getElementById("page-container");
    loadPage("main-page");

    function loadPage(name) {
        let page = document.getElementById(name);

        for (let node of pageContainer.childNodes) {
            node.hidden = true;
        }

        page.hidden = false;

        if(!document.getElementById("main-page").hidden)
            document.getElementById("back").style.display = "none";
        else
            document.getElementById("back").style.display = "initial";
    }

    /**
     * Close & rewind system
     */
     document.getElementById("close").addEventListener("click", () => {sendCode("close", {});});
     document.getElementById("back").addEventListener('click', () => {loadPage("main-page");});

     document.addEventListener("keyup", (event) => {
         if (event.isComposing || event.keyCode === 229)
             return;
 
         if (event.code == "Escape") 
             sendCode("close", {});
     });

     /**
      * History system
      */

    
    console.log("Main script loaded !");
});