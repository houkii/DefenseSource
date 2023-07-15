using UnityEngine;

namespace Defense
{
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

    /// <summary>
    /// Base class for units that can acquire target.
    /// </summary>
    public abstract class TargettingUnit : Unit, ITargetter
    {
        public ITargetter Parent { get; set; }
        public ITarget Target { get; protected set; } = new NullTarget();

        // Should depend on currently used controller (ai, player, unit type) on targetting object.
        [SerializeField] protected bool resetTargetEachFrame;

        protected virtual void Awake()
        {
            ResetTarget();
        }

        public virtual void SetParent(ITargetter targetter)
        {
            Parent = targetter;
        }

        public virtual void SetTarget(ITarget target)
        {
            Target = target;
        }

        public virtual void ResetTarget()
        {
            if (Player != null)
                Target = Utils.GetTarget(Player.targetProvider.Targets, transform.position, 1000);
        }

        public virtual HitInfo GetHitInfo()
        {
            return new HitInfo 
            { 
                damage = 0,
                owner = this,
                player = (IPlayer)Player
            };
        }

        protected virtual void Update()
        {
            if(resetTargetEachFrame)
            {
                ResetTarget();
            }
        }
    }
}
