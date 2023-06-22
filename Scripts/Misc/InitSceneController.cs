namespace Pool.Startup
{
    using DG.Tweening;
    using TMPro;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    /// <summary>
    /// Game startup script, shows logo
    /// </summary>
    public class InitSceneController : MonoBehaviour
    {
        [SerializeField] private string nextSceneName = "Pool";
        [SerializeField] private RectTransform logo;
        [SerializeField] private TextMeshProUGUI text;

        private void Awake()
        {
            logo.localScale = Vector3.zero;
            var textRt = text.GetComponent<RectTransform>();
            Vector3 textPos = textRt.anchoredPosition;
            textRt.anchoredPosition = new Vector2(-textRt.sizeDelta.x, textRt.anchoredPosition.y);
            logo.rotation = Quaternion.Euler(0, 0, 160);
            text.alpha = 0f;

            Sequence initSceneSequence = DOTween.Sequence()
                .AppendInterval(.5f)
                //.Append(logo.DOAnchorPos(Vector2.zero, .5f).SetEase(Ease.OutElastic))
                .Append(logo.DOScale(1, .5f).SetEase(Ease.OutBack))
                .Join(logo.DORotateQuaternion(Quaternion.identity, .5f).SetEase(Ease.OutExpo))
                .AppendInterval(.5f)
                .Append(textRt.DOAnchorPos(textPos, .5f).SetEase(Ease.OutBack))
                .Join(text.DOFade(1f, .5f).SetEase(Ease.InExpo))
                .AppendInterval(2f)
                .Append(logo.GetComponent<Image>().DOFade(0f, .3f).SetEase(Ease.OutExpo))
                .Join(text.DOFade(0f, .3f).SetEase(Ease.OutExpo))
                .OnComplete(LoadNextScene);
        }

        private void LoadNextScene()
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
