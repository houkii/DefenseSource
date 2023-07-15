namespace Defense
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Shows health.
    /// </summary>
    public class EntityView : MonoBehaviour, IFreeAble
    {
        public bool IsFree() => followObject == null;

        [SerializeField] private Slider slider;
        [SerializeField] private GameObject followObject;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float lerpSpeed = 10f;
        private Func<float> valueGetter;
        private float initialValue;

        private void Start()
        {
            slider.value = 1;
        }

        private void Update()
        {
            if (followObject == null)
            {
                DetachFromObject();
                return;
            }

            transform.localPosition = Utils.WorldPosToParentRectPos(followObject.transform.position + offset, transform.parent.GetComponent<RectTransform>());

            // Call the action to update the slider value.
            if (valueGetter != null)
            {
                slider.value = Mathf.Lerp(slider.value, valueGetter() / initialValue, Time.deltaTime * lerpSpeed);
            }
        }

        // Attaches the slider to a follow object and takes an action to update the slider value.
        public void AttachToObject(GameObject obj, Func<float> valueGetter)
        {
            followObject = obj;
            offset = new Vector3(0, obj.transform.localScale.y * 2f, 0);
            this.valueGetter = valueGetter;
            initialValue = valueGetter();
            gameObject.SetActive(true);
        }

        // Detaches the slider from any follow object.
        public void DetachFromObject()
        {
            followObject = null;
            valueGetter = null;
            gameObject.SetActive(false);
        }
    }
}