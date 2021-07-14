let container = document.getElementById("bank-main");
let pageContainer = document.getElementById("bank-page-container");

function bankDisplay(bool) {
    container.hidden = !bool;
}

bankDisplay(false);

/**
 * Pages system
 */
document.getElementById("bank-withdrawal-button").addEventListener('click', () => {loadBankPage("bank-page-withdrawal");});
document.getElementById("bank-deposit-button").addEventListener('click', () => {loadBankPage("bank-page-deposit");});
document.getElementById("bank-transfer-button").addEventListener('click', () => {loadBankPage("bank-page-transfer");});

loadBankPage("bank-main-page");

function loadBankPage(name) {
    let page = document.getElementById(name);

    for (let node of pageContainer.childNodes) {
        node.hidden = true;
    }

    page.hidden = false;

    if(!document.getElementById("bank-main-page").hidden)
        document.getElementById("bank-back").style.display = "none";
    else
        document.getElementById("bank-back").style.display = "initial";
}

/**
 * Close & rewind system
 */
    document.getElementById("bank-close").addEventListener("click", () => {sendCode("close", {});});
    document.getElementById("bank-back").addEventListener('click', () => {loadBankPage("bank-main-page");});

    document.addEventListener("keyup", (event) => {
        if (event.isComposing || event.keyCode === 229)
            return;

        if (event.code == "Escape") 
            sendCode("close", {});
    });

/**
 * Withdrawal System
 */
 document.getElementById("bank-withdrawal_50").addEventListener("click", () => {if(!document.getElementById("bank-withdrawal_50").hasAttribute("disabled")) {sendCode("withdrawal", {"amount": 50});document.getElementById("bank-withdrawal_50").setAttribute("disabled", true);document.getElementById("bank-withdrawal_100").setAttribute("disabled", true);document.getElementById("bank-withdrawal_500").setAttribute("disabled", true);document.getElementById("bank-withdrawal_1000").setAttribute("disabled", true);document.getElementById("bank-withdrawal_5000").setAttribute("disabled", true);}});
 document.getElementById("bank-withdrawal_100").addEventListener("click", () => {if(!document.getElementById("bank-withdrawal_100").hasAttribute("disabled")) {sendCode("withdrawal", {"amount": 100});document.getElementById("bank-withdrawal_50").setAttribute("disabled", true);;document.getElementById("bank-withdrawal_100").setAttribute("disabled", true);document.getElementById("bank-withdrawal_500").setAttribute("disabled", true);document.getElementById("bank-withdrawal_1000").setAttribute("disabled", true);document.getElementById("bank-withdrawal_5000").setAttribute("disabled", true);}});
 document.getElementById("bank-withdrawal_500").addEventListener("click", () => {if(!document.getElementById("bank-withdrawal_500").hasAttribute("disabled")) {sendCode("withdrawal", {"amount": 500});document.getElementById("bank-withdrawal_50").setAttribute("disabled", true);;document.getElementById("bank-withdrawal_100").setAttribute("disabled", true);document.getElementById("bank-withdrawal_500").setAttribute("disabled", true);document.getElementById("bank-withdrawal_1000").setAttribute("disabled", true);document.getElementById("bank-withdrawal_5000").setAttribute("disabled", true);}});
 document.getElementById("bank-withdrawal_1000").addEventListener("click", () => {if(!document.getElementById("bank-withdrawal_1000").hasAttribute("disabled")) {sendCode("withdrawal", {"amount": 1000});document.getElementById("bank-withdrawal_50").setAttribute("disabled", true);;document.getElementById("bank-withdrawal_100").setAttribute("disabled", true);document.getElementById("bank-withdrawal_500").setAttribute("disabled", true);document.getElementById("bank-withdrawal_1000").setAttribute("disabled", true);document.getElementById("bank-withdrawal_5000").setAttribute("disabled", true);}});
 document.getElementById("bank-withdrawal_5000").addEventListener("click", () => {if(!document.getElementById("bank-withdrawal_5000").hasAttribute("disabled")) {sendCode("withdrawal", {"amount": 5000});document.getElementById("bank-withdrawal_50").setAttribute("disabled", true);;document.getElementById("bank-withdrawal_100").setAttribute("disabled", true);document.getElementById("bank-withdrawal_500").setAttribute("disabled", true);document.getElementById("bank-withdrawal_1000").setAttribute("disabled", true);document.getElementById("bank-withdrawal_5000").setAttribute("disabled", true);}});

