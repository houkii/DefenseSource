namespace Defense
{
    using System.Collections;
    using UnityEngine;

    public enum TurretState
    {
        Idle,
        Active,
        Inactive,
        TargetLocked
    }

    public enum WeaponState
    {
        Reloading,
        Charging,
        Ready
    }

    /// <summary>
    /// Base class for units with ranged weapons.
    /// </summary>
    public class RangedUnit : TargettableUnit
    {
        public float Range => range;
        public float SearchRange => searchRange;
        public bool HasTarget() => Target.IsRealTarget;
        public TurretState state = TurretState.Idle;
        public WeaponState weaponState = WeaponState.Ready;
        public ITargetProvider targetProvider;
        public uint ammoCount = 10;
        public float reloadTime = .5f;

        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameObject tower;
        [SerializeField] private GameObject barrel;
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private ParticleSystem firePS;
        [SerializeField] private float range = 100f;
        [SerializeField] private float searchRange = 100f;
        [SerializeField] private float rotationSpeed = 10f;
        private Coroutine shootCoroutine;

        protected override void Awake()
        {
            base.Awake();
            CanBeTargeted = true;
            this.Parent = this;
            shootCoroutine = StartCoroutine(DOShoot());
        }

        public override void ResetTarget()
        {
            if(shootCoroutine != null)
            {
                StopCoroutine(shootCoroutine);
            }

            Target = new NullTarget();
            shootCoroutine = StartCoroutine(DOShoot());
        }

        protected virtual void Update()
        {
            if (state == TurretState.Idle)
            {
                Target = Utils.GetTarget(Player.targetProvider.Targets, transform.position, SearchRange);
            }

            if (HasTarget())
            {
                Aim();
            }
        }

        /// <summary>
        /// Rotates transform and barrel to match the target rotation
        /// </summary>
        private void Aim()
        {
            Vector3 direction = GetDirection(Target.GetPosition());

            // Get y rotations - for transform only
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            Quaternion targetTowerRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);

            // Get x rotations - for barrel only
            float angle = targetRotation.eulerAngles.x;
            if (angle > 180f)
            {
                angle -= 360f;
            }
            angle = Mathf.Clamp(angle, -15f, 15f);
            Quaternion targetBarrelRotation = Quaternion.Euler(angle, 0, 0);

            // Apply rotations
            tower.transform.rotation = Quaternion.Lerp(
                tower.transform.rotation,
                targetTowerRotation,
                Time.deltaTime * rotationSpeed
            );

            barrel.transform.localRotation = Quaternion.Lerp(
                barrel.transform.localRotation,
                targetBarrelRotation,
                Time.deltaTime * rotationSpeed
            );

            // Lock target when rotation matches target rotation
            var dotProduct = Quaternion.Dot(tower.transform.rotation.normalized, targetTowerRotation.normalized);
            if (Mathf.Abs(dotProduct) > .999f && Vector3.Distance(transform.position, Target.GetPosition()) < Range)
            {
                state = TurretState.TargetLocked;
            }
        }

        /// <summary>
        /// Base shooting method - instantiates projectile and attaches it to owner player as a unit.
        /// </summary>
        /// <param name="target"></param>
        private void Shoot(ITarget target)
        {
            GameObject projectileObj = Instantiate(
                projectilePrefab,
                projectileSpawnPoint.position,
                Quaternion.FromToRotation(Vector3.up, (GetDirection(target.GetPosition())))
            );

            var projectile = projectileObj.GetComponent<Projectile>();
            projectile.SetTarget(target);
            projectile.Owner = this;
            projectile.Parent = this;
            projectile.Player = Player;
            projectile.TargetProvider = this.targetProvider;
            Player.AddUnit(projectile);
            ammoCount--;
            ShowFirePS();
            StartCoroutine(DOReload());
        }

        /// <summary>
        /// Shoot coroutine that manages shooting cycle
        /// </summary>
        private IEnumerator DOShoot()
        {
            state = TurretState.Active;
            while (state != TurretState.Inactive)
            {
                state = TurretState.Idle;
                yield return new WaitUntil(() => HasTarget() && ammoCount > 0);
                state = TurretState.Active;
                yield return new WaitUntil(() => state == TurretState.TargetLocked && weaponState == WeaponState.Ready);
                Shoot(Target);
                Target = new NullTarget();
            }
        }

        /// <summary>
        /// Reload coroutine that manages weaponstate.
        /// </summary>
        /// <returns></returns>
        private IEnumerator DOReload()
        {
            weaponState = WeaponState.Reloading;

            float recoilTime = Mathf.Clamp(reloadTime / 2f, 0, .5f);
            var defaultPos = barrel.transform.localPosition.z;

            //Sequence barrelSequence = DOTween.Sequence()
            //    .Append(barrel.transform.DOLocalMoveZ(defaultPos - .005f, recoilTime).SetEase(Ease.OutExpo))
            //    .Append(barrel.transform.DOLocalMoveZ(defaultPos, recoilTime).SetEase(Ease.InExpo));
            //;

            yield return new WaitForSeconds(reloadTime);
            weaponState = WeaponState.Ready;
        }

        /// <summary>
        /// Triggers FX when weapon is used.
        /// </summary>
        private void ShowFirePS()
        {
            // Configure the emission module to emit a burst of particles
            ParticleSystem.EmissionModule emissionModule = firePS.emission;
            emissionModule.enabled = true;
            firePS.Emit(30);
        }

        private Vector3 GetDirection(Vector3 position) => (position - transform.position).normalized;
    }
}