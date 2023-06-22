namespace Defense
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(MapController))]
    public class MapControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MapController mapController = (MapController)target;

            if (GUILayout.Button("Spawn Grid"))
            {
                mapController.CreateGrid();
            }
        }
    }
}

