using Fran.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Fran.Components;

/// <summary>
/// Universal Zero-&lt;table&gt; Semantic Flex Table component.
/// Renders purely semantic &lt;div&gt; elements equipped with ARIA table roles:
/// <c>role="table"</c>, <c>role="rowgroup"</c>, <c>role="row"</c>, <c>role="columnheader"</c>,
/// and <c>role="cell"</c>.
///
/// On mobile screens, automatically collapses into clean responsive cards with
/// left-aligned labels sourced from each cell's <c>data-label</c> attribute.
/// </summary>
public sealed class FaFlexTable : ComponentBase
{
    [Parameter, EditorRequired] public IReadOnlyList<string> Columns { get; set; } = Array.Empty<string>();
    [Parameter, EditorRequired] public IReadOnlyList<IReadOnlyDictionary<string, RenderFragment>> Rows { get; set; } = Array.Empty<IReadOnlyDictionary<string, RenderFragment>>();
    [Parameter] public string? CssClass { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", CssClassNames.Combine("fa-flex-table", "fa-table", CssClass));
        builder.AddAttribute(2, "role", "table");
        builder.AddMultipleAttributes(3, AdditionalAttributes);

        // Header
        builder.OpenElement(4, "div");
        builder.AddAttribute(5, "class", "fa-flex-header");
        builder.AddAttribute(6, "role", "row");

        var seq = 7;
        foreach (var column in Columns)
        {
            builder.OpenElement(seq++, "div");
            builder.AddAttribute(seq++, "class", "fa-flex-th");
            builder.AddAttribute(seq++, "role", "columnheader");
            builder.AddContent(seq++, column);
            builder.CloseElement();
        }
        builder.CloseElement(); // .fa-flex-header

        // Body
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", "fa-flex-body");
        builder.AddAttribute(seq++, "role", "rowgroup");

        foreach (var row in Rows)
        {
            builder.OpenElement(seq++, "div");
            builder.AddAttribute(seq++, "class", "fa-flex-row");
            builder.AddAttribute(seq++, "role", "row");

            foreach (var column in Columns)
            {
                builder.OpenElement(seq++, "div");
                builder.AddAttribute(seq++, "class", "fa-flex-td");
                builder.AddAttribute(seq++, "role", "cell");
                builder.AddAttribute(seq++, "data-label", column);

                if (row.TryGetValue(column, out var cell))
                {
                    builder.AddContent(seq++, cell);
                }

                builder.CloseElement(); // .fa-flex-td
            }

            builder.CloseElement(); // .fa-flex-row
        }

        builder.CloseElement(); // .fa-flex-body
        builder.CloseElement(); // .fa-flex-table
    }
}
