namespace Chinchillada.Behavior
{
    /// <summary>
    /// Component wrapper of <see cref="IBehaviorScheduler"/>.
    /// </summary>
    public class BehaviorSchedulerComponent : BehaviorComponent<IBehaviorScheduler>, IBehaviorScheduler
    {
        public void ScheduleFirst(IBehavior behavior) => this.Behavior.ScheduleFirst(behavior);

        public void Unschedule(IBehavior behavior) => this.Behavior.Unschedule(behavior);
    }
}