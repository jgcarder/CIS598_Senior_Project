/* File: CollisionHelper.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace CIS598_Senior_Project.Collisions
{
    public static class CollisionHelper
    {

        /// <summary>
        /// Detects a collision between two circles.
        /// </summary>
        /// <param name="a">Bounding Circle 1</param>
        /// <param name="b">Bounding Circle 2</param>
        /// <returns>true for collision</returns>
        public static bool Collides(BoundingCircle a, BoundingCircle b)
        {
            return Math.Pow(a.Radius + b.Radius, 2) >= Math.Pow(a.Center.X - b.Center.X, 2) + Math.Pow(a.Center.Y - b.Center.Y, 2);
        }

        /// <summary>
        /// Detects a collision between two rectangles
        /// </summary>
        /// <param name="a">Bounding rect 1</param>
        /// <param name="b">Bounding rect 2</param>
        /// <returns>true if collision</returns>
        public static bool Collides(BoundingRectangle a, BoundingRectangle b)
        {
            return !(a.Right < b.Left || a.Left > b.Right || a.Top > b.Bottom || a.Bottom < b.Top);
        }

        /// <summary>
        /// Detects a collision between a rect and a circle.
        /// </summary>
        /// <param name="c">The bounding circle</param>
        /// <param name="r">The bounding rect</param>
        /// <returns>True if colliding</returns>
        public static bool Collides(BoundingCircle c, BoundingRectangle r)
        {
            float nearestX = MathHelper.Clamp(c.Center.X, r.Left, r.Right);
            float nearestY = MathHelper.Clamp(c.Center.Y, r.Top, r.Bottom);
            return Math.Pow(c.Radius, 2) >= Math.Pow(c.Center.X - nearestX, 2) + Math.Pow(c.Center.Y - nearestY, 2);
        }

        /// <summary>
        /// Detects a collision between a rect and a circle.
        /// </summary>
        /// <param name="r">The bounding rect</param>
        /// <param name="c">The bounding circle</param>
        /// <returns>True if colliding</returns>
        public static bool Collides(BoundingRectangle r, BoundingCircle c) => Collides(c, r);

    }
}
