namespace Defense
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Basic interface for any target
    /// </summary>
    public interface ITarget
    {
        Action OnTargetExpired { get; set; }
        Vector3 GetPosition();
        bool CanBeTargeted { get; set; }
        bool IsRealTarget => !(this is NullTarget);

    }

    /// <summary>
    /// Dummy target used for movement or spawn position initialization.
    /// </summary>
    public class NullTarget : ITarget
    {
        public Action OnTargetExpired { get; set; }
        public bool CanBeTargeted { get; set; }

        public Vector3 GetPosition()
        {
            return Vector3.zero;
        }
    }
}