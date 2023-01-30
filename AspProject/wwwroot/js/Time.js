(() => {
    let T = new Date().getHours();
    if (T >= 22 && T < 6) {
        $("#TimeRespond").text("Night");
    }
    else if (T >= 6 && T < 12) {
        $("#TimeRespond").text("Morning");
    }
    else {
        $("#TimeRespond").text("Afternoon");
    }
})()