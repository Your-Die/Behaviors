namespace Chinchillada.Behavior
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// The Parallel executes each of it's children in parallel.
    /// When the Parallel is considered to be finished is defined through
    /// the <see cref="successPolicy"/> and <see cref="failurePolicy"/>.
    /// </summary>
    [Serializable]
    public class ParallelBehavior : CompositeBehavior
    {
        /// <summary>
        /// Determines how many children need to succeed before we terminate in success.
        /// </summary>
        [SerializeField] private ParallelPolicy successPolicy;
        
        /// <summary>
        /// Determines how many children need to fail before we terminate in failure.
        /// </summary>
        [SerializeField] private ParallelPolicy failurePolicy;

        /// <summary>
        /// Amount of children that have terminated with <see cref="BehaviorStatus.Success"/> so far.
        /// </summary>
        private int successCount;
        
        /// <summary>
        /// Amount of children that have terminated with <see cref="BehaviorStatus.Failure"/> so far.
        /// </summary>
        private int failureCount;

        /// <summary>
        /// collection of all children that have finished so far.
        /// </summary>
        private HashSet<IBehavior> finishedChildren;

        protected override void Initialize()
        {
            this.successCount = 0;
            this.failureCount = 0;

            this.finishedChildren = new HashSet<IBehavior>();
        }

        public override void Abort(BehaviorStatus status = BehaviorStatus.Aborted)
        {
            this.AbortActiveChildren(status);
            base.Abort(status);
        }

        protected override BehaviorStatus TickInternal()
        {
            foreach (var child in this.Children)
            {
                if (this.finishedChildren.Contains(child))
                    continue;

                var childStatus = child.Tick();

                if (childStatus == BehaviorStatus.Success)
                {
                    if (this.successPolicy == ParallelPolicy.RequireOne)
                        return BehaviorStatus.Success;

                    this.finishedChildren.Add(child);
                }
                else if (childStatus == BehaviorStatus.Failure)
                {
                    this.failureCount++;

                    if (this.failurePolicy == ParallelPolicy.RequireOne)
                        return BehaviorStatus.Failure;
                    
                    this.finishedChildren.Add(child);
                }
            }

            if (this.successPolicy == ParallelPolicy.RequireAll && this.successCount == this.Children.Count)
                return BehaviorStatus.Success;

            if (this.failurePolicy == ParallelPolicy.RequireAll && this.failureCount == this.Children.Count)
                return BehaviorStatus.Failure;

            return BehaviorStatus.Running;
        }
        
        private void AbortActiveChildren(BehaviorStatus status)
        {
            var activeChildren = this.Children.Except(this.finishedChildren).ToArray();

            foreach (var child in activeChildren) 
                child.Abort(status);
        }
    }

    public enum ParallelPolicy
    {
        RequireOne,
        RequireAll
    }
}