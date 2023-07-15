namespace Defense
{
    using System.Collections.Generic;
    using UnityEngine;
    using Zenject;

    [System.Serializable]
    public struct UnitEntry
    {
        public UnitType type;
        public GameObject prefab;
    }

    /// <summary>
    /// Unit spawner utility class.
    /// </summary>
    public class UnitSpawner : MonoBehaviour
    {
        public static UnitSpawner Instance;

        [SerializeField] private List<UnitEntry> units;
        [Inject] private Unit.Factory unitFactory;
        [Inject] private EntityViews entityViews;
        [Inject] private HitIndicators hitIndicators;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public Unit Spawn(UnitType unitType, Player player)
        {
            var unit = Spawn(unitType);
            unit.Player = player;
            player.AddUnit(unit);
            return unit;
        }

        public Unit Spawn(UnitType unitType)
        {
            var prefab = units.Find(x => x.type == unitType).prefab;
            var unit = unitFactory.Create(prefab);

            if (unit is TargettableUnit)
            {
                var targettableUnit = (unit as TargettableUnit);

                // Add health indicator.
                entityViews.AddView(unit.gameObject, () => targettableUnit.Health);

                // Spawn hit info.
                targettableUnit.OnTargetHit += (hitInfo) => hitIndicators.AddView(
                    targettableUnit.transform.position, 
                    $"-{Mathf.RoundToInt(hitInfo.damage * 10)}"
                );
            }

            return unit;
        }
    }
}

