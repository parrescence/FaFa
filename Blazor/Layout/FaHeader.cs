using Fran.Components;
using Fran.Icons;
using Fran.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Fran.Layout;

/// <summary>
/// Header bar — brand on the left, user name + avatar + login/logout at the far right.
/// Purely presentational: the consuming app supplies auth state and wires up the
/// login/logout callbacks (e.g. FinanceApp.Web's MainLayout, an example consumer, for
/// real wiring).
/// </summary>
public sealed class FaHeader : ComponentBase
{
    [Parameter, EditorRequired] public string BrandText { get; set; } = "";
    [Parameter] public string BrandHref { get; set; } = "";

    /// <summary>
    /// Optional logo/icon shown to the left of <see cref="BrandText"/> inside the brand
    /// link — any URL an &lt;img&gt; src accepts (a static asset path, a data: URI, a
    /// CDN URL). Omit for text-only branding (the default). Rendered decorative
    /// (<c>alt=""</c>) since <see cref="BrandText"/> already supplies the link's
    /// accessible name — this never doubles as the only content conveying meaning.
    /// </summary>
    [Parameter] public string? BrandIconUrl { get; set; }

    [Parameter] public bool IsAuthenticated { get; set; }
    [Parameter] public string? UserDisplayName { get; set; }
    [Parameter] public string? UserImageUrl { get; set; }
    [Parameter] public EventCallback OnLogin { get; set; }
    [Parameter] public EventCallback OnLogout { get; set; }

    /// <summary>
    /// When true, renders <see cref="FaAvatarForm"/> in the header user area instead
    /// of the inline theme switcher, username text, and standalone login/logout button.
    /// Default is false for backward compatibility.
    /// </summary>
    [Parameter] public bool UseAvatarForm { get; set; }

    /// <summary>
    /// When <see cref="UseAvatarForm"/> is true, controls whether the user's name is
    /// shown next to the avatar in the header bar. Defaults to false (only the avatar
    /// is shown in the topbar).
    /// </summary>
    [Parameter] public bool ShowUserNameInHeader { get; set; }

    /// <summary>Optional user email or subtitle shown in the opened avatar form.</summary>
    [Parameter] public string? UserEmail { get; set; }

    /// <summary>Optional URL navigating to the user's account settings/profile form.</summary>
    [Parameter] public string? AccountHref { get; set; }

    /// <summary>Optional callback invoked when the user clicks the account settings action.</summary>
    [Parameter] public EventCallback OnAccountClick { get; set; }

    /// <summary>Label for the account link/button. Defaults to "Account settings".</summary>
    [Parameter] public string AccountText { get; set; } = "Account settings";

    /// <summary>Optional application-specific content rendered inside the opened avatar form.</summary>
    [Parameter] public RenderFragment? UserMenuContent { get; set; }

    /// <summary>Optional navigation buttons or links rendered between the brand and the user area.</summary>
    [Parameter] public RenderFragment? NavContent { get; set; }

    /// <summary>Alias for <see cref="NavContent"/> for convenience.</summary>
    [Parameter] public RenderFragment? NavButtons { get; set; }

    /// <summary>Optional secondary navigation row / subnavbar rendered as an attachment under the header.</summary>
    [Parameter] public RenderFragment? SubNavContent { get; set; }

    /// <summary>Alias for <see cref="SubNavContent"/> for convenience.</summary>
    [Parameter] public RenderFragment? SubNav { get; set; }

    /// <summary>Whether the header scrolls away with the page (default) or stays pinned to the top.</summary>
    [Parameter] public FaNavPosition Position { get; set; } = FaNavPosition.Standard;

    /// <summary>
    /// Renders a hamburger button left of the brand that calls window.faToggleSidebarMobile()
    /// (js/sidebar.js) on click. Only meaningful when this header is paired with a sidebar —
    /// FaSidebarShell sets this itself, so a consumer using FaHeader standalone (FaStandardShell,
    /// no sidebar to toggle) never needs to touch it. The button itself is always in the markup
    /// once set; it's <c>_responsive.scss</c>'s breakpoint that hides it above the mobile width and
    /// hides/shows <c>.fa-sidebar</c> off-canvas below it — independent of FaSidebar.Collapsible,
    /// which is a separate, desktop-only icon-rail affordance.
    /// </summary>
    [Parameter] public bool ShowSidebarToggle { get; set; }

