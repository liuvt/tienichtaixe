// wwwroot/js/tvtModal.js
window.tvtModal = {
    lock: function (on) {
        const html = document.documentElement;
        const body = document.body;

        if (on) {
            // Giúp modal có thể scroll được khi nội dung dài, nhưng vẫn khóa scroll của trang chính
            html.classList.add("overflow-hidden");
            body.classList.add("overflow-hidden");
            // Giúp footer không bị click được khi modal mở, tránh lỗi do footer có thể nằm ngoài modal
            document.getElementById("siteFooter")?.classList.add("pointer-events-none");
        } else {
            html.classList.remove("overflow-hidden");
            body.classList.remove("overflow-hidden");
            document.getElementById("siteFooter")?.classList.remove("pointer-events-none");
        }
    }
};