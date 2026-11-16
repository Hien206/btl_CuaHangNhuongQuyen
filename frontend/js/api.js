// API Service Helper with JWT Token & File Download Support
const API = {
  getToken() {
    return localStorage.getItem("jwt_token") || "";
  },

  setToken(token) {
    localStorage.setItem("jwt_token", token);
  },

  getCurrentUser() {
    try {
      return JSON.parse(localStorage.getItem("current_user") || "null");
    } catch {
      return null;
    }
  },

  setCurrentUser(user) {
    localStorage.setItem("current_user", JSON.stringify(user));
  },

  logout() {
    localStorage.removeItem("jwt_token");
    localStorage.removeItem("current_user");
    window.location.href = "index.html";
  },

  async request(endpoint, options = {}) {
    const baseUrl = CONFIG.getApiUrl();
    const url = `${baseUrl}${endpoint.startsWith('/') ? endpoint : '/' + endpoint}`;

    const headers = {
      "Content-Type": "application/json",
      ...(options.headers || {})
    };

    const token = this.getToken();
    if (token) {
      headers["Authorization"] = `Bearer ${token}`;
    }

    try {
      const response = await fetch(url, {
        ...options,
        headers
      });

      if (response.status === 401) {
        Swal.fire({
          icon: 'warning',
          title: 'Phiên đăng nhập hết hạn',
          text: 'Vui lòng đăng nhập lại để tiếp tục.',
          confirmButtonText: 'Đăng nhập'
        }).then(() => {
          API.logout();
        });
        throw new Error("Unauthorized (401)");
      }

      // Xử lý các loại response
      const contentType = response.headers.get("content-type") || "";
      if (contentType.includes("application/json")) {
        const data = await response.json();
        if (!response.ok) {
          throw new Error(data.message || `Lỗi yêu cầu: ${response.statusText}`);
        }
        return data;
      } else {
        const text = await response.text();
        if (!response.ok) {
          throw new Error(text || `Lỗi yêu cầu: ${response.statusText}`);
        }
        return text;
      }
    } catch (err) {
      console.error(`[API Error] ${endpoint}:`, err);
      throw err;
    }
  },

  // Tải file nhị phân (Excel .xlsx, PDF .pdf)
  async downloadFile(endpoint, defaultFilename = "download") {
    const baseUrl = CONFIG.getApiUrl();
    const url = `${baseUrl}${endpoint.startsWith('/') ? endpoint : '/' + endpoint}`;
    const token = this.getToken();

    const headers = {};
    if (token) {
      headers["Authorization"] = `Bearer ${token}`;
    }

    try {
      const response = await fetch(url, { method: "GET", headers });
      if (!response.ok) {
        throw new Error(`Không thể tải file: ${response.statusText}`);
      }

      // Lấy filename từ header nếu có
      let filename = defaultFilename;
      const disposition = response.headers.get("content-disposition");
      if (disposition && disposition.indexOf("filename=") !== -1) {
        const matches = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/.exec(disposition);
        if (matches != null && matches[1]) {
          filename = matches[1].replace(/['"]/g, '');
        }
      }

      const blob = await response.blob();
      const downloadUrl = window.URL.createObjectURL(blob);
      const a = document.createElement("a");
      a.href = downloadUrl;
      a.download = filename;
      document.body.appendChild(a);
      a.click();
      a.remove();
      window.URL.revokeObjectURL(downloadUrl);
    } catch (err) {
      console.error(`[Download Error] ${endpoint}:`, err);
      throw err;
    }
  }
};
