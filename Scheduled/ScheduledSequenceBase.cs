namespace Chinchillada.Behavior
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Variant of <see cref="SequenceBehavior"/> that uses a <see cref="IBehaviorScheduler"/>.
    /// </summary>
    [Serializable]
    public class ScheduledSequence : ScheduledSequenceBase
    {
        protected override BehaviorStatus RequiredToContinue   => BehaviorStatus.Success;
        protected override BehaviorStatus ProcessedAllChildren => BehaviorStatus.Success;
    }

    /// <summary>
    /// Variant of <see cref="SelectorBehavior"/> that uses a <see cref="IBehaviorScheduler"/>.
    /// </summary>
    [Serializable]
    public class ScheduledSelector : ScheduledSequenceBase
    {
        protected override BehaviorStatus RequiredToContinue   => BehaviorStatus.Failure;
        protected override BehaviorStatus ProcessedAllChildren => BehaviorStatus.Failure;
    }

    /// <summary>
    /// Variant of <see cref="CustomSequenceBehavior"/> that uses a <see cref="IBehaviorScheduler"/>.
    /// </summary>
    [Serializable]
    public class CustomScheduledSequence : ScheduledSequenceBase
    {
        [SerializeField] private BehaviorStatus requiredToContinue;
        [SerializeField] private BehaviorStatus processedAllChildren;

        protected override BehaviorStatus RequiredToContinue => this.requiredToContinue;

        protected override BehaviorStatus ProcessedAllChildren => this.processedAllChildren;
    }
    
    /// <summary>
    /// Variant of <see cref="SequenceBehaviorBase"/> that uses a <see cref="IBehaviorScheduler"/>.
    /// </summary>
    public abstract class ScheduledSequenceBase : ScheduledComposite
    {
        protected int ChildIndex { get; private set; }

        private IBehavior CurrentChild => this.Children[this.ChildIndex];

        protected abstract BehaviorStatus RequiredToContinue   { get; }
        protected abstract BehaviorStatus ProcessedAllChildren { get; }

        protected override void Initialize()
        {
            this.ChildIndex = 0;

            this.ScheduleCurrentChild();
        }

        protected override void Terminate()
        {
            this.UnsubscribeFromCurrentChild();
            base.Terminate();
        }

        public override void Abort(BehaviorStatus status = BehaviorStatus.Aborted)
        {
            this.UnsubscribeFromCurrentChild();
            this.CurrentChild.Abort(status);

            base.Abort(status);
        }

        private void ScheduleCurrentChild() => this.Schedule(this.CurrentChild);

        private void UnsubscribeFromCurrentChild()
        {
            if (this.ChildIndex < this.Children.Count) 
                this.UnsubscribeFromChild(this.CurrentChild);
        }

        protected override void OnChildTerminated(IBehavior child, BehaviorStatus status)
        {
            base.OnChildTerminated(child, status);

            if (status != this.RequiredToContinue)
            {
                this.Abort(status);
            }
            else
            {
                this.ChildIndex++;

                if (this.ChildIndex >= this.Children.Count)
                {
                    this.Abort(this.ProcessedAllChildren);
                }
                else
                {
                    this.ScheduleCurrentChild();
                }
            }
        }
    }
}