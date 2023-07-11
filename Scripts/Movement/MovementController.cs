using UnityEngine;

namespace Defense
{
    /// <summary>
    /// Temporary axis based movement control for player
    /// </summary>
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotSpeed = 10f;

        private Vector3 moveDirection;

        private void Update()
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            moveDirection = new Vector3(horizontalInput, 0, verticalInput);
            float cameraYRot = Camera.main.transform.rotation.eulerAngles.y;
            Quaternion additionalRot = Quaternion.Euler(0, cameraYRot, 0);
            moveDirection = additionalRot * moveDirection;

            // Move the object based on the input and speed
            transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
            if (moveDirection.magnitude > .1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotSpeed);
            }
        }
    }
}