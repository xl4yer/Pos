window.ctrlP = (dotNetHelper) => {
    document.addEventListener('keydown', function (event) {
        if (event.shiftKey && event.key === 'P') {
            event.preventDefault(); // Prevent default action if needed
            dotNetHelper.invokeMethodAsync("ctrlPShortcut");
        }
    });
};