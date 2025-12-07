
function selectRole(role) {
     document.getElementById('UserRole').value = role;

     document.getElementById('tenantRole').classList.remove('active');
     document.getElementById('ownerRole').classList.remove('active');

     document.getElementById(role + 'Role').classList.add('active');
 }

$("#btnSignup").click(function (e) {
    e.preventDefault(); // Prevent default form submit

    let password = $("#passWord").val();
    let confirmPassword = $("#cpassWord").val();
    let phone = $("#phone").val();

    if (password !== confirmPassword) {
        alert("Password didn't match. Try again!");
        return;
    }
    if (phone.length > 11) {
        $("#phone").val(phone.substring(0, phone.length - 1))
        alert("Mobile Number must be 11 digit");
        return false;
    }

    let isOwner = $("#UserRole").val() === "owner";
    let isAdmin = false;

    const request = {
        firstName: $("#fname").val(),
        lastName: $("#lname").val(),
        phone: $("#phone").val(),
        email: $("#email").val(),
        address: $("#address").val(),
        nidNo: $("#nidNo").val(),
        password: $("#passWord").val(),
        confirmPassword: $("#cpassWord").val(),
        isOwner: isOwner,
        isAdmin: isAdmin
    };

    $.ajax({
        url: "https://localhost:7152/api/Auth/register",
        method: "POST",
        contentType: "application/json",
        data: JSON.stringify(request),

        success: function (res) {
            if (res.success) {
                toastr.success("Account created successfully!");

                setTimeout(() => {
                    window.location.href = "/Auth/Login";
                }, 3000);
            } else {
                toastr.error(res.message);
            }
        },

        error: function (err) {
            toastr.error("Something went wrong. Check console.");
            console.log(err);
        }
    });
});


$("#btnLogIn").click(function (e) {
    e.preventDefault(); // Prevent default form submit


    let email = $("#txtemail").val();
    let password = $("#txtpass").val();



    let isOwner = $("#UserRole").val() === "owner";
    let isAdmin = false;

    const request = {
        email: email,
        password: password
    };

    $.ajax({
        url: "https://localhost:7152/api/Auth/login",
        method: "POST",
        contentType: "application/json",
        data: JSON.stringify(request),

        success: function (res) {
            if (res.success) {
                toastr.success("Account created successfully!");

                setTimeout(() => {
                    window.location.href = "/Auth/Login";
                }, 3000);
            } else {
                toastr.error(res.message);
            }
        },

        error: function (err) {
            toastr.error("Something went wrong. Check console.");
            console.log(err);
        }
    });
});