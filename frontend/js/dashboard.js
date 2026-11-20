// Dashboard Logic for Franchise Master Frontend
let g_listCuaHang = [];
let g_listHopDong = [];
let g_listDoanhThu = [];
let g_listHoaDon = [];
let g_listKhuyenMai = [];
let g_listThongBao = [];

// Khởi chạy khi load trang
document.addEventListener("DOMContentLoaded", () => {
  // 1. Kiểm tra đăng nhập
  const user = API.getCurrentUser();
  const token = API.getToken();
  if (!token || !user) {
    window.location.href = "index.html";
    return;
  }

  // 2. Cập nhật thông tin Header & Huy hiệu vai trò
  document.getElementById("navUserName").innerText = user.hoten || user.email;
  document.getElementById("navUserEmail").innerText = user.email;
  document.getElementById("topApiBadge").innerText = "API: " + CONFIG.getApiUrl();

  // 3. Phân quyền hiển thị giao diện theo vai trò (Role-Based UI)
  applyRolePermissions();

  // 4. Tải toàn bộ dữ liệu
  loadAllData();
});

// Hàm thiết lập phân quyền giao diện hiển thị
function applyRolePermissions() {
  const role = API.getUserRole();
  const badge = document.getElementById("navUserBadge");
  if (badge) {
    if (role === "ADMIN") {
      badge.className = "badge bg-primary ms-1";
      badge.innerText = "Quản Trị Viên";
    } else if (role === "CHU_CUA_HANG") {
      badge.className = "badge bg-success ms-1";
      badge.innerText = "Chủ Cửa Hàng";
    } else if (role === "KE_TOAN") {
      badge.className = "badge bg-warning text-dark ms-1";
      badge.innerText = "Kế Toán";
    } else {
      badge.className = "badge bg-secondary ms-1";
      badge.innerText = role;
    }
  }

  // Nếu là Chủ Cửa Hàng: Ẩn các nút quản trị cao cấp của Admin
  if (role === "CHU_CUA_HANG") {
    document.querySelectorAll(".role-admin").forEach(el => el.classList.add("d-none"));
  } else {
    document.querySelectorAll(".role-admin").forEach(el => el.classList.remove("d-none"));
  }
}

// Chuyển đổi qua lại giữa các Tab
function switchTab(tabName) {
  const tabs = ['overview', 'cuahang', 'hopdong', 'doanhthu', 'hoadon', 'khuyenmai', 'thongbao'];
  tabs.forEach(t => {
    const pane = document.getElementById(`tab-${t}`);
    const btn = document.getElementById(`tab-btn-${t}`);
    if (pane) pane.classList.add('d-none');
    if (btn) btn.classList.remove('active');
  });

  const activePane = document.getElementById(`tab-${tabName}`);
  const activeBtn = document.getElementById(`tab-btn-${tabName}`);
  if (activePane) activePane.classList.remove('d-none');
  if (activeBtn) activeBtn.classList.add('active');
}

// Format tiền tệ VNĐ
function formatMoney(amount) {
  if (isNaN(amount) || amount === null) return "0 đ";
  return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(amount);
}

// Format ngày tháng dd/mm/yyyy
function formatDate(dateStr) {
  if (!dateStr) return "-";
  const d = new Date(dateStr);
  if (isNaN(d.getTime())) return dateStr;
  return d.toLocaleDateString('vi-VN');
}

// ================= TẢI TOÀN BỘ DỮ LIỆU =================
async function loadAllData() {
  try {
    await Promise.allSettled([
      loadCuaHang(),
      loadHopDong(),
      loadDoanhThu(),
      loadHoaDon(),
      loadKhuyenMai(),
      loadThongBao()
    ]);
    updateOverviewStats();
  } catch (err) {
    console.error("Lỗi khi tải dữ liệu tổng:", err);
  }
}

function updateOverviewStats() {
  // Cập nhật số liệu trên thẻ thống kê Dashboard
  document.getElementById("statCuaHang").innerText = g_listCuaHang.length;

  const validContracts = g_listHopDong.filter(h => h.trangthai === 'hieuluc' || !h.trangthai);
  document.getElementById("statHopDong").innerText = validContracts.length;

  const totalRev = g_listDoanhThu.reduce((sum, item) => sum + (Number(item.tongdoanhthu) || 0), 0);
  document.getElementById("statDoanhThu").innerText = formatMoney(totalRev);

  document.getElementById("statThongBao").innerText = g_listThongBao.length;

  // Cập nhật badge unread ở header
  const unreadCount = g_listThongBao.filter(t => !t.dadoc).length;
  const unreadBadge = document.getElementById("unreadBadge");
  if (unreadCount > 0) {
    unreadBadge.innerText = unreadCount;
    unreadBadge.classList.remove("d-none");
  } else {
    unreadBadge.classList.add("d-none");
  }
}

