var bar = document.querySelector('#speedbar');
var barBg = document.querySelector('#speedbar_backbround');

var radius = 72;
var circumference = radius * 2 * Math.PI;

bar.style.strokeDasharray = `${circumference} ${circumference}`;
barBg.style.strokeDasharray = `${circumference} ${circumference}`;
bar.style.strokeDashoffset = `${circumference}`;
barBg.style.strokeDashoffset = `${circumference}`;

function setProgress(el, percent) {
    if(percent > 100){
        const offset = circumference - 100 / 140 * circumference;
        el.style.strokeDashoffset = offset;
    }
    else{
        const offset = circumference - percent / 140 * circumference;
        el.style.strokeDashoffset = offset;
    }
}

setProgress(bar, 40);
setProgress(barBg, 100);

/*document.addEventListener("keyup", (event) => {
    if (event.isComposing || event.keyCode === 229)
        return;

    if (event.code == "Space") 
        document.getElementById("brakes").removeAttribute("red");
});

document.addEventListener("keydown", (event) => {
    if (event.isComposing || event.keyCode === 229)
        return;

    if (event.code == "Space") 
        document.getElementById("brakes").setAttribute("red","");
});*/