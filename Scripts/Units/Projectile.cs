using UnityEngine;

namespace Defense
{
    /// <summary>
    /// Basic projectile class.
    /// Moves directly to target with constant speed.
    /// </summary>
    public class Projectile : ProjectileBase
    {
        protected Rigidbody rb;
        [SerializeField] protected float speed = 15f;
        [SerializeField] protected Vector2 damageRange = new Vector2(.7f, 1.4f);

        protected override void Awake()
        {
            base.Awake();
            rb = transform.GetComponent<Rigidbody>();
        }

        public override HitInfo GetHitInfo()
        {
            return new HitInfo 
            { 
                owner = Parent, 
                damage = Random.Range(damageRange.x, damageRange.y), 
                player = Player 
            };
        }

        public override void ResetTarget()
        {
            SetTarget(new NullTarget());
        }

        protected override void Update()
        {
            if (Target.IsRealTarget)
            {
                transform.position += (Target.GetPosition() - transform.position).normalized * speed * Time.deltaTime;
            }
            else
            {
                Kill(new DestructionData
                {
                    killer = Player,
                    type = DestructionType.Destroy
                });
            }
        }
    }
}