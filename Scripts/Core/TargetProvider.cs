namespace Defense
{
    using System.Collections.Generic;
    using System.Linq;

    public interface ITargetProvider
    {
        List<ITarget> Targets { get; }
    }

    /// <summary>
    /// Class that can provide desired set of targets to objects that are targetting
    /// </summary>
    public class TargetProvider : ITargetProvider
    {
        private readonly List<ITargetProvider> activeProviders;
        public List<ITarget> Targets => activeProviders.SelectMany(provider => provider.Targets).ToList()
            .FindAll(x => x.CanBeTargeted == true);

        public bool HasTargets => Targets.Count > 0;

        public TargetProvider(List<ITargetProvider> providers)
        {
            activeProviders = providers;
        }
    }
}
