namespace Defense
{
    using UnityEngine;

    /// <summary>
    /// Basic camera controller.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float dragSpeed = 2f;           // Speed of camera movement
        [SerializeField] private float zoomSpeed = 2f;           // Speed of camera zoom
        [SerializeField] private float minZoomDistance = -20f;   // Minimum zoom distance
        [SerializeField] private float maxZoomDistance = 20f;    // Maximum zoom distance
        [SerializeField] private float rotationSpeed = 2f;       // Speed of camera rotation
        private Vector3 dragOrigin;                              // Initial position of the ray intersection
        private Vector3 defaultPos;
        private Vector3 pivotPoint;                              // Pivot point for camera rotation
        private float currentZoomAmt = 0f;

        private void Awake()
        {
            defaultPos = transform.position;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetDragOrigin();
            }

            if (Input.GetMouseButton(0))
            {
                Drag();
            }

            // Check for scroll wheel input for zooming
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scrollInput) > 0f)
            {
                Zoom(scrollInput);
            }

            // Check for middle mouse button being held down
            if (Input.GetMouseButton(2))
            {
                Rotate();
            }
        }

        private void Rotate()
        {
            // Find the pivot point using a raycast from the camera's forward direction
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                pivotPoint = hit.point;
                //transform.LookAt(pivotPoint);
            }

            // Rotate the camera around the pivot point based on mouse movement
            float rotationInput = Input.GetAxis("Mouse X") * rotationSpeed;
            transform.RotateAround(pivotPoint, Vector3.up, rotationInput);
        }

        private void Zoom(float scrollInput)
        {
            var amt = scrollInput * zoomSpeed;
            var newZoomAmt = currentZoomAmt + amt;

            // Apply zoom in desired ranges only
            if (newZoomAmt > minZoomDistance && newZoomAmt < maxZoomDistance)
            {
                currentZoomAmt = newZoomAmt;
                transform.position += (transform.forward * scrollInput * zoomSpeed);
            }
        }

        private void Drag()
        {
            // Cast a ray from the camera to the world
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray intersects with any object
            if (Physics.Raycast(ray, out hit))
            {
                // Calculate the delta movement based on the difference between current and drag origin positions
                Vector3 move = dragOrigin - hit.point;

                // Apply the movement to the camera's position, only in the X and Z axes
                transform.Translate(new Vector3(move.x, 0f, move.z) * dragSpeed * Time.deltaTime, Space.World);
            }
        }

        private void SetDragOrigin()
        {
            // Cast a ray from the camera to the world
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray intersects with any object
            if (Physics.Raycast(ray, out hit))
            {
                // Set the drag origin to the intersection point
                dragOrigin = hit.point;
            }
        }
    }
}
