# Admin (Vendora Admin)

The back office lives in Vendora.Web at `/admin`. Only `Admin` and `Employee` accounts can enter (see `_Imports.razor`). Signed-out visitors are sent to `/login` with a `returnUrl`. Employees get a read-only view; every write needs the `Admin` role (the Catalog API enforces this with its own policy).

## Screens

| Route | Content |
| --- | --- |
| `/admin` | Overview (shortcuts to Categories and Products) |
| `/admin/categories` | List, add, rename, delete with Undo |
| `/admin/products` | Paged list, filters, open by ID, discontinue, delete with Undo |
| `/admin/products/new` | Four-step product creation (details, images, variants, review); the step bar is clickable |
| `/admin/products/{id}` | Detail: edit details; variants and images are read-only |

## Shared parts (`Shared/`)

`AdminLayout`, `AdminNavMenu`, `AdminPageHeader` (breadcrumbs, description, status chip, actions), `ConfirmDialog` + `ConfirmAsync` (focus starts on Cancel, Esc closes), `TableSkeleton`, `FormSection`, `UnsavedChangesGuard`, `ProductThumb`.
Shared with the storefront in `Components/UI/Feedback`: `StatusChip`, `EmptyState`, `ErrorState`, `SessionExpiryBanner`.

`@layout AdminLayout` is set in the `_Imports.razor` of each page folder (`Dashboard`, `Categories`, `Products`), not in `Admin/_Imports.razor`: there it would also apply to `AdminLayout` itself, which then wraps itself forever.

## API calls

`Gateway/Admin/AdminGatewayClient.cs` (A1–A14 in the spec). Failures come back as an `ApiResult` with `Status` and `Code`; nothing throws. Specs: `src/Web/Designs/`.

Web has no HTTP authentication scheme (the session lives in `ProtectedSessionStorage`). `PassThroughAuthorizationResultHandler` lets requests through the middleware and `AuthorizeRouteView` enforces roles when the page renders.

UI permissions are only a convenience. The real gate is the policy on each service API.
