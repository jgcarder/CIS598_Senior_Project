using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CIS598_Senior_Project.FleetObjects.DefenseTokenObjects
{
    public class RedirectDefenseToken : DefenseToken
    {
        private DefenseTokenStateEnum _state;

        public override int Id { get; }

        public override string Name { get { return "Redirect"; } }

        public override string Text
        {
            get
            {
                return "The defender chooses one of his hull zones adjacent to the " +
                       "defending hull zone. When the defender suffers damage, it " +
                       "may suffer any amount of damage on the chosen zone’s remaining " +
                       "shields before it must suffer the remaining damage on the defending " +
                       "hull zone.";
            }
        }

        public override Texture2D Texture { get; } //To be assigned

        public override Rectangle Source { get; } //To be assigned

        public override DefenseTokenStateEnum State { get { return _state; } set { _state = value; } }

        public RedirectDefenseToken(int id)
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
