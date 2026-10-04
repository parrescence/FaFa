using Bunit;
using Fran.Components;
using Fran.Templates;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Fran.Tests;

public class FaTemplatesPhase1Tests : BunitContext
{
    [Fact]
    public void FaPricingTemplate_RendersHeaderPlansFaq()
    {
        var cut = Render<FaPricingTemplate>(p => p
            .Add(x => x.BrandText, "Acme")
            .Add(x => x.Title, "Our Plans")
            .Add(x => x.Description, "Pick a plan that works for you")
            .Add(x => x.Plans, (RenderFragment)(b => b.AddMarkupContent(0, "<div class=\"pricing-card\">Pro Plan</div>")))
            .Add(x => x.FaqContent, (RenderFragment)(b => b.AddMarkupContent(0, "<p>Cancel anytime</p>"))));

        var title = cut.Find("h1.fa-template-pricing-title");
        Assert.Equal("Our Plans", title.TextContent.Trim());

        var desc = cut.Find("p.fa-template-pricing-description");
        Assert.Equal("Pick a plan that works for you", desc.TextContent.Trim());

        var plans = cut.Find(".fa-template-pricing-plans");
        Assert.Contains("Pro Plan", plans.TextContent);

        var faq = cut.Find(".fa-template-pricing-faq");
        Assert.Contains("Cancel anytime", faq.TextContent);
    }

    [Fact]
    public void FaContactTemplate_RendersInfoAndForm()
    {
        var cut = Render<FaContactTemplate>(p => p
            .Add(x => x.BrandText, "Acme")
            .Add(x => x.Title, "Contact Us")
            .Add(x => x.ContactInfo, (RenderFragment)(b => b.AddMarkupContent(0, "<p class=\"info-email\">support@example.com</p>")))
            .Add(x => x.FormContent, (RenderFragment)(b => b.AddMarkupContent(0, "<form class=\"test-form\"><input /></form>"))));

        var title = cut.Find("h1.fa-template-contact-title");
        Assert.Equal("Contact Us", title.TextContent.Trim());

        var info = cut.Find(".fa-template-contact-info");
        Assert.Contains("support@example.com", info.TextContent);

        var form = cut.Find(".fa-template-contact-form");
        Assert.NotNull(form.QuerySelector("form.test-form"));
    }

    [Fact]
    public void FaFaqTemplate_RendersSearchCategoriesAndItems()
    {
        var cut = Render<FaFaqTemplate>(p => p
            .Add(x => x.BrandText, "Acme")
            .Add(x => x.Title, "Help Center FAQ")
            .Add(x => x.SearchContent, (RenderFragment)(b => b.AddMarkupContent(0, "<input class=\"faq-search\" />")))
            .Add(x => x.Categories, (RenderFragment)(b => b.AddMarkupContent(0, "<span class=\"faq-cat\">Billing</span>")))
            .Add(x => x.ChildContent, (RenderFragment)(b => b.AddMarkupContent(0, "<div class=\"faq-item\">How to reset password?</div>")))
            .Add(x => x.SupportTitle, "Need direct assistance?"));

        var title = cut.Find("h1.fa-template-faq-title");
        Assert.Equal("Help Center FAQ", title.TextContent.Trim());

        var search = cut.Find(".fa-template-faq-search");
        Assert.NotNull(search.QuerySelector("input.faq-search"));

        var categories = cut.Find(".fa-template-faq-categories");
        Assert.Contains("Billing", categories.TextContent);

        var items = cut.Find(".fa-template-faq-items");
        Assert.Contains("How to reset password?", items.TextContent);

        var supportTitle = cut.Find("h3.fa-template-faq-support-title");
        Assert.Equal("Need direct assistance?", supportTitle.TextContent.Trim());
    }

    [Fact]
    public void FaNotFoundTemplate_Standalone_RendersStatusCodeAndHomeButton()
    {
        var cut = Render<FaNotFoundTemplate>(p => p
            .Add(x => x.BrandText, "Acme")
            .Add(x => x.StatusCode, "404")
            .Add(x => x.Title, "Missing Page")
            .Add(x => x.HomeHref, "/"));

        var code = cut.Find(".fa-template-notfound-code");
        Assert.Equal("404", code.TextContent.Trim());

        var title = cut.Find(".fa-template-notfound-title");
        Assert.Equal("Missing Page", title.TextContent.Trim());

        var btn = cut.Find("a.fa-btn");
        Assert.Equal("/", btn.GetAttribute("href"));
    }

