namespace Chinchillada.Behavior
{
    public interface IBehaviorScheduler : IBehavior
    {
        void ScheduleFirst(IBehavior behavior);
        void Unschedule(IBehavior    behavior);
    }
}