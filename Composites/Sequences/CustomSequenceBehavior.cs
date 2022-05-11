namespace Chinchillada.Behavior
{
    using System;
    using UnityEngine;

    /// <summary>
    /// The CustomSequenceBehavior allows for the <see cref="RequiredToContinue"/> and <see cref="ProcessedAllChildren"/>
    /// to be defined through the editor.
    ///
    /// The <see cref="SequenceBehavior"/> and <see cref="SelectorBehavior"/> are used commonly,
    /// so they've been predefined explicitly.
    /// </summary>
    [Serializable]
    public class CustomSequenceBehavior : SequenceBehaviorBase
    {
        [SerializeField] private BehaviorStatus requiredToContinue;
        [SerializeField] private BehaviorStatus processedAllChildren;

        protected override BehaviorStatus RequiredToContinue => this.requiredToContinue;

        protected override BehaviorStatus ProcessedAllChildren => this.processedAllChildren;
    }
}