# Home

The storefront homepage at `/` and `/home` lists Catalog products with category filtering, server-side search, pagination, and cart/account panels. All data comes from the API Gateway through [GatewayClient](../../Gateway/GatewayClient.cs).

## Try it

Start the backend, run Web using the [shared setup instructions](../../README.md#run-and-explore), then open `http://localhost:5111/`.

## Current behavior

- Categories come from `GET /api/categories`; selecting one reloads `GET /api/products?categoryId=…`. Search sends `search` (name contains, case-insensitive) to the same endpoint; `Load more` fetches the next page (24 per page). The count is the server's total.
- Products must have a primary image, otherwise the Catalog list endpoint fails (known backend issue T15).
- Cards show the price range of the product's variants. The list has no stock information, so cards do not show an out-of-stock label; stock is shown on the detail page.
- `Add to cart` requires sign-in (guests are sent to `/login`). The list carries no variants, so Home loads the product: a product with exactly one in-stock variant is added directly (compact screens first open `AddToCartSheet`); a product with several variants opens its detail page to choose one.
- Signed-in users get `Cart` and `Account` controls. The cart panel (`/?panel=cart` opens it) lists items from `GET /api/carts/list`, names and prices from `POST /api/products/batch`, and supports quantity change and removal. The account panel shows the signed-in email and `Sign out`.
- Failures show the backend message with a retry action.

## Source layout

| File | Responsibility |
| --- | --- |
| [Home.razor](Home.razor) | Listing, filtering, search, panels, and cart actions |
| [HomeLayout.razor](HomeLayout.razor) | Layout and skip navigation link |
| [Home.razor.css](Home.razor.css) | Layout and compact-screen adjustments |

## Verification when making changes

Build using the [Web instructions](../../README.md#verification-when-making-changes). Manually check category and search combinations, empty results, `Load more`, a stopped Gateway, adding as guest vs signed in, single- vs multi-variant products, cart quantity/remove, sign out, themes and compact/wide layouts.

These behaviors were written against the source; builds and browser checks have not been run.

[Product details](../Products/README.md) · [Authentication](../Authentication/README.md) · [Web](../../README.md).
