# Vendora: Brand Guidelines

> Nguồn: `src/Web/wwwroot/app.css`, `src/Web/Components/Theming/VendoraTheme.cs`, `src/Web/Designs/2026-11-24-dac-ta-frontend-catalog-profile.md`, `src/Web/Designs/2026-11-25-tien-do-web.md`. Cập nhật 2026-11-25.
> Giá trị màu lấy từ code hiện tại, không phải từ spec cũ (spec ghi primary `#285EA8`, code đã đổi sang `#2563EB`).

## 1. Brand essence

- **Tính cách:** tối giản, đáng tin, sang nhờ tiết chế. Màu là tín hiệu, không là trang trí.
- **Nguyên tắc:** một việc chính trên mỗi màn; viền mảnh thay bóng đổ; khoảng trắng rộng, nhịp 4px; phân cấp bằng cỡ và độ đậm chữ; thao tác phá huỷ luôn có đường lui; không hiện tính năng chưa có API.

## 2. Màu sắc

| Vai trò | Sáng | Tối | Dùng cho |
| --- | --- | --- | --- |
| Primary | `#2563EB` | `#60A5FA` | Nút chính, link, focus, mục đang chọn |
| On primary | `#FFFFFF` | `#0B0F19` | Chữ trên primary |
| Primary container | `#EFF6FF` | `#1E3A8A` | Nền mục chọn, chip role |
| On primary container | `#1E40AF` | `#DBEAFE` | Chữ trên primary container |
| Surface | `#F8FAFC` | `#0B0F19` | Nền trang |
| Surface lowest | `#FFFFFF` | `#111827` | Thẻ, dialog |
| Surface low | `#F2F5FA` | `#16191E` | Vùng lọc, hover hàng bảng |
| Surface container | `#F1F5F9` | `#1A2234` | Nền khối phụ |
| Surface high | `#E6EBF3` | `#23272F` | Chip trung tính, ô ảnh trống |
| On surface | `#0F172A` | `#F8FAFC` | Chữ chính |
| On surface variant | `#475569` | `#94A3B8` | Chữ phụ, header cột |
| Outline | `#CBD5E1` | `#334155` | Viền ô nhập |
| Outline variant | `#E2E8F0` | `#1E293B` | Viền mảnh thẻ, bảng |
| Success / container | `#1E6B3A` / `#DDF3E3` | `#8DD9A4` / `#1B3A25` | "Đang bán", "Còn hàng" |
| Warning / container | `#7A5300` / `#FFF1D6` | `#F3C46B` / `#3D2E0B` | "Sắp hết", phiên sắp hết hạn |
| Error / container | `#EF4444` / `#FEF2F2` | `#F87171` / `#450A0A` | Lỗi, nút nguy hiểm |
| On error container | `#991B1B` | `#FECACA` | Chữ trên error container |

**Phân bố màu (60-30-10):** ~60% surface trung tính, ~30% chữ và viền slate, ≤10% primary. Màu trạng thái chỉ xuất hiện trong chip và thông báo, luôn kèm icon hoặc nhãn.

**Vấn đề đã biết:** chữ trắng trên error `#EF4444` đạt 3.76:1 và error làm chữ trên nền sáng đạt 3.6–3.76:1, đều dưới AA (4.5:1). Đề xuất đổi error sáng sang `#DC2626` (4.83) hoặc `#B91C1C` (6.47). Primary, success, warning và toàn bộ cặp ở chế độ tối đều đạt AA.

**Quy tắc:** không viết hex cứng trong `.razor` / `.razor.css`; chỉ dùng token `--md-*`. Dark mode qua `prefers-color-scheme` và `[data-theme="dark"]`.

## 3. Typography

Font: **Inter** (variable 100–900, `wwwroot/fonts/inter`), không thêm font thứ hai.

| Token | Cỡ | Dùng cho |
| --- | --- | --- |
| `--type-display` | clamp(2.25rem, 4vw, 3.75rem), 600 | Hero storefront |
| `--type-headline` | 1.875rem (1.5rem trên mobile), 600 | H1 storefront, trang hồ sơ |
| `--type-admin-title` | 1.5rem, 600 | H1 trang admin |
| `--type-title` | 1.25rem, 600 | Tiêu đề dialog, thẻ |
| `--type-section` | 1rem, 600 | Nhóm trong form |
| `--type-body` | 0.9375rem, 400 | Văn bản |
| `--type-table` | .875rem | Ô bảng |
| `--type-label` | .875rem, 500 | Nhãn, nút |
| `--type-caption` | .75rem, 500, in hoa .04–.12em | Header cột, eyebrow |

Tối đa 3 cỡ chữ trên một màn. Số (giá, id, số lượng) dùng `tabular-nums`, cột số căn phải. Nút: không in hoa, weight 600.

## 4. Hình khối, khoảng cách, chuyển động

- **Khoảng cách:** `--space-1…8` = 4, 8, 12, 16, 24, 32, 48, 64px.
- **Bo góc:** xs 6, small 10, medium 16, large 24, full 9999; nút 10px, cao 40px (38px trên mobile).
- **Viền:** `--border-hairline` = 1px `--md-outline-variant`. Bóng chỉ cho lớp nổi (`--elevation-overlay`); thẻ có `--shadow-card` nhẹ.
- **Bố cục:** appbar 56px, drawer 248px, nội dung tối đa 1280px (720px cho hồ sơ), gutter 16/24/32px. Breakpoint MudBlazor: 600 / 960 / 1280.
- **Chuyển động:** 150–240ms, easing `cubic-bezier(.2, 0, 0, 1)`; tắt khi `prefers-reduced-motion`. Icon `Icons.Material.Outlined`, 20px trong nút/bảng, 24px ở điều hướng.

## 5. Giọng văn

- **Ngôn ngữ:** giao diện mặc định là tiếng Anh, có tuỳ chọn tiếng Việt (menu ngôn ngữ cạnh menu giao diện). Viết chuỗi bằng tiếng Anh trong code và thêm bản dịch vào `src/Web/Localization/SharedResource.vi.resx`.
- **Câu chữ:** câu ngắn, động từ đầu câu ("Add category" / "Thêm danh mục", "Save changes" / "Lưu thay đổi"). Thông báo thành công nói điều đã xảy ra ("Deleted category T-shirts" / "Đã xoá danh mục Áo thun").
- **Lỗi:** nói chuyện gì xảy ra và làm gì tiếp ("This SKU already exists. Change it and try again."). Không hiện stack trace hay JSON thô.

## 6. Logo và tài sản

`wwwroot/vendora-light.png`, `vendora-dark.png` (đổi theo theme bằng `.theme-logo-light/.theme-logo-dark`), favicon `favicon-vendora-light/dark.png`. Chưa có quy tắc vùng an toàn hay kích thước tối thiểu; cần bổ sung khi có file logo gốc.

## 7. Cần chốt

1. Giữ `#2563EB` hay quay lại `#285EA8` của spec (câu D1)? Nếu giữ, nên cập nhật spec cho khớp.
2. Sửa error về mức đạt AA (mục 2).
3. `Secondary` của MudTheme đang trùng màu success (`#1E6B3A`); `Color.Secondary` sẽ ra xanh lá, dễ đọc nhầm là trạng thái thành công.
