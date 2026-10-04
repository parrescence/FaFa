using System;
using Bunit;
using Fran.Icons;
using Xunit;

namespace Fran.Tests;

public class FaIconTests : BunitContext
{
    [Theory]
    [InlineData(FaIconName.Document)]
    [InlineData(FaIconName.DocumentSearch)]
    [InlineData(FaIconName.Stopwatch)]
    [InlineData(FaIconName.Trophy)]
    [InlineData(FaIconName.Bolt)]
    [InlineData(FaIconName.Upload)]
    [InlineData(FaIconName.Search)]
    [InlineData(FaIconName.Target)]
    [InlineData(FaIconName.TrendingUp)]
    [InlineData(FaIconName.BarChart)]
    [InlineData(FaIconName.Swimmer)]
    [InlineData(FaIconName.Medal)]
    [InlineData(FaIconName.BatchEdit)]
    public void FaIcon_RendersSvg_ForIconName(FaIconName iconName)
    {
        var cut = Render<FaIcon>(p => p
            .Add(x => x.Name, iconName)
            .Add(x => x.Size, 24)
            .Add(x => x.Color, FaIconColor.Black));

        var svg = cut.Find("svg");
        Assert.NotNull(svg);
        Assert.Contains("fa-icon", svg.GetAttribute("class"));
        Assert.Contains("fa-icon-black", svg.GetAttribute("class"));
        Assert.Equal("0 0 24 24", svg.GetAttribute("viewBox"));
    }

    [Fact]
    public void FaIcon_AllEnumValues_RenderWithoutException()
    {
        foreach (FaIconName icon in Enum.GetValues<FaIconName>())
        {
            var cut = Render<FaIcon>(p => p.Add(x => x.Name, icon));
            Assert.NotNull(cut.Find("svg"));
        }
    }

    [Theory]
    [InlineData(FaIconColor.White, "fa-icon-white")]
    [InlineData(FaIconColor.Black, "fa-icon-black")]
    [InlineData(FaIconColor.Primary, "fa-icon-primary")]
    [InlineData(FaIconColor.Secondary, "fa-icon-secondary")]
    [InlineData(FaIconColor.Success, "fa-icon-success")]
    [InlineData(FaIconColor.Warning, "fa-icon-warning")]
    [InlineData(FaIconColor.Danger, "fa-icon-danger")]
    [InlineData(FaIconColor.Info, "fa-icon-info")]
    [InlineData(FaIconColor.Muted, "fa-icon-muted")]
    [InlineData(FaIconColor.Inherit, "fa-icon-inherit")]
    public void FaIcon_RendersCorrectColorClass(FaIconColor color, string expectedClass)
    {
        var cut = Render<FaIcon>(p => p
            .Add(x => x.Name, FaIconName.Star)
            .Add(x => x.Color, color));

        var svg = cut.Find("svg");
        Assert.Contains(expectedClass, svg.GetAttribute("class"));
    }

    [Fact]
    public void FaIcon_RendersCustomColorStyle()
    {
        var cut = Render<FaIcon>(p => p
            .Add(x => x.Name, FaIconName.Target)
            .Add(x => x.CustomColor, "#FF00AA"));

        var svg = cut.Find("svg");
        Assert.Contains("color: #FF00AA;", svg.GetAttribute("style"));
    }
}
