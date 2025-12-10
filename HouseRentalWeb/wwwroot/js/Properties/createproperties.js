// wwwroot/js/properties-create.js
$(function () {
    const steps = [1, 2, 3, 4, 5];
    let current = 1;
    const uploadedImages = []; // {url, id, file, isCover, sortOrder}
    const apiBase = "https://localhost:7152/api/Properties";

    // stepper UI
    function setStep(n) {
        current = n;
        $(".step").removeClass("active");
        $(`.step[data-step=${n}]`).addClass("active");
        $(".step-page").removeClass("active");
        $(`#step-${n}`).addClass("active");

        $("#btnPublish").toggle(n === 5);
        $("#btnNext").toggle(n < 5);
        $("#btnPrev").toggle(n > 1);
    }
    setStep(1);

    $("#btnNext").on("click", () => {
        if (!validateStep(current)) return;
        if (current < 5) setStep(current + 1);
        if (current + 1 === 5) renderReview();
    });

    $("#btnPrev").on("click", () => setStep(Math.max(1, current - 1)));

    $("#btnSaveDraft").on("click", () => {
        submitProperty("Draft");
    });

    $("#btnPublish").on("click", () => {
        submitProperty($("#status").val() || "Published");
    });

    function showToast(text) {
        $("#toast").text(text).fadeIn(200).delay(1800).fadeOut(400);
    }

    // Basic validation per step
    function validateStep(step) {
        let ok = true;
        $(".error").text("");
        if (step === 1) {
            const title = $("#title").val().toString().trim();
            if (!title) { $("#err-title").text("Title is required"); ok = false; }
        }
        if (step === 2) {
            const price = Number($("#price").val());
            const sqft = Number($("#sqft").val());
            const bedrooms = Number($("#bedrooms").val());
            if (!price || price <= 0) { $("#err-price").text("Enter a valid price"); ok = false; }
            if (!sqft || sqft <= 0) { $("#err-sqft").text("Enter sqft"); ok = false; }
            if (isNaN(bedrooms) || bedrooms < 0) { $("#err-bedrooms").text("Enter bedrooms"); ok = false; }
        }
        if (step === 3) {
            const address = $("#address").val().toString().trim();
            const district = $("#district").val().toString().trim();
            if (!address) { $("#err-address").text("Address is required"); ok = false; }
            if (!district) { $("#err-district").text("District is required"); ok = false; }
        }
        if (step === 4) {
            if (uploadedImages.length === 0) { $("#err-images").text("Please upload at least one image"); ok = false; }
        }
        return ok;
    }

    // Dropzone and file selection
    $("#btnSelectFiles").on("click", () => $("#fileInput").click());
    $("#fileInput").on("change", function () {
        const files = Array.from(this.files || []);
        uploadFiles(files);
        $(this).val("");
    });

    const dz = $("#dropzone");
    dz.on("dragover", (e) => { e.preventDefault(); dz.addClass("dragover"); });
    dz.on("dragleave", (e) => { e.preventDefault(); dz.removeClass("dragover"); });
    dz.on("drop", (e) => {
        e.preventDefault();
        dz.removeClass("dragover");
        const files = Array.from(e.originalEvent.dataTransfer.files || []);
        uploadFiles(files);
    });

    function uploadFiles(files) {
        if (!files || files.length === 0) return;
        // small client-side filter
        const images = files.filter(f => /image\//.test(f.type));
        if (images.length === 0) { showToast("No image files found"); return; }

        images.forEach(file => {
            const fd = new FormData();
            fd.append("file", file);
            // progress UI
            const placeholder = $(`<div style="width:120px;height:90px;display:flex;align-items:center;justify-content:center;border-radius:6px;background:#f5f7fb;">Uploading...</div>`);
            $("#thumbList").append(placeholder);

            $.ajax({
                url: apiBase + "/upload-image",
                method: "POST",
                data: fd,
                processData: false,
                contentType: false,
                success: function (resp) {
                    placeholder.remove();
                    const absoluteUrl = apiBase.replace("/api/Properties", "") + resp.url;

                    const item = {
                        url: absoluteUrl,
                        id: resp.id || absoluteUrl,
                        sortOrder: uploadedImages.length
                    };

                    //const item = { url: resp.url, id: resp.id || resp.url, sortOrder: uploadedImages.length };
                    uploadedImages.push(item);
                    renderThumbs();
                    renderReview();
                },
                error: function (xhr) {
                    placeholder.remove();
                    showToast("Upload failed");
                }
            });
        });
    }

    function renderThumbs() {
        const list = $("#thumbList").empty();
        uploadedImages.forEach((img, idx) => {
            const card = $(`
                <div style="position:relative;">
                    <img class="thumb" src="${img.url}" data-idx="${idx}" />
                    <div class="remove" data-idx="${idx}" title="Remove">&times;</div>
                </div>
            `);
            card.find(".remove").on("click", function () {
                const i = parseInt($(this).data("idx"));
                uploadedImages.splice(i, 1);
                renderThumbs();
            });
            list.append(card);
        });
    }

    function renderReview() {
        const ownerId = $("#ownerId").val();
        const title = $("#title").val();
        const price = $("#price").val();
        const sqft = $("#sqft").val();
        const bedrooms = $("#bedrooms").val();
        const address = $("#address").val();
        const area = $("#area").val();
        const district = $("#district").val();

        const html = `
            <div style="display:flex;gap:12px;">
                <div style="width:240px;">
                    <img src="${uploadedImages[0]?.url || ''}" style="width:100%; height:160px; object-fit:cover; border-radius:8px" />
                </div>
                <div>
                    <h4>${escapeHtml(title)}</h4>
                    <p style="margin:6px 0; color:#666">${address} ${area ? ',' + area : ''} ${district ? ',' + district : ''}</p>
                    <div style="display:flex; gap:16px; color:#333; margin-top:8px;">
                        <div><strong>${price}</strong> / month</div>
                        <div>${sqft} sqft</div>
                        <div>${bedrooms} beds</div>
                    </div>
                    <div style="margin-top:8px;">${uploadedImages.length} images</div>
                </div>
            </div>
        `;
        $("#reviewCard").html(html);
    }

    function escapeHtml(s) {
        if (!s) return "";
        return $('<div>').text(s).html();
    }

    // Final payload and create

    function getUserIdFromToken() {
        const token = localStorage.getItem("token");
        if (!token) return "";

        try {
            const payload = JSON.parse(atob(token.split(".")[1]));
            return payload["UserID"] || "";
        } catch {
            return "";
        }
    }

    let ownerIDFromToken = getUserIdFromToken();

    $(document).ready(function () {
        console.log(ownerIDFromToken);
    })

    function submitProperty(targetStatus) {
        // Validate all steps
        if (!validateStep(1) || !validateStep(2) || !validateStep(3)) {
            toastr.error("Please fix validation errors");
            return;
        }
        if (uploadedImages.length === 0) {
            toastr.error("Upload at least one image");
            return;
        }

        const payload = {
            title: $("#title").val(),
            description: $("#description").val(),
            price: Number($("#price").val()),
            sqft: Number($("#sqft").val()),
            bedrooms: Number($("#bedrooms").val()),
            balcony: Number($("#balcony").val()),
            washroom: Number($("#washroom").val()),
            address: $("#address").val(),
            area: $("#area").val(),
            district: $("#district").val(),
            ownerId: getUserIdFromToken(),
            status: targetStatus || $("#status").val(),
            images: uploadedImages.map((x, i) => ({ url: x.url, sortOrder: i }))
        };

        // disable buttons
        $("#btnPublish, #btnSaveDraft, #btnNext, #btnPrev").prop("disabled", true);

        $.ajax({
            url: apiBase + "/createproperty",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(payload),
            success: function (resp) {
                if (!resp) {
                    toastr.error("Failed to create property");
                    return;
                }
                toastr.success("Property created");
                // redirect to Owner properties list
                setTimeout(() => { window.location.href = "/Dashboard/OwnerDashboard"; }, 1200);
            },
            error: function () {
                toastr.error("Failed to create property");
                $("#btnPublish, #btnSaveDraft, #btnNext, #btnPrev").prop("disabled", false);
            }
        });
    }
});
