namespace Defense
{
    using UnityEngine;

    public class HitIndicators : PooledViewBase<HitIndicator>
    {
        public void AddView(Vector3 worldPosition, string info)
        {
            var view = GetView();
            view.Show(worldPosition, info);
        }
    }
}