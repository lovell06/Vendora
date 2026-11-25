using MudBlazor;

namespace Vendora.Web.Features.Admin.Shared;

public static class ConfirmExtensions
{
    // Opens a ConfirmDialog; returns true only when the user confirms. Esc / Cancel return false. Focus starts on Cancel.
    public static async Task<bool> ConfirmAsync(
        this IDialogService dialogs, string title, string? message, string confirmText,
        bool danger = false, IReadOnlyList<string>? extraLines = null)
    {
        var parameters = new DialogParameters<ConfirmDialog>
        {
            { x => x.Title, title },
            { x => x.Message, message },
            { x => x.ConfirmText, confirmText },
            { x => x.Danger, danger },
            { x => x.ExtraLines, extraLines }
        };
        var options = new DialogOptions { MaxWidth = MaxWidth.ExtraSmall, FullWidth = true, CloseOnEscapeKey = true };
        var dialog = await dialogs.ShowAsync<ConfirmDialog>(title, parameters, options);
        var result = await dialog.Result;
        return result is { Canceled: false };
    }
}
