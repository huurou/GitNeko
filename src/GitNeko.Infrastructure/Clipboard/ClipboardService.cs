using GitNeko.Domain.Services;
using System.Windows;

namespace GitNeko.Infrastructure.Clipboard;

public sealed class ClipboardService : IClipboardService
{
    public string? GetText()
    {
        return System.Windows.Clipboard.ContainsText()
            ? System.Windows.Clipboard.GetText()
            : null;
    }
}
