create proc spTaiKhoan_Get
as
select * from tblTaiKhoan;

create proc spTaiKhoan_Insert
@sTenTaiKhoan varchar(50),
@sMatKhau varchar(50),
@sEmail varchar(50)
as
insert into tblTaiKhoan(sTenTaiKhoan, sMatKhau, sEmail) values(@sTenTaiKhoan, @sMatKhau, @sEmail);

exec spTaiKhoan_Insert 'admin123', '123456', '';