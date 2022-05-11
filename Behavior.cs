namespace Chinchillada.Behavior
{
    using System;

    /// <summary>
    /// Base class for behaviors. Calls <see cref="Initialize"/> on the first <see cref="Tick"/> on inactive behaviors,
    /// and <see cref="Terminate"/> on the tick that the behavior finishes,
    /// by either returning <see cref="BehaviorStatus.Success"/> or <see cref="BehaviorStatus.Failure"/>.
    /// </summary>
    public abstract class Behavior : IBehavior
    {
        private bool IsActive => this.CurrentStatus.IsActive();

        public BehaviorStatus CurrentStatus { get; private set; }

        public event Action<IBehavior, BehaviorStatus> Terminated;

        public BehaviorStatus Tick()
        {
            if (!this.IsActive)
                this.Initialize();

            this.CurrentStatus = this.TickInternal();

            if (!this.IsActive)
                this.Terminate();

            return this.CurrentStatus;
        }

        public virtual void Abort(BehaviorStatus status = BehaviorStatus.Aborted)
        {
            this.CurrentStatus = status;
            this.Terminate();
        }

        /// <summary>
        /// Inheriting classes implement should implement their behavior here.
        /// </summary>
        /// <returns></returns>
        protected abstract BehaviorStatus TickInternal();

        protected virtual void Initialize()
        {
        }

        protected virtual void Terminate() => this.Terminated?.Invoke(this, this.CurrentStatus);
    }
}