// ================= 1. CỬA HÀNG =================
async function loadCuaHang() {
  try {
    const data = await API.request('/api/CuaHang/get-all');
    g_listCuaHang = Array.isArray(data) ? data : [];
    renderTableCuaHang();
    populateCuaHangSelects();
  } catch (err) {
    document.getElementById("tableCuaHangBody").innerHTML = `<tr><td colspan="7" class="text-center text-danger py-4">Lỗi: ${err.message}</td></tr>`;
  }
}

function renderTableCuaHang() {
  const tbody = document.getElementById("tableCuaHangBody");
  if (g_listCuaHang.length === 0) {
    tbody.innerHTML = `<tr><td colspan="7" class="text-center py-4 text-muted">Chưa có cửa hàng nào được đăng ký.</td></tr>`;
    return;
  }

  const isAdmin = API.getUserRole() === "ADMIN";

  tbody.innerHTML = g_listCuaHang.map(ch => {
    const statusBadge = ch.trangthai === 'hoatdong' 
      ? `<span class="badge bg-success badge-status">Hoạt động</span>`
      : `<span class="badge bg-secondary badge-status">${ch.trangthai}</span>`;

    const actionBtn = isAdmin
      ? `<button class="btn btn-sm btn-outline-danger" onclick="deleteCuaHang(${ch.id})" title="Xóa chi nhánh">
           <i class="bi bi-trash"></i>
         </button>`
      : `<span class="badge bg-light text-muted border"><i class="bi bi-shop me-1"></i>Chi nhánh của bạn</span>`;

    return `
      <tr>
        <td class="ps-3 fw-bold">#${ch.id}</td>
        <td><span class="badge bg-light text-dark border font-monospace">${ch.macuahang}</span></td>
        <td class="fw-semibold text-primary">${ch.tencuahang}</td>
        <td>${ch.diachi || '<span class="text-muted">Chưa cập nhật</span>'}</td>
        <td><span class="badge bg-info bg-opacity-10 text-info">Chủ ID: ${ch.chu_cua_hang_id}</span></td>
        <td>${statusBadge}</td>
        <td class="text-end pe-3">
          ${actionBtn}
        </td>
      </tr>
    `;
  }).join('');
}

function populateCuaHangSelects() {
  const optionsHtml = g_listCuaHang.map(ch => 
    `<option value="${ch.id}">${ch.tencuahang} (${ch.macuahang})</option>`
  ).join('');

  const dtSelect = document.getElementById("dt_cuahangid");
  const hdSelect = document.getElementById("hd_cuahangid");
  const kmSelect = document.getElementById("km_cuahangid");

  if (dtSelect) dtSelect.innerHTML = optionsHtml;
  if (hdSelect) hdSelect.innerHTML = optionsHtml;
  if (kmSelect) kmSelect.innerHTML = `<option value="">-- Toàn Hệ Thống --</option>` + optionsHtml;
}

function openModalCreateCuaHang() {
  document.getElementById("ch_macuahang").value = `CH_${Date.now().toString().slice(-4)}`;
  document.getElementById("ch_tencuahang").value = "";
  document.getElementById("ch_diachi").value = "";
  new bootstrap.Modal(document.getElementById("modalCuaHang")).show();
}

async function handleSaveCuaHang(e) {
  e.preventDefault();
  const user = API.getCurrentUser();
  const payload = {
    macuahang: document.getElementById("ch_macuahang").value.trim(),
    tencuahang: document.getElementById("ch_tencuahang").value.trim(),
    diachi: document.getElementById("ch_diachi").value.trim(),
    trangthai: document.getElementById("ch_trangthai").value,
    chu_cua_hang_id: user ? user.id : 1
  };

  try {
    await API.request('/api/CuaHang/create', { method: 'POST', body: JSON.stringify(payload) });
    bootstrap.Modal.getInstance(document.getElementById("modalCuaHang")).hide();
    Swal.fire({ icon: 'success', title: 'Đăng ký cửa hàng thành công!', timer: 1500, showConfirmButton: false });
    loadCuaHang();
  } catch (err) {
    Swal.fire({ icon: 'error', title: 'Lỗi', text: err.message });
  }
}

async function deleteCuaHang(id) {
  const res = await Swal.fire({
    title: 'Xóa cửa hàng này?',
    text: "Hệ thống sẽ thực hiện xóa mềm (Soft-delete) bản ghi này.",
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#ef4444',
    confirmButtonText: 'Đồng ý xóa',
    cancelButtonText: 'Hủy'
  });

  if (res.isConfirmed) {
    try {
      await API.request(`/api/CuaHang/delete/${id}`, { method: 'DELETE' });
      Swal.fire({ icon: 'success', title: 'Đã xóa thành công', timer: 1200, showConfirmButton: false });
      loadCuaHang();
    } catch (err) {
      Swal.fire({ icon: 'error', title: 'Không thể xóa', text: err.message });
    }
  }
}

