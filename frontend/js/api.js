// API Service Helper with JWT Token & Dynamic Gateway Routing Support
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

  getUserRole() {
    const user = this.getCurrentUser();
    if (!user) return "";
    if (user.vaitro) return user.vaitro;
    if (user.role) return user.role;
    if (user.vaitroid === 1) return "ADMIN";
    if (user.vaitroid === 2) return "CHU_CUA_HANG";
    if (user.vaitroid === 3) return "KE_TOAN";

    // Giải mã vai trò từ JWT Claim nếu user object chưa có
    try {
      const token = this.getToken();
      if (token) {
        const payload = JSON.parse(atob(token.split('.')[1]));
        const r = payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || payload.role;
        if (r) return r;
      }
    } catch {}

    return "ADMIN";
  },

  // Tự động phân luồng URL thông minh qua API Gateway
  resolveUrl(endpoint) {
    const baseUrl = CONFIG.getApiUrl();
    const cleanEp = endpoint.startsWith('/') ? endpoint : '/' + endpoint;

    // Kiểm tra xem địa chỉ có phải là Gateway (port 7221 hoặc 5045)
    const isGateway = baseUrl.includes("7221") || baseUrl.includes("5045");
    if (!isGateway) {
      return `${baseUrl}${cleanEp}`;
    }

    // Nếu endpoint đã có tiền tố admin/user thì giữ nguyên
    if (cleanEp.startsWith('/api/user/') || cleanEp.startsWith('/api/admin/')) {
      return `${baseUrl}${cleanEp}`;
    }

    // Các hàm công khai (Đăng nhập, đăng ký)
    if (cleanEp.includes('/NguoiDung/login') || cleanEp.includes('/NguoiDung/register')) {
      return `${baseUrl}${cleanEp}`;
    }

    // Phân luồng Gateway theo vai trò thực tế
    const role = this.getUserRole();
    if (role === "CHU_CUA_HANG") {
      // Ví dụ: /api/CuaHang/get-all ==> /api/user/CuaHang/get-all (Đẩy sang USER API: 7005)
      const userEp = cleanEp.replace('/api/', '/api/user/');
      return `${baseUrl}${userEp}`;
    } else {
      // ADMIN hoặc KE_TOAN: /api/CuaHang/get-all ==> /api/admin/CuaHang/get-all (Đẩy sang ADMIN API: 7134)
      const adminEp = cleanEp.replace('/api/', '/api/admin/');
      return `${baseUrl}${adminEp}`;
    }
  },

  logout() {
    localStorage.removeItem("jwt_token");
    localStorage.removeItem("current_user");
    window.location.href = "index.html";
  },

  async request(endpoint, options = {}) {
    const url = this.resolveUrl(endpoint);

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

      // 1. Xử lý hết hạn phiên (401 Unauthorized)
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

      // 2. Xử lý vi phạm phân quyền (403 Forbidden)
      if (response.status === 403) {
        Swal.fire({
          icon: 'error',
          title: 'Không có quyền truy cập',
          text: 'Tài khoản của bạn không có quyền thực hiện thao tác này (HTTP 403 Forbidden).',
          confirmButtonText: 'Đã hiểu'
        });
        throw new Error("Forbidden (403): Không có quyền truy cập");
      }

      // 3. Xử lý dữ liệu trả về
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
      console.error(`[API Error] ${endpoint} -> ${url}:`, err);
      throw err;
    }
  },

  // Tải file nhị phân (Excel .xlsx, PDF .pdf)
  async downloadFile(endpoint, defaultFilename = "download") {
    const url = this.resolveUrl(endpoint);
    const token = this.getToken();

    const headers = {};
    if (token) {
      headers["Authorization"] = `Bearer ${token}`;
    }

    try {
      const response = await fetch(url, { method: "GET", headers });
      if (!response.ok) {
        if (response.status === 403) {
          Swal.fire({
            icon: 'error',
            title: 'Từ chối quyền tải file',
            text: 'Tài khoản của bạn không được phép tải tệp tin này.'
          });
        }
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
      console.error(`[Download Error] ${endpoint} -> ${url}:`, err);
      throw err;
    }
  }
};
