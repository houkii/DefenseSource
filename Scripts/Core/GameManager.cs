namespace Defense
{
    using System.Collections.Generic;
    using UnityEngine;
    using Zenject;

    /// <summary>
    /// Players' info storage and initialization
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<Player> players;
        [Inject] private GUI gui;

        private void Awake()
        {
            InitializePlayers();
        }

        private void Update()
        {
            gui.ShowPlayersInfo(players);
        }

        /// <summary>
        /// Initialize player, set each player as enemy to the other
        /// </summary>
        private void InitializePlayers()
        {
            for(int i=0;i<players.Count;i++)
            {
                var player = players[i];
                var enemies = players.FindAll(x => x != player);
                player.Initialize(enemies);
            }
        }
    }
}
