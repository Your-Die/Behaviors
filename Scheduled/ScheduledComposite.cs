namespace Chinchillada.Behavior
{
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using Sirenix.Serialization;

    /// <summary>
    /// Scheduled composites are an alternative to the normal <see cref="CompositeBehavior"/>
    /// that use a <see cref="BehaviorScheduler"/> as an intermediary for running their child behaviors.
    /// </summary>
    public abstract class ScheduledComposite : ScheduledBehavior, IComposite<IBehavior>
    {
        [OdinSerialize, Required, FindNestedComponents]
        private List<IBehavior> children = new List<IBehavior>();

        public List<IBehavior> Children => this.children;

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

        protected void SubscribeToChild(IBehavior     child) => child.Terminated += this.OnChildTerminated;
        protected void UnsubscribeFromChild(IBehavior child) => child.Terminated -= this.OnChildTerminated;

        protected virtual void OnChildTerminated(IBehavior child, BehaviorStatus status)
        {
            this.UnsubscribeFromChild(child);
        }
    }
}