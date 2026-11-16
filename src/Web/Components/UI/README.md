# Shared UI

Reusable MudBlazor-backed components for all Vendora features.
MudBlazor 9.12.0 is registered with AddMudServices in Program.cs. These components have no
Gateway, catalog, account or authentication dependencies. Pages own data,
authorization, requests and navigation decisions.

```
Components/
  UI/
    Branding/       BrandLink, AppIcon
    Buttons/        AppButton, ButtonVariant
    Forms/          AppTextField
    Feedback/       InlineFeedback, ConnectionAlert, StatusPage
    Navigation/     SkipLink
    Typography/     PageHeading
  Shared/           ThemePicker (MudMenu)
  Theming/          AppProviders, VendoraTheme, ThemeState
Features/
  Authentication/   Routes, models, validation rules, Gateway and auth layout
  Home/             Catalog state, search, account actions and storefront layout
wwwroot/
  app.css           Semantic tokens, shared component styling, mobile rules
  theme.js          Theme preference and system/browser synchronization
```

Feature and component imports expose the UI namespaces. Reuse an existing
component before adding another. Keep a component inside its feature until it
has a cross-feature responsibility. Do not put business DTOs or service calls
in UI components. ThemePicker uses MudMenu/MudMenuItem and Material icons.
MudBlazor components require an interactive render boundary. Routes and
HeadOutlet use InteractiveServer globally; do not reintroduce isolated page
boundaries around the existing features. Routes includes AppProviders exactly
once, providing MudThemeProvider, MudPopoverProvider, MudDialogProvider and
MudSnackbarProvider to every route through the shared interactive circuit.

VendoraTheme defines the branded light/dark palettes, Inter typography and 4px
corners. ThemeState is scoped to the circuit and synchronizes MudThemeProvider
and ThemePicker. The small head-loaded theme.js script applies the saved theme
before Blazor starts, observes system/storage changes, and notifies ThemeState.
Keep the bootstrap CSS color roles in app.css aligned with VendoraTheme.
`vendora-theme` stores appearance only; credentials are never stored here.

## Examples

```razor
<PageHeading Title="Create account" Eyebrow="VENDORA ACCOUNT" />

<EditForm Model="model" OnValidSubmit="Save" FormName="example">
    <DataAnnotationsValidator />
    <AppTextField Id="email" Label="Email address" Type="email"
                  @bind-Value="model.Email" Autocomplete="email" />
    <AppTextField Id="password" Label="Password"
                  @bind-Value="model.Password" Autocomplete="new-password"
                  RevealPassword="true" Hint="Use at least 8 characters." />
    <AppButton Type="submit" Busy="busy" BusyText="Saving…">Save</AppButton>
</EditForm>

<AppButton Variant="ButtonVariant.Text" OnClick="Cancel">Cancel</AppButton>
<AppButton Variant="ButtonVariant.Icon" aria-label="Open cart" OnClick="OpenCart">
    <AppIcon Name="bag" />
</AppButton>
<InlineFeedback Message="Saved." Success="true" />
```

- **AppButton** defaults to `type="button"`. Explicitly use `Type="submit"` in
  forms. Busy disables the button and optionally replaces its label. It supports
  primary/text/icon variants, a class, event callback and additional HTML/ARIA
  attributes. It delegates rendering, states and ripple to MudButton. Attributes
  are merged into UserAttributes; do not pass UserAttributes alongside unmatched
  ARIA attributes to the same Mud component. Navigation uses MudButton Href or
  native links. Icon actions require an accessible name.
- **AppTextField** belongs inside an EditForm. `@bind-Value` supplies the field
  expression to MudTextField.For, preserving EditContext/DataAnnotations
  validation and field notifications. IDs must be unique per document.
  MudTextField owns floating labels, hints and validation errors. Password reveal
  uses its labeled end adornment. Password rules remain in feature models.
  Keep EditForm + DataAnnotationsValidator; do not nest a MudForm inside it.
- **InlineFeedback** uses MudAlert and accepts a message, success state and optional contextual
  action content. It does not depend on AuthResult or assume transport success.
- **BrandLink** links to `/` and follows the resolved theme.
- **AppIcon** wraps MudIcon with Material outlined icons; accessible names belong to the containing
  control. Existing names: search, bag, user, grid.
- **PageHeading** owns the h1 and optional decorative eyebrow. Use one per page.
- **SkipLink** targets a focusable main region; **ConnectionAlert** should appear
  once per layout, using the ID expected by Blazor.

Styles consume semantic tokens in app.css. Button dimensions and radius are
shared tokens; compact-window overrides preserve the agreed 32px/4px buttons.
Inputs stay 16px. Shared styles are global; feature CSS isolation only styles
feature composition. When targeting component descendants from isolated CSS,
use a scoped parent with `::deep`.

Official setup reference: https://mudblazor.com/getting-started/installation

