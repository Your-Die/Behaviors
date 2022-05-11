namespace Chinchillada.Behavior
{
    using Sirenix.OdinInspector;
    using Sirenix.Serialization;

    /// <summary>
    /// Scheduled composites are an alternative to the normal <see cref="CompositeBehavior"/>
    /// that use a <see cref="BehaviorScheduler"/> as an intermediary for running their child behaviors.
    /// </summary>
    public abstract class ScheduledComposite : CompositeBehavior
    {
        [OdinSerialize, Required, FindComponent(SearchStrategy.InParent)]
        private IBehaviorScheduler scheduler;

        protected IBehaviorScheduler Scheduler => this.scheduler;

        protected override BehaviorStatus TickInternal() => BehaviorStatus.Suspended;

        protected void Schedule(IBehavior behavior)
        {
            this.SubscribeToChild(behavior);
            this.scheduler.ScheduleFirst(behavior);
        }

        protected void AbortChild(IBehavior child, BehaviorStatus status = BehaviorStatus.Aborted)
        {
            this.UnsubscribeFromChild(child);
            child.Abort(status);
        }

        protected void SubscribeToChild(IBehavior child) => child.Terminated += this.OnChildTerminated;
        protected void UnsubscribeFromChild(IBehavior child) => child.Terminated -= this.OnChildTerminated;

        protected virtual void OnChildTerminated(IBehavior child, BehaviorStatus status)
        {
            this.UnsubscribeFromChild(child);
        }
    }
}