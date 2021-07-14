window.addEventListener("message", function(event) {
    var item = event.data;
    if(item.ui == "BankNUI"){
        if (item.action == "show") {
            if (item.data == "True") {
                bankDisplay(true);
                loadBankPage("bank-main-page")
            } else {
                bankDisplay(false);
            }
        }
        else if(item.action == "update") {
            if(item.type == "page"){
                loadBankPage(item.data);
            }
            else if(item.type == "playerinfo"){
                document.getElementById("bank-name").innerText = item.data.Name;
                document.getElementById("bank-sender-name").innerText = item.data.Name;

                document.getElementById("bank-sender-amount").max = item.data.Banking.Money;

                if(item.data.Banking.Money < 50) document.getElementById("bank-withdrawal_50").setAttribute("disabled", true);
                else document.getElementById("bank-withdrawal_50").removeAttribute("disabled");
                if(item.data.Banking.Money < 100) document.getElementById("bank-withdrawal_100").setAttribute("disabled", true);
                else document.getElementById("bank-withdrawal_100").removeAttribute("disabled");
                if(item.data.Banking.Money < 500) document.getElementById("bank-withdrawal_500").setAttribute("disabled", true);
                else document.getElementById("bank-withdrawal_500").removeAttribute("disabled");
                if(item.data.Banking.Money < 1000) document.getElementById("bank-withdrawal_1000").setAttribute("disabled", true);
                else document.getElementById("bank-withdrawal_1000").removeAttribute("disabled");
                if(item.data.Banking.Money < 5000) document.getElementById("bank-withdrawal_5000").setAttribute("disabled", true);
                else document.getElementById("bank-withdrawal_5000").removeAttribute("disabled");

                if(item.data.Cash < 50) document.getElementById("bank-deposit_50").setAttribute("disabled", true);
                else document.getElementById("bank-deposit_50").removeAttribute("disabled");
                if(item.data.Cash < 100) document.getElementById("bank-deposit_100").setAttribute("disabled", true);
                else document.getElementById("bank-deposit_100").removeAttribute("disabled");
                if(item.data.Cash < 500) document.getElementById("bank-deposit_500").setAttribute("disabled", true);
                else document.getElementById("bank-deposit_500").removeAttribute("disabled");
                if(item.data.Cash < 1000) document.getElementById("bank-deposit_1000").setAttribute("disabled", true);
                else document.getElementById("bank-deposit_1000").removeAttribute("disabled");
                if(item.data.Cash < 5000) document.getElementById("bank-deposit_5000").setAttribute("disabled", true);
                else document.getElementById("bank-deposit_5000").removeAttribute("disabled");

                let history_table = document.getElementById("bank_history");
                history_table.innerHTML = "";
                /*
                <tr>
                                        <th>Date</th>
                                        <th>Intitulé</th>
                                        <th style="text-align: center;">Catégorie</th>
                                        <th style="text-align: right;">Montant</th>
                                    </tr>
                                    */
                let ttr = document.createElement("tr");
                let tthDate = document.createElement("th");
                let tthFrom = document.createElement("th");
                let tthCat = document.createElement("th");
                let tthAmount = document.createElement("th");
                tthDate.innerText = "Date";
                tthFrom.innerText = "Intitulé";
                tthCat.innerText = "Catégorie";
                tthCat.style.textAlign = "center";
                tthAmount.innerText = "Montant";
                tthAmount.style.textAlign = "right";
                tthAmount.style.paddingRight = "5px";
                ttr.appendChild(tthDate);
                ttr.appendChild(tthFrom);
                ttr.appendChild(tthCat);
                ttr.appendChild(tthAmount);
                history_table.appendChild(ttr);
                for(let transaction of item.data.Banking.Transactions){
                    /*
                                    <tr>
                                        <td>02/07/2021</td>
                                        <td>M. Jéreme DUBOIS</td>
                                        <td style="color: #959595;text-align: center;">Virement</td>
                                        <td style="color: #01852E;text-align: right;">1 100 900$</td>
                                    </tr>
                                    0001-01-01T00:00:00
                    */
                    let tr = document.createElement("tr");
                    let tdDate = document.createElement("td");
                    let tdName = document.createElement("td");
                    let tdDesc = document.createElement("td");
                    let tdAmount = document.createElement("td");
                    tdDate.innerText = transaction.Date.substring(9,10) + "/" + transaction.Date.substring(6,7) + "/" + transaction.Date.substring(0,4);
                    tdName.innerText = transaction.Receiver;
                    tdDesc.innerText = transaction.Description;
                    tdDesc.style.color = "#959595";
                    tdDesc.style.textAlign = "center";
                    tdAmount.innerText = transaction.Amount + "$";
                    if(transaction.Amount > 0) tdAmount.style.color = "#01852E";
                    else tdAmount.style.color = "#C90000";
                    tdAmount.style.textAlign = "right";
                    tdAmount.style.paddingRight = "5px";
                    tr.appendChild(tdDate);
                    tr.appendChild(tdName);
                    tr.appendChild(tdDesc);
                    tr.appendChild(tdAmount);
                    history_table.appendChild(tr);
                }

                document.getElementById("bank-money").innerText = item.data.Banking.Money.toString().replace(/\B(?=(\d{3})+(?!\d))/g, " ") + " $";
                //this.document.getElementById("account_number").innerText = item.data.Banking.Money + "$";
            }
            else if(item.type == "playerinfos"){
                document.getElementById("bank-receiver-List").innerHTML = "";
                for(let pi of item.data){
                    if(pi.Name != document.getElementById("bank-name").innerText){
                        let el = document.createElement("a");
                        el.innerText = pi.Name;
                        document.getElementById("bank-receiver-List").appendChild(el);
                    }
                }
                let dropdownslists = document.querySelectorAll(".dropdown-list");

                for(let dropdownslist of dropdownslists){
                    if(!dropdownslist.hasAttribute("dropdown")) continue;

                    let dropdown = document.getElementById(dropdownslist.getAttribute("dropdown"));
                    if(dropdown){
                        for(let el of dropdownslist.querySelectorAll("a")){
                            el.onclick = () => {
                                dropdown.value = el.innerText;
                                dropdown.dispatchEvent(new Event("input"))
                            }
                        }
                    }
                }
            }
        }
    }
    else if(item.ui == "SpeedNUI"){
        if (item.action == "show") {
            if(item.data == "True"){
                document.getElementById("SpeedNUI").hidden = false;
            }
            else{
                document.getElementById("SpeedNUI").hidden = true;
            }
        }
        else if(item.action == "update") {
            if(item.type == "speed"){
                document.querySelector('#speed').innerText = Math.round(parseInt(item.data, 0) * 3.6);
                var bar = document.querySelector('#speedbar');
                setProgress(bar, parseInt(item.data, 0));
            }
            else if(item.type == "brakes"){
                if(item.data == "True"){
                    document.getElementById("brakes").setAttribute("red", "");
                    document.getElementById("brakes").removeAttribute("transparent");
                }
                else{
                    document.getElementById("brakes").removeAttribute("red");
                    document.getElementById("brakes").setAttribute("transparent", "");
                }
            }
            else if(item.type == "lights"){
                document.getElementById("lights").removeAttribute("red");
                if(item.data == "True"){
                    document.getElementById("lights").setAttribute("green", "");
                    document.getElementById("lights").removeAttribute("opacity");
                }
                else{
                    document.getElementById("lights").removeAttribute("green");
                    document.getElementById("lights").setAttribute("opacity", "");
                }
            }
            else if(item.type == "lock"){
                if(item.data == "True"){
                    document.getElementById("key").removeAttribute("red");
                    document.getElementById("key").setAttribute("opacity", "");
                }
                else{
                    document.getElementById("key").setAttribute("red", "");
                    document.getElementById("key").removeAttribute("opacity");
                }
            }
            else if(item.type == "belt"){
                if(item.data == "True"){
                    document.getElementById("belt").removeAttribute("red");
                    document.getElementById("belt").setAttribute("opacity", "");
                }
                else{
                    document.getElementById("belt").setAttribute("red", "");
                    document.getElementById("belt").removeAttribute("opacity");
                }
            }
            else if(item.type == "fuel"){
                if(item.data == "True"){
                    document.getElementById("fuel").removeAttribute("red");
                }
                else{
                    document.getElementById("fuel").setAttribute("red", "");
                }
            }
            else if(item.type == "engine"){
                if(item.data != "True"){
                    document.querySelector('#speed').innerText = 0;
                    var bar = document.querySelector('#speedbar');
                    setProgress(bar, 0);

                    document.getElementById("brakes").setAttribute("red", "");
                    document.getElementById("brakes").removeAttribute("transparent");
                    document.getElementById("brakes").removeAttribute("opacity");

                    document.getElementById("lights").setAttribute("red", "");
                    document.getElementById("lights").removeAttribute("green");
                    document.getElementById("lights").removeAttribute("transparent");
                    document.getElementById("lights").removeAttribute("opacity");

                    document.getElementById("fuel").setAttribute("red", "");
                    document.getElementById("fuel").removeAttribute("transparent");
                    document.getElementById("fuel").removeAttribute("opacity");

                    document.getElementById("belt").setAttribute("red", "");
                    document.getElementById("belt").removeAttribute("transparent");
                    document.getElementById("belt").removeAttribute("opacity");

                    document.getElementById("key").setAttribute("red", "");
                    document.getElementById("key").removeAttribute("transparent");
                    document.getElementById("key").removeAttribute("opacity");
                }
            }
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