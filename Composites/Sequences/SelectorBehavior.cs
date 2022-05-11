namespace Chinchillada.Behavior
{
    using System;

    /// <summary>
    /// The selector attempts to process it's children until it finds a child that doesn't fail.
    /// The selector fails if all children fail.
    ///
    /// As a result, the selector succeeds the first time a child succeeds.
    /// </summary>
    [Serializable]
    public class SelectorBehavior : SequenceBehaviorBase
    {
        protected override BehaviorStatus RequiredToContinue   => BehaviorStatus.Failure;
        protected override BehaviorStatus ProcessedAllChildren => BehaviorStatus.Failure;
    }
}