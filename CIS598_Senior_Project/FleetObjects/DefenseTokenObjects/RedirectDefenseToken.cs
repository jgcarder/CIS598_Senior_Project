/* File: RedirectDefenseToken.cs
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
    /// The class for the Redirect defense token
    /// </summary>
    public class RedirectDefenseToken : DefenseToken
    {
        private DefenseTokenStateEnum _state;

        /// <summary>
        /// The ID
        /// </summary>
        public override int Id { get; }

        /// <summary>
        /// The string name of the token
        /// </summary>
        public override string Name { get { return "Redirect"; } }

        /// <summary>
        /// The text on the card
        /// </summary>
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

        /// <summary>
        /// The texture of he screw
        /// /// </summary>
        public override Texture2D Texture { get; } //To be assigned

        /// <summary>
        /// The source of the image 
        /// </summary>
        public override Rectangle Source { get; } //To be assigned
        /// <summary>
        /// The state of the 
        /// </summary>
        public override DefenseTokenStateEnum State { get { return _state; } set { _state = value; } }

        /// <summary>
        /// The constructor for the object
        /// </summary>
        /// <param name="id">The ID you want to give it</param>
        public RedirectDefenseToken(int id)
        {
            _state = DefenseTokenStateEnum.Ready;
        }

        /// <summary>
        /// Method used to us the token
        /// </summary>
        public override void Use()
        {
            if (_state == DefenseTokenStateEnum.Ready) _state = DefenseTokenStateEnum.Exhausted;
            else if (_state == DefenseTokenStateEnum.Exhausted) _state = DefenseTokenStateEnum.Discarded;
        }

         /// <summary>
         /// resets the way they were.
         /// </summary>
        public override void Reset()
        {
            if (_state == DefenseTokenStateEnum.Exhausted) _state = DefenseTokenStateEnum.Ready;
        }
    }
}
