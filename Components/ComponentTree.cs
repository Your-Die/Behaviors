namespace Chinchillada.Behavior
{
    using UnityEngine;

    /// <summary>
    /// Component trees are a way to visualize the structure of a <see cref="IBehavior"/> tree using Unity's
    /// hierarchy view. Each individual behavior in the tree gets wrapped in a <see cref="BehaviorComponent"/>.
    ///
    /// The intend here is mainly for visualization during editing. Component trees can also be reverted back into
    /// just plain-old-c# classes, so at runtime we don't have to deal with a collection of GameObjects and components
    /// that are only there for show.
    /// </summary>
    public static class ComponentTree
    {
        /// <summary>
        /// Take the behavior tree with the <paramref name="behavior"/> as the root and transform it into a
        /// component tree by wrapping each behavior into a <see cref="BehaviorComponent"/>, each on it's own gameObject.
        /// The resulting tree is nested under the <paramref name="parent"/>.
        /// </summary>
        /// <returns>
        /// The root of the component tree.
        /// </returns>
        public static BehaviorComponent ToTree(Transform parent, IBehavior behavior)
        {
            var behaviorType = behavior.GetType();
            var gameObject = new GameObject(behaviorType.Name);

            var component = gameObject.AddComponent<BehaviorComponent>();
            component.Behavior = behavior;
                
            var transform = gameObject.transform;
            transform.parent = parent;

            // If this behavior has any children, we recursively operate on each of the children.
            switch (behavior)
            {
                case IComposite<IBehavior> composite:
                    BuildComposite(transform, composite);
                    break;
                case IDecorator<IBehavior> decorator:
                    ToTree(transform, decorator.Decoratee);
                    break;
            }

            return component;
        }

        /// <summary>
        /// Attempts to extract the wrapped <see cref="IBehavior"/> from <see cref="BehaviorComponent"/> in the tree
        /// starting at the <paramref name="behavior"/>.
        /// </summary>
        public static IBehavior FromTree(IBehavior behavior)
        {
            if (behavior is BehaviorComponent component)
                behavior = component.Behavior;
            else
                component = null;

            switch (behavior)
            {
                case IComposite<IBehavior> composite:
                {
                    for (var i = 0; i < composite.Children.Count; i++)
                    {
                        var child = composite.Children[i];
                        composite.Children[i] = FromTree(child);
                    }

                    break;
                }
                case IDecorator<IBehavior> decorator:
                    decorator.Decoratee = FromTree(decorator.Decoratee);
                    break;
            }

            if (component != null) 
                Object.DestroyImmediate(component.gameObject);
            
            return behavior;
        }

        /// <summary>
        /// Make component trees out of each of the <paramref name="composite"/>'s children,
        /// and nest them under the <paramref name="parent"/>.
        /// </summary>
        private static void BuildComposite(Transform parent, IComposite<IBehavior> composite)
        {
            for (var index = 0; index < composite.Children.Count; index++)
            {
                var child = composite.Children[index];

                composite.Children[index] = ToTree(parent, child);
            }
        }
    }
}