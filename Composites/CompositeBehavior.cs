namespace Chinchillada.Behavior
{
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using Sirenix.Serialization;

    /// <summary>
    /// Base class for Composite behaviors. Composite behaviors have children.
    /// </summary>
    public abstract class CompositeBehavior : Behavior, IComposite<IBehavior>
    {
        [OdinSerialize, Required, FindNestedComponents]
        private List<IBehavior> children = new List<IBehavior>();

        public List<IBehavior> Children => this.children;
    }
}