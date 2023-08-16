namespace Defense
{
    using UnityEngine;
    using Zenject;

    /// <summary>
    /// Scene installer:
    /// 1. Bindings for ui / ui elements
    /// 2. UnitFactory initialization
    /// </summary>
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private GameObject guiPrefab;
        public override void InstallBindings()
        {
            Container.Bind<GUI>().FromComponentInNewPrefab(guiPrefab)
                .WithGameObjectName("GUI")
                .AsSingle()
                .NonLazy();

            Container.Bind<EntityViews>().FromComponentsInHierarchy()
                .AsSingle()
                .NonLazy();

            Container.Bind<HitIndicators>().FromComponentsInHierarchy()
                .AsSingle()
                .NonLazy();

            Container.BindFactory<UnityEngine.Object, Unit, Unit.Factory>()
                .FromFactory<UnitFactory>();
        }
    }
}