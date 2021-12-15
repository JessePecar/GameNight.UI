using System;
using System.Collections.Generic;
using System.Text;

namespace GameNight.Models.LobbyLog
{
    public class TurnLog
    {
        public string User { get; set; }
        public string TurnResult { get; set; }
        public bool IsPlayer { get; set; }
        public bool IsNotPlayer { get => !IsPlayer; }
    }
}
