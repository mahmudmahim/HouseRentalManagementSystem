$(function () {
    const apiBase = "https://localhost:7152/api/Properties";
    const propertyId = window.location.pathname.split("/").pop();;
    let current = 1;
    const uploadedImages = [];


    document.addEventListener("DOMContentLoaded", function () {
        loadProperty(propertyId);
    });

    setStep(1);

    loadProperty(propertyId);

    function loadProperty(id) {
        $.get(apiBase + "/editproperty/" + id, function (p) {
            $("#title").val(p.title);
            $("#description").val(p.description);
            $("#price").val(p.price);
            $("#sqft").val(p.sqft);
            $("#bedrooms").val(p.bedrooms);
            $("#washroom").val(p.washroom);
            $("#balcony").val(p.balcony);
            $("#address").val(p.address);
            $("#area").val(p.area);
            $("#district").val(p.district);
            $("#status").val(p.status);

            p.images.forEach((img, idx) => {
                uploadedImages.push({
                    id: img.imageId,
                    url: img.url,
                    sortOrder: idx
                });
            });

            renderThumbs();
            renderReview();
        });
    }


  
    function setStep(n) {
        current = n;
        $(".step").removeClass("active");
        $(`.step[data-step=${n}]`).addClass("active");

        $(".step-page").removeClass("active");
        $(`#step-${n}`).addClass("active");

        $("#btnUpdate").toggle(n === 5);
        $("#btnNext").toggle(n < 5);
        $("#btnPrev").toggle(n > 1);
    }

    $("#btnNext").on("click", () => {
        if (!validateStep(current)) return;
        setStep(current + 1);
        if (current + 1 === 5) renderReview();
    });

    $("#btnPrev").on("click", () => setStep(Math.max(1, current - 1)));
    $("#btnBack").on("click", function () {
        window.location.href = "/Dashboard/OwnerDashboard";
    })

    function validateStep(step) {
        let ok = true;
        $(".error").text("");

        if (step === 1 && !$("#title").val().trim()) {
            $("#err-title").text("Title is required");
            ok = false;
        }

        if (step === 2) {
            if ($("#price").val() <= 0) { $("#err-price").text("Price required"); ok = false; }
            if ($("#sqft").val() <= 0) { $("#err-sqft").text("Sqft required"); ok = false; }
        }

        if (step === 3) {
            if (!$("#address").val().trim()) { $("#err-address").text("Address required"); ok = false; }
            if (!$("#district").val().trim()) { $("#err-district").text("District required"); ok = false; }
        }

        return ok;
    }

    function renderThumbs() {
        const list = $("#thumbList").empty();

        uploadedImages.forEach((img, idx) => {
            const item = $(`
                <div style='position:relative;'>
                    <img class='thumb' src='${img.url}' />
                    <div class='remove' data-idx='${idx}'>&times;</div>
                </div>
            `);

            item.find(".remove").on("click", function () {
                const index = $(this).data("idx");
                uploadedImages.splice(index, 1);
                renderThumbs();
            });

            list.append(item);
        });
    }

    function renderReview() {
        const html = `
            <div style='display:flex;gap:12px;'>
                <div style='width:240px;'>
                    <img src='${uploadedImages[0]?.url || ""}' style='width:100%;height:160px;object-fit:cover;border-radius:8px;'/>
                </div>
                <div>
                    <h4>${$("#title").val()}</h4>
                    <p>${$("#address").val()}, ${$("#district").val()}</p>
                    <div>${uploadedImages.length} total images</div>
                </div>
            </div>
        `;

        $("#reviewCard").html(html);
    }

    $("#btnSelectFiles").click(() => $("#fileInput").click());

    $("#fileInput").on("change", function () {
        const files = Array.from(this.files);
        uploadNewImages(files);
        $(this).val("");
    });

    function uploadNewImages(files) {
        files.forEach(file => {
            const fd = new FormData();
            fd.append("file", file);

            $.ajax({
                url: apiBase + "/upload-image",
                method: "POST",
                data: fd,
                contentType: false,
                processData: false,
                success: function (resp) {
                    uploadedImages.push({
                        id: resp.id,
                        url: resp.url,
                        sortOrder: uploadedImages.length
                    });
                    renderThumbs();
                }
            });
        });
    }

    $("#btnUpdate").on("click", function () {
        const payload = {
            propertyId: propertyId,
            title: $("#title").val(),
            description: $("#description").val(),
            price: Number($("#price").val()),
            sqft: Number($("#sqft").val()),
            bedRooms: Number($("#bedrooms").val()),
            washRooms: Number($("#washroom").val()),
            balcony: Number($("#balcony").val()),
            address: $("#address").val(),
            area: $("#area").val(),
            district: $("#district").val(),
            status: $("#status").val(),
            images: uploadedImages.map((x, i) => ({ imageId: x.id, url: x.url, sortOrder: i }))
        };

        $.ajax({
            url: apiBase + "/update",
            method: "PUT",
            contentType: "application/json",
            data: JSON.stringify(payload),
            success: function () {
                toastr.success("Property updated");
                setTimeout(() => {
                    window.location.href = "/Dashboard/OwnerDashboard";
                }, 1200);
            },
            error: function () {
                toastr.error("Update failed");
            }
        });
    });
});
