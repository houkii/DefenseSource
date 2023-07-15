namespace Defense
{
    /// <summary>
    /// Base building class.
    /// </summary>
    public class Building : TargettableUnit
    {
        protected override void Awake()
        {
            base.Awake();
            resetTargetEachFrame = false;
        }

        public override HitInfo GetHitInfo()
        {
            return new HitInfo { 
                damage = 0, 
                owner = this, 
                player = (IPlayer)Player 
            };
        }
    }
}

