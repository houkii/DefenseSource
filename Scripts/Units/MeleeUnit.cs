namespace Defense
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Base melee unit class
    /// </summary>
    public class MeleeUnit : TargettableUnit
    {
        [SerializeField] float hitPeriod = 1f;
        protected bool isFighting = false;
        private IMover mover;
        private Rigidbody rb;

        public override void ResetTarget()
        {
            if(Player != null)
                Target = Utils.GetTarget(Player.targetProvider.Targets, transform.position, 1000);
        }

        public override HitInfo GetHitInfo()
        {
            return new HitInfo { damage = 1f, owner = this, player = (IPlayer)Player };
        }

        protected override void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody>();
            CanBeTargeted = true;
            mover = gameObject.AddComponent<Mover>();
        }

        protected virtual void FixedUpdate()
        {
            // dont move when fighting - only rb related forces move obj
            // (which they shouldn't i think by the way - todo)
            if (!isFighting)
            {
                mover.Move(Target, Time.fixedDeltaTime);
            }
        }

        /// <summary>
        /// Hits other target when collides with it
        /// </summary>
        /// <param name="other"></param>
        private void OnCollisionStay(Collision other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                Fight(other.gameObject);
            }
        }

        private void Fight(GameObject obj)
        {
            var otherOwner = obj.GetComponent<Unit>().Player;
            if (otherOwner && Player.Enemies.Contains(otherOwner))
            {
                IHitable target = obj.GetComponentInParent<IHitable>();
                if (target != null && target == this.Target && !isFighting)
                {
                    StartCoroutine(DOFight(target));
                }
            }
        }

        private IEnumerator DOFight(IHitable target)
        {
            isFighting = true;
            target.Hit(this);
            yield return new WaitForSeconds(hitPeriod);
            yield return new WaitForEndOfFrame();
            isFighting = false;
        }
    }
}