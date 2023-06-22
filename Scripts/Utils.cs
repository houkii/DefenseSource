namespace Defense
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public static class Utils
    {
        public static ITarget GetTarget(List<ITarget> possibleTargets, Vector3 position, float range)
        {
            ITarget target = possibleTargets.OrderBy(target => Vector3.Distance(target.GetPosition(), position)).FirstOrDefault();
            return (target != null && Vector3.Distance(target.GetPosition(), position) < range) ? target : new NullTarget();
        }

        public static Vector3 WorldPosToParentRectPos(Vector3 worldPos, RectTransform parentRect)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            Vector2 canvasPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenPos, null, out canvasPos);
            return canvasPos;
        }

        public static GameObject SpawnFX(GameObject prefab, Vector3 pos, float lifetime)
        {
            var fxObj = GameObject.Instantiate(prefab, pos, Quaternion.identity);
            GameObject.Destroy(fxObj, lifetime);
            return fxObj;
        }
    }
}