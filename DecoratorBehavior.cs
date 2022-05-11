namespace Chinchillada.Behavior
{
    using Sirenix.OdinInspector;
    using Sirenix.Serialization;

    /// <summary>
    /// Base class for behavior decorators. Decorators take another behavior and decorate how they're handled.
    /// </summary>
    public abstract class DecoratorBehavior : Behavior, IDecorator<IBehavior>
    {
        [OdinSerialize, Required, FindNestedComponents]
        private IBehavior behavior;

        public IBehavior Decoratee
        {
            get => this.behavior;
            set => this.behavior = value;
        }


        protected override BehaviorStatus TickInternal() => this.Tick(this.behavior);

        protected abstract BehaviorStatus Tick(IBehavior behavior);
    }

    public interface IDecorator<T>
    {
        T Decoratee { get; set; }
    }
}