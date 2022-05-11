namespace Chinchillada.Behavior
{
    using System;

    /// <summary>
    /// The active selector acts like a <see cref="SelectorBehavior"/>, except that it "restarts" every frame.
    /// If in a frame, the selector progresses less far than the frame before,
    /// the behavior that was active at the end of the previous frame gets aborted.
    /// </summary>
    [Serializable]
    public class ActiveSelectorBehavior : SelectorBehavior
    {
        protected override void Terminate()
        {
            this.ResetChildIndex();
            base.Terminate();
        }

        protected override BehaviorStatus TickInternal()
        {
            var previous = this.ChildIndex;
            
            this.Initialize();
            var result = base.TickInternal();

            this.AbortIfOverwritten(previous, this.ChildIndex);

            return result;
        }

        private void AbortIfOverwritten(int previousIndex, int currentIndex)
        {
            if (currentIndex >= previousIndex || previousIndex == this.Children.Count)
                return;
            
            var previousChild = this.Children[previousIndex];
            previousChild.Abort();
        }
    }
}