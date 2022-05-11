namespace Chinchillada.Behavior
{
    using System;

    /// <summary>
    /// Behavior that returns Success on a true evaluation by the condition, and Failure on a false.
    /// </summary>
    [Serializable]
    public class ConditionBehavior : ConditionBehaviorBase
    {
        protected override BehaviorStatus TrueStatus  => BehaviorStatus.Success;
        protected override BehaviorStatus FalseStatus => BehaviorStatus.Failure;
    }
}