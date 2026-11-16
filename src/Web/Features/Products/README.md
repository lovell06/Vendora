# Product details

Route: `/products/{Id}`. Sample IDs: headphones, speaker, backpack, keyboard,
bottle, lamp. Product cards on the homepage link to these routes.

Uses the existing DemoCatalog presentation fixtures, shared StoreHeader,
ProductCard/ProductGrid, AppButton, QuantitySelector and StatusPage. Desktop uses
an image/summary split; compact windows use one column. Existing theme tokens,
compact button dimensions, sticky header and BackToTop remain shared.

The image reserves square space and falls back to a labeled icon on load failure.
Quantity is a UI limit of 1–99, not a statement of inventory quantity. Out-of-stock
items disable purchasing. Demo add-to-cart emits a snackbar only; it never places
orders or persists a cart. Descriptions are sample copy, not verified product
specifications. Real pricing, stock, cart and detail data need a Gateway contract.

Missing IDs return HTTP 404 on initial server rendering and a recovery page on
client navigation. Quantity/image state resets when route parameters change.
Related products prioritize the current category and exclude the current product.

Validation: `dotnet build --no-restore`. Browser review should cover compact/wide
windows, light/dark, quantity limits, out-of-stock state, related links and missing
IDs. Browser interaction checks have not yet been performed for this page.

## Mobile add-to-cart sheet

Below 600px, quantity is hidden on the detail page. Add to cart opens the shared
AddToCartSheet through MudDialog: image/name/price header, QuantitySelector,
and a full-width confirmation action. It targets 40dvh, with a 300px minimum
where space permits and scrollable content for small landscape windows/zoom.
A dark translucent backdrop has 3px blur; close button, backdrop and Escape
cancel without feedback or cart mutation. MudDialog handles focus trapping,
return focus, page scroll locking and navigation dismissal. Desktop keeps the
inline quantity picker. Homepage cards open the same sheet on mobile.
Confirmation still emits demo feedback only. Build checked; browser interactions
for the sheet still need visual verification.

## Product variants and shared gallery

The product detail UI keeps all image metadata in one product-level `Gallery`.
`ProductVariant` references that gallery through `ImageIds` and one
`PrimaryImageId`; the representative image must be one of its selected images.
This is a presentation model, pending a catalog Gateway contract.

`/products/headphones` demonstrates three color variants and six local SVG
illustrations. Choosing a variant resets quantity, shows its representative
image and filters thumbnails to that variant's images. “View all product photos”
opens the shared gallery without changing the selected purchase variant. Choosing
another variant returns to its filtered gallery. Product code and availability
follow the selected variant; an unavailable variant remains inspectable with
purchasing disabled. Products without variants retain their existing behavior.

`ProductGallery` stays feature-local and uses labeled native thumbnail buttons,
pressed states, reserved image space, keyboard focus, live image captions and
image-error placeholders. It does not auto-rotate. Variant controls include
text labels and a selected checkmark. Layout and colors use the existing tokens.
The mobile cart sheet receives the selected variant label and representative
image, independent of the currently browsed photo; desktop demo feedback also
includes the variant. Route changes reset gallery and variant state.

This change provides storefront selection only. Uploading images, editing
variant/image assignments, persistence, variant prices and real cart requests
require the respective administration UI and Gateway contracts.

Verification for this change: `dotnet build --no-restore` passes with no warnings.
Firefox headless interaction checks passed for thumbnail selection, variant
representative image/SKU changes, all-photo gallery, out-of-stock purchasing,
variant-aware desktop feedback and mobile sheet, products without variants and
unknown product recovery. No horizontal overflow at 375×812, 812×375 and
1280×900. The add-to-cart action now checks the live compact media query so
resizing a desktop window also opens the mobile sheet correctly.