/**
 * Deposit System
 */

 document.getElementById("bank-deposit_50").addEventListener("click", () => {if(!document.getElementById("bank-deposit_50").hasAttribute("disabled")) {sendCode("deposit", {"amount": 50});document.getElementById("bank-deposit_50").setAttribute("disabled", true);document.getElementById("bank-deposit_100").setAttribute("disabled", true);document.getElementById("bank-deposit_500").setAttribute("disabled", true);document.getElementById("bank-deposit_1000").setAttribute("disabled", true);document.getElementById("bank-deposit_5000").setAttribute("disabled", true);}});
 document.getElementById("bank-deposit_100").addEventListener("click", () => {if(!document.getElementById("bank-deposit_100").hasAttribute("disabled")) {sendCode("deposit", {"amount": 100});document.getElementById("bank-deposit_50").setAttribute("disabled", true);document.getElementById("bank-deposit_100").setAttribute("disabled", true);document.getElementById("bank-deposit_500").setAttribute("disabled", true);document.getElementById("bank-deposit_1000").setAttribute("disabled", true);document.getElementById("bank-deposit_5000").setAttribute("disabled", true);}});
 document.getElementById("bank-deposit_500").addEventListener("click", () => {if(!document.getElementById("bank-deposit_500").hasAttribute("disabled")) {sendCode("deposit", {"amount": 500});document.getElementById("bank-deposit_50").setAttribute("disabled", true);document.getElementById("bank-deposit_100").setAttribute("disabled", true);document.getElementById("bank-deposit_500").setAttribute("disabled", true);document.getElementById("bank-deposit_1000").setAttribute("disabled", true);document.getElementById("bank-deposit_5000").setAttribute("disabled", true);}});
 document.getElementById("bank-deposit_1000").addEventListener("click", () => {if(!document.getElementById("bank-deposit_1000").hasAttribute("disabled")) {sendCode("deposit", {"amount": 1000});document.getElementById("bank-deposit_50").setAttribute("disabled", true);document.getElementById("bank-deposit_100").setAttribute("disabled", true);document.getElementById("bank-deposit_500").setAttribute("disabled", true);document.getElementById("bank-deposit_1000").setAttribute("disabled", true);document.getElementById("bank-deposit_5000").setAttribute("disabled", true);}});
 document.getElementById("bank-deposit_5000").addEventListener("click", () => {if(!document.getElementById("bank-deposit_5000").hasAttribute("disabled")) {sendCode("deposit", {"amount": 5000});document.getElementById("bank-deposit_50").setAttribute("disabled", true);document.getElementById("bank-deposit_100").setAttribute("disabled", true);document.getElementById("bank-deposit_500").setAttribute("disabled", true);document.getElementById("bank-deposit_1000").setAttribute("disabled", true);document.getElementById("bank-deposit_5000").setAttribute("disabled", true);}});

/**
 * Transaction System
 */
document.getElementById("bank-transfer-validate").addEventListener("click", () => {
    if(!document.getElementById("bank-transfer-validate").hasAttribute("disabled") && document.getElementById("bank-sender-amount").value > 0 && parseInt(document.getElementById("bank-sender-amount").getAttribute("max"))){
        document.getElementById("bank-transfer-validate").setAttribute("disabled", true);
        sendCode("transfer", {
            "amount": document.getElementById("bank-sender-amount").value,
            "reason": document.getElementById("bank-sender-description").innerText,
            "receiver": document.getElementById("bank-receiver").value
        });
    }
});

document.getElementById("bank-sender-description").addEventListener("keydown", (e) => {
    if (e.keyCode == 13 && !e.shiftKey)
    {
        e.preventDefault();
        return false;
    }
});


let dropdownCorrect = false;
let amountCorrect = false;
let dropdownslists = document.querySelectorAll(".dropdown-list");
let dropdowns = document.querySelectorAll(".dropdown");

for(let dropdown of dropdowns){
    let datael;
    for(let dropdownslist of dropdownslists){
        if(!dropdownslist.hasAttribute("dropdown")) continue;
        if(dropdown.id == dropdownslist.getAttribute("dropdown")){
            datael = dropdownslist;
            break;
        }
    }

    if(datael){
        dropdown.addEventListener("input", () => {
            let hasOne = false;
            let correct = 0;

            for(let el of datael.querySelectorAll("a")){
                let data = el.innerText;

                el.hidden = !data.toLowerCase().replace(" ", "").includes(dropdown.value.toLowerCase().replace(" ", ""));
                if(!el.hidden) hasOne = true;
                if(data.toLowerCase() == dropdown.value.toLowerCase()) correct++;
            }

            if(dropdown.value.length == 0){
                document.getElementById("bank-receiver-error").innerText = "La valeur ne peut être vide !";
                document.getElementById("bank-transfer-validate").setAttribute("disabled", true);
                dropdownCorrect = false;
            }
            else if(!hasOne){
                document.getElementById("bank-receiver-error").innerText = "Ce nom n'existe pas !";
                document.getElementById("bank-transfer-validate").setAttribute("disabled", true);
                dropdownCorrect = false;
            }
            else if(correct != 1){
                document.getElementById("bank-receiver-error").innerText = "Ce nom ne corresponds pas complètement !";
                document.getElementById("transfer-validate").setAttribute("disabled", true);
                dropdownCorrect = false;
            }
            else {
                document.getElementById("bank-receiver-error").innerText = "";
                dropdownCorrect = true;
                if(dropdownCorrect && amountCorrect) document.getElementById("bank-transfer-validate").removeAttribute("disabled");
            }

        });
    }
}

let amountEl =  document.getElementById("bank-sender-amount");
amountEl.addEventListener("keyup", () => {
    if(amountEl.value > parseInt(amountEl.getAttribute("max"))){
        document.getElementById("bank-sender-amount-error").innerText = "La valeur ne peut être supérieur à la somme du compte !";
        document.getElementById("bank-transfer-validate").setAttribute("disabled", true);
        amountCorrect = false;
    }
    else if(amountEl.value <= 0){
        document.getElementById("bank-sender-amount-error").innerText = "La valeur ne peut être inférieur à 1 !";
        document.getElementById("bank-transfer-validate").setAttribute("disabled", true);
        amountCorrect = false;
    }
    else{
        document.getElementById("bank-sender-amount-error").innerText = "";
        amountCorrect = true;
        if(dropdownCorrect && amountCorrect) document.getElementById("bank-transfer-validate").removeAttribute("disabled");
    }
});



console.log("Main script loaded !");