
namespace Defense
{
    using UnityEngine;

    /// <summary>
    /// Homing missile controller.
    /// Interpolates rotation to acquired target, can retarget.
    /// </summary>
    public class HomingMissile : Projectile
    {
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float retargetRange = 8;
        [SerializeField] private float noTargetTime = 5f;   // Time projectile will live without a target
        private Vector3 currentDirection;


        private float noTargetTimeElapsed = 0f;

        protected override void Awake()
        {
            base.Awake();
            currentDirection = transform.up;
        }

        public override void ResetTarget()
        {
            if(Player != null)
            {
                SetTarget(Utils.GetTarget(Player.targetProvider.Targets, transform.position, retargetRange));
            }
        }

        private void Update()
        {
            if (Target.IsRealTarget)
            {
                Vector3 direction = (Target.GetPosition() - transform.position).normalized;
                currentDirection = Vector3.Lerp(currentDirection, direction, Time.deltaTime * rotationSpeed).normalized;
                transform.position += currentDirection * speed * Time.deltaTime;
                transform.rotation = Quaternion.FromToRotation(Vector3.up, currentDirection);
            }
            else
            {
                noTargetTimeElapsed += Time.deltaTime;

                // Destroy projectile if its no target time has expired.
                if(noTargetTimeElapsed > noTargetTime)
                {
                    Kill(new DestructionData { 
                        killer = Player,
                        type = DestructionType.Destroy 
                    });
                }

                transform.position += currentDirection * speed * Time.deltaTime;
                ResetTarget();
            }
        }
    }
}