namespace Chinchillada.Behavior
{
    using System;

    /// <summary>
    /// The sequence behavior attempts to process all children. If a child fails, the sequence fails.
    /// The sequence succeeds if each individual child succeeds.
    /// </summary>
    [Serializable]
    public class SequenceBehavior : SequenceBehaviorBase
    {
        protected override BehaviorStatus RequiredToContinue   => BehaviorStatus.Success;
        protected override BehaviorStatus ProcessedAllChildren => BehaviorStatus.Success;
    }
}