window.escShortcut = (dotNetHelper) => {
    document.addEventListener('keydown', function (event) {
        if (event.key === 'Escape') {  // Check if the pressed key is 'Escape'
            event.preventDefault(); // Prevent default action if needed
            dotNetHelper.invokeMethodAsync("ExecuteEscShortcut");
        }
    });
};
