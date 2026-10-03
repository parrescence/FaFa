namespace Fran.Layout;

using Fran.Services;
using Microsoft.AspNetCore.Components;

/// <summary>
/// Declarative component placed on a page to inject a subnav row into the header
/// of <see cref="FaStandardShell"/> or <see cref="FaSidebarShell"/>.
/// Cleans up automatically when the component is disposed.
/// </summary>
public sealed class FaHeaderSubNav : ComponentBase, IDisposable
{
    [Inject] private IServiceProvider Services { get; set; } = default!;
    private FaSubNavService? _subNavService;
    private RenderFragment? _lastContent;

    /// <summary>Subnav content to inject into the shell's header.</summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected override void OnInitialized()
    {
        _subNavService = Services.GetService(typeof(FaSubNavService)) as FaSubNavService;
    }

    protected override void OnParametersSet()
    {
        if (!ReferenceEquals(_lastContent, ChildContent))
        {
            _lastContent = ChildContent;
            _subNavService?.SetSubNav(ChildContent);
        }
    }

    public void Dispose()
    {
        _subNavService?.Clear();
    }
}
