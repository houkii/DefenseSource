namespace Defense
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Interface for targets that can be hit
    /// </summary>
    public interface IHitable : ITarget
    {
        public float Health { get; }
        void Hit(ITargetter targetter);
    }

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
    /// Dummy target for movement or spawn position initialization.
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

    /// <summary>
    /// Data that is passed from hitting to damaged unit when damage is applied.
    /// </summary>
    public struct HitInfo
    {
        public IPlayer player;
        public ITargetter owner;
        public float damage;
    }

    /// <summary>
    /// Basic interface for targetter - entity that can acquire target.
    /// </summary>
    public interface ITargetter
    {
        ITargetter Parent { get; }
        ITarget Target { get; }
        void SetTarget(ITarget target);
        void SetParent(ITargetter targetter);
        void ResetTarget();
        HitInfo GetHitInfo();
    }
}