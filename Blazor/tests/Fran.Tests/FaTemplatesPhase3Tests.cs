using Bunit;
using Fran.Components;
using Fran.Templates;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Fran.Tests;

public class FaTemplatesPhase3Tests : BunitContext
{
    [Fact]
    public void FaTutorialTemplate_RendersStepsMetadataAndNavigation()
    {
        var cut = Render<FaTutorialTemplate>(p => p
            .Add(x => x.BrandText, "Acme")
            .Add(x => x.Title, "Building Realtime Dashboards")
            .Add(x => x.Description, "Learn how to stream live telemetry")
            .Add(x => x.Difficulty, "Intermediate")
            .Add(x => x.ReadTime, "15 min")
            .Add(x => x.BreadcrumbContent, (RenderFragment)(b => b.AddMarkupContent(0, "<span>Tutorials &gt; Web</span>")))
            .Add(x => x.AuthorContent, (RenderFragment)(b => b.AddMarkupContent(0, "<span>By Sarah Lin</span>")))
            .Add(x => x.StepJumpList, (RenderFragment)(b => b.AddMarkupContent(0, "<ol class=\"step-links\"><li>Step 1</li></ol>")))
            .Add(x => x.ChildContent, (RenderFragment)(b => b.AddMarkupContent(0, "<div class=\"step-content\">Step 1 details</div>")))
            .Add(x => x.PrevNextNav, (RenderFragment)(b => b.AddMarkupContent(0, "<a class=\"next-tut\">Next Tutorial</a>"))));

        var title = cut.Find("h1.fa-tutorial-title");
        Assert.Equal("Building Realtime Dashboards", title.TextContent.Trim());

        var desc = cut.Find("p.fa-tutorial-description");
        Assert.Equal("Learn how to stream live telemetry", desc.TextContent.Trim());

        var diff = cut.Find(".fa-tutorial-difficulty");
        Assert.Equal("Intermediate", diff.TextContent.Trim());

        var aside = cut.Find(".fa-tutorial-aside");
        Assert.Contains("Tutorial Steps", aside.TextContent);
        Assert.Contains("Step 1", aside.TextContent);

        var content = cut.Find(".fa-tutorial-main");
        Assert.Contains("Step 1 details", content.TextContent);
        Assert.Contains("Next Tutorial", content.TextContent);
    }

    [Fact]
    public void FaKnowledgeBaseTemplate_RendersHeroCategoriesAndPopular()
    {
        var cut = Render<FaKnowledgeBaseTemplate>(p => p
            .Add(x => x.BrandText, "Acme")
            .Add(x => x.Title, "How can we help?")
            .Add(x => x.Description, "Browse docs or search topics")
            .Add(x => x.SearchContent, (RenderFragment)(b => b.AddMarkupContent(0, "<input class=\"kb-search-box\" placeholder=\"Search...\" />")))
            .Add(x => x.CategoryContent, (RenderFragment)(b => b.AddMarkupContent(0, "<div class=\"kb-card\">Account &amp; Setup</div>")))
            .Add(x => x.PopularArticlesContent, (RenderFragment)(b => b.AddMarkupContent(0, "<div class=\"kb-article\">Reset your API Key</div>")))
            .Add(x => x.SupportActionContent, (RenderFragment)(b => b.AddMarkupContent(0, "<button class=\"kb-contact\">Contact Us</button>"))));

        var title = cut.Find("h1.fa-kb-title");
        Assert.Equal("How can we help?", title.TextContent.Trim());

        var search = cut.Find(".fa-kb-search");
        Assert.NotNull(search.QuerySelector("input.kb-search-box"));

        var categories = cut.Find(".fa-kb-categories");
        Assert.Contains("Account & Setup", categories.TextContent);

        var popular = cut.Find(".fa-kb-popular");
        Assert.Contains("Reset your API Key", popular.TextContent);

        var support = cut.Find(".fa-kb-support");
        Assert.Contains("Contact Us", support.TextContent);
    }

