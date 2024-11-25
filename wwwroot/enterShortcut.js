window.enterShortcut = (dotNetHelper) => {
    document.addEventListener('keydown', function (event) {
        if (event.key === 'Enter') {  // Check if the pressed key is 'Enter'
            event.preventDefault(); // Prevent default action if needed
            dotNetHelper.invokeMethodAsync("ExecuteShortcut");
        }
    });
};
