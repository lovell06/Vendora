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
