# Authentication

Login and registration forms for Vendora Web. Each screen owns its model, validation, and submission behavior; `Shared` contains the layout and API Gateway HTTP client.

> Web does not yet establish sessions, handle tokens/cookies, redirect after authentication, verify email, or recover passwords. Registration is also missing a field required by the backend.

## Explore the forms

Follow the [Web instructions](../../README.md#run-and-explore), then open `/login` or `/register` at `http://localhost:5111`. Their aliases are `/dang-nhap` and `/dang-ky`.

The forms and validation do not require the backend. With the default settings (empty login/register paths), a valid submission displays an authentication-unavailable message rather than simulating success.

## Configure the API Gateway

Start the backend using the [Compose guide](../../../../composes/README.md). From the **repository root**, use Bash/Zsh or another POSIX shell to configure the Web process:

```sh
ApiGateway__BaseUrl=http://localhost:8080/ \
ApiGateway__LoginPath=/api/auth/login \
ApiGateway__RegisterPath=/api/auth/register \
dotnet run --project src/Web/Vendora.Web.csproj --launch-profile http
```

The corresponding .NET settings are `ApiGateway:BaseUrl`, `ApiGateway:LoginPath`, and `ApiGateway:RegisterPath`; in [appsettings.json](../../appsettings.json), `BaseUrl` defaults to `http://127.0.0.1:8080/` and both paths are empty. `BaseUrl` must be an absolute URL; paths are combined using `new Uri(baseUrl, path)`. Include a trailing `/` on the base URL and use a relative path when a path prefix needs to be preserved.

[AuthGateway.cs](Shared/AuthGateway.cs) sends JSON to the Gateway without calling services directly. HTTP 401 becomes an invalid-credentials message, 409 becomes an email-already-in-use message, and other HTTP/connection errors have their own feedback. A 2xx response only means the Gateway accepted the request; the client does not read or store tokens yet.

## Current contracts and limitations

| Flow | JSON sent by Web | Backend contract |
| --- | --- | --- |
| Login | `email`, `password` | [LoginRequest.cs](../../../Services/Identity/Api/Auth/Login/LoginRequest.cs) accepts these two fields |
| Register | `fullName`, `email`, `password` | [RegisterRequest.cs](../../../Services/Identity/Api/Auth/Register/RegisterRequest.cs) also requires `phoneNumber` |

Align the registration form with the API before using the real workflow. URL configuration does not resolve this contract mismatch or add session management. Web does not persist credentials/tokens in browser storage.

## Validation and source layout

| File | Responsibility |
| --- | --- |
| [Login.razor](Login/Login.razor) | Requires a valid email and a password |
| [Register.razor](Register/Register.razor) | Name up to 100 characters, valid email, password of at least 8 characters, and matching confirmation |
| [AuthGateway.cs](Shared/AuthGateway.cs) | Destination configuration, requests, and error feedback |
| [AuthLayout.razor](Shared/AuthLayout.razor) | Branding, themes, and form layout |

Forms use `EditForm`, `DataAnnotationsValidator`, and [AppTextField/AppButton](../../Components/UI/README.md). A busy flag prevents repeated submissions; password reveal only changes the field's display. Web password rules describe current form validation and do not replace the backend's complete rules.

## Verification when making changes

Build using the [Web instructions](../../README.md#verification-when-making-changes). Manually check empty fields, invalid email, long names, short passwords, mismatched confirmation, password reveal, repeated submission, and empty/unreachable Gateway settings. When integrating the backend, separately check the registration contract, tokens, and authenticated state.

Builds, browser checks, and real authentication requests were not run during this documentation update.

[Shared UI](../../Components/UI/README.md) · [Web](../../README.md).
