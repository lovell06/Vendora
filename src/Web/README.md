# Vendora Web

A Blazor Interactive Server storefront for developers working on Vendora's frontend. It uses .NET 10 and MudBlazor to provide a catalog, product details, a cart, and login/registration backed by the API Gateway. The interface is in English.

> Web reads products, categories, availability and the cart from the backend through the API Gateway; there is no mock data. Orders (checkout) have no screen yet. The backend must be running (see the [backend guide](../../composes/README.md)).

## Run and explore

Install the .NET SDK 10, then run from the **repository root**:

```sh
dotnet restore src/Web/Vendora.Web.csproj
dotnet run --project src/Web/Vendora.Web.csproj --launch-profile http
```

Start the backend first (the Gateway defaults to `http://127.0.0.1:8080/`, see `ApiGateway:BaseUrl` in [appsettings.json](appsettings.json)), then open [http://localhost:5111](http://localhost:5111). Register, sign in, browse products, open a product, choose a variant and add it to your cart.

The `http` profile sets the environment to `Development`. The `https` profile in [launchSettings.json](Properties/launchSettings.json) uses `https://localhost:7265` and `http://localhost:5111`; configure a development certificate if you choose HTTPS.

## Screens

| Route | Purpose | Guide |
| --- | --- | --- |
| `/`, `/home` | Listing, searching, and filtering Catalog products; cart and account panels | [Home](Features/Home/README.md) |
| `/products/{Id}` | Details, variants, image gallery, and quantity selection | [Products](Features/Products/README.md) |
| `/login`, `/dang-nhap` | Login form; creates the session | [Authentication](Features/Authentication/README.md) |
| `/register`, `/dang-ky` | Registration form (name, email, phone, password) | [Authentication](Features/Authentication/README.md) |
| `/verify-email?userId=…&token=…` | Opened from the verification email; a "Verify email" button confirms the address | [Authentication](Features/Authentication/README.md) |
| `/preview/product-cards` | Product card component samples in Development (static, no API) | [Shared UI](Components/UI/README.md) |
| `/not-found`, `/Error` | Recovery pages for 404/500 errors | [Shared UI](Components/UI/README.md) |

## Structure and integrations

| Location | Responsibility |
| --- | --- |
| [Program.cs](Program.cs) | Registers Blazor, MudBlazor, themes, and the Gateway client/session/cart services |
| `Gateway/` | `GatewayClient` (all backend HTTP calls), `UserSession` (tokens), `CartState`, response contracts |
| `Features/` | Screens, state, validation, and interactions grouped by feature |
| `Components/UI/` | Shared presentation components |
| `Components/Theming/` | MudBlazor providers, palettes, and theme state |
| `wwwroot/` | CSS, JavaScript, and Inter fonts |

All data comes from the API Gateway through `GatewayClient` (`/api/products`, `/api/categories`, `/api/auth/*`, `/api/carts/*`, `/api/products/batch`). Errors use the backend's `{ code, message, type }` body and the message is shown to the user.

Sign-in stores the access and refresh tokens in the browser's protected session storage (per tab) through `UserSession`; the access token is sent as a `Bearer` header on cart calls. The backend has no refresh endpoint, so when a token expires (or the API answers 401) the user is signed out. Because the session lives in the browser, `App.razor` renders without prerendering (`InteractiveServerRenderMode(prerender: false)`). The cart is read from and written to the Cart service; product names and prices in the cart are looked up through `POST /api/products/batch`.

## Themes and shared components

`App.razor` renders `Routes` and `HeadOutlet` in Interactive Server mode without prerendering. `Routes` contains one `AppProviders` instance for themes, popovers, dialogs, and snackbars. System/Light/Dark themes are synchronized through `ThemeState` and `theme.js`; the browser key `vendora-theme` stores the appearance preference.

CSS uses semantic color roles and locally hosted Inter. Shared CSS lives in `wwwroot/app.css`; feature CSS isolation handles screen composition. Read the [UI guide](Components/UI/README.md) before reusing or adding components.

## Verification when making changes

Build from the repository root:

```sh
dotnet build src/Web/Vendora.Web.csproj
```

Manually check compact/wide windows, light/dark/system themes, keyboard operation, navigation, empty results, and an unavailable Gateway. Feature READMEs list additional scenarios. The Web changes that connected it to the API were made without a `dotnet` SDK, so they have not been built or run yet; the repository has no automated test projects.

[Back to the project overview](../../README.md).