    [Fact]
    public void FaSettingsTemplate_RendersNavAndSectionCard()
    {
        var cut = Render<FaSettingsTemplate>(p => p
            .Add(x => x.BrandText, "Acme")
            .Add(x => x.Title, "Account Settings")
            .Add(x => x.SettingsNav, (RenderFragment)(b => b.AddMarkupContent(0, "<a class=\"nav-item\" href=\"#sec\">Security</a>")))
            .Add(x => x.SectionTitle, "Security Settings")
            .Add(x => x.ChildContent, (RenderFragment)(b => b.AddMarkupContent(0, "<div class=\"field\">Password input</div>"))));

        var title = cut.Find("h1.fa-template-settings-title");
        Assert.Equal("Account Settings", title.TextContent.Trim());

        var nav = cut.Find("nav.fa-template-settings-nav");
        Assert.Contains("Security", nav.TextContent);

        var secTitle = cut.Find("h2.fa-template-settings-section-title");
        Assert.Equal("Security Settings", secTitle.TextContent.Trim());

        var content = cut.Find(".fa-template-settings-content");
        Assert.Contains("Password input", content.TextContent);
    }

    [Fact]
    public void FaListViewTemplate_RendersHeaderToolbarAndGrid()
    {
        var cut = Render<FaListViewTemplate>(p => p
            .Add(x => x.BrandText, "Acme")
            .Add(x => x.Title, "Orders")
            .Add(x => x.PrimaryAction, (RenderFragment)(b => b.AddMarkupContent(0, "<button class=\"add-order\">Create Order</button>")))
            .Add(x => x.SearchFilterBar, (RenderFragment)(b => b.AddMarkupContent(0, "<input class=\"order-filter\" />")))
            .Add(x => x.ChildContent, (RenderFragment)(b => b.AddMarkupContent(0, "<div class=\"orders-table fa-flex-table\" role=\"table\"><div class=\"fa-flex-row\" role=\"row\"><div class=\"fa-flex-td\" role=\"cell\">Order #1</div></div></div>"))));

        var title = cut.Find("h1.fa-template-list-title");
        Assert.Equal("Orders", title.TextContent.Trim());

        var action = cut.Find(".fa-template-list-actions");
        Assert.Contains("Create Order", action.TextContent);

        var toolbar = cut.Find(".fa-template-list-toolbar");
        Assert.NotNull(toolbar.QuerySelector("input.order-filter"));

        var content = cut.Find(".fa-template-list-content");
        Assert.Contains("Order #1", content.TextContent);
    }

    [Fact]
    public void FaDetailViewTemplate_RendersBackLinkStatusBadgeAndTabs()
    {
        var cut = Render<FaDetailViewTemplate>(p => p
            .Add(x => x.BrandText, "Acme")
            .Add(x => x.BackHref, "/orders")
            .Add(x => x.BackText, "Orders")
            .Add(x => x.Title, "Order #1024")
            .Add(x => x.StatusBadge, (RenderFragment)(b => b.AddMarkupContent(0, "<span class=\"status-pill\">Paid</span>")))
            .Add(x => x.Tabs, (RenderFragment)(b => b.AddMarkupContent(0, "<div class=\"detail-tabs\">Overview | History</div>")))
            .Add(x => x.ChildContent, (RenderFragment)(b => b.AddMarkupContent(0, "<p>Order items details</p>")))
            .Add(x => x.AsideContent, (RenderFragment)(b => b.AddMarkupContent(0, "<div class=\"customer-summary\">Customer John</div>"))));

        var backLink = cut.Find("a.fa-template-back-link");
        Assert.Equal("/orders", backLink.GetAttribute("href"));
        Assert.Contains("Orders", backLink.TextContent);

        var title = cut.Find("h1.fa-template-detail-title");
        Assert.Equal("Order #1024", title.TextContent.Trim());

        var badge = cut.Find(".fa-template-detail-badge");
        Assert.Contains("Paid", badge.TextContent);

        var tabs = cut.Find(".fa-template-detail-tabs");
        Assert.Contains("Overview", tabs.TextContent);

        var aside = cut.Find("aside.fa-template-detail-aside");
        Assert.Contains("Customer John", aside.TextContent);
    }
}
