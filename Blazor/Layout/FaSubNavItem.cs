using Fran.Components;
using Fran.Icons;
using Microsoft.AspNetCore.Components;

namespace Fran.Layout;

/// <summary>
/// Model for an item in a subnavbar row (<see cref="FaSubNav"/>).
/// Supports links, action buttons, active indicator, icons, and badges.
/// </summary>
public sealed class FaSubNavItem
{
    public string Title { get; set; } = string.Empty;
    public string? Id { get; set; }
    public string? Href { get; set; }
    public EventCallback OnClick { get; set; }
    public bool IsActive { get; set; }
    public FaIconName? Icon { get; set; }
    public string? Badge { get; set; }
    public FaBadgeVariant BadgeVariant { get; set; } = FaBadgeVariant.Neutral;
    public object? Tag { get; set; }
}