// ================= 2. HỢP ĐỒNG =================
async function loadHopDong() {
  try {
    const data = await API.request('/api/HopDong/get-all');
    g_listHopDong = Array.isArray(data) ? data : [];
    renderTableHopDong();
    populateHopDongSelects();
  } catch (err) {
    document.getElementById("tableHopDongBody").innerHTML = `<tr><td colspan="9" class="text-center text-danger py-4">Lỗi: ${err.message}</td></tr>`;
  }
}

function renderTableHopDong() {
  const tbody = document.getElementById("tableHopDongBody");
  if (g_listHopDong.length === 0) {
    tbody.innerHTML = `<tr><td colspan="9" class="text-center py-4 text-muted">Chưa có hợp đồng nào.</td></tr>`;
    return;
  }

  const isAdmin = API.getUserRole() === "ADMIN";

  tbody.innerHTML = g_listHopDong.map(hd => {
    const ch = g_listCuaHang.find(c => c.id === hd.cuahangid);
    const tenCH = ch ? ch.tencuahang : `Cửa hàng #${hd.cuahangid}`;

    const actionBtn = isAdmin
      ? `<button class="btn btn-sm btn-outline-danger" onclick="deleteHopDong(${hd.id})" title="Xóa hợp đồng">
           <i class="bi bi-trash"></i>
         </button>`
      : `<span class="badge bg-light text-success border"><i class="bi bi-check2-circle me-1"></i>Hiệu lực</span>`;

    return `
      <tr>
        <td class="ps-3 fw-bold">#${hd.id}</td>
        <td><span class="badge bg-light text-primary border font-monospace">${hd.sohopdong}</span></td>
        <td><strong>${tenCH}</strong></td>
        <td><span class="badge bg-warning bg-opacity-25 text-dark fw-bold">${hd.phantramphi}%</span></td>
        <td>${formatDate(hd.ngaybatdau)}</td>
        <td>${formatDate(hd.ngayketthuc)}</td>
        <td><span class="badge bg-secondary">v${hd.phienban || 1}</span></td>
        <td><span class="badge bg-success badge-status">${hd.trangthai || 'Hiệu lực'}</span></td>
        <td class="text-end pe-3">
          ${actionBtn}
        </td>
      </tr>
    `;
  }).join('');
}

function populateHopDongSelects() {
  const select = document.getElementById("hdon_hopdongid");
  if (!select) return;

  select.innerHTML = g_listHopDong.map(hd => {
    const ch = g_listCuaHang.find(c => c.id === hd.cuahangid);
    return `<option value="${hd.id}" data-cuahangid="${hd.cuahangid}" data-fee="${hd.phantramphi}">
      ${hd.sohopdong} - ${ch ? ch.tencuahang : 'CH #' + hd.cuahangid} (Phí: ${hd.phantramphi}%)
    </option>`;
  }).join('');
}

function openModalCreateHopDong() {
  const now = new Date();
  const nextYear = new Date();
  nextYear.setFullYear(now.getFullYear() + 2);

  document.getElementById("hd_sohopdong").value = `HD-${now.getFullYear()}-${Date.now().toString().slice(-3)}`;
  document.getElementById("hd_ngaybatdau").value = now.toISOString().split('T')[0];
  document.getElementById("hd_ngayketthuc").value = nextYear.toISOString().split('T')[0];
  new bootstrap.Modal(document.getElementById("modalHopDong")).show();
}

async function handleSaveHopDong(e) {
  e.preventDefault();
  const payload = {
    cuahangid: parseInt(document.getElementById("hd_cuahangid").value),
    sohopdong: document.getElementById("hd_sohopdong").value.trim(),
    phantramphi: parseFloat(document.getElementById("hd_phantramphi").value),
    ngaybatdau: document.getElementById("hd_ngaybatdau").value,
    ngayketthuc: document.getElementById("hd_ngayketthuc").value,
    trangthai: "hieuluc",
    phienban: 1
  };

  try {
    await API.request('/api/HopDong/create', { method: 'POST', body: JSON.stringify(payload) });
    bootstrap.Modal.getInstance(document.getElementById("modalHopDong")).hide();
    Swal.fire({ icon: 'success', title: 'Tạo hợp đồng thành công!', timer: 1500, showConfirmButton: false });
    loadHopDong();
  } catch (err) {
    Swal.fire({ icon: 'error', title: 'Lỗi', text: err.message });
  }
}

