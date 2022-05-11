namespace Chinchillada.Behavior
{
    using System;

    /// <summary>
    /// Variant of <see cref="ActiveSelectorBehavior"/> that uses a <see cref="IBehaviorScheduler"/>.
    /// </summary>
    [Serializable]
    public class ScheduledActiveSelector : ScheduledSelector
    {
        private int previousIndex;

        private Guard guard;

        public override void Abort(BehaviorStatus status = BehaviorStatus.Aborted)
        {
            this.AbortGuard();
            base.Abort(status);
        }
        
        protected override void Initialize()
        {
            this.guard = new Guard();
        }

        protected override void Terminate()
        {
            this.previousIndex = 0;
            base.Terminate();
        }

        protected override BehaviorStatus TickInternal()
        {
            this.previousIndex = this.ChildIndex;

            if (this.previousIndex > 0)
            {
                var previousChild = this.Children[this.previousIndex];
                this.Scheduler.Unschedule(previousChild);
            }

            this.ScheduleGuard();
            base.Initialize();

            return BehaviorStatus.Running;
        }
        
        private void ScheduleGuard()
        {
            this.guard.Ticked += this.OnGuardReached;
            this.Scheduler.ScheduleFirst(this.guard);
        }
        
        private void AbortGuard()
        {
            this.guard.Ticked -= this.OnGuardReached;
            this.Scheduler.Unschedule(this.guard);
        }

        private void OnGuardReached()
        {
            this.guard.Ticked -= this.OnGuardReached;

            if (this.ChildIndex >= this.previousIndex || this.previousIndex == this.Children.Count)
                return;

            var previousChild = this.Children[this.previousIndex];
            previousChild.Abort();
        }

        private class Guard : Behavior
        {
            public event Action Ticked;

            protected override BehaviorStatus TickInternal()
            {
                this.Ticked?.Invoke();
                return BehaviorStatus.Success;
            }
        }
    }
}