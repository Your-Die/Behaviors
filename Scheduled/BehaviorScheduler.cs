namespace Chinchillada.Behavior
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sirenix.OdinInspector;
    using Sirenix.Serialization;

    /// <summary>
    /// Schedules behavior into a FIFO LinkedList. This eliminates the need for a full tree traversal every frame.
    /// Types of <see cref="ScheduledComposite"/> schedule their children through the scheduler and
    /// listen to events from their children to know when to proceed or finish.
    /// </summary>
    /// <remarks>
    /// Uses a <see cref="LinkedList{T}"/> rather than a <see cref="Queue{T}"/> because we need to be able to remove
    /// items from anywhere in the list, in case a behavior gets aborted.
    /// </remarks>
    [Serializable]
    public class BehaviorScheduler : Behavior, IBehaviorScheduler
    {
        [OdinSerialize, Required, FindNestedComponents]
        private IBehavior root;

        private VoidBehavior guard = new VoidBehavior();

        private LinkedList<IBehavior> behaviors = new LinkedList<IBehavior>();
        
        protected override void Initialize()
        {
            this.guard     = new VoidBehavior();
            this.behaviors = new LinkedList<IBehavior>();

            this.behaviors.AddFirst(this.root);
        }

        public override void Abort(BehaviorStatus status = BehaviorStatus.Aborted)
        {
            foreach (var behavior in this.behaviors)
            {
                behavior.Abort(status);
            }
            
            this.behaviors.Clear();
            base.Abort(status);
        }

        public void ScheduleFirst(IBehavior behavior)
        {
            this.SubscribeToTermination(behavior);
            this.behaviors.AddFirst(behavior);
        }
        
        public void Unschedule(IBehavior behavior)
        {
            this.UnsubscribeFromTermination(behavior);
            this.behaviors.Remove(behavior);
        }

        protected override BehaviorStatus TickInternal()
        {
            this.behaviors.AddLast(this.guard);

            while (this.Step())
            {
            }

            return this.behaviors.Any()
                ? BehaviorStatus.Running
                : BehaviorStatus.Success;
        }

        private bool Step()
        {
            var behavior = this.behaviors.ExtractFirst();
            
            // The guard marks the end of this frame.
            if (behavior == this.guard)
                return false;

            var result = behavior.Tick();
            
            // add the behavior to the back of the queue for next frame if it's still running.
            if (result == BehaviorStatus.Running) 
                this.behaviors.AddLast(behavior);
            else
            {
                this.UnsubscribeFromTermination(behavior);
            }

            return true;
        }

        private void SubscribeToTermination(IBehavior behavior)
        {
            behavior.Terminated += this.OnScheduledBehaviorTerminated;
        }

        private void UnsubscribeFromTermination(IBehavior behavior)
        {
            behavior.Terminated -= this.OnScheduledBehaviorTerminated;
        }
        
        private void OnScheduledBehaviorTerminated(IBehavior behavior, BehaviorStatus _)
        {
            this.Unschedule(behavior);
        }
    }

    public static class LinkedListExtensions
    {
        public static T ExtractFirst<T>(this LinkedList<T> list)
        {
            var first = list.First.Value;
            
            list.RemoveFirst();

            return first;
        }
    }
}