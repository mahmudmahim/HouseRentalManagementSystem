
function selectRole(role) {
     document.getElementById('UserRole').value = role;

     document.getElementById('tenantRole').classList.remove('active');
     document.getElementById('ownerRole').classList.remove('active');

     document.getElementById(role + 'Role').classList.add('active');
}

$("#phone").keyup(function () {
    let phone = $(this).val();

    console.log(phone);

    if (phone.length > 11) {
        $("#phone").val(phone.substring(0, phone.length - 1))
        alert("Mobile Number must be 11 digit");
        return false;
    }

});



$("#btnSignup").click(function (e) {
    e.preventDefault(); // Prevent default form submit
    ShowLoader();

    let password = $("#passWord").val();
    let confirmPassword = $("#cpassWord").val();

    if (password !== confirmPassword) {
        alert("Password didn't match. Try again!");
        return;
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
            if (res) {
                toastr.success("Account created successfully!");

                setTimeout(() => {
                    window.location.href = "/Auth/Login";
                }, 3000);
                HideLoader();
            } else {
                toastr.error(res.message);
                HideLoader();
            }
        },

        error: function (err) {
            toastr.error("Something went wrong. Check console.");
            console.log(err);
        }
    });
}); 
//$("#email").blur(function () {
//    checkUnique("email", $(this).val());
//});

$("#phone").blur(function () {
    checkUnique("phone", $(this).val());
});

$("#nidNo").blur(function () {
    checkUnique("nidNo", $(this).val());
    
});

$("#email").on("blur", function () {
    checkUnique("email", $(this).val());
    
});

function checkUnique(field, value) {

    const request = {};
    request[field] = value; // dynamic object creation

    $.ajax({
        url: "https://localhost:7152/api/Auth/check-unique",
        method: "POST",
        contentType: "application/json",
        data: JSON.stringify(request),
        success: function (res) {

            if (field === "email" && res.emailExists) {
                toastr.error("Email already exists!");
                $('#email').val('');
            }
            if (field === "phone" && res.phoneExists) {
                toastr.error("Phone number already exists!");
                $('#phone').val('');

            }
            if (field === "nidNo" && res.nidExists) {
                toastr.error("NID already exists!");
                $('#nidNo').val('');

            }
        }
    });
}


$("#btnLogIn").click(function (e) {
    e.preventDefault();

    const btn = $(this);

    // Disable button + show spinner
    btn.prop("disabled", true);
    const oldHtml = btn.html();
    btn.data("old-html", oldHtml);   // Save old text

    btn.html(`
        <span class="spinner-border spinner-border-sm me-2" role="status"></span>
        Processing...`);

    const request = {
        email: $("#txtemail").val(),
        password: $("#txtpass").val()
    };
    $.ajax({
        url: "https://localhost:7152/api/Auth/login",
        method: "POST",
        contentType: "application/json",
        data: JSON.stringify(request),

        success: function (res) {
            if (!res.success) {
                toastr.error(res.message);

                btn.prop("disabled", false);
                btn.html(btn.data("old-html"));

                return;
            }

            // save token
            localStorage.setItem("token", res.token);
            localStorage.setItem("role", res.role);

            console.log(res.token);
            console.log(res.role);

            toastr.success("Login Successful!");

            setTimeout(() => {
                if (res.role === "Owner") {
                    window.location.href = "/Dashboard/OwnerDashboard";
                }
                else {
                    window.location.href = "/Dashboard/TenantDashboard";
                }
            }, 3000);
        },

        error: function () {
            toastr.error("Server error. Try again.");

            // Restore button
            btn.prop("disabled", false);
            btn.html(btn.data("old-html"));
        }
    });
});

