namespace Shared.IntegrationEvents;

public record ClientCreatedIntegrationEvent(
    Guid ClientId,
    string Name,
    string Document,
    string Email,
    double Renda,
    int Score
);