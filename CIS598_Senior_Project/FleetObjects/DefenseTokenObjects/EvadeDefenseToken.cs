/* File: EvadeDefenceToken.cs
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
    /// The class for the evade defense token used by some shiups
    /// </summary>
    public class EvadeDefenseToken : DefenseToken
    {
        private DefenseTokenStateEnum _state;

        /// <summary>
        /// The token's ID
        /// </summary>
        public override int Id { get; }

        /// <summary>
        /// The token's name
        /// </summary>
        public override string Name { get { return "Evade"; } }

        /// <summary>
        /// The token's text
        /// </summary>
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

        /// <summary>
        /// The token's textures
        /// </summary>
        public override Texture2D Texture { get; } //To be assigned

        /// <summary>
        /// Potential image source if the image is too big
        /// </summary>
        public override Rectangle Source { get; } //To be assigned

        /// <summary>
        /// The state of the token itself
        /// </summary>
        public override DefenseTokenStateEnum State { get { return _state; } set { _state = value; } }

        /// <summary>
        /// The condtruction got eht object
        /// </summary>
        /// <param name="id">The id to use</param>
        public EvadeDefenseToken(int id)
        {
            _state = DefenseTokenStateEnum.Ready;
        }

        /// <summary>
        /// A method used to increment it's state
        /// </summary>
        public override void Use()
        {
            if (_state == DefenseTokenStateEnum.Ready) _state = DefenseTokenStateEnum.Exhausted;
            else if (_state == DefenseTokenStateEnum.Exhausted) _state = DefenseTokenStateEnum.Discarded;
        }

        /// <summary>
        /// A method to decrement the state;
        /// </summary>
        public override void Reset()
        {
            if (_state == DefenseTokenStateEnum.Exhausted) _state = DefenseTokenStateEnum.Ready;
        }
    }
}
