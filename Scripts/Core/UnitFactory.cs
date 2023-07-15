namespace Defense
{
    using Zenject;

    public class UnitFactory : IFactory<UnityEngine.Object, Unit>
    {
        readonly DiContainer _container;

        public UnitFactory(DiContainer container)
        {
            _container = container;
        }

        public Unit Create(UnityEngine.Object prefab)
        {
            return _container.InstantiatePrefabForComponent<Unit>(prefab);
        }
    }
}