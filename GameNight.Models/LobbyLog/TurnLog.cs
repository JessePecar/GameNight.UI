namespace GameNight.Models.LobbyLog
{
    public class ChatLog
    {
        public string User { get; set; }
        public string TurnResult { get; set; }
        public bool IsPlayer { get; set; }
        public bool IsNotPlayer { get => !IsPlayer; }
    }
}