async function deleteHopDong(id) {
  const res = await Swal.fire({
    title: 'Xóa hợp đồng này?',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#ef4444',
    confirmButtonText: 'Đồng ý'
  });
  if (res.isConfirmed) {
    try {
      await API.request(`/api/HopDong/delete/${id}`, { method: 'DELETE' });
      Swal.fire({ icon: 'success', title: 'Đã xóa hợp đồng', timer: 1200, showConfirmButton: false });
      loadHopDong();
    } catch (err) {
      Swal.fire({ icon: 'error', title: 'Lỗi', text: err.message });
    }
  }
}

// ================= 3. DOANH THU =================
async function loadDoanhThu() {
  try {
    const data = await API.request('/api/DoanhThu/get-all');
    g_listDoanhThu = Array.isArray(data) ? data : [];
    renderTableDoanhThu();
  } catch (err) {
    document.getElementById("tableDoanhThuBody").innerHTML = `<tr><td colspan="7" class="text-center text-danger py-4">Lỗi: ${err.message}</td></tr>`;
  }
}

function renderTableDoanhThu() {
  const tbody = document.getElementById("tableDoanhThuBody");
  if (g_listDoanhThu.length === 0) {
    tbody.innerHTML = `<tr><td colspan="7" class="text-center py-4 text-muted">Chưa có bản ghi doanh thu nào.</td></tr>`;
    return;
  }

  const isAdmin = API.getUserRole() === "ADMIN";

  tbody.innerHTML = g_listDoanhThu.map(dt => {
    const ch = g_listCuaHang.find(c => c.id === dt.cuahangid);
    const tenCH = ch ? ch.tencuahang : `Cửa hàng #${dt.cuahangid}`;
    const statusBadge = dt.trangthai === 'dunghan' 
      ? `<span class="badge bg-success badge-status">Đúng hạn</span>`
      : `<span class="badge bg-danger badge-status">Trễ hạn</span>`;

    const actionBtn = isAdmin
      ? `<button class="btn btn-sm btn-outline-danger" onclick="deleteDoanhThu(${dt.id})" title="Xóa bản ghi">
           <i class="bi bi-trash"></i>
         </button>`
      : `<span class="badge bg-light text-success border"><i class="bi bi-check me-1"></i>Đã ghi nhận</span>`;

    return `
      <tr>
        <td class="ps-3 fw-bold">#${dt.id}</td>
        <td><strong>${tenCH}</strong></td>
        <td><i class="bi bi-calendar3 me-1 text-muted"></i>${formatDate(dt.ngaybaocao)}</td>
        <td class="fw-bold text-success fs-6">${formatMoney(dt.tongdoanhthu)}</td>
        <td>${statusBadge}</td>
        <td class="text-muted small">${dt.ghichu || ''}</td>
        <td class="text-end pe-3">
          ${actionBtn}
        </td>
      </tr>
    `;
  }).join('');
}

function openModalCreateDoanhThu() {
  document.getElementById("dt_ngaybaocao").value = new Date().toISOString().split('T')[0];
  document.getElementById("dt_tongdoanhthu").value = "";
  document.getElementById("dt_ghichu").value = "Báo cáo doanh số ngày";
  new bootstrap.Modal(document.getElementById("modalDoanhThu")).show();
}

async function handleSaveDoanhThu(e) {
  e.preventDefault();
  const user = API.getCurrentUser();
  const payload = {
    cuahangid: parseInt(document.getElementById("dt_cuahangid").value),
    ngaybaocao: document.getElementById("dt_ngaybaocao").value,
    tongdoanhthu: parseFloat(document.getElementById("dt_tongdoanhthu").value),
    ghichu: document.getElementById("dt_ghichu").value.trim(),
    trangthai: "dunghan",
    nguoinhapid: user ? user.id : 1
  };

  try {
    await API.request('/api/DoanhThu/create', { method: 'POST', body: JSON.stringify(payload) });
    bootstrap.Modal.getInstance(document.getElementById("modalDoanhThu")).hide();
    Swal.fire({ icon: 'success', title: 'Ghi nhận doanh thu thành công!', timer: 1500, showConfirmButton: false });
    loadDoanhThu();
  } catch (err) {
    Swal.fire({ icon: 'error', title: 'Lỗi', text: err.message });
  }
}

async function deleteDoanhThu(id) {
  const res = await Swal.fire({
    title: 'Xóa bản ghi doanh thu này?',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#ef4444',
    confirmButtonText: 'Đồng ý'
  });
  if (res.isConfirmed) {
    try {
      await API.request(`/api/DoanhThu/delete/${id}`, { method: 'DELETE' });
      Swal.fire({ icon: 'success', title: 'Đã xóa bản ghi', timer: 1200, showConfirmButton: false });
      loadDoanhThu();
    } catch (err) {
      Swal.fire({ icon: 'error', title: 'Lỗi', text: err.message });
    }
  }
}

