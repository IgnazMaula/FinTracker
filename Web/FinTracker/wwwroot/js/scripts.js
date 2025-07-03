// --------------------
// GLOBAL FUNCTION DEFINITIONS
// --------------------

// Enable tooltips globally
window.enableTooltips = function () {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
};

// Enable popovers globally
window.enablePopovers = function () {
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });
};

// Activate Bootstrap scrollspy for the sticky nav component
window.activateScrollSpy = function () {
    const stickyNav = document.body.querySelector('#stickyNav');
    if (stickyNav) {
        new bootstrap.ScrollSpy(document.body, {
            target: '#stickyNav',
            offset: 82,
        });
    }
};

// Toggle the side navigation
window.toggleSidebar = function () {
    const sidebarToggle = document.body.querySelector('#sidebarToggle');
    if (sidebarToggle) {
        sidebarToggle.addEventListener('click', event => {
            event.preventDefault();
            document.body.classList.toggle('sidenav-toggled');
            localStorage.setItem('sb|sidebar-toggle', document.body.classList.contains('sidenav-toggled'));
        });
    }
};

// Close side navigation when width < LG
window.closeSidebar = function () {
    const sidenavContent = document.body.querySelector('#layoutSidenav_content');
    if (sidenavContent) {
        sidenavContent.addEventListener('click', () => {
            const BOOTSTRAP_LG_WIDTH = 992;
            if (window.innerWidth < BOOTSTRAP_LG_WIDTH && document.body.classList.contains("sidenav-toggled")) {
                document.body.classList.remove("sidenav-toggled");
            }
        });
    }
};

// Add active state to sidebar nav links
window.addActiveState = function () {
    let activatedPath = window.location.pathname.match(/([\w-]+\.html)/);
    activatedPath = activatedPath ? activatedPath[0] : 'index.html';

    const targetAnchors = document.body.querySelectorAll('[href="' + activatedPath + '"].nav-link');

    targetAnchors.forEach(targetAnchor => {
        let parentNode = targetAnchor.parentNode;
        while (parentNode !== null && parentNode !== document.documentElement) {
            if (parentNode.classList.contains('collapse')) {
                parentNode.classList.add('show');
                const parentNavLink = document.body.querySelector(
                    '[data-bs-target="#' + parentNode.id + '"]'
                );
                if (parentNavLink) {
                    parentNavLink.classList.remove('collapsed');
                    parentNavLink.classList.add('active');
                }
            }
            parentNode = parentNode.parentNode;
        }
        targetAnchor.classList.add('active');
    });
};

// Close bootstrap modal
window.closeModal = function (modalId) {
    var modalElement = document.getElementById(modalId);
    if (modalElement) {
        var bootstrapModal = bootstrap.Modal.getInstance(modalElement);
        if (bootstrapModal) bootstrapModal.hide();
    }
};

// Initialize Select2
window.initializeSelect2 = function () {
    $('.select2').select2({
        theme: "bootstrap-5",
        width: $(this).data('width') ? $(this).data('width') :
            $(this).hasClass('w-100') ? '100%' : 'style',
    });
};

//// --------------------
//// OPTIONAL DOMContentLoaded CALLS
//// --------------------
//document.addEventListener('DOMContentLoaded', event => {
//    // Safe to call these immediately for static pages
//    if (window.feather) window.feather.replace();
//    window.enableTooltips();
//    window.enablePopovers();
//    window.toggleSidebar();
//    window.closeSidebar();
//    window.addActiveState();
//});
