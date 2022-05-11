namespace Chinchillada.Behavior
{
    using System;
    using Sirenix.OdinInspector;
    using Sirenix.Serialization;

    /// <summary>
    /// Component wrapper for <see cref="IBehavior"/>.
    /// Uses a generic type argument so inheriting classes can specify what sub-type of behavior they wrap.
    /// </summary>
    public abstract class BehaviorComponent<TBehavior> : AutoRefBehaviour, IBehavior where TBehavior : IBehavior
    {
        [OdinSerialize, Required, FindNestedComponents]
        private TBehavior behavior;

        public TBehavior Behavior
        {
            get => this.behavior;
            set => this.behavior = value;
        }

        public BehaviorStatus CurrentStatus => this.Behavior.CurrentStatus;

        public event Action<IBehavior, BehaviorStatus> Terminated
        {
            add => this.behavior.Terminated += value;
            remove => this.behavior.Terminated -= value;
        }

        public BehaviorStatus Tick() => this.Behavior.Tick();
        public void Abort(BehaviorStatus status) => this.Behavior.Abort(status);
    }

    /// <summary>
    /// Component wrapper for all <see cref="IBehavior"/> adds inspector buttons for transforming the behavior tree
    /// into a component tree and back.
    /// </summary>
    public class BehaviorComponent : BehaviorComponent<IBehavior>
    {
        [Button]
        private void ExpandToComponentTree() => this.Behavior = ComponentTree.ToTree(this.transform, this.Behavior);

        [Button]
        private void FromComponentTree() => this.Behavior = ComponentTree.FromTree(this.Behavior);
    }
}