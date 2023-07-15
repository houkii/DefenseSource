namespace Defense
{
    using UnityEngine;

    public interface IMovementStrategy
    {
        void Move(Vector3 targetPos, float delta, float speed);
    }

    /// <summary>
    /// Can be set to freeze unit movement
    /// </summary>
    public class NoMovement : IMovementStrategy
    {
        public void Move(Vector3 targetPos, float delta, float speed)
        {

        }
    }

    /// <summary>
    /// Basic movement strategy for units moving on ground
    /// </summary>
    public class BasicGroundMovement : IMovementStrategy
    {
        private Transform transform;
        private Vector3 direction;
        private Vector3 adjustedTargetPos;

        public BasicGroundMovement(Transform transform)
        {
            this.transform = transform;
        }

        public void Move(Vector3 targetPos, float delta, float speed)
        {
            adjustedTargetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            direction = (adjustedTargetPos - transform.position).normalized;
            direction = Vector3.Scale(direction, new Vector3(1f, 0f, 1f));
            transform.position += (direction.normalized * speed * delta);
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}