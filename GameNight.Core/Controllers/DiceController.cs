using GameNight.Core.Http.Interfaces;
using GameNight.Models.Dice;
using System.Threading.Tasks;

namespace GameNight.Core.Controllers
{
    public class DiceController : BaseController
    {
        public DiceController(IGameHttpClient client) : base(client)
        { }

        public async Task<DiceResult> DndRoll(int numberOfDice, DiceType diceType = DiceType.D20)
        {
            DiceResult result = await Client.GetAsync<DiceResult>(
                "Dice", 
                nameof(DndRoll), 
                GetParams(
                    new { Key = nameof(numberOfDice), Value = numberOfDice }, 
                    new { Key = nameof(diceType), Value = ((int)diceType).ToString() }));

            return result;
        }
    }
}
