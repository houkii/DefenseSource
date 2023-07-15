namespace Defense
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Controller for object that stores info of units that should be indicated.
    /// Passes units' data to floor indicator shader.
    /// </summary>
    public class Floor : MonoBehaviour
    {
        [SerializeField] private float indicatorScaler = 1.4f;
        private RangedUnit[] objectsWithRange;
        private Material material;
        private List<IndicatorEntry> indicators;
        private int previousFrameIndicatorsCount = 0;
        private int currentFrameIndicatorsCount;

        private void Awake()
        {
            material = GetComponent<Renderer>().material;
        }

        /// <summary>
        /// Adds indicator position and size to the floor shader based on unit's parameters.
        /// </summary>
        /// <param name="unit"></param>
        public void AddIndicator(Unit unit)
        {
            if (unit is RangedUnit)
            {
                var rangedUnit = (unit as RangedUnit);
                AddIndicator(new IndicatorEntry(rangedUnit.transform, rangedUnit.Range));
            }
            else if (!(unit is Projectile))
            {
                var size = unit.transform.localScale;
                var unitSize = Mathf.Max(size.x, size.y) * Mathf.Max(unit.transform.localScale.x, unit.transform.localScale.z);
                AddIndicator(new IndicatorEntry(unit.transform, indicatorScaler * unitSize));
            }
        }

        public void RemoveIndicator(Unit unit)
        {
            var entry = indicators.Find(x => x.transform == unit.transform);
            if (entry != null)
            {
                indicators.Remove(entry);
            }
        }

        private void AddIndicator(IndicatorEntry indicator)
        {
            if (indicators == null)
            {
                indicators = new List<IndicatorEntry>();
            }

            indicators.Add(indicator);
        }

        private void Update()
        {
            if (indicators.Count <= 0) return;

            SetMaterialParams();
        }

        private void SetMaterialParams()
        {
            //currentFrameIndicatorsCount = indicators.Count >= previousFrameIndicatorsCount
            //    ? indicators.Count
            //    : previousFrameIndicatorsCount;

            currentFrameIndicatorsCount = 128;

            Vector4[] positions = new Vector4[currentFrameIndicatorsCount];
            float[] ranges = new float[currentFrameIndicatorsCount];
            for (int i = 0; i < currentFrameIndicatorsCount; i++)
            {
                positions[i] = new Vector4(0, 0, 0, 0);
                if (i < indicators.Count)
                {
                    Vector3 localPosition = transform.InverseTransformPoint(indicators[i].transform.position);
                    positions[i] = localPosition;
                    positions[i].w = 1;
                    ranges[i] = indicators[i].range * 1f / transform.localScale.x;
                }
                else
                {
                    positions[i].w = 0;
                    ranges[i] = 0;
                }
            }
            previousFrameIndicatorsCount = indicators.Count;
            material.SetFloatArray("_Ranges", ranges);
            material.SetVectorArray("_Positions", positions);
        }

        /// <summary>
        /// Class for indicator entry data storage
        /// </summary>
        public class IndicatorEntry
        {
            public Transform transform;
            public float range;

            public IndicatorEntry(Transform transform, float range)
            {
                this.transform = transform;
                this.range = range;
            }
        }
    }
}