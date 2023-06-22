using UnityEngine;

namespace Defense
{
    public class DrawObjectVectors : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private bool drawLocalVectors;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            DrawVector(target.right);

            Gizmos.color = Color.green;
            DrawVector(target.up);

            Gizmos.color = Color.blue;
            DrawVector(target.forward);

            if (drawLocalVectors)
            {
                Gizmos.matrix = target.localToWorldMatrix;

                Gizmos.color = Color.red;
                DrawVector(Vector3.right);

                Gizmos.color = Color.green;
                DrawVector(Vector3.up);

                Gizmos.color = Color.blue;
                DrawVector(Vector3.forward);
            }
        }

        private void DrawVector(Vector3 vector)
        {
            Gizmos.DrawRay(target.position, vector);
        }
    }
}