    /// <summary>
    /// When true and navigation content is present (<see cref="NavContent"/> or <see cref="NavButtons"/>)
    /// without a sidebar toggle, renders a hamburger button on mobile screens that toggles the header
    /// nav menu slide-down via window.faToggleNavMobile(). Defaults to true.
    /// </summary>
    [Parameter] public bool ShowNavToggle { get; set; } = true;

    private string? PositionClass => Position switch
    {
        FaNavPosition.Sticky => "fa-header-sticky",
        FaNavPosition.Floating => "fa-header-floating",
        _ => null
    };

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var effectiveNav = NavContent ?? NavButtons;
        var effectiveSubNav = SubNavContent ?? SubNav;

        builder.OpenElement(0, "header");
        builder.AddAttribute(1, "class", CssClassNames.Combine("fa-header", PositionClass, effectiveSubNav is not null ? "fa-header-has-subnav" : null));

        builder.OpenElement(2, "div");
        builder.AddAttribute(3, "class", "fa-header-left");

        if (ShowSidebarToggle)
        {
            builder.OpenElement(4, "button");
            builder.AddAttribute(5, "type", "button");
            builder.AddAttribute(6, "class", "fa-header-menu-toggle fa-header-sidebar-toggle");
            builder.AddAttribute(7, "data-sidebar-mobile-toggle", true);
            builder.AddAttribute(8, "title", "Toggle menu");
            builder.AddAttribute(9, "aria-label", "Toggle menu");
            builder.AddAttribute(10, "aria-expanded", "false");
            builder.AddAttribute(11, "onclick", "faToggleSidebarMobile()");

            builder.OpenComponent<FaIcon>(12);
            builder.AddComponentParameter(13, nameof(FaIcon.Name), FaIconName.Menu);
            builder.AddComponentParameter(14, nameof(FaIcon.Color), FaIconColor.White);
            builder.AddComponentParameter(15, nameof(FaIcon.Size), 18);
            builder.CloseComponent();

            builder.CloseElement(); // .fa-header-menu-toggle
        }
        else if (ShowNavToggle && effectiveNav is not null)
        {
            builder.OpenElement(16, "button");
            builder.AddAttribute(17, "type", "button");
            builder.AddAttribute(18, "class", "fa-header-menu-toggle fa-header-nav-toggle");
            builder.AddAttribute(19, "data-nav-mobile-toggle", true);
            builder.AddAttribute(20, "title", "Toggle navigation");
            builder.AddAttribute(21, "aria-label", "Toggle navigation");
            builder.AddAttribute(22, "aria-expanded", "false");
            builder.AddAttribute(23, "onclick", "faToggleNavMobile()");

            builder.OpenComponent<FaIcon>(24);
            builder.AddComponentParameter(25, nameof(FaIcon.Name), FaIconName.Menu);
            builder.AddComponentParameter(26, nameof(FaIcon.Color), FaIconColor.White);
            builder.AddComponentParameter(27, nameof(FaIcon.Size), 18);
            builder.CloseComponent();

            builder.CloseElement(); // .fa-header-nav-toggle
        }

        builder.OpenElement(28, "a");
        builder.AddAttribute(29, "class", "fa-header-brand");
        builder.AddAttribute(30, "href", BrandHref);

        if (!string.IsNullOrEmpty(BrandIconUrl))
        {
            builder.OpenElement(31, "img");
            builder.AddAttribute(32, "class", "fa-header-brand-icon");
            builder.AddAttribute(33, "src", BrandIconUrl);
            builder.AddAttribute(34, "alt", "");
            builder.CloseElement();
        }

        builder.AddContent(35, BrandText);
        builder.CloseElement(); // .fa-header-brand

