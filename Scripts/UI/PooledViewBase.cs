namespace Defense
{
    using System.Collections.Generic;
    using UnityEngine;

    public interface IFreeAble
    {
        bool IsFree();
    }

    /// <summary>
    /// Base class used for pooled views' instantiation.
    /// </summary>
    public abstract class PooledViewBase<T> : MonoBehaviour where T : MonoBehaviour, IFreeAble
    {
        [SerializeField] protected GameObject prefab;
        protected List<T> views;

        private void Awake()
        {
            views = new List<T>();
        }

        protected T GetView()
        {
            T view = views.Find(x => x.IsFree());
            if (!view)
            {
                GameObject viewObj = Instantiate(prefab, transform);
                view = viewObj.GetComponent<T>();
                views.Add(view);
            }
            return view;
        }
    }
}
