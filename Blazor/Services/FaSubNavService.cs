namespace Fran.Services;

using Microsoft.AspNetCore.Components;

/// <summary>
/// Scoped service allowing pages or components to supply a secondary subnavigation row
/// to the shell header without having to couple directly to layout parameters.
/// </summary>
public sealed class FaSubNavService
{
    private RenderFragment? _subNavContent;

    public RenderFragment? SubNavContent => _subNavContent;

    public event Action? OnChange;

    public void SetSubNav(RenderFragment? content)
    {
        if (ReferenceEquals(_subNavContent, content))
        {
            return;
        }
        _subNavContent = content;
        OnChange?.Invoke();
    }

    public void Clear()
    {
        if (_subNavContent is not null)
        {
            _subNavContent = null;
            OnChange?.Invoke();
        }
    }
}
