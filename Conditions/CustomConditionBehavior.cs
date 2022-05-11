namespace Chinchillada.Behavior
{
    using System;
    using UnityEngine;

    [Serializable]
    public class CustomConditionBehavior : ConditionBehaviorBase
    {
        [SerializeField] private BehaviorStatus trueStatus = BehaviorStatus.Success;
        [SerializeField] private BehaviorStatus falseStatus = BehaviorStatus.Failure;

        protected override BehaviorStatus TrueStatus  => this.trueStatus;
        protected override BehaviorStatus FalseStatus => this.falseStatus;
    }
}