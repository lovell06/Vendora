# Products

The product detail page at `/products/{Id}` (numeric Catalog product id) loads `GET /api/products/{id}` through the Gateway: name, brand, category, description, images, and variants with price and availability.

## Try it

Start the backend, follow the [Web instructions](../../README.md#run-and-explore), open a product from the homepage (for example `http://localhost:5111/products/1`).

## Current behavior

| Action or state | Result |
| --- | --- |
| Open a product | Loads from the API; shows a loading indicator, then details. Unknown or invalid id shows the 404 recovery page; other failures show an error page |
| Variants | Every variant is selectable; the first in-stock one is preselected. Price and product code (SKU) follow the selected variant; stock comes from Inventory (`InStock`) |
| Images | The variant's own images are shown; `View all product photos` shows the whole gallery. A variant with no assigned images shows the whole gallery |
| Add to cart | Guests are sent to `/login?returnUrl=…`. Signed-in users add `POST /api/carts/add-item` with the variant id; compact screens first open the quantity sheet. Success or the backend error is shown in a snackbar |
| Related products | Up to four other products of the same category from `GET /api/products?categoryId=…` |
| Change product route | Reloads data and resets quantity, variant, and gallery state |

The quantity range of 1–99 is a UI limit; the backend (Inventory/Cart) decides what is really available.

## Image and variant models

[ProductMedia.cs](ProductMedia.cs) defines presentation models mapped from the API response: `ProductImage` (`Id`, `Url`, `Alt`) and `ProductVariant` (`Id`, `Name`, `ProductCode`, `ImageIds`, `PrimaryImageId`, `InStock`). Ids are the numeric Catalog ids as strings. A variant's primary image is its first assigned image.

[ProductDetails.razor](ProductDetails.razor) coordinates loading, selection and cart feedback. [ProductGallery.razor](ProductGallery.razor) renders images/thumbnails and image-error states. `QuantitySelector` and `AddToCartSheet` belong to [Shared UI](../../Components/UI/README.md).

## Backend changes made for this screen

- `GET /api/products/{id}`: each variant now includes `id`, `inStock`, and `availableQuantity`; availability is checked per variant (`GET /api/inventory-items/{variantId}/availability`). A variant without an inventory record is treated as out of stock. Previously the product id was sent to the variant route.
- `GET /api/products`: new optional `search` parameter (name contains, case-insensitive).

## Verification when making changes

Build using the [Web instructions](../../README.md#verification-when-making-changes). Manually check variant changes, the gallery, out-of-stock variants, adding as guest vs signed in, canceling/confirming the sheet, route changes, unknown ids, and a stopped Gateway or Inventory service.

These behaviors were written against the source; builds and browser checks have not been run.

[Home](../Home/README.md) · [Web](../../README.md).
