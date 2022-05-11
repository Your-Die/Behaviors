namespace Chinchillada.Behavior
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Variant of <see cref="ParallelBehavior"/> that uses a <see cref="IBehaviorScheduler"/>.
    /// </summary>
    [Serializable]
    public class ScheduledParallel : ScheduledComposite
    {
        [SerializeField] private ParallelPolicy successPolicy;
        [SerializeField] private ParallelPolicy failurePolicy;

        private int successCount;
        private int failureCount;

        private HashSet<IBehavior> finishedChildren = new HashSet<IBehavior>();

        protected override void Initialize()
        {
            this.successCount = 0;
            this.failureCount = 0;
            
            this.finishedChildren.Clear();

            for (var index = this.Children.Count - 1; index >= 0; index--)
            {
                var child = this.Children[index];
                this.Schedule(child);
            }
        }

        public override void Abort(BehaviorStatus status = BehaviorStatus.Aborted)
        {
            this.AbortActiveChildren(status);
            base.Abort(status);
        }

        protected override void OnChildTerminated(IBehavior child, BehaviorStatus status)
        {
            this.UnsubscribeFromChild(child);
            this.finishedChildren.Add(child);

            if (status == BehaviorStatus.Success)
                this.successCount++;
            else if (status == BehaviorStatus.Failure)
                this.failureCount++;

            if (EvaluatePolicy(this.successCount, this.successPolicy))
                this.Abort(BehaviorStatus.Success);
            else if (EvaluatePolicy(this.failureCount, this.failurePolicy)) 
                this.Abort(BehaviorStatus.Failure);
            
            bool EvaluatePolicy(int matchingCount, ParallelPolicy policy)
            {
                switch (policy)
                {
                    case ParallelPolicy.RequireOne: return matchingCount >= 1;
                    case ParallelPolicy.RequireAll: return matchingCount >= this.Children.Count;

                    default: throw new ArgumentOutOfRangeException(nameof(policy), policy, null);
                }
            }
        }

        private void AbortActiveChildren(BehaviorStatus status)
        {
            var activeChildren = this.Children.Except(this.finishedChildren).ToList();

            foreach (var child in activeChildren)
                this.AbortChild(child, status);
        }
    }
}