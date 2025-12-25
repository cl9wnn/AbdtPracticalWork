namespace PracticalWork.Reports.SharedKernel.Events;

public interface IEventTypeRegistry
{
    Type GetEventType(string eventType);
}