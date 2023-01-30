$("#Password_Verification").on("change", () => {
    if (!($("#Password").val() === $("#Password_Verification").val())) {
        $("#Password_messenger")
            .removeClass("valid-feedback")
            .addClass("invalid-feedback")
            .text("Passwords does'nt match!")
            .fadeIn();
        $("#Submit_New_User_Button").prop("disabled", true);
    }
    else {
        $("#Password_messenger")
            .removeClass("invalid-feedback")
            .addClass("valid-feedback")
            .text("Passwords match!")
            .fadeIn();
        if ($("#Username_messenger").text() === "Valid username!") {
            $("#Submit_New_User_Button").prop("disabled", false);
        }
    }
});


$("#Password_messenger").on("change", () => {
    if (!($("#Password").val() === $("#Password_Verification").val())) {
        $("#Password_messenger")
            .removeClass("valid-feedback")
            .addClass("invalid-feedback")
            .text("Passwords does'nt match!")
            .fadeIn();
        $("#Submit_New_User_Button").prop("disabled", true);
    }
    else {
        $("#Password_messenger")
            .removeClass("invalid-feedback")
            .addClass("valid-feedback")
            .text("Passwords match!")
            .fadeIn();
        if ($("#Username_messenger").text() === "Valid username!") {
            $("#Submit_New_User_Button").prop("disabled", false);
        }
    }
});




