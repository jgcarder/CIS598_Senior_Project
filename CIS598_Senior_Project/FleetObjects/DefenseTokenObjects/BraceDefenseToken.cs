/* File: BraceDefenceToken.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CIS598_Senior_Project.FleetObjects.DefenseTokenObjects
{
    /// <summary>
    /// The class for the brace defence token
    /// </summary>
    public class BraceDefenseToken : DefenseToken
    {
        private DefenseTokenStateEnum _state;

        /// <summary>
        /// Token's id
        /// </summary>
        public override int Id { get; }

        /// <summary>
        /// Token's name
        /// </summary>
        public override string Name { get { return "Brace"; } }

        /// <summary>
        /// The text about the token
        /// </summary>
        public override string Text
        {
            get
            {
                return "When damage is totaled during the 'Resolve Damage'" +
                       " step, the total is reduced to half, rounded up.";
            }
        }
        
        /// <summary>
        /// The texture the token uses
        /// </summary>
        public override Texture2D Texture { get; } //To be assigned

        /// <summary>
        /// The potential source for the image
        /// </summary>
        public override Rectangle Source { get; } //To be assigned

        /// <summary>
        /// The state of the token, ready, exhausted, depleted
        /// </summary>
        public override DefenseTokenStateEnum State { get { return _state; } set { _state = value; } }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="id">The id of the token</param>
        public BraceDefenseToken(int id)
        {
            _state = DefenseTokenStateEnum.Ready;
        }

        /// <summary>
        /// The method that automatically uses the token
        /// </summary>
        public override void Use()
        {
            if (_state == DefenseTokenStateEnum.Ready) _state = DefenseTokenStateEnum.Exhausted;
            else if (_state == DefenseTokenStateEnum.Exhausted) _state = DefenseTokenStateEnum.Discarded;
        }

        /// <summary>
        /// Resets the token if it's exhausted
        /// </summary>
        public override void Reset()
        {
            if (_state == DefenseTokenStateEnum.Exhausted) _state = DefenseTokenStateEnum.Ready;
        }
    }
}
