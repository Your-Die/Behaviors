namespace Chinchillada.Behavior
{
    using Sirenix.OdinInspector;
    using Sirenix.Serialization;

    /// <summary>
    /// Evaluates a boolean <see cref="ICondition"/>.
    /// Inheriting classes define what <see cref="BehaviorStatus"/> to return on both true and false.
    /// </summary>
    public abstract class ConditionBehaviorBase : Behavior
    {
        [OdinSerialize, FindNestedComponents, Required]
        private ICondition condition;

        protected abstract BehaviorStatus TrueStatus  { get; }
        protected abstract BehaviorStatus FalseStatus { get; }
        
        protected override BehaviorStatus TickInternal()
        {
            return this.condition.Validate()
                ? this.TrueStatus
                : this.FalseStatus;
        }
    }
}