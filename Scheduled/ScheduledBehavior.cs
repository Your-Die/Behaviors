namespace Chinchillada.Behavior
{
    using Sirenix.OdinInspector;
    using Sirenix.Serialization;

    public abstract class ScheduledBehavior : Behavior
    {
        [OdinSerialize, Required, FindComponent(SearchStrategy.InParent)]
        protected IBehaviorScheduler scheduler;

        protected IBehaviorScheduler Scheduler => this.scheduler;
        protected override BehaviorStatus TickInternal() => BehaviorStatus.Suspended;
    }
}