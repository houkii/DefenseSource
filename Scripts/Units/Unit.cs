namespace Defense
{
    using System;
    using UnityEngine;
    using Zenject;

    public interface IUnit
    {
        Player Player { get; set; }
        IUnit Owner { get; set; }
    }

    /// <summary>
    /// Base unit class.
    /// </summary>
    public abstract class Unit : MonoBehaviour, IUnit
    {
        [SerializeField] private GameObject deathFX;
        [SerializeField] private GameObject captureFx;
        public event Action<Unit> OnUnitDestroyed;
        public Player Player { get; set; }
        public IUnit Owner { get; set; }
        public bool IsAlive { get; private set; } = true;

        /// <summary>
        /// Handle unit when it's hp is below zero.
        /// </summary>
        /// <param name="destructionData"></param>
        protected virtual void Kill(DestructionData destructionData)
        {
            if(destructionData.type == DestructionType.Destroy)
            {
                Destroy();
            }
            else
            {
                Capture(destructionData.killer);
            }
        }

        /// <summary>
        /// Change ownership of the unit - killer becomes the owner.
        /// </summary>
        /// <param name="newOwner"></param>
        private void Capture(IPlayer newOwner)
        {
            if (captureFx != null)
            {
                Utils.SpawnFX(captureFx, transform.position, 5.0f);
            }

            Player.RemoveUnit(this);
            newOwner.AddUnit(this);
            this.Reset();
        }

        /// <summary>
        /// Remove and destroy unit.
        /// </summary>
        private void Destroy()
        {
            if (deathFX != null)
            {
                Utils.SpawnFX(deathFX, transform.position, 5.0f);
            }

            IsAlive = false;
            OnUnitDestroyed?.Invoke(this);
        }

        protected virtual void Reset()
        {

        }

        public class Factory : PlaceholderFactory<UnityEngine.Object, Unit>
        {
        }
    }

    public struct DestructionData
    {
        public IPlayer killer;
        public DestructionType type;
    }

    public enum DestructionType
    {
        Destroy,
        Capture
    }

    public enum UnitType
    {
        WalkingUnit,
        Building,
        Turret,
        MovingTurret,
        Alien1,
        Soldier1,
        Robot1,
        SpaceShip1,
        SpaceShip2,
        ArmoredSoldier
    }
}