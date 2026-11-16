# Home

The storefront homepage at `/` and `/home` supports browsing, searching, and filtering products. It uses sample data from [DemoCatalog.cs](DemoCatalog.cs) and does not call the Catalog or Cart API.

## Try it

Run Web using the [shared setup instructions](../../README.md#run-and-explore), then open `http://localhost:5111/`.

The initial list contains six sample products. Enter `headphones` and submit the search to filter by name or category; select a category to combine filters. `Clear search` clears the query, while `Reset filters` resets both the query and category. Select a product name/image to open its details.

## Current behavior

- Search uses the submitted query, trims leading/trailing whitespace, and ignores case. Categories are derived from the sample products.
- The displayed count reflects filtered results; an empty result offers a reset action.
- Sample out-of-stock cards disable adding to the cart. On compact screens, adding opens `AddToCartSheet` for quantity selection; confirmation only displays a demo snackbar.
- Guests see `Sign in`. Cart/account controls appear only when an authenticated `AuthenticationState` is supplied; Web does not currently establish that state through login.
- The cart panel only displays an empty state when opened; it does not query or persist a cart.

## Source layout

| File | Responsibility |
| --- | --- |
| [Home.razor](Home.razor) | Listing, filtering, search, panels, and demo cart feedback |
| [HomeLayout.razor](HomeLayout.razor) | Layout and skip navigation link |
| [DemoCatalog.cs](DemoCatalog.cs) | Sample products, prices, images, variants, and availability |
| [Home.razor.css](Home.razor.css) | Layout and compact-screen adjustments |

The page uses `StoreHeader`, `ProductCard`, `ProductGrid`, and controls from [Shared UI](../../Components/UI/README.md). Categories sit beside results on wide screens; the layout switches to one column at widths of 839px or less.

## Verification when making changes

Build using the [Web instructions](../../README.md#verification-when-making-changes). Manually check:

- Searching by name/category, case differences, clearing search, and combined filters.
- Empty results and `Reset filters`.
- Product links, out-of-stock products, and canceling/confirming the compact-screen sheet.
- Keyboard navigation, themes, and layouts without horizontal overflow.

These behaviors were checked against source code; builds and browser checks were not run during this documentation update.

[Product details](../Products/README.md) · [Authentication](../Authentication/README.md) · [Web](../../README.md).