    [Fact]
    public void FaRoadmapTemplate_RendersKanbanColumnsAndFilters()
    {
        var cut = Render<FaRoadmapTemplate>(p => p
            .Add(x => x.BrandText, "Acme")
            .Add(x => x.Title, "Public Roadmap")
            .Add(x => x.Description, "What we're building next")
            .Add(x => x.HeaderAction, (RenderFragment)(b => b.AddMarkupContent(0, "<button class=\"btn-suggest\">Suggest Feature</button>")))
            .Add(x => x.FilterBar, (RenderFragment)(b => b.AddMarkupContent(0, "<span class=\"filter-chip\">Core</span>")))
            .Add(x => x.PlannedColumn, (RenderFragment)(b => b.AddMarkupContent(0, "<div class=\"item-planned\">Dark Mode v2</div>")))
            .Add(x => x.InProgressColumn, (RenderFragment)(b => b.AddMarkupContent(0, "<div class=\"item-progress\">Mobile App</div>")))
            .Add(x => x.CompletedColumn, (RenderFragment)(b => b.AddMarkupContent(0, "<div class=\"item-completed\">GraphQL API</div>"))));

        var title = cut.Find("h1.fa-roadmap-title");
        Assert.Equal("Public Roadmap", title.TextContent.Trim());

        var actions = cut.Find(".fa-roadmap-actions");
        Assert.Contains("Suggest Feature", actions.TextContent);

        var kanban = cut.Find(".fa-roadmap-kanban");
        Assert.Contains("Dark Mode v2", kanban.TextContent);
        Assert.Contains("Mobile App", kanban.TextContent);
        Assert.Contains("GraphQL API", kanban.TextContent);
    }

    [Fact]
    public void FaGlossaryTemplate_RendersAlphabetNavAndTerms()
    {
        var cut = Render<FaGlossaryTemplate>(p => p
            .Add(x => x.BrandText, "Acme")
            .Add(x => x.Title, "Design Systems Glossary")
            .Add(x => x.SearchContent, (RenderFragment)(b => b.AddMarkupContent(0, "<input placeholder=\"Filter terms\" />")))
            .Add(x => x.AlphabetNav, (RenderFragment)(b => b.AddMarkupContent(0, "<a href=\"#A\">A</a><a href=\"#B\">B</a>")))
            .Add(x => x.ChildContent, (RenderFragment)(b => b.AddMarkupContent(0, "<section id=\"A\"><h3>Accessibility (a11y)</h3><p>Designing for all users.</p></section>"))));

        var title = cut.Find("h1.fa-glossary-title");
        Assert.Equal("Design Systems Glossary", title.TextContent.Trim());

        var nav = cut.Find(".fa-glossary-alphabet-nav");
        Assert.Contains("A", nav.TextContent);
        Assert.Contains("B", nav.TextContent);

        var main = cut.Find(".fa-glossary-main");
        Assert.Contains("Accessibility (a11y)", main.TextContent);
    }

    [Fact]
    public void FaBlogIndexTemplate_RendersFeaturedPostAndGrid()
    {
        var cut = Render<FaBlogIndexTemplate>(p => p
            .Add(x => x.BrandText, "Acme")
            .Add(x => x.Title, "Engineering Blog")
            .Add(x => x.FeaturedPost, (RenderFragment)(b => b.AddMarkupContent(0, "<div class=\"featured-card\">Announcing v1.0</div>")))
            .Add(x => x.CategoryBar, (RenderFragment)(b => b.AddMarkupContent(0, "<span class=\"tag\">All</span>")))
            .Add(x => x.ChildContent, (RenderFragment)(b => b.AddMarkupContent(0, "<div class=\"post-card\">Post 1</div><div class=\"post-card\">Post 2</div>")))
            .Add(x => x.PaginationContent, (RenderFragment)(b => b.AddMarkupContent(0, "<button>Next Page</button>"))));

        var title = cut.Find("h1.fa-blog-index-title");
        Assert.Equal("Engineering Blog", title.TextContent.Trim());

        var feat = cut.Find(".fa-blog-index-featured");
        Assert.Contains("Announcing v1.0", feat.TextContent);

        var grid = cut.Find(".fa-blog-index-grid");
        Assert.Contains("Post 1", grid.TextContent);
        Assert.Contains("Post 2", grid.TextContent);

        var pag = cut.Find(".fa-blog-index-pagination");
        Assert.Contains("Next Page", pag.TextContent);
    }

