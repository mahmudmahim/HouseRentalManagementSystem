const ownerID = getUserIdFromToken();
const apiBase = "https://localhost:7152/api/Properties";
function loadOwnerProperties() {

    $.ajax({
        url: apiBase + "/owner/" + ownerID,
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
    const container = $("#ownerPropertiesContainer");
    container.empty();

    if (!list || list.length === 0) {
        container.html("<p>You have no properties listed.</p>");
        return;
    }

    list.forEach(p => {
        const imgUrl = p.firstImageUrl !== "" ?  p.firstImageUrl : "/images/no-image.png";

        const html = `
            <div class="col-md-4 col-sm-6">
                <div class="property-card card border-0 shadow-sm rounded-4 overflow-hidden h-100">
                    
                    <div class="property-image position-relative">
                        <img src="${imgUrl}" class="w-100" alt="Property" />
                    </div>

                    <div class="card-body">
                        <h6 class="fw-semibold">${p.title}</h6>

                        <p class="text-muted small mb-1">
                            ${p.sqft} sqft · ৳${p.price}/month
                        </p>

                        <p class="text-muted small mb-3">
                            <i class="bi bi-geo-alt"></i> ${p.address}, ${p.area}, ${p.district}
                        </p>

                        <div class="d-flex gap-2">
                            <a href="/Properties/EditProperties/${p.propertyId}" 
                               class="btn btn-primary btn-gradient btn-sm w-50">
                                Edit
                            </a>

                            <button class="btn btn-outline-danger btn-sm w-50 btn-delete-property" data-id="${p.propertyId}">
                                Delete
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


let propertyIdToDelete = null;

// Step 1: When delete button clicked — open modal
$(document).on("click", ".btn-delete-property", function () {
    propertyIdToDelete = $(this).attr("data-id");
    const deleteModal = new bootstrap.Modal(document.getElementById("modalDeleteProperty"));
    deleteModal.show();
});

// Step 2: Confirm delete
$("#btnConfirmDelete").click(async function () {
    if (!propertyIdToDelete) return;

    try {
        $.ajax({
            url: apiBase + "/delete/" + propertyIdToDelete,
            method: "DELETE",
            success: function () {
                toastr.success("Deleted Successfully");
            },
            error: function () {
                toastr.error("Failed to load properties");
            }
        });
        // Optional: remove from UI without reload
        $(`button[data-id='${propertyIdToDelete}']`)
            .closest(".col-md-4")
            .fadeOut(300, function () { $(this).remove(); });

        // Close modal
        bootstrap.Modal.getInstance(document.getElementById("modalDeleteProperty")).hide();

    } catch (err) {
        console.error(err);
        alert("An unexpected error occurred.");
    }
});

$("#btnLogout").click(function () {
    localStorage.removeItem("token");
    localStorage.removeItem("role");
    window.location.href = "/Auth/Login";
});
