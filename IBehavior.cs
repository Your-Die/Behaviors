namespace Chinchillada.Behavior
{
    using System;

    public interface IBehavior
    {
        BehaviorStatus CurrentStatus { get; }

        event Action<IBehavior, BehaviorStatus> Terminated;

        BehaviorStatus Tick();

        void Abort(BehaviorStatus status = BehaviorStatus.Aborted);
    }
}