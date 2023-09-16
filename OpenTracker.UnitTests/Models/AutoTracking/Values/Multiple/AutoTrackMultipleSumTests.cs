using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using OpenTracker.Models.AutoTracking.Values;
using OpenTracker.Models.AutoTracking.Values.Multiple;
using Xunit;

namespace OpenTracker.UnitTests.Models.AutoTracking.Values.Multiple;

[ExcludeFromCodeCoverage]
public sealed class AutoTrackMultipleSumTests
{
    [Theory]
    [InlineData(null, null, null)]
    [InlineData(0, 0, null)]
    [InlineData(1, 1, null)]
    [InlineData(2, 2, null)]
    [InlineData(0, null, 0)]
    [InlineData(0, 0, 0)]
    [InlineData(1, 1, 0)]
    [InlineData(2, 2, 0)]
    [InlineData(1, null, 1)]
    [InlineData(1, 0, 1)]
    [InlineData(2, 1, 1)]
    [InlineData(3, 2, 1)]
    [InlineData(2, null, 2)]
    [InlineData(2, 0, 2)]
    [InlineData(3, 1, 2)]
    [InlineData(4, 2, 2)]
    [InlineData(null, null, null, null)]
    [InlineData(0, 0, null, null)]
    [InlineData(1, 1, null, null)]
    [InlineData(2, 2, null, null)]
    [InlineData(0, null, 0, null)]
    [InlineData(0, 0, 0, null)]
    [InlineData(1, 1, 0, null)]
    [InlineData(2, 2, 0, null)]
    [InlineData(1, null, 1, null)]
    [InlineData(1, 0, 1, null)]
    [InlineData(2, 1, 1, null)]
    [InlineData(3, 2, 1, null)]
    [InlineData(2, null, 2, null)]
    [InlineData(2, 0, 2, null)]
    [InlineData(3, 1, 2, null)]
    [InlineData(4, 2, 2, null)]
    [InlineData(0, null, null, 0)]
    [InlineData(0, 0, null, 0)]
    [InlineData(1, 1, null, 0)]
    [InlineData(2, 2, null, 0)]
    [InlineData(0, null, 0, 0)]
    [InlineData(0, 0, 0, 0)]
    [InlineData(1, 1, 0, 0)]
    [InlineData(2, 2, 0, 0)]
    [InlineData(1, null, 1, 0)]
    [InlineData(1, 0, 1, 0)]
    [InlineData(2, 1, 1, 0)]
    [InlineData(3, 2, 1, 0)]
    [InlineData(2, null, 2, 0)]
    [InlineData(2, 0, 2, 0)]
    [InlineData(3, 1, 2, 0)]
    [InlineData(4, 2, 2, 0)]
    [InlineData(1, null, null, 1)]
    [InlineData(1, 0, null, 1)]
    [InlineData(2, 1, null, 1)]
    [InlineData(3, 2, null, 1)]
    [InlineData(1, null, 0, 1)]
    [InlineData(1, 0, 0, 1)]
    [InlineData(2, 1, 0, 1)]
    [InlineData(3, 2, 0, 1)]
    [InlineData(2, null, 1, 1)]
    [InlineData(2, 0, 1, 1)]
    [InlineData(3, 1, 1, 1)]
    [InlineData(4, 2, 1, 1)]
    [InlineData(3, null, 2, 1)]
    [InlineData(3, 0, 2, 1)]
    [InlineData(4, 1, 2, 1)]
    [InlineData(5, 2, 2, 1)]
    [InlineData(2, null, null, 2)]
    [InlineData(2, 0, null, 2)]
    [InlineData(3, 1, null, 2)]
    [InlineData(4, 2, null, 2)]
    [InlineData(2, null, 0, 2)]
    [InlineData(2, 0, 0, 2)]
    [InlineData(3, 1, 0, 2)]
    [InlineData(4, 2, 0, 2)]
    [InlineData(3, null, 1, 2)]
    [InlineData(3, 0, 1, 2)]
    [InlineData(4, 1, 1, 2)]
    [InlineData(5, 2, 1, 2)]
    [InlineData(4, null, 2, 2)]
    [InlineData(4, 0, 2, 2)]
    [InlineData(5, 1, 2, 2)]
    [InlineData(6, 2, 2, 2)]
    public void CurrentValue_ShouldEqualExpected(int? expected, params int?[] values)
    {
        var valueList = values
            .Select(value => new MockAutoTrackValue { CurrentValue = value })
            .Cast<IAutoTrackValue>()
            .ToList();

        var sut = new AutoTrackMultipleSum(valueList);

        sut.CurrentValue.Should().Be(expected);
    }
        
    [Fact]
    public void ValueChanged_ShouldRaisePropertyChanged()
    {
        var mockValues = new List<MockAutoTrackValue>
        {
            new(),
            new(),
            new()
        };

        foreach (var value in mockValues)
        {
            value.CurrentValue = 0;
        }

        var sut = new AutoTrackMultipleSum(mockValues);

        using var monitor = sut.Monitor();

        mockValues[0].CurrentValue = 1;

        monitor.Should().RaisePropertyChangeFor(x => x.CurrentValue);
    }
}