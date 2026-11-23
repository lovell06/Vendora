# Products

The product detail page at `/products/{Id}` shares Home's sample data. This feature demonstrates variant selection, image galleries, and quantity selection; it does not retrieve backend prices/stock or persist a cart.

## Try it

Follow the [Web instructions](../../README.md#run-and-explore), then open `http://localhost:5111/products/headphones`.

Select `Graphite` to change the representative image and product code; enable `View all product photos` to browse the entire gallery without changing the selected variant. Select `Sand` to inspect the sample out-of-stock state and disabled purchase action.

Other sample IDs are `speaker`, `backpack`, `keyboard`, `bottle`, and `lamp`. The quantity range of 1–99 is a UI limit, not a statement of available stock.

## Current behavior

| Action or state | Result |
| --- | --- |
| Select a variant | Resets quantity to 1, selects its representative image, and filters its images |
| View all photos | Opens the product gallery while keeping the purchase variant selected |
| Image failure | Displays a fallback; thumbnails retain navigation labels |
| Out-of-stock product/variant | Allows inspection but disables purchasing |
| Add to cart on desktop | Displays a demo snackbar with quantity and variant name, when present |
| Add to cart at widths up to 599px | Opens a quantity sheet; confirmation displays demo feedback |
| Change product route | Resets quantity, variant, and gallery state |
| Unknown ID | Displays recovery content and sets HTTP 404 if the server response has not started |

Related products exclude the current product, prioritize its category, and include at most four samples. Browsing another image does not change the representative image supplied to the cart sheet.

## Image and variant models

[ProductMedia.cs](ProductMedia.cs) defines the presentation models:

- `ProductImage`: `Id`, `Url`, `Alt`.
- `ProductVariant`: `Id`, `Name`, `ProductCode`, `ImageIds`, `PrimaryImageId`, `InStock`.

Each product owns a shared gallery; variants reference images by ID. When adding sample data, keep `PrimaryImageId` within `ImageIds` and ensure every referenced ID exists in the gallery. This is a presentation-data convention, not the Catalog API contract.

[ProductDetails.razor](ProductDetails.razor) coordinates selection and feedback. [ProductGallery.razor](ProductGallery.razor) renders images/thumbnails, labels, and image-error states; it does not auto-rotate images. `QuantitySelector` and `AddToCartSheet` belong to [Shared UI](../../Components/UI/README.md).

Image uploads, editing variant/image assignments, variant pricing, real inventory data, and cart persistence require further integration. This feature does not provide those administration workflows.

## Verification when making changes

Build using the [Web instructions](../../README.md#verification-when-making-changes). Manually check variant changes, representative images, the full gallery, quantity limits, out-of-stock products, canceling/confirming the sheet, route changes, and unknown IDs. Check keyboard operation, image errors, themes, and compact/wide layouts.

Content was checked against source code; builds and browser checks were not run during this documentation update.

[Home](../Home/README.md) · [Web](../../README.md).
