


var PDFMethods = {
    async GeneratePDF(contentStreamReference, fileName) {
        var arrayBuffer = await contentStreamReference.arrayBuffer();
        const blob = new Blob([arrayBuffer],{
            type: "application/pdf",
        });
        const url = URL.createObjectURL(blob);
        const anchorElement = document.createElement('a');
        anchorElement.href = url;
        anchorElement.download = fileName ?? '';
        anchorElement.click();
        anchorElement.remove();
        URL.revokeObjectURL(url);
    },
    async GeneratePreview(contentStreamReference) {

        var arrayBuffer = await contentStreamReference.arrayBuffer();
        const blob = new Blob([arrayBuffer], {
            type: "application/pdf",

        });
        const url = URL.createObjectURL(blob);
        return url
    },
    DeletePreview(url) {
        URL.revokeObjectURL(url);
    }
}

export default PDFMethods;