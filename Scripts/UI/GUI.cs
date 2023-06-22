namespace Defense
{
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class GUI : MonoBehaviour
    {
        [SerializeField] private EntityViews entityViews;
        public EntityViews EntityViews => entityViews;

        [SerializeField] private TextMeshProUGUI playersInfo;
        [SerializeField] private Button resetButton;

        private void Awake()
        {
            resetButton.onClick.AddListener(ResetLevel);
        }

        public void ShowPlayersInfo(List<Player> players)
        {
            string info = string.Empty;
            foreach(var player in players)
            {
                int numBuildings = player.Units.FindAll(x => x is Building).Count;
                int numOther = player.Units.Count - numBuildings;

                string playerName = string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGB(player.PlayerParams.color), player.PlayerParams.name);
                string playerInfo = string.Format("{0}: Buildings - {1}, Units - {2}", playerName, numBuildings, numOther);
                info += playerInfo + "\n";
            }

            playersInfo.text = info;

        }

        private void ResetLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}