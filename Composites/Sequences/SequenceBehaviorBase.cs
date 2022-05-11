namespace Chinchillada.Behavior
{
    /// <summary>
    /// Base-class for sequence composite behaviors.
    /// Sequence behaviors iterate through each of their children in turn.
    ///
    /// If the child returns the <see cref="RequiredToContinue"/>, we continue on to the next child, otherwise we stop.
    ///
    /// If we process all the children, we return the <see cref="ProcessedAllChildren"/> status.
    ///
    /// What those statuses are is left to inheriting classes, as can vary into drastically different behaviors. 
    /// </summary>
    public abstract class SequenceBehaviorBase : CompositeBehavior
    {
        /// <summary>
        /// What child are we currently processing.
        /// </summary>
        protected int ChildIndex { get; private set; }

        /// <summary>
        /// The status a child needs to return for the sequence to continue to the next child.
        /// </summary>
        protected abstract BehaviorStatus RequiredToContinue { get; }

        /// <summary>
        /// The status we return if all children have been processed.
        /// </summary>
        protected abstract BehaviorStatus ProcessedAllChildren { get; }

        protected override void Initialize()
        {
            this.ResetChildIndex();
        }

        protected void ResetChildIndex() => this.ChildIndex = 0;

        public override void Abort(BehaviorStatus status = BehaviorStatus.Aborted)
        {
            var currentChild = this.Children[this.ChildIndex];
            currentChild.Abort(status);

            base.Abort(status);
        }

        protected override BehaviorStatus TickInternal()
        {
            for (; this.ChildIndex < this.Children.Count; this.ChildIndex++)
            {
                var currentChild = this.Children[this.ChildIndex];
                var childStatus  = currentChild.Tick();

                if (childStatus != this.RequiredToContinue)
                    return childStatus;
            }

            return this.ProcessedAllChildren;
        }
    }
}