    [Fact]
    public void FaBlogPostTemplate_RendersBackLinkMetaCoverAndProse()
    {
        var cut = Render<FaBlogPostTemplate>(p => p
            .Add(x => x.BrandText, "Acme")
            .Add(x => x.BackHref, "/blog")
            .Add(x => x.Title, "Scaling to 1 Million WebSocket Connections")
            .Add(x => x.PublishedDate, "Oct 12, 2026")
            .Add(x => x.ReadTime, "8 min read")
            .Add(x => x.CoverImageUrl, "/images/hero.png")
            .Add(x => x.ChildContent, (RenderFragment)(b => b.AddMarkupContent(0, "<p>In this post we explain memory optimizations...</p>")))
            .Add(x => x.AuthorBio, (RenderFragment)(b => b.AddMarkupContent(0, "<div>Alex Mercer is Staff Engineer</div>")))
            .Add(x => x.ShareContent, (RenderFragment)(b => b.AddMarkupContent(0, "<button>Share on X</button>"))));

        var back = cut.Find(".fa-blog-post-back-link");
        Assert.Equal("/blog", back.GetAttribute("href"));

        var title = cut.Find("h1.fa-blog-post-title");
        Assert.Equal("Scaling to 1 Million WebSocket Connections", title.TextContent.Trim());

        var img = cut.Find(".fa-blog-post-cover img");
        Assert.Equal("/images/hero.png", img.GetAttribute("src"));

        var content = cut.Find(".fa-blog-post-content");
        Assert.Contains("In this post we explain memory optimizations...", content.TextContent);

        var bio = cut.Find(".fa-blog-post-author-bio");
        Assert.Contains("Alex Mercer is Staff Engineer", bio.TextContent);
    }

    [Fact]
    public void FaBillingTemplate_RendersPlansPaymentAndInvoices()
    {
        var cut = Render<FaBillingTemplate>(p => p
            .Add(x => x.BrandText, "Acme")
            .Add(x => x.Title, "Plans & Invoices")
            .Add(x => x.HeaderAction, (RenderFragment)(b => b.AddMarkupContent(0, "<button class=\"btn-upgrade\">Upgrade</button>")))
            .Add(x => x.CurrentPlanContent, (RenderFragment)(b => b.AddMarkupContent(0, "<div class=\"plan-info\">Enterprise tier</div>")))
            .Add(x => x.PaymentMethodsContent, (RenderFragment)(b => b.AddMarkupContent(0, "<div class=\"pm-card\">Visa ending in 4242</div>")))
            .Add(x => x.InvoicesContent, (RenderFragment)(b => b.AddMarkupContent(0, "<div class=\"inv-table fa-flex-table\" role=\"table\"><div class=\"fa-flex-row\" role=\"row\"><div class=\"fa-flex-td\" role=\"cell\">INV-001</div></div></div>"))));

        var title = cut.Find("h1.fa-billing-title");
        Assert.Equal("Plans & Invoices", title.TextContent.Trim());

        var action = cut.Find(".fa-billing-header-action");
        Assert.Contains("Upgrade", action.TextContent);

        var plan = cut.Find(".fa-billing-plan-section");
        Assert.Contains("Enterprise tier", plan.TextContent);

        var pm = cut.Find(".fa-billing-payment-section");
        Assert.Contains("Visa ending in 4242", pm.TextContent);

        var inv = cut.Find(".fa-billing-invoices-section");
        Assert.Contains("INV-001", inv.TextContent);
    }

