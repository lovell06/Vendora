# Shared UI

Reusable presentation components for Vendora features, built on MudBlazor 9.12.0 as specified in [Vendora.Web.csproj](../../Vendora.Web.csproj). Pages/features own data access, authorization, requests, and business behavior.

## Preview and usage

Follow the [Web instructions](../../README.md#run-and-explore). In Development, open `http://localhost:5111/preview/product-cards` to inspect sample cards, image fallbacks, and out-of-stock states. Preview cart actions only display a snackbar; outside Development, the route displays a 404 page and sets the status code if the response has not started.

Look for a suitable component below before creating another. Components used by only one feature should remain in that feature; for example, `ProductGallery` belongs to Products.

## Component catalog

| Group | Components | Responsibility |
| --- | --- | --- |
| Branding | `BrandLink`, `AppIcon` | Links to `/`, theme-aware logos, and shared icons |
| Buttons | `AppButton`, `ButtonVariant` | Primary/text/icon actions, disabled and busy states |
| Forms | `AppTextField`, `QuantitySelector` | Validated inputs and integer quantity selection |
| Feedback | `InlineFeedback`, `ConnectionAlert`, `StatusPage` | Result messages, connection errors, and status-page content |
| Navigation | `SkipLink`, `StoreHeader`, `BackToTop` | Skip navigation, storefront header, and scrolling to the top |
| Typography | `PageHeading` | An h1 heading and optional eyebrow |
| Catalog | `ProductCard`, `ProductGrid`, `AddToCartSheet` | Cards, product grids, and quantity dialogs |

## Forms and buttons

Insert this Razor fragment inside an `EditForm` in a component that already defines `model`, `busy`, and a `Save` handler:

```razor
<DataAnnotationsValidator />
<AppTextField Id="email" Label="Email address" Type="email"
              @bind-Value="model.Email" Autocomplete="email" />
<AppButton Type="submit" Busy="busy" BusyText="Saving…">Save</AppButton>
```

- `AppButton` defaults to `Type="button"`; use `Type="submit"` to submit forms. `Busy` disables the button and can replace its label through `BusyText`. Icon buttons require an accessible name through `aria-label`.
- `AppTextField` forwards the `ValueExpression` supplied by `@bind-Value` to `MudTextField.For`. Use it with `EditForm` and `DataAnnotationsValidator`, and supply a unique `Id`. `RevealPassword` adds a password visibility control; data rules belong to the feature model.
- Additional attributes are merged into MudBlazor's `UserAttributes`; avoid passing that dictionary alongside duplicate attributes.
- `InlineFeedback` accepts `Message`, `Success`, and optional child content. It does not decide whether a business operation succeeded.
- `AppIcon` supports `search`, `bag`, `user`, and `grid`. Accessible names belong to the containing control.
- Use links or `MudButton Href` for navigation; use `AppButton` for actions.

## Product cards and grids

[ProductCard.razor](Catalog/ProductCard.razor) requires `Name` and `PriceText`; the feature formats prices before passing them in. Optional parameters are `Category`, `ImageUrl`, `ImageAlt`, `DetailsHref`, `InStock`, `Busy`, and `OnAddToCart`.

Missing/broken images display a fallback; changing `ImageUrl` resets the error state. `InStock` is nullable: unknown availability does not produce an out-of-stock label. The cart button appears only when a callback is supplied and is disabled when busy or out of stock. The feature owns cart requests and result feedback.

Insert this example into a component with a `products` collection exposing the referenced properties:

```razor
<ProductGrid>
    @foreach (var product in products)
    {
        <ProductCard @key="product.Id" Name="@product.Name"
                     PriceText="@product.FormattedPrice"
                     ImageUrl="@product.ImageUrl" />
    }
</ProductGrid>
```

Use `@` for string expressions; omitting it passes a literal string. Only supply `DetailsHref` when the route exists. The product name link extends over the image/content, while the cart button retains its separate action. CSS provides hover/press feedback and reduces motion according to `prefers-reduced-motion`.

`ProductGrid` uses one column below 360px, two by default, three from 900px, and four from 1200px. Cards reserve square image space to keep the layout stable during loading.

## Quantity selection and cart sheet

[QuantitySelector.razor](Forms/QuantitySelector.razor) accepts `Value`/`ValueChanged`, `Min` (default 1), `Max` (99), `Step` (1), `Label`, `Id`, `Disabled`, `Busy`, and `Class`.

Example for a component with an integer variable named `quantity`:

```razor
<QuantitySelector @bind-Value="quantity" Min="1" Max="99" />
```

Requirements: `0 <= Min <= Max` and `Step > 0`. Out-of-range values are clamped for display/on commit; the feature must keep its bound state valid. Increment/decrement buttons use `Step`; direct input accepts integers within the range. Empty/invalid input restores the previous quantity. Controls disable at their limits, while disabled/busy, or while a callback is pending. Stock limits and persistence belong to the feature.

[AddToCartSheet.razor](Catalog/AddToCartSheet.razor) displays the name/image/price, an optional variant, and a quantity of 1–99. It returns `DialogResult.Ok(quantity)` on confirmation or a canceled result; it does not call the Cart API. The feature opens the dialog through `IDialogService`, supplies parameters, and decides what happens after it closes.

## Providers, themes, and CSS

[Program.cs](../../Program.cs) registers `AddMudServices` and `ThemeState`. [App.razor](../App.razor) renders `Routes`/`HeadOutlet` in Interactive Server mode; [Routes.razor](../Routes.razor) contains one [AppProviders](../Theming/AppProviders.razor) instance with theme, popover, dialog, snackbar, and `BackToTop` components.

Keep providers at the shared interactive boundary so all routes share state. `ThemeState` is scoped to the Blazor circuit; `theme.js` applies preferences before Blazor starts and synchronizes system/browser changes. `vendora-theme` stores appearance preferences only.

`VendoraTheme` defines palettes and typography; [app.css](../../wwwroot/app.css) contains semantic color roles and shared styles. Feature CSS isolation handles feature composition; use a scoped parent and `::deep` to target component descendants from isolated CSS.

Current tokens use 4px button corners, a default height of 36px, and 32px at widths up to 599px. These are Vendora interface choices; check both CSS tokens and themes when changing them. Inter has a [separate font license](../../wwwroot/fonts/inter/LICENSE.txt).

## Navigation and status pages

`BrandLink` targets `/`; `SkipLink` should target a focusable main region. `ConnectionAlert` uses the ID `blazor-error-ui` and appears once per layout. Headers use sticky positioning; `BackToTop` appears after 240px of scrolling.

`StatusPage` renders only a code, title, description, icon, and recovery link. The `NotFound`, `Error`, and ProductDetails pages set HTTP status codes when the server response has not started; response codes belong to the coordinating page, not the presentation component.

## Verification when making changes

Build using the [Web instructions](../../README.md#verification-when-making-changes). Manually check valid/invalid forms, busy states, keyboard operation, accessible labels, themes, image failures, unknown/out-of-stock availability, quantity limits, dialogs, and compact/wide layouts. Verify business behavior separately in the calling feature.

Builds and browser checks were not run during this documentation update; descriptions and examples were checked statically against source code.

[Web](../../README.md) · [Home](../../Features/Home/README.md) · [Products](../../Features/Products/README.md) · [Authentication](../../Features/Authentication/README.md).

## Design tokens

Tokens live in `wwwroot/app.css` (three blocks: light, `prefers-color-scheme: dark`, `[data-theme="dark"]`) and are mirrored for MudBlazor in `Components/Theming/VendoraTheme.cs`. Do not write hex values in `.razor` or `.razor.css`.

| Group | Tokens |
| --- | --- |
| Colour | `--md-primary`, `--md-on-primary`, `--md-primary-container`, `--md-on-primary-container`, `--md-surface`, `--md-surface-container-lowest/-low/-high` và `--md-surface-container`, `--md-on-surface`, `--md-on-surface-variant`, `--md-outline`, `--md-outline-variant`, `--md-error` (+`-container`, `--md-on-error-container`), `--md-success` / `--md-warning` (+`-container`, `--md-on-*-container`), `--md-scrim` |
| Type | `--type-display/headline/admin-title/title/section/body/table/label/supporting/caption` |
| Space | `--space-1…8` = 4, 8, 12, 16, 24, 32, 48, 64 px |
| Shape | `--shape-xs/small/medium/large/full`, `--button-radius`, `--border-hairline`, `--elevation-overlay` |
| Layout | `--layout-appbar-height` 56px, `--layout-drawer-width` 248px, `--layout-content-max`, `--layout-gutter` |
| Motion | `--motion-fast`, `--motion-medium`, `--motion-ease` |

### Contrast (WCAG AA needs 4.5:1)

| Cặp (chữ / nền) | Chế độ | Tỉ lệ | AA |
| --- | --- | --- | --- |
| Chữ trắng trên primary (`#FFFFFF` / `#2563EB`) | sáng | 5.17 | Đạt |
| Primary trên surface (`#2563EB` / `#F8FAFC`) | sáng | 4.94 | Đạt |
| On primary container / container (`#1E40AF` / `#EFF6FF`) | sáng | 8.01 | Đạt |
| Success / success container (`#1E6B3A` / `#DDF3E3`) | sáng | 5.59 | Đạt |
| Warning / warning container (`#7A5300` / `#FFF1D6`) | sáng | 6.14 | Đạt |
| On error container / container (`#991B1B` / `#FEF2F2`) | sáng | 7.60 | Đạt |
| Chữ trắng trên error (`#FFFFFF` / `#DC2626`) | sáng | 4.83 | Đạt |
| Error trên surface (`#DC2626` / `#F8FAFC`) | sáng | 4.62 | Đạt |
| On surface variant / surface (`#475569` / `#F8FAFC`) | sáng | 7.24 | Đạt |
| On primary / primary (`#0B0F19` / `#60A5FA`) | tối | 7.53 | Đạt |
| On primary container / container (`#DBEAFE` / `#1E3A8A`) | tối | 8.49 | Đạt |
| Success / container (`#8DD9A4` / `#1B3A25`) | tối | 7.50 | Đạt |
| Warning / container (`#F3C46B` / `#3D2E0B`) | tối | 8.10 | Đạt |
| On error container / container (`#FECACA` / `#450A0A`) | tối | 11.16 | Đạt |
| Error trên surface (`#F87171` / `#0B0F19`) | tối | 6.92 | Đạt |
| On surface variant / surface (`#94A3B8` / `#0B0F19`) | tối | 7.47 | Đạt |

## Feedback components

| Component | Use |
| --- | --- |
| `StatusChip` (`StatusKind`) | Status as colour + icon + label: Active, Discontinued, Deleted, OutOfStock, Warning, Admin, Employee, Customer |
| `EmptyState` | Icon, title, description, optional action as child content |
| `ErrorState` | Error message and a "Thử lại" button when `OnRetry` is set |
| `SessionExpiryBanner` | Warns in the last 5 minutes of the session; at expiry signs out and goes to `/login?returnUrl=…` |
