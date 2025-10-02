let html5QrcodeScanner = null;

function startScanner(dotNetHelper) {
    const reader = document.getElementById('reader');
    if (!reader) {
        console.error('Element with id "reader" not found.');
        return;
    }

    html5QrcodeScanner = new Html5QrcodeScanner(
        "reader",
        {
            fps: 10,
            qrbox: { width: 250, height: 250 },
            rememberLastUsedCamera: true,
        },
        false
    );

    html5QrcodeScanner.render((decodedText, decodedResult) => {
        dotNetHelper.invokeMethodAsync('OnBarcodeScanned', decodedText);
    }, (errorMessage) => {
        // handle scan error
    });
}

function stopScanner() {
    if (html5QrcodeScanner) {
        html5QrcodeScanner.clear().catch(error => {
            console.error("Failed to clear html5QrcodeScanner.", error);
        });
        html5QrcodeScanner = null;
    }
}