    [Fact]
    public void FaActivityFeedTemplate_RendersStatsFiltersAndStream()
    {
        var cut = Render<FaActivityFeedTemplate>(p => p
            .Add(x => x.BrandText, "Acme")
            .Add(x => x.Title, "Team Audit Feed")
            .Add(x => x.SummaryStats, (RenderFragment)(b => b.AddMarkupContent(0, "<div class=\"stat-box\">45 events today</div>")))
            .Add(x => x.FilterToolbar, (RenderFragment)(b => b.AddMarkupContent(0, "<select><option>All Users</option></select>")))
            .Add(x => x.ChildContent, (RenderFragment)(b => b.AddMarkupContent(0, "<ul class=\"feed-items\"><li>User updated billing</li></ul>")))
            .Add(x => x.PaginationContent, (RenderFragment)(b => b.AddMarkupContent(0, "<button>Load More</button>"))));

        var title = cut.Find("h1.fa-activity-feed-title");
        Assert.Equal("Team Audit Feed", title.TextContent.Trim());

        var stats = cut.Find(".fa-activity-feed-stats");
        Assert.Contains("45 events today", stats.TextContent);

        var stream = cut.Find(".fa-activity-feed-stream");
        Assert.Contains("User updated billing", stream.TextContent);

        var pag = cut.Find(".fa-activity-feed-pagination");
        Assert.Contains("Load More", pag.TextContent);
    }

    [Fact]
    public void FaStatusTemplate_RendersBannerServicesAndIncidents()
    {
        var cut = Render<FaStatusTemplate>(p => p
            .Add(x => x.BrandText, "Acme")
            .Add(x => x.OverallStatus, "All Systems Operational")
            .Add(x => x.OverallStatusVariant, FaBadgeVariant.Success)
            .Add(x => x.ServicesContent, (RenderFragment)(b => b.AddMarkupContent(0, "<div class=\"service-row\">API Gateway: 100%</div>")))
            .Add(x => x.UptimeMetricsContent, (RenderFragment)(b => b.AddMarkupContent(0, "<div class=\"uptime-bar\">99.98% uptime</div>")))
            .Add(x => x.PastIncidentsContent, (RenderFragment)(b => b.AddMarkupContent(0, "<div class=\"incident\">No incidents reported.</div>"))));

        var banner = cut.Find(".fa-status-banner");
        Assert.Contains("fa-status-banner-success", banner.ClassName);
        Assert.Contains("All Systems Operational", banner.TextContent);

        var services = cut.Find(".fa-status-services");
        Assert.Contains("API Gateway: 100%", services.TextContent);

        var metrics = cut.Find(".fa-status-metrics");
        Assert.Contains("99.98% uptime", metrics.TextContent);

        var incidents = cut.Find(".fa-status-incidents");
        Assert.Contains("No incidents reported.", incidents.TextContent);
    }

    [Fact]
    public void FaOnboardingTemplate_RendersStepProgressAndActions()
    {
        var cut = Render<FaOnboardingTemplate>(p => p
            .Add(x => x.BrandText, "Acme")
            .Add(x => x.Title, "Set up your workspace")
            .Add(x => x.CurrentStep, 2)
            .Add(x => x.TotalSteps, 4)
            .Add(x => x.StepTitle, "Connect Repository")
            .Add(x => x.StepDescription, "Link your GitHub or GitLab account")
            .Add(x => x.ChildContent, (RenderFragment)(b => b.AddMarkupContent(0, "<button class=\"gh-auth\">Authorize GitHub</button>")))
            .Add(x => x.BackAction, (RenderFragment)(b => b.AddMarkupContent(0, "<button class=\"btn-back\">Back</button>")))
            .Add(x => x.ContinueAction, (RenderFragment)(b => b.AddMarkupContent(0, "<button class=\"btn-continue\">Continue</button>"))));

        var brand = cut.Find("h1.fa-onboarding-brand-title");
        Assert.Equal("Set up your workspace", brand.TextContent.Trim());

        var counter = cut.Find(".fa-onboarding-step-counter");
        Assert.Equal("Step 2 of 4", counter.TextContent.Trim());

        var stepTitle = cut.Find("h2.fa-onboarding-step-title");
        Assert.Equal("Connect Repository", stepTitle.TextContent.Trim());

        var nav = cut.Find(".fa-onboarding-nav");
        Assert.NotNull(nav.QuerySelector("button.btn-back"));
        Assert.NotNull(nav.QuerySelector("button.btn-continue"));
    }

