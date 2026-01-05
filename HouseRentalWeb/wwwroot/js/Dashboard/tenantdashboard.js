const ownerID = getUserIdFromToken();
const apiBase = "https://localhost:7152/api/Properties";
function loadOwnerProperties() {

    $.ajax({
        url: apiBase + "/properties/",
        method: "GET",
        success: function (list) {
            renderOwnerProperties(list);
        },
        error: function () {
            toastr.error("Failed to load properties");
        }
    });
}

$(document).ready(function () {
    loadOwnerProperties();
});


function renderOwnerProperties(list) {

    const container = $("#allProperties");
    container.empty();

    if (!list || list.length === 0) {
        container.html("<p>There's no properties listed.</p>");
        return;
    }

    list.forEach(p => {
        const imgUrl = (p.images && p.images.length > 0)
            ? p.images[0].url
            : "/images/no-image.png";

        const html = `
            <div class="col-md-4 col-sm-6">
                <div class="property-card card border-0 shadow-sm rounded-4 overflow-hidden h-100">
                    
                    <div class="property-image position-relative">
                        <img src="${imgUrl}" class="w-100" alt="Property" />
                        <span class="fav-icon" title="Add to Wishlist"><i class="bi bi-heart"></i></span>
                    </div>

                    <div class="card-body">
                        <h6 class="fw-semibold">${p.title}</h6>

                        <p class="text-muted small mb-1">
                            ${p.sqft} sqft · ৳${p.price}/month
                        </p>

                        <p class="text-muted small mb-3">
                            <i class="bi bi-geo-alt"></i> ${p.address}, ${p.area}, ${p.district}
                        </p>

                       <div class="card-body">
                                <button id="sentRequest" class="btn btn-gradient btn-sm w-100"
                                        data-bs-toggle="toast">
                                    Send Request Using 10 Credits
                                </button>
                                <br />
                                <br />
                                <button class="btn btn-outline-dark btn-sm w-100"
                                        data-bs-toggle="toast" data-bs-target="#toastRequestSent">
                                    See Details
                                </button>
                         </div>
                    </div>
                </div>
            </div>
        `;
        console.log(imgUrl);
        container.append(html);
    });
}



$("#sentRequest").on("click", function () {
    toastr.success("Request Send Successful!");
});

$("#btnLogout").click(function () {
    localStorage.removeItem("token");
    localStorage.removeItem("role");
    window.location.href = "/Auth/Login";
});
