# Home

Storefront home at `/` and `/home`. The slice owns its page, styles, layout
and feature composition. Shared components provide branding, icons, buttons and headings.
English UI, shared Inter typography and semantic blue
color tokens support both system themes and compact windows.

Search stores the submitted query and supports clearing it. The cart expands
inline without a modal. Keyboard focus and a skip link support navigation.

No catalog endpoint or cart persistence is configured. Product and category
collections are intentionally empty; counts represent only this empty UI state.
No requests, product data, prices or authenticated session are simulated.
Connect catalog and cart through the API Gateway when their contracts exist.

Validation: `dotnet build Vendora.Web --no-restore`.

Guest navigation contains only Sign in. Cart and account controls require a
cascaded authenticated AuthenticationState; until authentication integration is
configured, they stay hidden. Mobile places cart and account icon buttons on
the right of the navbar. Search inputs retain 16px text with compact padding.

Across all screens, the Vendora logo links to the home route `/`.

## Material 3 refactor

Shared semantic blue tokens and locally hosted Inter are defined in `wwwroot/app.css`.
The catalog uses a two-pane layout above 839px and a single-column layout below it.
The existing empty catalog and authentication-gated actions remain truthful.
`Components/Shared/ThemePicker.razor` offers System / Light / Dark on the catalog
and auth pages. `wwwroot/theme.js` applies the saved preference before first paint,
tracks system changes, and synchronizes ThemeState and MudThemeProvider.
Logos follow the resolved theme. CSS isolation owns catalog-specific layout;
shared controls, auth styling, keyboard focus and reduced motion use global CSS.

At compact widths (up to 599px), decorative eyebrows and the empty-state icon
are hidden. The category selector appears directly below search as a compact,
26px tonal filter chip with 12px text and 4px corners. Its heading is visually hidden
on mobile. Currently only “All products” is available until catalog categories
are connected. The results heading remains accessible without repeating the page
heading visually. The header stays on one row with a text sign-in action.

Shared component architecture and examples: [UI component guide](../../Components/UI/README.md).

The search field and category selection use MudTextField and MudChipSet/MudChip;
shared AppButton/AppIcon render MudButton/MudIcon. MudBlazor providers are
mounted once in interactive Routes. Native layout elements remain for semantic
header/main/aside/section regions. Catalog data remains unconfigured.

Category selection uses single-select filter chips with a checkmark, tonal
selected state, keyboard focus, wrapping labels and 8px gaps. Desktop uses 32px
chips; compact screens retain the requested 26px chip and 12px label. The current
category is bound to page state. No categories or product counts are fabricated.
