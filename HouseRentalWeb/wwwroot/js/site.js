//Showing Loader Option Start


document.addEventListener("DOMContentLoaded", function () {
    $.busyLoadSetup({ animation: "fade", background: "rgba(87, 112, 221,0.5)", spinner: "cube-grid" });
    ShowLoader();
    const preloader = document.getElementById("preloader");
    const progressCircle = document.querySelector(".circle-progress-bar");
    const circleText = document.querySelector(".circle-text");
    const mainContent = document.getElementById("main-container");

    const totalImages = document.images.length;
    let loadedImages = 0;
    let currentDisplayedPercent = 0; // Tracks the animated text value
    let animationFrameId = null; // For canceling animation

    // Update progress bar & animate text
    function updateProgress() {
        const targetPercent = totalImages > 0 ? Math.floor((loadedImages / totalImages) * 100) : 100;
        const offset = 339.292 - (339.292 * (targetPercent / 100));

        // Immediately update progress bar
        progressCircle.style.strokeDashoffset = offset;

        // Cancel any ongoing animation to avoid overlaps
        if (animationFrameId) cancelAnimationFrame(animationFrameId);

        // Animate text from currentDisplayedPercent → targetPercent
        const animateText = () => {
            if (currentDisplayedPercent < targetPercent) {
                currentDisplayedPercent += 5; // Increment by 1 (adjust for speed)
                circleText.textContent = currentDisplayedPercent + "%";
                animationFrameId = requestAnimationFrame(animateText);
            } else {
                circleText.textContent = targetPercent + "%"; // Ensure final value is exact
            }
        };

        animateText(); // Start animation

        // Hide preloader at 100%
        if (targetPercent >= 100) {
            setTimeout(() => {
                //preloader.style.display = "none";
                //mainContent.style.display = "block";
                HideLoader();
            }, 500);
        }
    }

    // If no images, skip loading
    if (totalImages === 0) {
        updateProgress();
    } else {
        // Track each image load/error
        Array.from(document.images).forEach(img => {
            if (img.complete) {
                loadedImages++;
                updateProgress();
            } else {
                img.addEventListener("load", () => {
                    loadedImages++;
                    updateProgress();
                });
                img.addEventListener("error", () => {
                    loadedImages++; // Count broken images as "loaded"
                    updateProgress();
                });
            }
        });
    }
});

function ShowLoader() {
    $.busyLoadFull("show", {
        animationDuration: "slow",
        text: "LOADING ...",
        textMargin: "2rem",
        fontSize: "1.8rem"
    });
}
function HideLoader() {
    $.busyLoadFull("hide");
}


//Showing Loader Option Finish



const sidebar = document.getElementById('sidebar');
const toggleBtn = document.getElementById('sidebarToggle');
const mainContent = document.getElementById('mainContent');

// Mobile toggle
if (toggleBtn) {
    toggleBtn.addEventListener('click', function () {
        sidebar.classList.toggle('show');
    });
}

// Desktop collapsible sidebar
const collapseBtn = document.createElement('button');
collapseBtn.className = 'btn btn-sm btn-light position-absolute top-0 end-0 m-2 d-none d-lg-block';
collapseBtn.innerHTML = '<i class="bi bi-chevron-left"></i>';
collapseBtn.id = 'collapseSidebar';
sidebar.appendChild(collapseBtn);

collapseBtn.addEventListener('click', () => {
    sidebar.classList.toggle('collapsed');
    mainContent.classList.toggle('expanded');
    collapseBtn.innerHTML = sidebar.classList.contains('collapsed')
        ? '<i class="bi bi-chevron-right"></i>'
        : '<i class="bi bi-chevron-left"></i>';
});