using Bunit;
using Fran.Components;
using Fran.Icons;
using Fran.Layout;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fran.Tests;

public class FaSubNavTests : BunitContext
{
    [Fact]
    public void FaSubNav_RendersItemsAndActiveState()
    {
        var items = new List<FaSubNavItem>
        {
            new() { Id = "all", Title = "All Meets", IsActive = true, Icon = FaIconName.Trophy },
            new() { Id = "quals", Title = "Qualifications", IsActive = false, Badge = "12" },
            new() { Id = "goals", Title = "Club Goals", IsActive = false, Href = "/goals" }
        };

        var cut = Render<FaSubNav>(p => p
            .Add(x => x.Title, "Filter:")
            .Add(x => x.Items, items));

        var nav = cut.Find("nav.fa-subnav");
        Assert.NotNull(nav);

        var title = cut.Find(".fa-subnav-title");
        Assert.Equal("Filter:", title.TextContent.Trim());

        var links = cut.FindAll(".fa-subnav-link");
        Assert.Equal(3, links.Count);

        // First item is active button
        Assert.Contains("fa-subnav-link-active", links[0].ClassName);
        Assert.Equal("true", links[0].GetAttribute("aria-current"));
        Assert.Contains("All Meets", links[0].TextContent);

        // Second item has badge
        Assert.Contains("12", links[1].TextContent);

        // Third item has href
        Assert.Equal("/goals", links[2].GetAttribute("href"));
        Assert.Equal("a", links[2].TagName.ToLowerInvariant());
    }

    [Fact]
    public void FaSubNav_WithChildContent_RendersCustomMarkup()
    {
        var cut = Render<FaSubNav>(p => p
            .Add(x => x.Title, "Standard Types")
            .AddChildContent("<span class=\"custom-filter\">Custom Item</span>"));

        var custom = cut.Find(".custom-filter");
        Assert.NotNull(custom);
        Assert.Equal("Custom Item", custom.TextContent.Trim());
    }

    [Fact]
    public void FaSubNav_ShowMobileToggle_RendersMobileBar()
    {
        var items = new List<FaSubNavItem>
        {
            new() { Id = "opt1", Title = "Overview", IsActive = true },
            new() { Id = "opt2", Title = "Details" }
        };

        var cut = Render<FaSubNav>(p => p
            .Add(x => x.ShowMobileToggle, true)
            .Add(x => x.Items, items));

        var mobileBar = cut.Find(".fa-subnav-mobile-bar");
        Assert.NotNull(mobileBar);

        var toggleBtn = cut.Find("button.fa-subnav-mobile-toggle");
        Assert.NotNull(toggleBtn);
        Assert.True(toggleBtn.HasAttribute("data-subnav-mobile-toggle"));
        Assert.Equal("faToggleSubNavMobile(this)", toggleBtn.GetAttribute("onclick"));
    }

    [Fact]
    public void FaSubNav_IconsUseInheritColor()
    {
        var items = new List<FaSubNavItem>
        {
            new() { Id = "test", Title = "Test Item", Icon = FaIconName.Target }
        };

        var cut = Render<FaSubNav>(p => p
            .Add(x => x.Items, items));

        var icon = cut.Find("svg.fa-icon");
        Assert.NotNull(icon);
        Assert.Contains("fa-icon-inherit", icon.ClassName);
    }

    [Fact]
    public void FaHeader_WithSubNavContent_RendersCollapseWrapperAndToggle()
    {
        RenderFragment subnav = builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "my-subnav-content");
            builder.AddContent(2, "SubNav Row");
            builder.CloseElement();
        };

        var cut = Render<FaHeader>(p => p
            .Add(x => x.BrandText, "Test App")
            .Add(x => x.SubNavContent, subnav));

        var header = cut.Find("header.fa-header");
        Assert.Contains("fa-header-has-subnav", header.ClassName);

        var collapse = cut.Find(".fa-header-collapse");
        Assert.NotNull(collapse);

        var subnavRow = cut.Find(".fa-header-subnav-row");
        Assert.NotNull(subnavRow);
        Assert.Contains("SubNav Row", subnavRow.TextContent);

        // Mobile hamburger toggle is present for subnav even without main nav
        var toggle = cut.Find("button.fa-header-nav-toggle");
        Assert.NotNull(toggle);
    }

    [Fact]
    public void FaStandardShell_WithSubNavService_ResolvesDynamically()
    {
        var subNavService = new Fran.Services.FaSubNavService();
        Services.AddSingleton(subNavService);

        var cut = Render<FaStandardShell>(p => p
            .Add(x => x.BrandText, "Test App")
            .AddChildContent<FaHeaderSubNav>(nav => nav
                .AddChildContent("<span class=\"injected-subnav\">Dynamic SubNav Content</span>")));

        var subnavRow = cut.Find(".fa-header-subnav-row");
        Assert.NotNull(subnavRow);
        Assert.Contains("Dynamic SubNav Content", subnavRow.TextContent);
    }
}
