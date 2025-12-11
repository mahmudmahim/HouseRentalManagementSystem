$("#sentRequest").on("click", function () {
    toastr.success("Request Send Successful!");
});

$("#btnLogout").click(function () {
    localStorage.removeItem("token");
    localStorage.removeItem("role");
    window.location.href = "/Auth/Login";
});