// Xuất file Excel báo cáo Doanh Thu qua API ClosedXML
async function exportExcelDoanhThu() {
  Swal.fire({
    title: 'Đang tạo báo cáo Excel...',
    text: 'Vui lòng chờ trong giây lát',
    allowOutsideClick: false,
    didOpen: () => Swal.showLoading()
  });

  try {
    await API.downloadFile('/api/DoanhThu/export-excel', `BaoCaoDoanhThu_${Date.now()}.xlsx`);
    Swal.close();
    Swal.fire({ icon: 'success', title: 'Đã tải xuống file Excel!', timer: 1500, showConfirmButton: false });
  } catch (err) {
    Swal.fire({ icon: 'error', title: 'Lỗi xuất file', text: err.message });
  }
}

// ================= 4. HÓA ĐƠN =================
async function loadHoaDon() {
  try {
    const data = await API.request('/api/HoaDon/get-all');
    g_listHoaDon = Array.isArray(data) ? data : [];
    renderTableHoaDon();
  } catch (err) {
    document.getElementById("tableHoaDonBody").innerHTML = `<tr><td colspan="8" class="text-center text-danger py-4">Lỗi: ${err.message}</td></tr>`;
  }
}

function renderTableHoaDon() {
  const tbody = document.getElementById("tableHoaDonBody");
  if (g_listHoaDon.length === 0) {
    tbody.innerHTML = `<tr><td colspan="8" class="text-center py-4 text-muted">Chưa có hóa đơn nào.</td></tr>`;
    return;
  }

  const isAdmin = API.getUserRole() === "ADMIN";

  tbody.innerHTML = g_listHoaDon.map(hd => {
    const ch = g_listCuaHang.find(c => c.id === hd.cuahangid);
    const tenCH = ch ? ch.tencuahang : `Cửa hàng #${hd.cuahangid}`;
    const statusBadge = hd.trangthai === 'dathanhtoan' 
      ? `<span class="badge bg-success badge-status">Đã thanh toán</span>`
      : `<span class="badge bg-danger badge-status">Chưa thanh toán</span>`;

    let actionBtns = `
      <button class="btn btn-sm btn-outline-danger me-1" onclick="exportPdfHoaDon(${hd.id})" title="Tải file PDF QuestPDF">
        <i class="bi bi-file-earmark-pdf-fill text-danger me-1"></i>Xuất PDF
      </button>
    `;

    // Nếu chưa thanh toán, chủ quán có thể thanh toán trực tuyến
    if (hd.trangthai !== 'dathanhtoan') {
      actionBtns += `
        <button class="btn btn-sm btn-success me-1" onclick="payHoaDon(${hd.id}, ${hd.tongtien})" title="Xác nhận nộp phí nhượng quyền">
          <i class="bi bi-credit-card me-1"></i>Thanh Toán
        </button>
      `;
    }

    if (isAdmin) {
      actionBtns += `
        <button class="btn btn-sm btn-outline-secondary" onclick="deleteHoaDon(${hd.id})" title="Xóa hóa đơn">
          <i class="bi bi-trash"></i>
        </button>
      `;
    }

    return `
      <tr>
        <td class="ps-3 fw-bold">#${hd.id}</td>
        <td><span class="badge bg-light text-dark border">${hd.kythanhtoan}</span></td>
        <td><strong>${tenCH}</strong></td>
        <td>${formatMoney(hd.tienphinhuongquyen)}</td>
        <td>${formatMoney(hd.tienthue)}</td>
        <td class="fw-bold text-danger">${formatMoney(hd.tongtien)}</td>
        <td>${statusBadge}</td>
        <td class="text-end pe-3">
          ${actionBtns}
        </td>
      </tr>
    `;
  }).join('');
}

function openModalCreateHoaDon() {
  const now = new Date();
  const currentMonth = `${now.getFullYear()}-${String(now.getMonth() + 1).padStart(2, '0')}`;
  document.getElementById("hdon_kythanhtoan").value = currentMonth;
  document.getElementById("hdon_tienphi").value = "1000000";
  calcHoaDonTotal();
  new bootstrap.Modal(document.getElementById("modalHoaDon")).show();
}

function calcHoaDonTotal() {
  const phi = parseFloat(document.getElementById("hdon_tienphi").value) || 0;
  const thue = parseFloat(document.getElementById("hdon_tienthue").value) || Math.round(phi * 0.1);
  document.getElementById("hdon_tienthue").value = thue;
  document.getElementById("hdon_tongtien").value = phi + thue;
}

