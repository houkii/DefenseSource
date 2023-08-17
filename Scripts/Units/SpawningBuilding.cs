namespace Defense
{
    using DG.Tweening;
    using System.Collections;
    using UnityEngine;
    using Zenject;

    /// <summary>
    /// Class controller for buildings that spawn desired unit type
    /// </summary>
    public class SpawningBuilding : Building
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private float tick = 2;
        [SerializeField] private int maxUnits = 5;
        [SerializeField] private int numPerTick = 1;
        [SerializeField] private int numUnits = 0;
        [SerializeField] private UnitType unitType;

        [Inject] private EntityViews entityViews;
        [Inject] private HitIndicators hitIndicators;

        private void Start()
        {
            StartCoroutine(DOSpawn());
            entityViews.AddView(gameObject, () => Health);
            OnTargetHit += (hitInfo) => hitIndicators.AddView(transform.position, $"-{Mathf.RoundToInt(hitInfo.damage * 10)}");
            CanBeTargeted = true;

            OnTargetHit += (info) =>
            {
                transform.DOShakeRotation(.2f, new Vector3(0, 6f, 0));
            };
        }

        /// <summary>
        /// Spawns chosen unit every tick time with a cap set by num units.
        /// </summary>
        private IEnumerator DOSpawn()
        {
            while (true)
            {
                yield return new WaitForSeconds(tick);
                for (int i = 0; i < numPerTick; i++)
                {
                    Spawn();
                }
                yield return new WaitUntil(() => numUnits < maxUnits);
            }
        }

        private void Spawn()
        {
            Vector3 position = transform.position + Quaternion.Euler(0, UnityEngine.Random.Range(0f, 45f), 0) * transform.forward * transform.localScale.x;
            var unit = UnitSpawner.Instance.Spawn(unitType);

            unit.transform.position = position;
            unit.transform.rotation = Quaternion.identity;

            InitializeUnit(unit);
        }

        /// <summary>
        /// Initializes unit by adding it to player, and setting initial target.
        /// </summary>
        /// <param name="unit"></param>
        private void InitializeUnit(Unit unit)
        {
            var unitCtrl = unit.gameObject.GetComponent<Unit>();
            numUnits++;
            unitCtrl.OnUnitDestroyed += unit => numUnits--;
            this.Player.AddUnit(unitCtrl);

            if (unit is TargettingUnit)
            {
                (unitCtrl as TargettingUnit).ResetTarget();
            }
        }
    }
}