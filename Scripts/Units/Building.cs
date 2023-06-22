namespace Defense
{
    /// <summary>
    /// Base building class.
    /// </summary>
    public class Building : TargettableUnit
    {
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