function onHoaDonContractChange() {
  calcHoaDonTotal();
}

async function handleSaveHoaDon(e) {
  e.preventDefault();
  const selectContract = document.getElementById("hdon_hopdongid");
  const selectedOption = selectContract.options[selectContract.selectedIndex];
  const cuahangid = selectedOption ? parseInt(selectedOption.getAttribute("data-cuahangid")) : 1;

  const payload = {
    cuahangid: cuahangid,
    hopdongid: parseInt(selectContract.value),
    kythanhtoan: document.getElementById("hdon_kythanhtoan").value.trim(),
    tienphinhuongquyen: parseFloat(document.getElementById("hdon_tienphi").value),
    tienthue: parseFloat(document.getElementById("hdon_tienthue").value) || 0,
    tongtien: parseFloat(document.getElementById("hdon_tongtien").value),
    trangthai: "chuathanhtoan"
  };

  try {
    await API.request('/api/HoaDon/create', { method: 'POST', body: JSON.stringify(payload) });
    bootstrap.Modal.getInstance(document.getElementById("modalHoaDon")).hide();
    Swal.fire({ icon: 'success', title: 'Phát hành hóa đơn thành công!', timer: 1500, showConfirmButton: false });
    loadHoaDon();
  } catch (err) {
    Swal.fire({ icon: 'error', title: 'Lỗi', text: err.message });
  }
}

async function exportPdfHoaDon(id) {
  Swal.fire({
    title: 'Đang tạo Hóa Đơn PDF...',
    text: 'Sử dụng thư viện QuestPDF',
    allowOutsideClick: false,
    didOpen: () => Swal.showLoading()
  });

  try {
    await API.downloadFile(`/api/HoaDon/export-pdf/${id}`, `HoaDon_NhuongQuyen_${id}.pdf`);
    Swal.close();
    Swal.fire({ icon: 'success', title: 'Đã tải xuống hóa đơn PDF!', timer: 1500, showConfirmButton: false });
  } catch (err) {
    Swal.fire({ icon: 'error', title: 'Lỗi tải PDF', text: err.message });
  }
}

async function deleteHoaDon(id) {
  const res = await Swal.fire({
    title: 'Xóa hóa đơn này?',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#ef4444',
    confirmButtonText: 'Đồng ý'
  });
  if (res.isConfirmed) {
    try {
      await API.request(`/api/HoaDon/delete/${id}`, { method: 'DELETE' });
      Swal.fire({ icon: 'success', title: 'Đã xóa hóa đơn', timer: 1200, showConfirmButton: false });
      loadHoaDon();
    } catch (err) {
      Swal.fire({ icon: 'error', title: 'Lỗi', text: err.message });
    }
  }
}

// ================= 5. KHUYẾN MÃI =================
async function loadKhuyenMai() {
  try {
    const data = await API.request('/api/KhuyenMai/get-all');
    g_listKhuyenMai = Array.isArray(data) ? data : [];
    renderTableKhuyenMai();
  } catch (err) {
    document.getElementById("tableKhuyenMaiBody").innerHTML = `<tr><td colspan="7" class="text-center text-danger py-4">Lỗi: ${err.message}</td></tr>`;
  }
}

function renderTableKhuyenMai() {
  const tbody = document.getElementById("tableKhuyenMaiBody");
  if (g_listKhuyenMai.length === 0) {
    tbody.innerHTML = `<tr><td colspan="7" class="text-center py-4 text-muted">Chưa có khuyến mãi nào.</td></tr>`;
    return;
  }

  const isAdmin = API.getUserRole() === "ADMIN";

  tbody.innerHTML = g_listKhuyenMai.map(km => {
    let scopeBadge = `<span class="badge bg-primary">Toàn hệ thống</span>`;
    if (km.cuahangid) {
      const ch = g_listCuaHang.find(c => c.id === km.cuahangid);
      scopeBadge = `<span class="badge bg-info text-dark">${ch ? ch.tencuahang : 'Cửa hàng #' + km.cuahangid}</span>`;
    }

    const actionBtn = isAdmin
      ? `<button class="btn btn-sm btn-outline-danger" onclick="deleteKhuyenMai(${km.id})" title="Xóa khuyến mãi">
           <i class="bi bi-trash"></i>
         </button>`
      : `<span class="badge bg-light text-info border">Đang áp dụng</span>`;

    return `
      <tr>
        <td class="ps-3 fw-bold">#${km.id}</td>
        <td class="fw-semibold">${km.tenkhuyenmai}</td>
        <td>${scopeBadge}</td>
        <td><span class="badge bg-danger fs-6">-${km.phantramgiam}%</span></td>
        <td>${formatDate(km.ngaybatdau)}</td>
        <td>${formatDate(km.ngayketthuc)}</td>
        <td class="text-end pe-3">
          ${actionBtn}
        </td>
      </tr>
    `;
  }).join('');
}

