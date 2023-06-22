namespace Defense
{
    using UnityEngine;

    /// <summary>
    /// Base projectile class.
    /// </summary>
    public abstract class ProjectileBase : TargettingUnit
    {
        public ITargetProvider TargetProvider { get; set; }

        /// <summary>
        /// When target is reached:
        /// 1. Kill projectile
        /// 2. Apply calculated damage
        /// </summary>
        protected void OnTriggerEnter(Collider other)
        {
            var otherOwner = other.gameObject.GetComponent<Unit>().Player;
            if (otherOwner && Player.Enemies.Contains(otherOwner))
            {
                IHitable enemyTarget = other.gameObject.GetComponent<IHitable>();
                if (enemyTarget != null)
                {
                    Kill(new DestructionData
                    {
                        killer = (IPlayer)Player,
                        type = DestructionType.Destroy
                    });

                    enemyTarget.Hit(this);
                }
            }
        }
    }
}
