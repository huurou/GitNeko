using System.Windows;

namespace GitNeko.Services;

public sealed class ClipboardService : IClipboardService
{
    public string? GetText()
    {
        try
        {
            return Clipboard.ContainsText()
                ? Clipboard.GetText()
                : null;
        }
        catch
        {
            return null;
        }
    }
}
