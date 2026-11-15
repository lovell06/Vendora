# Authentication UI

The Web project contains two vertical slices: `Login` and `Register`.
Each slice owns its route, form model, validation and submit interaction.
`Shared` contains only the authentication layout and API Gateway transport.

Routes: `/login` (`/dang-nhap`) and `/register` (`/dang-ky`). `/` opens the storefront home page.

The shared UI wraps MudBlazor components, styled with Vendora Material 3 color roles: outlined fields,
filled primary buttons, text navigation, semantic color roles, keyboard focus,
inline validation, busy and result feedback. CSS supports compact windows and
system dark mode and a System / Light / Dark MudMenu. The layout contains the brand, theme selector, form and navigation between authentication routes and the catalog. Theme preference is stored under `vendora-theme`; credentials are never stored by this preference mechanism.

## Gateway integration

Set `ApiGateway:BaseUrl`, `ApiGateway:LoginPath`, and
`ApiGateway:RegisterPath` in configuration or environment variables.
Paths resolve relative to BaseUrl; include a trailing slash on a base URL with a path.
The defaults are intentionally empty until the backend contract is available.
The UI never calls Services directly and never simulates successful authentication.

Current provisional request shapes:
- Login: `{ email, password }`
- Register: `{ fullName, email, password }`

These DTOs and password rules must be aligned with the gateway contract.
Success currently means the gateway accepted the request. Session/cookie/token
handling, authenticated redirects, email verification and password recovery
require the actual authentication contract and are not implemented here.
No credentials or tokens are persisted in browser storage.

## Validation

Run `dotnet build Vendora.Web` from the Web directory.
Manual browser checks: keyboard navigation, empty/invalid inputs, mismatched
password confirmation, password reveal, repeat submission protection, unavailable
gateway, 360/760/1280px widths, 200% zoom and system dark theme.

## Typography

Inter variable (100–900), self-hosted with its OFL license in `wwwroot/fonts/inter`,
is the system font. It uses `font-display: swap` and a preload.
Semantic type tokens in `app.css` use rem units so browser font preferences scale.
Headline: 32px desktop / 24px compact. Body and inputs: 16px.
Labels and supporting text: 14px. Buttons and theme controls use shared compact tokens: 36px minimum height,
4px corners, and 44px minimum height on touch-capable devices above 599px.
Compact windows up to 599px use 32px buttons, 13px labels and 4px/10px padding. Auth forms use an outlined surface with adaptive padding.
These are Vendora's responsive web choices, not mandatory M3 dimensions.

## Content principle

For all future use cases, show only content that helps complete the task:
clear titles, necessary inputs, actions, validation and recovery feedback.
Avoid slogans, greetings, redundant captions and implementation details in the UI.

The default UI language is English, including validation, status messages and accessible labels.

On compact screens, forms use the page surface without an extra card border,
with 48px inputs and 16px field gaps. Decorative account eyebrows are hidden;
labels, password hints, validation and navigation remain available.

## Shared components

Forms use AppTextField, AppButton, PageHeading and InlineFeedback from
`Components/UI`. AppTextField forwards ValueExpression to MudTextField.For; feature-owned DataAnnotations remain the validation
source of truth. Shared layout pieces use BrandLink, SkipLink, ThemePicker and
ConnectionAlert. See [UI component guide](../../Components/UI/README.md).

## MudBlazor integration checks

Interactive Firefox checks cover required fields, password visibility,
MudMenu dark-theme selection and persistence across pages, mobile form width,
32px submit buttons and 16px input text. The actual account/session integration
still depends on the Gateway contract described above.
