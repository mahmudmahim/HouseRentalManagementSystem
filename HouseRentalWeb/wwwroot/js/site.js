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