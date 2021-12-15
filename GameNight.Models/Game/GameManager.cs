using System;

namespace GameNight.Models.Game
{
    public class GameManager
    {
        public Guid AdminKey { get; set; }
        public string LobbyKey { get; set; }
        public GameType GameType { get; set; }
    }
}
