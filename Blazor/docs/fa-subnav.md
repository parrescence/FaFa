# FaSubNav & FaSubNavItem

Secondary navigation / subnavbar row for pages, view selectors, and category filters. Can be placed standalone directly under the header or embedded as an attached row to `FaHeader`, `FaStandardShell`, or `FaSidebarShell`.

Includes built-in responsive behavior for small windows and mobile screens, automatically collapsing into a hamburger dropdown menu.

---

## Standalone Usage

```razor
<FaSubNav Title="Standard Type:" Sticky="true">
    <ChildContent>
        <button type="button" class="fa-subnav-link @(_selectedType == null ? "fa-subnav-link-active" : "")" @onclick="() => SelectType(null)">
            All Standards
        </button>
        <button type="button" class="fa-subnav-link @(_selectedType == StandardScope.SpecificMeet ? "fa-subnav-link-active" : "")" @onclick="() => SelectType(StandardScope.SpecificMeet)">
            Specific Meets
        </button>
        <button type="button" class="fa-subnav-link @(_selectedType == StandardScope.QualificationStandard ? "fa-subnav-link-active" : "")" @onclick="() => SelectType(StandardScope.QualificationStandard)">
            Qualification Standards
        </button>
        <button type="button" class="fa-subnav-link @(_selectedType == StandardScope.ClubProgression ? "fa-subnav-link-active" : "")" @onclick="() => SelectType(StandardScope.ClubProgression)">
            Club Progression Goals
        </button>
    </ChildContent>
</FaSubNav>
```

---

## Structured Items Usage

```razor
<FaSubNav Title="Views:"
          Items="_subnavItems"
          ActiveId="@_activeViewId"
          OnItemClick="HandleItemClick"
          ShowMobileToggle="true" />

@code {
    private string _activeViewId = "all";
    private readonly List<FaSubNavItem> _subnavItems = new()
    {
        new() { Id = "all", Title = "All Meets", Icon = FaIconName.Trophy, IsActive = true },
        new() { Id = "quals", Title = "Qualifications", Badge = "14" },
        new() { Id = "goals", Title = "Club Goals", Href = "/goals" }
    };

    private void HandleItemClick(FaSubNavItem item)
    {
        _activeViewId = item.Id ?? item.Title;
    }
}
```

---

## Attached to FaStandardShell or FaHeader

```razor
<FaStandardShell BrandText="My App"
                 HeaderNav="@MyPrimaryNav"
                 SubNav="@MySubNavContent">
    @Body
</FaStandardShell>

@code {
    private RenderFragment MySubNavContent => __builder =>
    {
        <FaSubNav Title="Meets:">
            <ChildContent>
                <a href="/meets" class="fa-subnav-link fa-subnav-link-active">All Meets</a>
                <a href="/standards/private" class="fa-subnav-link">Private Standards</a>
            </ChildContent>
        </FaSubNav>
    };
}
```

---

## Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Title` | `string?` | `null` | Optional section or category title shown on the left. |
| `Items` | `IReadOnlyList<FaSubNavItem>?` | `null` | Structured subnav items. |
| `ActiveId` | `string?` | `null` | Target element ID or key of active item. |
| `OnItemClick` | `EventCallback<FaSubNavItem>` | — | Callback invoked when an item in `Items` is clicked. |
| `ChildContent` | `RenderFragment?` | `null` | Custom buttons, links, or inputs. |
| `RightContent` | `RenderFragment?` | `null` | Content aligned to the right. |
| `Sticky` | `bool` | `false` | When true, pins stickily below the header. |
| `ShowMobileToggle` | `bool` | `true` | When true, collapses into a hamburger toggle on mobile. |
| `MobileToggleText` | `string?` | `null` | Custom text on mobile hamburger button. |
