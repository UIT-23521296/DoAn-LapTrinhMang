using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyWinForms.GameLogic
{
    public class Card
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string EffectType { get; set; }
        public int Value { get; set; }
        public Card(int id, string desc, string effect, int value = 0)
        {
            Id = id;
            Description = desc;
            EffectType = effect;
            Value = value;
        }
    }
}
