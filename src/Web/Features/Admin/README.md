# Admin (Vendora Admin)

Khu vực quản trị nằm trong Vendora.Web, tại `/admin`. Chỉ tài khoản `Admin` và `Employee` vào được (xem `_Imports.razor`). Khách hàng `Customer` và khách chưa đăng nhập không vào được; chưa đăng nhập sẽ được chuyển tới `/login` kèm `returnUrl`.

Giai đoạn hiện tại (G0): khung layout (`Shared/AdminLayout.razor`), menu, tiêu đề trang dùng chung, và trang Tổng quan trống. Các màn quản lý danh mục, sản phẩm, tồn kho, đơn hàng và người dùng sẽ thêm ở các giai đoạn sau theo [plan](../../../../reviews/2026-11-24-plan-giao-dien-admin.md).

Phân quyền ở giao diện chỉ là trải nghiệm. Chốt chặn thật là policy ở từng service API.
