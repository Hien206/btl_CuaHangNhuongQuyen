// Configuration for Franchise Management API
const CONFIG = {
  // Cổng mặc định trỏ vào API Gateway (Ocelot - .NET 8)
  // https://localhost:7221 (Gateway HTTPS)
  // http://localhost:5045 (Gateway HTTP)
  // https://localhost:7134 (Trực tiếp Admin API)
  // https://localhost:7005 (Trực tiếp User API)
  DEFAULT_API_URL: localStorage.getItem("API_BASE_URL") || "https://localhost:7221",

  getApiUrl() {
    return localStorage.getItem("API_BASE_URL") || this.DEFAULT_API_URL;
  },

  setApiUrl(url) {
    if (!url) return;
    url = url.replace(/\/+$/, ""); // xóa dấu / ở cuối nếu có
    localStorage.setItem("API_BASE_URL", url);
  }
};
