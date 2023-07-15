namespace Defense
{
    using UnityEngine;

    /// <summary>
    /// Controls character unit animations
    /// </summary>
    public class Character : MeleeUnit
    {
        private Animator animator;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponentInChildren<Animator>();
        }

        protected override void Update()
        {
            base.Update();
            animator.SetBool("attacking", this.isFighting);
        }
    }

}
