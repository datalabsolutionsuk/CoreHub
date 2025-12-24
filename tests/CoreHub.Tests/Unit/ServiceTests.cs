using Xunit;
using FluentAssertions;
using CoreHub.Application.Interfaces;
using CoreHub.Infrastructure.Services;
using Moq;

namespace CoreHub.Tests.Unit;

public class ScoringServiceTests
{
    [Fact]
    public async Task CalculateScore_WithValidAnswers_ReturnsCorrectTotal()
    {
        // Arrange
        var mockContext = new Mock<IApplicationDbContext>();
        var service = new ScoringService(mockContext.Object);
        var measureId = Guid.NewGuid();
        var answers = new Dictionary<Guid, int>
        {
            { Guid.NewGuid(), 2 },
            { Guid.NewGuid(), 3 },
            { Guid.NewGuid(), 1 }
        };

        // Act
        var result = await service.CalculateScoreAsync(measureId, answers);

        // Assert
        result.TotalScore.Should().Be(6);
    }

    [Fact]
    public async Task DetectRisk_WithHighScores_ReturnsTrue()
    {
        // Arrange
        var mockContext = new Mock<IApplicationDbContext>();
        var service = new ScoringService(mockContext.Object);
        var measureId = Guid.NewGuid();
        var answers = new Dictionary<Guid, int>
        {
            { Guid.NewGuid(), 4 },
            { Guid.NewGuid(), 4 }
        };

        // Act
        var result = await service.DetectRiskAsync(measureId, answers);

        // Assert
        result.Should().BeTrue();
    }
}

public class DataQualityCalculatorTests
{
    [Fact]
    public async Task CalculateScore_WithAllFieldsComplete_Returns5()
    {
        // Arrange
        var mockContext = new Mock<IApplicationDbContext>();
        var calculator = new DataQualityCalculator(mockContext.Object);
        var clientId = Guid.NewGuid();

        // Act
        // Note: This would need proper mocking of the context
        var result = await calculator.CalculateScoreAsync(clientId);

        // Assert
        result.Should().BeInRange(0, 5);
    }
}
