namespace Defense
{
    using DG.Tweening;
    using TMPro;
    using UnityEngine;

    /// <summary>
    /// Shows damage.
    /// </summary>
    public class HitIndicator : MonoBehaviour, IFreeAble
    {
        [SerializeField] private Vector3 offset = new Vector3(0, 60, 0);
        private TextMeshProUGUI info;
        private bool isFree = true;
        public bool IsFree() => isFree;

        private void Awake()
        {
            info = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void Show(Vector3 worldPosition, string info)
        {
            gameObject.SetActive(true);
            isFree = false;
            this.info.text = info;
            Vector3 startPosition = Utils.WorldPosToParentRectPos(worldPosition, transform.parent.GetComponent<RectTransform>());

            float angle = Random.Range(-30f, 30f);
            Vector3 targetPosition = startPosition + Quaternion.Euler(0f, 0f, angle) * offset;
            transform.localPosition = startPosition;

            transform.DOLocalMove(targetPosition, .5f)
                .SetEase(Ease.OutExpo)
                .OnComplete(() =>
                {
                    isFree = true;
                    gameObject.SetActive(false);
                });

        }
    }
}