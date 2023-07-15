namespace Defense
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public interface IPlayer
    {
        void AddUnit(Unit unit);
        void RemoveUnit(Unit unit);
    }

    [System.Serializable]
    public struct PlayerParams
    {
        public string name;
        public Color color;
    }

    /// <summary>
    /// Base player class. Manages owned units and sets their interactions.
    /// </summary>
    public class Player : MonoBehaviour, IPlayer, ITargetProvider
    {
        public event Action<Unit> OnUnitAdded;
        public event Action<Unit> OnUnitRemoved;

        public TargetProvider targetProvider { get; private set; }
        public List<ITarget> Targets => units.OfType<IHitable>().Cast<ITarget>().ToList();

        [SerializeField] private PlayerParams playerParams;
        public PlayerParams PlayerParams => playerParams;

        [SerializeField] private List<Player> enemies;
        public List<Player> Enemies => enemies;

        [SerializeField] private List<Unit> units;
        public List<Unit> Units => units;

        [SerializeField] private Floor floor;
        private Queue<Unit> unitsToDestroy = new Queue<Unit>();

        /// <summary>
        /// Sets up enemies, existing units and initializes target provider
        /// </summary>
        /// <param name="enemies"></param>
        public void Initialize(List<Player> enemies)
        {
            this.enemies = enemies;
            units.ForEach(AddUnit);
            targetProvider = new TargetProvider(enemies.Select(enemy => enemy as ITargetProvider).ToList());
        }

        public void AddUnit(Unit unit)
        {
            unit.Player = this;
            unit.OnUnitDestroyed += RemoveUnit;

            if (!units.Contains(unit))
            {
                units.Add(unit);
            }

            floor.AddIndicator(unit);
            OnUnitAdded?.Invoke(unit);
        }

        public void RemoveUnit(Unit unit)
        {
            if (units.Contains(unit))
            {
                units.Remove(unit);
            }

            if (unit is ITarget)
            {
                enemies.ForEach(x => x.ResetTargettingUnits());
            }

            floor.RemoveIndicator(unit);
            OnUnitRemoved?.Invoke(unit);

            if(!unit.IsAlive)
            {
                unitsToDestroy.Enqueue(unit);
            }

            if(units.Count == 0)
            {
                floor.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Resets targets for all units that can acquire target
        /// </summary>
        private void ResetTargettingUnits()
        {
            var targettingUnits = units.OfType<ITargetter>().ToList();
            targettingUnits.ForEach(x => x.ResetTarget());
        }

        private void LateUpdate()
        {
            // Destroy units at the end of frame
            while (unitsToDestroy.Count > 0)
            {
                Destroy(unitsToDestroy.Dequeue().gameObject);
            }
        }
    }
}