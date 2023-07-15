namespace Defense
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Interface for targets that can be hit
    /// </summary>
    public interface IHitable : ITarget
    {
        public event Action<HitInfo> OnTargetHit;
        public float Health { get; }
        void Hit(ITargetter targetter);
    }

    /// <summary>
    /// Base class for units that can be targetted.
    /// </summary>
    public abstract class TargettableUnit : TargettingUnit, IHitable
    {
        public event Action<HitInfo> OnTargetHit;
        public Action OnTargetExpired { get; set; }
        public bool CanBeTargeted { get; set; }
        public float Health => health;

        [SerializeField] private DestructionType destructionType = DestructionType.Destroy;
        [SerializeField] protected float startHealth;
        protected float health;

        protected override void Awake()
        {
            base.Awake();
            health = startHealth;
        }

        /// <summary>
        /// Basic hit method used mainly for combat.
        /// </summary>
        /// <param name="targetter"></param>
        public virtual void Hit(ITargetter targetter)
        {
            var hitInfo = targetter.GetHitInfo();

            health -= hitInfo.damage;
            OnTargetHit?.Invoke(hitInfo);

            if (health <= 0f && IsAlive)
            {
                CanBeTargeted = false;
                Kill(new DestructionData
                {
                    killer = hitInfo.player,
                    type = destructionType
                });
            }
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        protected override void Reset()
        {
            base.Reset();

            health = startHealth;
            CanBeTargeted = true;
        }
    }
}
