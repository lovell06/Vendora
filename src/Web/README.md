# Vendora Web

A Blazor Interactive Server storefront for developers working on Vendora's frontend. It uses .NET 10 and MudBlazor to provide a sample catalog, product details, and login/registration forms. The interface is in English.

> Product data consists of local samples. Web does not yet provide authenticated sessions, a persisted cart, or real order placement.

## Run and explore

Install the .NET SDK 10, then run from the **repository root**:

```sh
dotnet restore src/Web/Vendora.Web.csproj
dotnet run --project src/Web/Vendora.Web.csproj --launch-profile http
```

Open [http://localhost:5111](http://localhost:5111). The homepage displays products labeled “Demo catalog.” Search for `headphones`, open its details, and change the color to see the sample image, product code, and availability change. This demo does not require the backend.

The `http` profile sets the environment to `Development`. The `https` profile in [launchSettings.json](Properties/launchSettings.json) uses `https://localhost:7265` and `http://localhost:5111`; configure a development certificate if you choose HTTPS.

## Screens

| Route | Purpose | Guide |
| --- | --- | --- |
| `/`, `/home` | Listing, searching, and filtering sample data | [Home](Features/Home/README.md) |
| `/products/{Id}` | Details, variants, image gallery, and quantity selection | [Products](Features/Products/README.md) |
| `/login`, `/dang-nhap` | Login form | [Authentication](Features/Authentication/README.md) |
| `/register`, `/dang-ky` | Registration form | [Authentication](Features/Authentication/README.md) |
| `/preview/product-cards` | Product card samples in Development | [Shared UI](Components/UI/README.md) |
| `/not-found`, `/Error` | Recovery pages for 404/500 errors | [Shared UI](Components/UI/README.md) |

## Structure and integrations

| Location | Responsibility |
| --- | --- |
| [Program.cs](Program.cs) | Registers Blazor, MudBlazor, themes, and the authentication HTTP client |
| `Features/` | Screens, state, validation, and interactions grouped by feature |
| `Components/UI/` | Shared presentation components |
| `Components/Theming/` | MudBlazor providers, palettes, and theme state |
| `wwwroot/` | CSS, JavaScript, sample images, and Inter fonts |

`DemoCatalog` in Home supplies both listing and detail data. Pages keep filter and selection state in UI memory. Adding an item to the cart only displays feedback; it does not send a persistence request.

Authentication forms call the Gateway through `AuthGateway`, but the `ApiGateway` settings are empty by default. See [authentication configuration and contract limits](Features/Authentication/README.md). Configuring URLs does not establish a session; registration also lacks the backend's required `phoneNumber` field. To run the APIs, see the [backend guide](../../composes/README.md).

## Themes and shared components

`App.razor` renders `Routes` and `HeadOutlet` in Interactive Server mode. `Routes` contains one `AppProviders` instance for themes, popovers, dialogs, and snackbars. System/Light/Dark themes are synchronized through `ThemeState` and `theme.js`; the browser key `vendora-theme` stores the appearance preference.

CSS uses semantic color roles and locally hosted Inter. Shared CSS lives in `wwwroot/app.css`; feature CSS isolation handles screen composition. Read the [UI guide](Components/UI/README.md) before reusing or adding components.

## Verification when making changes

Build from the repository root:

```sh
dotnet build src/Web/Vendora.Web.csproj
```

Manually check compact/wide windows, light/dark/system themes, keyboard operation, navigation, empty results, and an unavailable Gateway. Feature READMEs list additional scenarios. Builds and browser checks were not run during this documentation update; the repository has no automated test projects yet.

[Back to the project overview](../../README.md).
