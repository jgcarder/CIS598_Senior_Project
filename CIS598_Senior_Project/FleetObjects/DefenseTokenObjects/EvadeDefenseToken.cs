using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CIS598_Senior_Project.FleetObjects.DefenseTokenObjects
{
    public class EvadeDefenseToken : DefenseToken
    {
        private DefenseTokenStateEnum _state;

        public override int Id { get; }

        public override string Name { get { return "Evade"; } }

        public override string Text
        {
            get
            {
                return "At long range, the defender cancels one " +
                       "attack die of its choice. At medium and " +
                       "close range, the defender chooses one " +
                       "attack die to be rerolled.";
            }
        }

        public override Texture2D Texture { get; } //To be assigned

        public override Rectangle Source { get; } //To be assigned

        public override DefenseTokenStateEnum State { get { return _state; } }

        public EvadeDefenseToken(int id)
        {
            _state = DefenseTokenStateEnum.Ready;
        }

        public override void Use()
        {
            if (_state == DefenseTokenStateEnum.Ready) _state = DefenseTokenStateEnum.Exhausted;
            else if (_state == DefenseTokenStateEnum.Exhausted) _state = DefenseTokenStateEnum.Discarded;
        }

        public override void Reset()
        {
            if (_state == DefenseTokenStateEnum.Exhausted) _state = DefenseTokenStateEnum.Ready;
        }
    }
}
