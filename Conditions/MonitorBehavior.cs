namespace Chinchillada.Behavior
{
    using System;

    /// <summary>
    /// Keeps the behavior running while the condition is true. Fails when the condition is false.
    ///
    /// Can be used to make the equivalent of while-loops in the behavior tree.
    /// </summary>
    [Serializable]
    public class MonitorBehavior : ConditionBehaviorBase
    {
        protected override BehaviorStatus TrueStatus  => BehaviorStatus.Running;
        protected override BehaviorStatus FalseStatus => BehaviorStatus.Failure;
    }
}