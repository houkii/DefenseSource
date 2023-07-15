
namespace Defense
{
    using UnityEngine;

    public interface IMover
    {
        void Move(ITarget target, float delta);
    }

    /// <summary>
    /// Class that handles movement of an object.
    /// </summary>
    public class Mover : MonoBehaviour, IMover
    {
        [SerializeField] private float speed = 5f;
        protected IMovementStrategy movementStrategy;
        public ITarget Target { get; private set; }

        protected virtual void Awake()
        {
            movementStrategy = new BasicGroundMovement(transform);
        }

        public void SetTarget(ITarget target)
        {
            Target = target;
        }

        public virtual void Move(ITarget target, float delta)
        {
            movementStrategy.Move(target.GetPosition(), delta, speed);
        }
    }
}