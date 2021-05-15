/* File: DefenceToken.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CIS598_Senior_Project.FleetObjects.DefenseTokenObjects
{
    /// <summary>
    /// The base class for the defense token object type
    /// </summary>
    public abstract class DefenseToken
    {
        /// <summary>
        /// The Id of the token
        /// </summary>
        public abstract int Id { get; }

        /// <summary>
        /// The token's name
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// What the token does/is about
        /// </summary>
        public abstract string Text { get; }

        /// <summary>
        /// The token's texture
        /// </summary>
        public abstract Texture2D Texture { get; }

        /// <summary>
        /// The potential sourc for the image
        /// </summary>
        public abstract Rectangle Source { get; }

        /// <summary>
        /// The state of the enum of the token
        /// </summary>
        public abstract DefenseTokenStateEnum State { get; set; }

        /// <summary>
        /// A method that uses the token
        /// </summary>
        public abstract void Use();

        /// <summary>
        /// Resets the token if exhausted
        /// </summary>
        public abstract void Reset();
    }
}
