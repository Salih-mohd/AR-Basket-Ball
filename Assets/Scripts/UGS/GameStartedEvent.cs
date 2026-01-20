using Unity.Services.Analytics;

public class GameStartedEvent : Event
{
    public GameStartedEvent() : base("GameStarted")
    {
    }
}

public class GameEndedEvent : Event
{
    public GameEndedEvent() : base("GameEnded")
    {
    }
}