function openModalCreateKhuyenMai() {
  const now = new Date();
  const nextMonth = new Date();
  nextMonth.setMonth(now.getMonth() + 1);

  document.getElementById("km_tenkhuyenmai").value = "Ưu đãi Mùa Thu - Giảm 15%";
  document.getElementById("km_phantramgiam").value = "15";
  document.getElementById("km_ngaybatdau").value = now.toISOString().split('T')[0];
  document.getElementById("km_ngayketthuc").value = nextMonth.toISOString().split('T')[0];
  new bootstrap.Modal(document.getElementById("modalKhuyenMai")).show();
}

async function handleSaveKhuyenMai(e) {
  e.preventDefault();
  const user = API.getCurrentUser();
  const cuahangVal = document.getElementById("km_cuahangid").value;

  const payload = {
    cuahangid: cuahangVal ? parseInt(cuahangVal) : null,
    tenkhuyenmai: document.getElementById("km_tenkhuyenmai").value.trim(),
    phantramgiam: parseFloat(document.getElementById("km_phantramgiam").value),
    ngaybatdau: document.getElementById("km_ngaybatdau").value,
    ngayketthuc: document.getElementById("km_ngayketthuc").value,
    nguoitaoid: user ? user.id : 1
  };

  try {
    await API.request('/api/KhuyenMai/create', { method: 'POST', body: JSON.stringify(payload) });
    bootstrap.Modal.getInstance(document.getElementById("modalKhuyenMai")).hide();
    Swal.fire({ icon: 'success', title: 'Tạo khuyến mãi thành công!', timer: 1500, showConfirmButton: false });
    loadKhuyenMai();
  } catch (err) {
    Swal.fire({ icon: 'error', title: 'Lỗi', text: err.message });
  }
}

async function deleteKhuyenMai(id) {
  const res = await Swal.fire({
    title: 'Xóa chương trình khuyến mãi?',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#ef4444',
    confirmButtonText: 'Đồng ý'
  });
  if (res.isConfirmed) {
    try {
      await API.request(`/api/KhuyenMai/delete/${id}`, { method: 'DELETE' });
      Swal.fire({ icon: 'success', title: 'Đã xóa', timer: 1200, showConfirmButton: false });
      loadKhuyenMai();
    } catch (err) {
      Swal.fire({ icon: 'error', title: 'Lỗi', text: err.message });
    }
  }
}

// ================= 6. THÔNG BÁO & CẢNH BÁO =================
async function loadThongBao() {
  try {
    const data = await API.request('/api/ThongBao/get-all');
    g_listThongBao = Array.isArray(data) ? data : [];
    
    // Cập nhật số lượng thông báo trên header và thẻ thống kê
    const unread = g_listThongBao.filter(t => !t.dadoc).length;
    const unreadBadge = document.getElementById("unreadBadge");
    if (unreadBadge) {
      unreadBadge.innerText = unread;
      if (unread > 0) unreadBadge.classList.remove("d-none");
      else unreadBadge.classList.add("d-none");
    }
    const statTb = document.getElementById("statThongBao");
    if (statTb) statTb.innerText = g_listThongBao.length;

    renderTableThongBao();
  } catch (err) {
    document.getElementById("tableThongBaoBody").innerHTML = `<tr><td colspan="6" class="text-center text-danger py-4">Lỗi: ${err.message}</td></tr>`;
  }
}

