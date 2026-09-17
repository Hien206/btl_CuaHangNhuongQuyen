create database BTLhuongDV;
go
use BTLhuongDV;
go

-- 1: Vai trò hệ thống (Admin, Chủ cửa hàng, Kế toán)
create table vaitro
(
	id int identity(1,1) primary key,
	mavaitro varchar(30) not null unique, -- ADMIN, CHU_CUA_HANG, KE_TOAN
	tenvaitro nvarchar(100) not null,
	ngaytao datetime2 not null default getdate()
);

-- 2: Tài khoản đăng nhập
create table nguoidung
(
	 id int identity(1,1) primary key,
	 email varchar(150) not null unique,
	 matkhauhash nvarchar(255) not null,
	 hoten nvarchar(150) not null,
	 vaitroid int not null foreign key references vaitro(id),
	 trang_thai_kich_hoat bit not null default 1,
	 token_lam_moi varchar(255) null,
	 han_token_lam_moi datetime2 null,
	 token_dat_lai_mat_khau varchar(255) null,
	 han_token_dat_lai datetime2 null,
	 ngay_tao datetime2 not null default getdate(),
	 ngay_cap_nhat datetime2 null,
	 ngay_xoa datetime2 null
);
create index idx_nguoi_dung_vai_tro on nguoidung(vaitroid);
create index idx_nguoi_dung_email on nguoidung(email);

-- 3: Cửa hàng nhượng quyền
create table cuahang 
(
	 id int identity(1,1) primary key,
	 macuahang varchar(30) not null unique,
	 tencuahang nvarchar(200) not null unique,
	 chu_cua_hang_id int not null foreign key references nguoidung(id),
	 diachi nvarchar(300) null,
	 trangthai varchar(30) not null default 'hoatdong', -- HOAT_DONG, CHO_DUYET, TAM_NGUNG
	 nguoi_tao_id int null foreign key references nguoidung(id),
	 ngay_tao datetime2 not null default getdate(),
	 ngay_cap_nhat datetime2 null,
	 ngay_xoa datetime2 null
);
create index idx_cua_hang_chu on cuahang(chu_cua_hang_id);
create index idx_cua_hang_trang_thai on cuahang(trangthai);
create index idx_cua_hang_ten on cuahang(tencuahang);

-- 4: Hợp đồng nhượng quyền (có versioning)
create table hopdong 
(
	id int identity(1,1) primary key,
	cuahangid int not null foreign key references cuahang(id),
	sohopdong varchar(50) not null unique,
	phantramphi decimal(5,2) not null check (phantramphi >= 0 and phantramphi <= 100),
	ngaybatdau date not null,
	ngayketthuc date not null,
	trangthai varchar(20) not null default 'hieuluc', -- HIEU_LUC, HET_HAN, CHAM_DUT
	duongdanpdf nvarchar(300) null,
	phienban int not null default 1,
	nguoitaoid int null foreign key references nguoidung(id),
	ngaytao datetime2 not null default getdate(),
	ngaycapnhat datetime2 null,
	ngayxoa datetime2 null
);
create index idx_hop_dong_cua_hang on hopdong(cuahangid);
create index idx_hop_dong_trang_thai on hopdong(trangthai);

-- 5: Doanh thu ghi nhận theo ngày
create table doanhthu
(
	id int identity(1,1) primary key,
	cuahangid int not null foreign key references cuahang(id),
	ngaybaocao date not null,
	tongdoanhthu decimal(15,2) not null check (tongdoanhthu >= 0),
	ghichu nvarchar(300) null,
	trangthai varchar(20) not null default 'dunghan', -- DUNG_HAN, TRE_HAN
	nguoinhapid int null foreign key references nguoidung(id),
	ngaytao datetime2 not null default getdate(),
	ngayxoa datetime2 null,
	constraint uq_doanhthu_cuahang_ngay unique (cuahangid, ngaybaocao)
);
create index idx_doanh_thu_cua_hang_ngay on doanhthu(cuahangid, ngaybaocao);

-- 6: Khuyến mãi
create table khuyenmai
(
	id int identity(1,1) primary key,
	cuahangid int null foreign key references cuahang(id), -- null = áp dụng toàn hệ thống
	tenkhuyenmai nvarchar(200) not null,
	phantramgiam decimal(5,2) not null check (phantramgiam >= 0 and phantramgiam <= 100),
	ngaybatdau date not null,
	ngayketthuc date not null,
	nguoitaoid int null foreign key references nguoidung(id),
	ngaytao datetime2 not null default getdate(),
	ngayxoa datetime2 null
);
create index idx_khuyen_mai_cua_hang on khuyenmai(cuahangid);