        builder.CloseElement(); // .fa-header-left

        if (effectiveNav is not null)
        {
            builder.OpenElement(36, "nav");
            builder.AddAttribute(37, "class", "fa-header-nav");
            builder.AddAttribute(38, "aria-label", "Header navigation");
            builder.AddContent(39, effectiveNav);
            builder.CloseElement(); // nav.fa-header-nav
        }

        builder.OpenElement(40, "div");
        builder.AddAttribute(41, "class", "fa-header-user");

        if (UseAvatarForm)
        {
            builder.OpenComponent<FaAvatarForm>(42);
            builder.AddComponentParameter(43, nameof(FaAvatarForm.IsAuthenticated), IsAuthenticated);
            builder.AddComponentParameter(44, nameof(FaAvatarForm.DisplayName), UserDisplayName);
            builder.AddComponentParameter(45, nameof(FaAvatarForm.ImageUrl), UserImageUrl);
            builder.AddComponentParameter(46, nameof(FaAvatarForm.Email), UserEmail);
            builder.AddComponentParameter(47, nameof(FaAvatarForm.ShowDisplayName), ShowUserNameInHeader);
            builder.AddComponentParameter(48, nameof(FaAvatarForm.AccountHref), AccountHref);
            builder.AddComponentParameter(49, nameof(FaAvatarForm.OnAccountClick), OnAccountClick);
            builder.AddComponentParameter(50, nameof(FaAvatarForm.AccountText), AccountText);
            builder.AddComponentParameter(51, nameof(FaAvatarForm.OnLogin), OnLogin);
            builder.AddComponentParameter(52, nameof(FaAvatarForm.OnLogout), OnLogout);
            builder.AddComponentParameter(53, nameof(FaAvatarForm.ChildContent), UserMenuContent);
            builder.CloseComponent();
        }
        else
        {
            builder.OpenComponent<FaThemeSwitcher>(54);
            builder.CloseComponent();

            if (IsAuthenticated)
            {
                builder.OpenComponent<FaAvatar>(55);
                builder.AddComponentParameter(56, nameof(FaAvatar.DisplayName), UserDisplayName);
                builder.AddComponentParameter(57, nameof(FaAvatar.ImageUrl), UserImageUrl);
                builder.CloseComponent();

                builder.OpenElement(58, "span");
                builder.AddAttribute(59, "class", "fa-header-username");
                builder.AddContent(60, UserDisplayName);
                builder.CloseElement();

                builder.OpenComponent<FaButton>(61);
                builder.AddComponentParameter(62, nameof(FaButton.Variant), FaButtonVariant.Secondary);
                builder.AddComponentParameter(63, nameof(FaButton.Size), FaSize.Small);
                builder.AddComponentParameter(64, nameof(FaButton.OnClick), EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, () => OnLogout.InvokeAsync()));
                builder.AddComponentParameter(65, nameof(FaButton.ChildContent), (RenderFragment)(b => b.AddContent(0, "Log out")));
                builder.CloseComponent();
            }
            else
            {
                builder.OpenComponent<FaButton>(66);
                builder.AddComponentParameter(67, nameof(FaButton.Variant), FaButtonVariant.Secondary);
                builder.AddComponentParameter(68, nameof(FaButton.Size), FaSize.Small);
                builder.AddComponentParameter(69, nameof(FaButton.OnClick), EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, () => OnLogin.InvokeAsync()));
                builder.AddComponentParameter(70, nameof(FaButton.ChildContent), (RenderFragment)(b => b.AddContent(0, "Log in")));
                builder.CloseComponent();
            }
        }

        builder.CloseElement(); // .fa-header-user

        if (effectiveSubNav is not null)
        {
            builder.OpenElement(71, "div");
            builder.AddAttribute(72, "class", "fa-header-subnav-row");
            builder.AddContent(73, effectiveSubNav);
            builder.CloseElement(); // div.fa-header-subnav-row
        }

        builder.CloseElement(); // header
    }
}
