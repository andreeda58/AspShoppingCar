$("#SignIn_Username").on("change", async () => {
    await Promise.resolve($.get("/User/UsernameCheck", { Username: $("#SignIn_Username").val() }))
        .then((Success) => {
            $("#LogIn_Username_messenger")
                .removeClass("invalid-feedback")
                .addClass("valid-feedback")
                .text("Username exists!")
                .fadeIn();
            if ($("#LogIn_Password_messenger").text() === "Password is correct!") {
                $("#logIn_Button").prop("disabled", false);
            }
        })
        .catch((Error) => {
            $("#LogIn_Username_messenger")
                .removeClass("valid-feedback")
                .addClass("invalid-feedback")
                .text("Username does'nt exist")
                .fadeIn();
            $("#logIn_Button").prop("disabled", true);
        });
});

$("#SignIn_Password").on("change", async () => {
    await Promise.resolve($.get("/User/PasswordCheck", { Username: $("#SignIn_Username").val(), Password: $("#SignIn_Password").val() }))
        .then((Success) => {
            $("#LogIn_Password_messenger")
                .removeClass("invalid-feedback")
                .addClass("valid-feedback")
                .text("Password is correct!")
                .fadeIn();
            if ($("#LogIn_Username_messenger").text() === "Username exists!") {
                $("#logIn_Button").prop("disabled", false);
            }
        })
        .catch((Error) => {
            $("#LogIn_Password_messenger")
                .removeClass("valid-feedback")
                .addClass("invalid-feedback")
                .text("Password is NOT correct!")
                .fadeIn();
            $("#logIn_Button").prop("disabled", true);
        });
});