-- 7: Hóa đơn phí nhượng quyền (franchise)
create table hoadon
(
	id int identity(1,1) primary key,
	cuahangid int not null foreign key references cuahang(id),
	hopdongid int not null foreign key references hopdong(id), -- Tham chiếu đúng bảng hopdong
	kythanhtoan varchar(20) not null, -- Vd: '2026-09'
	tienphinhuongquyen decimal(15,2) not null,
	tienthue decimal(15,2) not null default 0,
	tongtien decimal(15,2) not null,
	trangthai varchar(20) not null default 'chuathanhtoan', -- CHUA_THANH_TOAN, DA_THANH_TOAN, QUA_HAN
	duongdanpdf nvarchar(300) null,
	ngaytao datetime2 not null default getdate(),
	ngayxoa datetime2 null,
	constraint uq_hoadon_hopdong_ky unique (hopdongid, kythanhtoan)
);
create index idx_hoa_don_cua_hang on hoadon(cuahangid);
create index idx_hoa_don_trang_thai on hoadon(trangthai);

-- 8: Thanh toán hóa đơn
create table thanhtoan
(
	id int identity(1,1) primary key,
	hoadonid int not null foreign key references hoadon(id),
	sotien decimal(15,2) not null,
	phuongthuc varchar(30) not null default 'chuyenkhoan',
	magiaodich varchar(100) null,
	ngaythanhtoan datetime2 not null default getdate()
);
create index idx_thanh_toan_hoa_don on thanhtoan(hoadonid);

-- 9: Nhật ký hệ thống / Audit logs
create table lichsuthaotac
(
	id int identity(1,1) primary key,
	nguoidungid int null foreign key references nguoidung(id),
	hanhdong varchar(30) not null, -- TAO, SUA, XOA
	tenbang varchar(50) not null, -- Vd: 'hopdong'
	idbanghi int null,
	dulieucu nvarchar(max) null,
	dulieumoi nvarchar(max) null,
	ngaytao datetime2 not null default getdate()
);
create index idx_lich_su_ten_bang on lichsuthaotac(tenbang, idbanghi);
create index idx_lich_su_nguoi_dung on lichsuthaotac(nguoidungid);

-- 10: Thông báo in-app / Cảnh báo
create table thongbao
(
	id int identity(1,1) primary key,
	nguoidungid int not null foreign key references nguoidung(id),
	loaithongbao varchar(30) not null, -- CHAM_BAO_CAO, DEN_HAN_HOA_DON, ...
	noidung nvarchar(500) not null,
	dadoc bit not null default 0,
	ngaytao datetime2 not null default getdate()
);
create index idx_thong_bao_nguoi_dung on thongbao(nguoidungid, dadoc);

-- Check constraints
alter table hopdong
add constraint chk_hopdong_ngay
check (ngayketthuc > ngaybatdau);
go

alter table khuyenmai
add constraint chk_khuyenmai_ngay
check (ngayketthuc >= ngaybatdau);
go

alter table thanhtoan
add constraint chk_thanhtoan_sotien
check (sotien > 0);
go

-- Seed data mẫu ban đầu
insert into vaitro (mavaitro, tenvaitro) values
    ('ADMIN', N'Quản trị viên'),
    ('CHU_CUA_HANG', N'Chủ cửa hàng'),
    ('KE_TOAN', N'Kế toán');

insert into nguoidung (email, matkhauhash, hoten, vaitroid) values
    ('admin@franchise.local',      '$2a$11$rB0ZzKk4vY.zG0X6Wv2e4.1.2.3.4.5.6.7.8.9.0.1.2.3.4', N'Quản trị viên hệ thống', 1),
    ('owner@franchise.local',      '$2a$11$rB0ZzKk4vY.zG0X6Wv2e4.1.2.3.4.5.6.7.8.9.0.1.2.3.4', N'Nguyễn Văn Chủ',         2),
    ('accountant@franchise.local', '$2a$11$rB0ZzKk4vY.zG0X6Wv2e4.1.2.3.4.5.6.7.8.9.0.1.2.3.4', N'Trần Thị Kế Toán',       3);
