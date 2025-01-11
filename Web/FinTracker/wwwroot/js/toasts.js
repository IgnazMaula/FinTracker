// For more details see: https://getbootstrap.com/docs/5.0/components/toasts/#usage

window.initializeToasts = () => {
    const toastBasicEl = document.getElementById('toastBasic');
    const toastBasic = new bootstrap.Toast(toastBasicEl);

    const toastBasicTrigger = document.getElementById('toastBasicTrigger');
    if (toastBasicTrigger) {
        toastBasicTrigger.addEventListener('click', event => {
            toastBasic.show();
        });
    }
};

window.showToast = (toastType, message) => {
    const toastEl = document.getElementById(toastType);
    const toast = new bootstrap.Toast(toastEl);
    const messageElement = toastEl.querySelector('.toast-message');
    if (messageElement) {
        messageElement.textContent = message;
    }

    toast.show();
};

document.addEventListener('DOMContentLoaded', function () {
    initializeToasts();
});