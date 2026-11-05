using CafeOrders.Core.Domain;
using Xunit;

namespace CafeOrders.Tests;

public class GuardTests
{
    // Лістинг 2 зі звіту
    [Fact]
    public void Calculate_NoLines_ThrowsArgument()
    {
        Assert.Throws<ArgumentException>(() =>
            DocumentGuards.EnsureLinesValid(new List<OrderLine>()));
    }

    [Fact]
    public void EnsureDocumentIdValid_NonPositive_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            DocumentGuards.EnsureDocumentIdValid(0));

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            DocumentGuards.EnsureDocumentIdValid(-5));
    }

    [Fact]
    public void EnsureClientNameValid_Empty_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            DocumentGuards.EnsureClientNameValid("   "));
    }
}
