// Configuration for Franchise Management API
const CONFIG = {
  // Default API Port for Admin (.NET 8):
  // https://localhost:7134 (Mặc định khi chạy profile HTTPS trong Visual Studio)
  // http://localhost:5160 (khi chạy profile HTTP)
  // https://localhost:44390 (khi chạy IIS Express)
  DEFAULT_API_URL: localStorage.getItem("API_BASE_URL") || "https://localhost:7134",

  getApiUrl() {
    return localStorage.getItem("API_BASE_URL") || this.DEFAULT_API_URL;
  },

  setApiUrl(url) {
    if (!url) return;
    url = url.replace(/\/+$/, ""); // xóa dấu / ở cuối nếu có
    localStorage.setItem("API_BASE_URL", url);
  }
};
