using System.Collections.Generic;

namespace GameNight.Models.Dice
{
    public class DiceResult
    {
        public DiceType Type { get; set; }
        public List<int> Rolls { get; set; }
        public int RollCount { get; set; }
    }
}
