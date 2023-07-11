namespace Defense
{
    using System;
    using UnityEngine;

    public class EntityViews : PooledViewBase<EntityView>
    {
        public void AddView(GameObject obj, Func<float> hpGetter)
        {
            var view = GetView();
            view.AttachToObject(obj, hpGetter);
        }
    }
}