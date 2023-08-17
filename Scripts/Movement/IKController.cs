using UnityEngine;

namespace Defense
{
    /// <summary>
    /// test IK controller for unit groups
    /// </summary>
    public class IKController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float maxReach;
        private Transform limb;

        private void Awake()
        {
            limb = transform;
        }

        private void LateUpdate()
        {
            Vector3 limbToTarget = target.position - limb.position;
            float distance = limbToTarget.magnitude;

            float angle = Mathf.Atan2(limbToTarget.x, limbToTarget.z) * Mathf.Rad2Deg;

            if (distance <= maxReach)
            {
                limb.rotation = Quaternion.Lerp(limb.rotation, Quaternion.Euler(new Vector3(0f, angle, 0)), Time.deltaTime * 10f);
            }
            else
            {
                limb.rotation = Quaternion.Lerp(limb.rotation, Quaternion.Euler(new Vector3(0f, angle, 0)), Time.deltaTime * 10f);
                limb.position = Vector3.Lerp(limb.position, target.position - limbToTarget.normalized * maxReach, Time.deltaTime * speed);
            }
        }
    }
}