namespace Chinchillada.Behavior
{
    using System;

    public enum BehaviorStatus
    {
        /// <summary>
        /// Status for when something went wrong.
        /// </summary>
        Invalid,
        
        /// <summary>
        /// Status for when a behavior is currently in progress.
        /// </summary>
        Running,
        
        /// <summary>
        /// Status for when a behavior terminated successfully.
        /// </summary>
        Success,
        
        /// <summary>
        /// Status for when a behavior terminated in failure.
        /// </summary>
        Failure,
        
        /// <summary>
        /// Status for when a behavior is aborted before it could normally finish.
        /// </summary>
        Aborted,
        
        /// <summary>
        /// Status for when a behavior is suspended, which removes it from being updated by
        /// a <see cref="IBehaviorScheduler"/>.
        /// </summary>
        Suspended
    }

    public static class BehaviorStatusExtensions
    {
        public static bool IsActive(this BehaviorStatus status)
        {
            return !status.IsTerminated();
        }
        
        public static bool IsTerminated(this BehaviorStatus status)
        {
            switch (status)
            {
                case BehaviorStatus.Running:
                case BehaviorStatus.Suspended:
                    return false;

                case BehaviorStatus.Invalid:
                case BehaviorStatus.Success:
                case BehaviorStatus.Failure:
                case BehaviorStatus.Aborted:
                    return true;

                default: throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}