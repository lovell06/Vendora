using System.Net;

namespace Vendora.Services.Identity.Worker.Emails;

// Content of the email sent after registration. The HTML uses inline styles and
// tables only, because most mail clients strip <style> blocks and modern CSS.
// The link opens the Web /verify-email page, which calls the verify API only
// when the user presses its button.
public static class VerificationEmail
{
    public const string Subject = "Verify your Vendora account";

    public static string BuildVerifyUrl(string webBaseUrl, Guid userId, string token) =>
        $"{webBaseUrl.TrimEnd('/')}/verify-email?userId={userId}&token={Uri.EscapeDataString(token)}";

    public static string PlainText(string email, string verifyUrl, TimeSpan ttl) =>
        $"""
        Hi there,

        Thanks for signing up for Vendora with {email}.
        Open the link below to verify your email address:

        {verifyUrl}

        This link expires in {FormatTtl(ttl)} and can only be used once.

        If you did not create a Vendora account, you can ignore this email.

        Vendora
        """;

    public static string Html(string email, string verifyUrl, TimeSpan ttl)
    {
        var safeEmail = WebUtility.HtmlEncode(email);
        var safeUrl = WebUtility.HtmlEncode(verifyUrl);
        var safeTtl = WebUtility.HtmlEncode(FormatTtl(ttl));

        return $"""
        <!doctype html>
        <html lang="en">
        <head>
          <meta charset="utf-8">
          <meta name="viewport" content="width=device-width, initial-scale=1">
          <title>{Subject}</title>
        </head>
        <body style="margin:0;padding:0;background-color:#f4f4f5;">
          <table role="presentation" width="100%" cellpadding="0" cellspacing="0" style="background-color:#f4f4f5;padding:32px 16px;">
            <tr>
              <td align="center">
                <table role="presentation" width="100%" cellpadding="0" cellspacing="0" style="max-width:560px;background-color:#ffffff;border-radius:12px;border:1px solid #e4e4e7;font-family:-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,Helvetica,Arial,sans-serif;color:#18181b;">
                  <tr>
                    <td style="padding:32px 32px 0 32px;">
                      <h1 style="margin:0 0 12px 0;font-size:22px;line-height:1.3;font-weight:600;">Verify your email</h1>
                      <p style="margin:0 0 8px 0;font-size:15px;line-height:1.6;color:#3f3f46;">Hi there,</p>
                      <p style="margin:0 0 24px 0;font-size:15px;line-height:1.6;color:#3f3f46;">
                        Thanks for signing up for Vendora with <strong>{safeEmail}</strong>.
                        Press the button below to verify your email address.
                      </p>
                    </td>
                  </tr>
                  <tr>
                    <td style="padding:0 32px;">
                      <table role="presentation" cellpadding="0" cellspacing="0">
                        <tr>
                          <td align="center" bgcolor="#18181b" style="border-radius:8px;">
                            <a href="{safeUrl}" target="_blank" rel="noopener" style="display:inline-block;padding:12px 28px;font-size:15px;font-weight:600;line-height:1.2;color:#ffffff;text-decoration:none;border-radius:8px;">Verify email</a>
                          </td>
                        </tr>
                      </table>
                      <p style="margin:16px 0 0 0;font-size:13px;line-height:1.5;color:#71717a;">
                        This link expires in <strong style="color:#3f3f46;">{safeTtl}</strong> and can only be used once.
                      </p>
                      <p style="margin:16px 0 0 0;font-size:12px;line-height:1.5;color:#71717a;">If the button doesn't work, copy this link into your browser:</p>
                      <p style="margin:4px 0 0 0;font-size:12px;line-height:1.5;word-break:break-all;"><a href="{safeUrl}" target="_blank" rel="noopener" style="color:#2563eb;text-decoration:underline;">{safeUrl}</a></p>
                    </td>
                  </tr>
                  <tr>
                    <td style="padding:24px 32px 28px 32px;">
                      <hr style="border:none;border-top:1px solid #e4e4e7;margin:0 0 16px 0;">
                      <p style="margin:0 0 12px 0;font-size:12px;line-height:1.6;color:#a1a1aa;">
                        If you did not create a Vendora account, you can ignore this email.
                      </p>
                      <p style="margin:0;font-size:13px;font-weight:600;color:#3f3f46;">Vendora</p>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
          </table>
        </body>
        </html>
        """;
    }

    private static string FormatTtl(TimeSpan ttl)
    {
        var minutes = (int)Math.Round(ttl.TotalMinutes);
        return minutes == 1 ? "1 minute" : $"{minutes} minutes";
    }
}
