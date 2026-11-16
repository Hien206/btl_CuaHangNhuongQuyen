# HƯỚNG DẪN CHẠY GIAO DIỆN FRONTEND (FRANCHISE MASTER)

## 1. Khởi động Backend API
Trước khi mở giao diện, hãy đảm bảo Backend API đang chạy:
- Cách 1 (Visual Studio): Bấm nút Run (IIS Express hoặc Profile `http` cổng 5160).
- Cách 2 (Terminal):
  ```powershell
  dotnet run --project btl_CuaHangNhuongQuyenADMIN
  ```
  *(Mặc định cổng HTTP là: `http://localhost:5160`)*

---

## 2. Khởi chạy Giao diện Frontend trong VS Code
1. Mở VS Code (VS xanh) và chọn **File** -> **Open Folder...** -> Chọn thư mục `frontend` (`c:\Users\Nguye\source\repos\btl_CuaHangNhuongQuyen\frontend`).
2. Nếu chưa có tiện ích mở rộng **Live Server**, hãy vào tab Extensions (Ctrl+Shift+X) tìm và cài đặt **Live Server** (của tác giả Ritwick Dey).
3. Nhấp chuột phải vào file **`index.html`** -> Chọn **Open with Live Server** (Phím tắt `Alt + L, Alt + O`).
4. Trình duyệt web sẽ tự động mở trang đăng nhập tại địa chỉ: `http://127.0.0.1:5500/index.html`.

---

## 3. Các chức năng có thể trải nghiệm trực tiếp trên giao diện:
- **Đăng nhập nhanh 1-Click**: Bấm nút "Admin" để điền sẵn tài khoản `admin@franchise.local` / `123456`.
- **Đổi cổng API trực tiếp trên giao diện**: Nếu bạn chạy bằng IIS Express (`https://localhost:44390`), bấm nút **Đổi** ở ngay trang đăng nhập để chuyển cổng.
- **Quản lý Cửa Hàng & Hợp Đồng**: Xem danh sách, thêm mới cửa hàng, lập hợp đồng với % phí nhượng quyền.
- **Khai báo Doanh Thu & Xuất Excel**: Nộp báo cáo ngày và bấm nút **"Xuất Báo Cáo Excel"** để tải file `.xlsx`.
- **Hóa Đơn & Xuất PDF**: Phát hành hóa đơn thu phí và bấm nút **"Xuất PDF"** để tải hóa đơn định dạng chuẩn QuestPDF.
- **Cảnh báo tiến trình ngầm**: Xem các thông báo do Background Service tự động quét và gửi về.
