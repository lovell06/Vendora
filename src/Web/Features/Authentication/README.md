# Authentication

Login and registration for Vendora Web. Each screen owns its model and validation; submission goes through [GatewayClient](../../Gateway/GatewayClient.cs) and the session is kept by [UserSession](../../Gateway/UserSession.cs). `Shared` contains the layout.

## Try it

Start the backend using the [Compose guide](../../../../composes/README.md), follow the [Web instructions](../../README.md#run-and-explore), then open `/register` or `/login` at `http://localhost:5111` (aliases `/dang-ky`, `/dang-nhap`).

Set the Gateway address with `ApiGateway__BaseUrl` (setting `ApiGateway:BaseUrl`; absolute URL, default `http://127.0.0.1:8080/` in [appsettings.json](../../appsettings.json)).

## Behavior

| Flow | Request through the Gateway | Result |
| --- | --- | --- |
| Register | `POST /api/auth/register` with `fullName`, `email`, `password`, `phoneNumber` | Success message with a link to sign in (the backend sends a verification email); 409 shows "email already in use" |
| Login | `POST /api/auth/login` with `email`, `password` | `UserId`, access and refresh tokens are saved in `UserSession`, then the user goes to `returnUrl` (local paths only) or `/` |
| Sign out | `POST /api/auth/logout` with `userId`, `refreshToken` | Local session is cleared even if the call fails |

- Tokens are stored in the browser's protected session storage (per tab, encrypted by ASP.NET Data Protection), not in cookies. The access token is attached as a `Bearer` header to cart calls only.
- There is no refresh endpoint: an expired token (or a 401 on a cart call) signs the user out.
- 401 on login shows "Incorrect email or password"; other errors show the backend `message`.
- Email verification and password recovery screens do not exist yet; the `/api/auth/verify-email` endpoint is not used by Web.

## Validation and source layout

| File | Responsibility |
| --- | --- |
| [Login.razor](Login/Login.razor) | Requires a valid email and a password; honors `?returnUrl=` |
| [Register.razor](Register/Register.razor) | Name up to 100 characters, valid email, phone number, password of at least 8 characters, matching confirmation |
| [AuthLayout.razor](Shared/AuthLayout.razor) | Branding, themes, and form layout |

Forms use `EditForm`, `DataAnnotationsValidator`, and [AppTextField/AppButton](../../Components/UI/README.md). A busy flag prevents repeated submissions. Web validation does not replace the backend's complete rules.

## Verification when making changes

Build using the [Web instructions](../../README.md#verification-when-making-changes). Manually check empty fields, invalid email/phone, short passwords, duplicate email, wrong password, sign-in then reload of the page (session survives in the same tab), `returnUrl`, sign out, and an unreachable Gateway.

These flows were written against the backend source; builds and real requests have not been run.

[Shared UI](../../Components/UI/README.md) · [Web](../../README.md).
