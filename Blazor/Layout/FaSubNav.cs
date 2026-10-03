using Fran.Components;
using Fran.Icons;
using Fran.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Fran.Layout;

/// <summary>
/// Secondary navigation / subnavbar row — provides category filters, view selectors, or additional navbar links.
/// Can be used standalone directly under the header or embedded as an attached row in <see cref="FaHeader"/>
/// via <see cref="FaHeader.SubNavContent"/> or <see cref="FaStandardShell.SubNav"/>.
/// Includes built-in small screen / mobile collapse behavior into a hamburger dropdown.
/// </summary>
public sealed class FaSubNav : ComponentBase
{
    [Parameter] public string? Id { get; set; }

    /// <summary>Optional category or section title shown in the subnav bar.</summary>
    [Parameter] public string? Title { get; set; }

    /// <summary>Structured subnav items to render.</summary>
    [Parameter] public IReadOnlyList<FaSubNavItem>? Items { get; set; }

    /// <summary>Target element ID or key of the currently active subnav item.</summary>
    [Parameter] public string? ActiveId { get; set; }

    /// <summary>Callback invoked when a subnav item is clicked.</summary>
    [Parameter] public EventCallback<FaSubNavItem> OnItemClick { get; set; }

    /// <summary>Arbitrary child content (custom buttons, toggles, or search inputs).</summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>Optional content aligned to the far right of the subnav row.</summary>
    [Parameter] public RenderFragment? RightContent { get; set; }

    /// <summary>Whether the subnav row pins stickily beneath the header when scrolling.</summary>
    [Parameter] public bool Sticky { get; set; }

    /// <summary>Whether to collapse into a hamburger toggle button on mobile screens. Defaults to true.</summary>
    [Parameter] public bool ShowMobileToggle { get; set; } = true;

    /// <summary>Custom label shown on the mobile hamburger toggle button.</summary>
    [Parameter] public string? MobileToggleText { get; set; }

    /// <summary>Accessible name for the nav element. Defaults to "Sub-navigation".</summary>
    [Parameter] public string AriaLabel { get; set; } = "Sub-navigation";

    /// <summary>Custom CSS class.</summary>
    [Parameter] public string? CssClass { get; set; }

    /// <summary>Optional inline CSS styles.</summary>
    [Parameter] public string? Style { get; set; }

    private string _elementId = string.Empty;

    protected override void OnInitialized()
    {
        _elementId = !string.IsNullOrWhiteSpace(Id) ? Id : $"fa-subnav-{Guid.NewGuid():N}";
    }

