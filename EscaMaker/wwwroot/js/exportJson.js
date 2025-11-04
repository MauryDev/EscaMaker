


var exportJsonMethods = {
    async GenerateJSONFile(contentStreamReference, fileName) {
        var arrayBuffer = await contentStreamReference.arrayBuffer();
        const blob = new Blob([arrayBuffer], {
            type: "application/json",
        });
        const url = URL.createObjectURL(blob);
        const anchorElement = document.createElement('a');
        anchorElement.href = url;
        anchorElement.download = fileName ?? '';
        anchorElement.click();
        anchorElement.remove();
        URL.revokeObjectURL(url);
    }
    
}

export default exportJsonMethods;