namespace Chinchillada.Behavior
{
    using System;

    /// <summary>
    /// Null-object <see cref="Behavior"/>.
    /// </summary>
    public class VoidBehavior : IBehavior
    {
        public BehaviorStatus CurrentStatus => BehaviorStatus.Invalid;

        public event Action<IBehavior, BehaviorStatus> Terminated
        {
            add { }
            remove { }
        }

        public virtual BehaviorStatus Tick() => this.CurrentStatus;

        public void Abort(BehaviorStatus status = BehaviorStatus.Aborted)
        {
        }
    }
}