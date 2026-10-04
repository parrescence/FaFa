using System.Collections.Generic;
using Bunit;
using Fran.Components;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Fran.Tests;

public class FaTableTests : BunitContext
{
    [Fact]
    public void FaTable_RendersSemanticZeroTableElementsWithAriaRoles()
    {
        var columns = new[] { "Name", "Role" };
        var rows = new List<IReadOnlyDictionary<string, RenderFragment>>
        {
            new Dictionary<string, RenderFragment>
            {
                ["Name"] = b => b.AddContent(0, "Alice"),
                ["Role"] = b => b.AddContent(0, "Engineer")
            }
        };

        var cut = Render<FaTable>(p => p
            .Add(x => x.Columns, columns)
            .Add(x => x.Rows, rows));

        // Zero-table policy: Must NOT render raw <table>, <thead>, <tbody>, <tr>, <th>, <td>
        Assert.Empty(cut.FindAll("table"));
        Assert.Empty(cut.FindAll("thead"));
        Assert.Empty(cut.FindAll("tbody"));
        Assert.Empty(cut.FindAll("tr"));
        Assert.Empty(cut.FindAll("th"));
        Assert.Empty(cut.FindAll("td"));

        // Must render semantic divs with ARIA roles
        var table = cut.Find("div[role='table']");
        Assert.Contains("fa-table", table.ClassName);
        Assert.Contains("fa-flex-table", table.ClassName);

        var headerRow = cut.Find("div.fa-flex-header[role='row']");
        Assert.NotNull(headerRow);

        var colHeaders = cut.FindAll("div.fa-flex-th[role='columnheader']");
        Assert.Equal(2, colHeaders.Count);
        Assert.Equal("Name", colHeaders[0].TextContent);
        Assert.Equal("Role", colHeaders[1].TextContent);

        var body = cut.Find("div.fa-flex-body[role='rowgroup']");
        Assert.NotNull(body);

        var row = cut.Find("div.fa-flex-row[role='row']");
        Assert.NotNull(row);

        var cells = cut.FindAll("div.fa-flex-td[role='cell']");
        Assert.Equal(2, cells.Count);
        Assert.Equal("Alice", cells[0].TextContent);
        Assert.Equal("Name", cells[0].GetAttribute("data-label"));
        Assert.Equal("Engineer", cells[1].TextContent);
        Assert.Equal("Role", cells[1].GetAttribute("data-label"));
    }

    [Fact]
    public void FaFlexTable_RendersSemanticZeroTableElementsWithDataLabels()
    {
        var columns = new[] { "SKU", "Price" };
        var rows = new List<IReadOnlyDictionary<string, RenderFragment>>
        {
            new Dictionary<string, RenderFragment>
            {
                ["SKU"] = b => b.AddContent(0, "ITEM-01"),
                ["Price"] = b => b.AddContent(0, "$19.99")
            }
        };

        var cut = Render<FaFlexTable>(p => p
            .Add(x => x.Columns, columns)
            .Add(x => x.Rows, rows));

        // Zero-table check
        Assert.Empty(cut.FindAll("table"));
        Assert.Empty(cut.FindAll("tr"));

        var table = cut.Find("div[role='table']");
        Assert.Contains("fa-flex-table", table.ClassName);

        var cells = cut.FindAll("div.fa-flex-td[role='cell']");
        Assert.Equal(2, cells.Count);
        Assert.Equal("ITEM-01", cells[0].TextContent);
        Assert.Equal("SKU", cells[0].GetAttribute("data-label"));
        Assert.Equal("$19.99", cells[1].TextContent);
        Assert.Equal("Price", cells[1].GetAttribute("data-label"));
    }
}