    [Fact]
    public void FaCalendarTemplate_RendersToolbarGridAndAside()
    {
        var cut = Render<FaCalendarTemplate>(p => p
            .Add(x => x.BrandText, "Acme")
            .Add(x => x.Title, "Company Calendar")
            .Add(x => x.DateRangeTitle, "October 2026")
            .Add(x => x.DateNav, (RenderFragment)(b => b.AddMarkupContent(0, "<button class=\"btn-prev\">&lt;</button>")))
            .Add(x => x.ViewSwitcher, (RenderFragment)(b => b.AddMarkupContent(0, "<span class=\"switcher\">Month | Week</span>")))
            .Add(x => x.PrimaryAction, (RenderFragment)(b => b.AddMarkupContent(0, "<button class=\"btn-new\">+ New Event</button>")))
            .Add(x => x.ChildContent, (RenderFragment)(b => b.AddMarkupContent(0, "<div class=\"calendar-grid\">31 days rendered</div>")))
            .Add(x => x.EventAsideContent, (RenderFragment)(b => b.AddMarkupContent(0, "<div class=\"event-detail\">Sprint Planning at 10 AM</div>"))));

        var title = cut.Find("h1.fa-calendar-title");
        Assert.Equal("Company Calendar", title.TextContent.Trim());

        var range = cut.Find(".fa-calendar-range-title");
        Assert.Equal("October 2026", range.TextContent.Trim());

        var controls = cut.Find(".fa-calendar-controls");
        Assert.Contains("+ New Event", controls.TextContent);

        var grid = cut.Find(".fa-calendar-main");
        Assert.Contains("31 days rendered", grid.TextContent);

        var aside = cut.Find(".fa-calendar-aside");
        Assert.Contains("Sprint Planning at 10 AM", aside.TextContent);
    }

    [Fact]
    public void FaFileManagerTemplate_RendersHeaderToolbarGridAndInspector()
    {
        var cut = Render<FaFileManagerTemplate>(p => p
            .Add(x => x.BrandText, "Acme")
            .Add(x => x.Title, "Cloud Storage")
            .Add(x => x.StorageQuotaContent, (RenderFragment)(b => b.AddMarkupContent(0, "<span>2.4 GB used</span>")))
            .Add(x => x.PrimaryAction, (RenderFragment)(b => b.AddMarkupContent(0, "<button class=\"btn-upload\">Upload</button>")))
            .Add(x => x.BreadcrumbContent, (RenderFragment)(b => b.AddMarkupContent(0, "<span>Documents &gt; Contracts</span>")))
            .Add(x => x.ChildContent, (RenderFragment)(b => b.AddMarkupContent(0, "<div class=\"file-item\">Agreement.pdf</div>")))
            .Add(x => x.InspectorContent, (RenderFragment)(b => b.AddMarkupContent(0, "<div class=\"meta-panel\">Size: 240 KB</div>"))));

        var title = cut.Find("h1.fa-file-manager-title");
        Assert.Equal("Cloud Storage", title.TextContent.Trim());

        var quota = cut.Find(".fa-file-manager-quota");
        Assert.Contains("2.4 GB used", quota.TextContent);

        var breadcrumbs = cut.Find(".fa-file-manager-breadcrumbs");
        Assert.Contains("Documents > Contracts", breadcrumbs.TextContent);

        var main = cut.Find(".fa-file-manager-main");
        Assert.Contains("Agreement.pdf", main.TextContent);

        var inspector = cut.Find(".fa-file-manager-inspector");
        Assert.Contains("Size: 240 KB", inspector.TextContent);
    }
}
