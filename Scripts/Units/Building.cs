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
    }
}

