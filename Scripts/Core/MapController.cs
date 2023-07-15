namespace Defense
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// C;ass used for map initialization
    /// Spawns terrain grid.
    /// </summary>
    [ExecuteInEditMode]
    public class MapController : MonoBehaviour
    {
        public GameObject[] prefabs;                 // Array of prefabs to spawn
        [SerializeField] private Transform holder;
        [SerializeField] private float scaler = 4f;
        [SerializeField] private int rows;
        [SerializeField] private int columns;
        private Vector3 prefabSize;                  // Size of the prefabs
        private List<GameObject> gridElements;       // List to cache the grid elements

        private void OnDestroy()
        {
            ClearGridElements();
        }

        public void CreateGrid()
        {
            prefabSize = prefabs[0].GetComponent<MeshFilter>().sharedMesh.bounds.size;
            ClearGridElements();
            SpawnGrid();
        }

        private void SpawnGrid()
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    // Calculate the position for the prefab
                    Vector3 spawnPosition = new Vector3(col * prefabSize.x * scaler, 0f, row * prefabSize.z * scaler);

                    // Instantiate a random prefab from the array
                    GameObject randomPrefab = prefabs[Random.Range(0, prefabs.Length)];
                    GameObject spawnedPrefab = Instantiate(randomPrefab, spawnPosition, Quaternion.identity);
                    spawnedPrefab.transform.localScale *= scaler;

                    // Parent the spawned prefab to this script's GameObject
                    spawnedPrefab.transform.parent = holder;
                    spawnedPrefab.transform.localPosition = spawnPosition;

                    // Add the spawned prefab to the grid elements cache
                    gridElements.Add(spawnedPrefab);
                }
            }
        }

        private void ClearGridElements()
        {
            if (gridElements == null)
            {
                gridElements = new List<GameObject>();
            }

            foreach (GameObject gridElement in gridElements)
            {
                DestroyImmediate(gridElement);
            }
            gridElements.Clear();
        }
    }
}

