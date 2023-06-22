namespace Defense
{
    /// <summary>
    /// Base class for units that can acquire target.
    /// </summary>
    public abstract class TargettingUnit : Unit, ITargetter
    {
        public ITargetter Parent { get; set; }
        public ITarget Target { get; protected set; } = new NullTarget();

        protected virtual void Awake()
        {
            ResetTarget();
        }

        public virtual void SetParent(ITargetter targetter)
        {
            this.Parent = targetter;
        }

        public virtual void SetTarget(ITarget target)
        {
            this.Target = target;
        }

        public virtual void ResetTarget()
        {
            Target = new NullTarget();
        }

        public virtual HitInfo GetHitInfo()
        {
            return new HitInfo { damage = 0, owner = this, player = (IPlayer)Player };
        }
    }
}