function renderTableThongBao() {
  const tbody = document.getElementById("tableThongBaoBody");
  const summaryEl = document.getElementById("thongBaoSummaryText");

  if (g_listThongBao.length === 0) {
    tbody.innerHTML = `<tr><td colspan="6" class="text-center py-4 text-muted">Không có cảnh báo ngầm nào. Hệ thống hoạt động bình thường!</td></tr>`;
    if (summaryEl) summaryEl.innerText = "Hệ thống hoạt động bình thường, không có cảnh báo tồn đọng.";
    return;
  }

  // Giới hạn hiển thị 50 cảnh báo mới nhất để đảm bảo giao diện phản hồi mượt mà
  const displayList = g_listThongBao.slice(0, 50);
  if (summaryEl) {
    summaryEl.innerHTML = `Hiển thị <b>${displayList.length}</b> / <b>${g_listThongBao.length}</b> cảnh báo mới nhất quét từ Background Service.`;
  }

  tbody.innerHTML = displayList.map(tb => {
    let typeBadge = `<span class="badge bg-secondary">${tb.loaithongbao}</span>`;
    if (tb.loaithongbao && tb.loaithongbao.includes("CHAM_BAO_CAO")) {
      typeBadge = `<span class="badge bg-danger"><i class="bi bi-clock-history me-1"></i>Chậm Báo Cáo</span>`;
    } else if (tb.loaithongbao && tb.loaithongbao.includes("HOP_DONG")) {
      typeBadge = `<span class="badge bg-warning text-dark"><i class="bi bi-exclamation-triangle me-1"></i>Sắp Hết Hạn Hợp Đồng</span>`;
    }

    return `
      <tr>
        <td class="ps-3 fw-bold">#${tb.id}</td>
        <td class="text-muted small">${formatDate(tb.ngaytao)}</td>
        <td>${typeBadge}</td>
        <td><strong>${tb.noidung}</strong></td>
        <td>${tb.dadoc ? '<span class="badge bg-light text-muted border">Đã xem</span>' : '<span class="badge bg-primary">Mới</span>'}</td>
        <td class="text-end pe-3">
          <button class="btn btn-sm btn-outline-danger" onclick="deleteThongBao(${tb.id})" title="Xóa cảnh báo này">
            <i class="bi bi-trash"></i>
          </button>
        </td>
      </tr>
    `;
  }).join('');
}

async function deleteThongBao(id) {
  try {
    await API.request(`/api/ThongBao/delete/${id}`, { method: 'DELETE' });
    loadThongBao();
  } catch (err) {
    Swal.fire({ icon: 'error', title: 'Lỗi', text: err.message });
  }
}

async function cleanupDuplicatesThongBao() {
  Swal.fire({
    title: 'Đang dọn dẹp thông báo trùng...',
    allowOutsideClick: false,
    didOpen: () => Swal.showLoading()
  });

  try {
    const res = await API.request('/api/ThongBao/cleanup-duplicates', { method: 'DELETE' });
    Swal.close();
    Swal.fire({
      icon: 'success',
      title: 'Dọn dẹp thành công!',
      text: res.message || `Đã loại bỏ ${res.deletedCount || 0} thông báo trùng lặp.`,
      timer: 2000,
      showConfirmButton: false
    });
    loadThongBao();
  } catch (err) {
    Swal.fire({ icon: 'error', title: 'Lỗi dọn dẹp', text: err.message });
  }
}

async function deleteAllThongBao() {
  const res = await Swal.fire({
    title: 'Xóa toàn bộ cảnh báo?',
    text: 'Tất cả các dòng cảnh báo trong cơ sở dữ liệu sẽ bị xóa hoàn toàn.',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#ef4444',
    confirmButtonText: 'Đồng ý xóa hết',
    cancelButtonText: 'Hủy'
  });

  if (res.isConfirmed) {
    try {
      await API.request('/api/ThongBao/delete-all', { method: 'DELETE' });
      Swal.fire({
        icon: 'success',
        title: 'Đã xóa tất cả thông báo',
        timer: 1500,
        showConfirmButton: false
      });
      loadThongBao();
    } catch (err) {
      Swal.fire({ icon: 'error', title: 'Lỗi', text: err.message });
    }
  }
}

// ================= 7. THANH TOÁN HÓA ĐƠN =================
async function payHoaDon(hoadonId, soTien) {
  const { value: maGD } = await Swal.fire({
    title: `Xác nhận thanh toán hóa đơn #${hoadonId}`,
    html: `Số tiền thanh toán: <strong class="text-danger fs-5">${formatMoney(soTien)}</strong><br><span class="text-muted small">Phương thức: <b>Chuyển khoản ngân hàng</b></span>`,
    input: 'text',
    inputLabel: 'Nhập mã giao dịch ngân hàng / biên nhận:',
    inputPlaceholder: 'VD: VCB-202611-99882',
    showCancelButton: true,
    confirmButtonText: '<i class="bi bi-check-circle me-1"></i> Xác nhận thanh toán',
    cancelButtonText: 'Hủy'
  });

  if (maGD !== undefined) {
    try {
      await API.request('/api/ThanhToan/create', {
        method: 'POST',
        body: JSON.stringify({
          hoadonid: hoadonId,
          sotien: soTien,
          phuongthuc: 'chuyenkhoan',
          magiaodich: maGD.trim() || `CK-${Date.now().toString().slice(-6)}`
        })
      });

      Swal.fire({
        icon: 'success',
        title: 'Thanh toán thành công!',
        text: `Đã nộp phí nhượng quyền cho hóa đơn #${hoadonId}.`,
        timer: 1500,
        showConfirmButton: false
      });
      loadHoaDon();
    } catch (err) {
      Swal.fire({
        icon: 'error',
        title: 'Thanh toán thất bại',
        text: err.message
      });
    }
  }
}