    protected override void OnParametersSet()
    {
        if (!string.IsNullOrWhiteSpace(Id))
        {
            _elementId = Id;
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        FaSubNavItem? activeItem = null;
        if (Items is { Count: > 0 })
        {
            activeItem = Items.FirstOrDefault(i => i.IsActive || (!string.IsNullOrEmpty(ActiveId) && string.Equals(i.Id, ActiveId, StringComparison.OrdinalIgnoreCase)));
        }

        var mobileLabel = !string.IsNullOrWhiteSpace(MobileToggleText)
            ? MobileToggleText
            : (activeItem?.Title ?? (!string.IsNullOrWhiteSpace(Title) ? Title : "Menu"));

        builder.OpenElement(0, "nav");
        builder.AddAttribute(1, "id", _elementId);
        builder.AddAttribute(2, "class", CssClassNames.Combine("fa-subnav", Sticky ? "fa-subnav-sticky" : null, CssClass));
        builder.AddAttribute(3, "aria-label", AriaLabel);
        if (!string.IsNullOrWhiteSpace(Style))
        {
            builder.AddAttribute(4, "style", Style);
        }

        // Mobile header bar with title & hamburger toggle
        if (ShowMobileToggle)
        {
            builder.OpenElement(5, "div");
            builder.AddAttribute(6, "class", "fa-subnav-mobile-bar");

            builder.OpenElement(7, "div");
            builder.AddAttribute(8, "class", "fa-subnav-mobile-title");
            if (!string.IsNullOrWhiteSpace(Title))
            {
                builder.OpenElement(9, "span");
                builder.AddAttribute(10, "class", "fa-subnav-mobile-section-label");
                builder.AddContent(11, Title);
                builder.CloseElement();
            }
            if (activeItem is not null && !string.Equals(activeItem.Title, Title, StringComparison.OrdinalIgnoreCase))
            {
                builder.OpenElement(12, "span");
                builder.AddAttribute(13, "class", "fa-subnav-mobile-active-label");
                builder.AddContent(14, activeItem.Title);
                builder.CloseElement();
            }
            builder.CloseElement(); // div.fa-subnav-mobile-title

            builder.OpenElement(15, "button");
            builder.AddAttribute(16, "type", "button");
            builder.AddAttribute(17, "class", "fa-subnav-mobile-toggle");
            builder.AddAttribute(18, "data-subnav-mobile-toggle", true);
            builder.AddAttribute(19, "title", "Toggle sub-navigation");
            builder.AddAttribute(20, "aria-label", "Toggle sub-navigation");
            builder.AddAttribute(21, "aria-expanded", "false");
            builder.AddAttribute(22, "onclick", "faToggleSubNavMobile(this)");

            builder.OpenComponent<FaIcon>(23);
            builder.AddComponentParameter(24, nameof(FaIcon.Name), FaIconName.Menu);
            builder.AddComponentParameter(25, nameof(FaIcon.Color), FaIconColor.Inherit);
            builder.AddComponentParameter(26, nameof(FaIcon.Size), 16);
            builder.CloseComponent();

            builder.OpenElement(26, "span");
            builder.AddAttribute(27, "class", "fa-subnav-mobile-toggle-text");
            builder.AddContent(28, mobileLabel);
            builder.CloseElement();

            builder.CloseElement(); // button.fa-subnav-mobile-toggle
            builder.CloseElement(); // div.fa-subnav-mobile-bar
        }

        // Subnav items container
        builder.OpenElement(29, "div");
        builder.AddAttribute(30, "class", "fa-subnav-items");

        if (!string.IsNullOrWhiteSpace(Title))
        {
            builder.OpenElement(31, "span");
            builder.AddAttribute(32, "class", "fa-subnav-title");
            builder.AddContent(33, Title);
            builder.CloseElement();
        }

        if (Items is { Count: > 0 })
        {
            var seq = 34;
            foreach (var item in Items)
            {
                var isItemActive = item.IsActive || (!string.IsNullOrEmpty(ActiveId) && string.Equals(item.Id, ActiveId, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrWhiteSpace(item.Href))
                {
                    builder.OpenElement(seq++, "a");
                    builder.AddAttribute(seq++, "class", CssClassNames.Combine("fa-subnav-link", isItemActive ? "fa-subnav-link-active" : null));
                    builder.AddAttribute(seq++, "href", item.Href);
                    if (isItemActive)
                    {
                        builder.AddAttribute(seq++, "aria-current", "page");
                    }
                    if (item.OnClick.HasDelegate || OnItemClick.HasDelegate)
                    {
                        builder.AddAttribute(seq++, "onclick", EventCallback.Factory.Create(this, async () =>
                        {
                            if (item.OnClick.HasDelegate) await item.OnClick.InvokeAsync();
                            if (OnItemClick.HasDelegate) await OnItemClick.InvokeAsync(item);
                        }));
                    }

                    RenderItemInner(builder, ref seq, item);
                    builder.CloseElement(); // a
                }
                else
                {
                    builder.OpenElement(seq++, "button");
                    builder.AddAttribute(seq++, "type", "button");
                    builder.AddAttribute(seq++, "class", CssClassNames.Combine("fa-subnav-link", "fa-subnav-button", isItemActive ? "fa-subnav-link-active" : null));
                    if (isItemActive)
                    {
                        builder.AddAttribute(seq++, "aria-current", "true");
                    }
                    builder.AddAttribute(seq++, "onclick", EventCallback.Factory.Create(this, async () =>
                    {
                        if (item.OnClick.HasDelegate) await item.OnClick.InvokeAsync();
                        if (OnItemClick.HasDelegate) await OnItemClick.InvokeAsync(item);
                    }));

                    RenderItemInner(builder, ref seq, item);
                    builder.CloseElement(); // button
                }
            }
        }

        if (ChildContent is not null)
        {
            builder.AddContent(70, ChildContent);
        }

        builder.CloseElement(); // div.fa-subnav-items

        if (RightContent is not null)
        {
            builder.OpenElement(71, "div");
            builder.AddAttribute(72, "class", "fa-subnav-right");
            builder.AddContent(73, RightContent);
            builder.CloseElement(); // div.fa-subnav-right
        }

        builder.CloseElement(); // nav.fa-subnav
    }

    private static void RenderItemInner(RenderTreeBuilder builder, ref int seq, FaSubNavItem item)
    {
        if (item.Icon.HasValue)
        {
            builder.OpenComponent<FaIcon>(seq++);
            builder.AddComponentParameter(seq++, nameof(FaIcon.Name), item.Icon.Value);
            builder.AddComponentParameter(seq++, nameof(FaIcon.Color), FaIconColor.Inherit);
            builder.AddComponentParameter(seq++, nameof(FaIcon.Size), 15);
            builder.CloseComponent();
        }

        builder.OpenElement(seq++, "span");
        builder.AddAttribute(seq++, "class", "fa-subnav-link-text");
        builder.AddContent(seq++, item.Title);
        builder.CloseElement();

        if (!string.IsNullOrEmpty(item.Badge))
        {
            builder.OpenComponent<FaBadge>(seq++);
            builder.AddComponentParameter(seq++, nameof(FaBadge.Variant), item.BadgeVariant);
            builder.AddComponentParameter(seq++, nameof(FaBadge.ChildContent), (RenderFragment)(b => b.AddContent(0, item.Badge)));
            builder.CloseComponent();
        }
    }
}
