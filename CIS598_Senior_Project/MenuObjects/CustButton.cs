/* File: CustButton.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CIS598_Senior_Project.MenuObjects
{
    /// <summary>
    /// A custom button class I came up with for buttons in the menu.
    /// </summary>
    public class CustButton
    {
        public int Id { get; set; }

        public Texture2D Texture { get; set; }

        public Color Color { get; set; }

        public Rectangle Area { get; set; }

        public Vector2 Position { get; set; }

        public bool IsActive { get; set; }

        public Action<object, ButtonClickedEventArgs> AnAction { get; set; } = null;

        /// <summary>
        /// A constructor for the Custom Button
        /// </summary>
        /// <param name="id">The button id/index</param>
        /// <param name="area">the area the button takes up</param>
        /// <param name="initialActivity">if the button is there or not</param>
        public CustButton(int id, Rectangle area, bool initialActivity)
        {
            IsActive = initialActivity;
            Id = id;
            Position = new Vector2(area.X, area.Y);
            Area = area;
            Color = Color.White;
        }

        /// <summary>
        /// Another constructor for the buttons
        /// </summary>
        /// <param name="id">The id for the button</param>
        /// <param name="area"> The area/position of the button</param>
        /// <param name="initialActivity">the visability of the button</param>
        /// <param name="image">An image for the button</param>
        public CustButton(int id, Rectangle area, bool initialActivity, Texture2D image)
        {
            IsActive = initialActivity;
            Id = id;
            Position = new Vector2(area.X, area.Y);
            Area = area;
            Color = Color.White;
            this.Texture = image;
        }
    }
}
