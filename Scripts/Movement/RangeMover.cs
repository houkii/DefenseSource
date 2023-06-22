using UnityEngine;

namespace Defense
{
    /// <summary>
    /// Class for movement control for ranged units (with double range)
    /// 1. search range - target acquirement
    /// 2. turret range - shooting
    /// </summary>
    [RequireComponent(typeof(RangedUnit))]
    public class RangeMover : Mover
    {
        private RangedUnit turret;

        protected override void Awake()
        {
            base.Awake();
            turret = GetComponent<RangedUnit>();
            SetTarget(turret.Target);

        }

        private void FixedUpdate()
        {
            if (turret.Target.IsRealTarget)
            {
                var distanceToTarget = Vector3.Distance(transform.position, turret.Target.GetPosition());
                if (distanceToTarget < turret.SearchRange && distanceToTarget > turret.Range)
                {
                    Move(turret.Target, Time.fixedDeltaTime);
                }
            }
        }
    }
}