Validation: `dotnet build --no-restore`. For changes to form behavior, also
check invalid/empty fields, password confirmation/reveal, busy submission and
Gateway feedback in a browser. Check light/dark and compact/desktop layouts.

## Integration verification

Firefox headless checks during migration passed: required-field messages on
login/registration; password reveal; cross-field password comparison; valid
registration reaching the unconfigured-Gateway feedback; MudMenu theme choices
and persisted appearance across page loads; catalog search; and no horizontal
overflow at 375x812, 812x375 and 1280x900. Compact submit height was 32px and
input font size 16px. These checks do not verify a real authenticated session.

## Status pages

StatusPage displays a status code, title, short description, Material icon and
home link. NotFound and Error compose it inside StatusLayout, which provides
branding, theme selection, a skip link and the main landmark. Mobile hides the
decorative icon and uses smaller spacing. The home link targets the top-level
page so recovery does not rely on preserving the previous interactive circuit.
HTTP response codes belong to the routed pages, not this shared component.
Checked `/not-found` and unknown routes return 404; `/Error` returns 500.

## Product cards

`Catalog/ProductCard` wraps MudCard, MudCardContent, MudCardActions and AppButton.
Required inputs: Name and PriceText (formatted by the calling feature).
Optional Category, ImageUrl, ImageAlt, DetailsHref, nullable InStock, Busy and
OnAddToCart. No stock claim is shown when stock is unknown. Missing/broken images
use a themed placeholder; changing ImageUrl resets image failure state.
An action only appears with a callback, and is disabled while busy or out of stock.
The feature owns actual cart requests and success/error handling. Only pass a
DetailsHref when a real route exists. Names wrap rather than hiding essential text.
The product name link extends across the card, so clicking its image or content
opens DetailsHref. Link text keeps the product name color without underlining,
including hover and visited states. Keyboard focus outlines the card. Cart actions
sit above the extended link and retain their own behavior.
On devices with a mouse, linked cards lift with a slight perspective tilt and
soft shadow on hover, rising a little farther while the link is held. The outer
card keeps its layout space; transitions animate transform and shadow opacity.
Touch devices lift the card and reveal its shadow while the product link is
pressed, with an 80ms response and no persistent hover or navigation delay.
Releasing or canceling the press restores the card. Reduced motion disables
the tilt/lift while preserving shadow feedback.

`ProductGrid` provides 2 compact columns, 3 from 900px, 4 from 1200px and one below
360px. Cards use square reserved image space, lazy images, local semantic tokens,
14px mobile names, 16px mobile prices and the shared 32px/4px mobile buttons.

Visit `/preview/product-cards` in Development to review clearly labeled samples,
including unavailable imagery and out-of-stock state. Preview cart clicks only
show a preview snackbar. In production that route returns 404. The storefront
continues to show its truthful empty catalog until a catalog contract is wired.

```razor
<ProductGrid>
    @foreach (var product in products)
    {
        <ProductCard @key="product.Id" Name="product.Name"
                     PriceText="product.FormattedPrice" ImageUrl="product.ImageUrl" />
    }
</ProductGrid>
```

## Sticky headers and back to top

Storefront, authentication and status headers use sticky positioning and an
opaque theme surface. BackToTop is mounted once beside the global MudBlazor
providers. It wraps MudScrollToTop and a labeled MudIconButton, appears after
240px of page scrolling, and stays below popovers/dialogs. ScrollBehavior.Auto
uses the document's smooth scrolling; reduced-motion switches it to auto.
Safe-area offsets, focus scroll padding and catalog bottom clearance keep
navigation and recovery controls usable on compact windows.

## QuantitySelector

Reusable integer quantity control in `Forms/QuantitySelector.razor`. Used on
`/products/headphones`. Uses MudIconButton and a native numeric input: labeled
minus/plus actions, keyboard entry, theme-aware tonal surfaces and 4px corners.
It uses the compact mobile sizing requested for the storefront.

```razor
<QuantitySelector @bind-Value="quantity" Min="1" Max="99" />
<QuantitySelector @bind-Value="quantity" Min="0" Max="available" Step="2"
                  Label="Quantity" Disabled="unavailable" Busy="saving" />
```

Parameters: Value/ValueChanged, Min (default 1), Max (99), Step (1), Label, Id,
Disabled, Busy, Class. Supply a unique Id when needed; otherwise one is generated.
Min must be nonnegative, Max >= Min and Step > 0. External values are displayed
clamped to the current range; callers should keep their bound state valid.
Step controls +/- increments; manual entry accepts any integer within the range.
Native step validation follows the Step parameter if used inside a form.
Invalid/empty input restores the prior quantity; out-of-range input clamps on
commit. Controls disable at their limits and while a change callback is pending.
Long arithmetic before clamping prevents overflow on increments. The feature
owns stock limits, persistence, async failures and confirmation feedback.

Validation: `dotnet build --no-restore` passes. Browser checks should cover
limits, direct entry, empty input, disabled/busy states, keyboard operation,
light/dark and compact layout.
