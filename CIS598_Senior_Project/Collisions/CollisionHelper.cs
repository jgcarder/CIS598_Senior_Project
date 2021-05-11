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

        /// <summary>
        /// A method that dynamically sets the positions of bounding circls on the ships in the game
        /// </summary>
        /// <param name="center">The center point of the ship</param>
        /// <param name="radius">The distance from the center point to the center of the new bounding circle</param>
        /// <param name="radians">The current rotation of the ship</param>
        /// <returns>The new center point of the bounds</returns>
        public static Vector2 GetNewCoords(Vector2 center, int radius, float radians)
        {
            Vector2 result = new Vector2(0, 0);

            int rot = Math.Abs((int)(radians / (MathHelper.PiOver4 / 2)));

            if (rot >= 16) rot -= 16;

            if(rot < 0) rot += 16;

            switch (rot)
            {
                case 0:
                    result.X = (radius * (1)) + center.X;
                    result.Y = (radius * (0)) + center.Y;
                    break;
                case 1:
                    result.X = (float)((radius * ( Math.Sqrt(2 + Math.Sqrt(2)) / 2 )) + center.X);
                    result.Y = (float)((radius * (-Math.Sqrt(2 - Math.Sqrt(2)) / 2)) + center.Y);
                    break;
                case 2:
                    result.X = (float)((radius * (Math.Sqrt(2) / 2)) + center.X);
                    result.Y = (float)((radius * (-Math.Sqrt(2) / 2)) + center.Y);
                    break;
                case 3:
                    result.X = (float)((radius * (Math.Sqrt(2 - Math.Sqrt(2)) / 2)) + center.X);
                    result.Y = (float)((radius * (-Math.Sqrt(2 + Math.Sqrt(2)) / 2)) + center.Y);
                    break;
                case 4:
                    result.X = (radius * (0)) + center.X;
                    result.Y = (radius * (-1)) + center.Y;
                    break;
                case 5:
                    result.X = (float)((radius * (-Math.Sqrt(2 - Math.Sqrt(2)) / 2)) + center.X);
                    result.Y = (float)((radius * (-Math.Sqrt(2 + Math.Sqrt(2)) / 2)) + center.Y);
                    break;
                case 6:
                    result.X = (float)((radius * (-Math.Sqrt(2) / 2)) + center.X);
                    result.Y = (float)((radius * (-Math.Sqrt(2) / 2)) + center.Y);
                    break;
                case 7:
                    result.X = (float)((radius * (-Math.Sqrt(2 + Math.Sqrt(2)) / 2)) + center.X);
                    result.Y = (float)((radius * (-Math.Sqrt(2 - Math.Sqrt(2)) / 2)) + center.Y);
                    break;
                case 8:
                    result.X = (radius * (-1)) + center.X;
                    result.Y = (radius * (0)) + center.Y;
                    break;
                case 9:
                    result.X = (float)((radius * (-Math.Sqrt(2 + Math.Sqrt(2)) / 2)) + center.X);
                    result.Y = (float)((radius * (Math.Sqrt(2 - Math.Sqrt(2)) / 2)) + center.Y);
                    break;
                case 10:
                    result.X = (float)((radius * (-Math.Sqrt(2) / 2)) + center.X);
                    result.Y = (float)((radius * (Math.Sqrt(2) / 2)) + center.Y);
                    break;
                case 11:
                    result.X = (float)((radius * (-Math.Sqrt(2 - Math.Sqrt(2)) / 2)) + center.X);
                    result.Y = (float)((radius * (Math.Sqrt(2 + Math.Sqrt(2)) / 2)) + center.Y);
                    break;
                case 12:
                    result.X = (radius * (0)) + center.X;
                    result.Y = (radius * (1)) + center.Y;
                    break;
                case 13:
                    result.X = (float)((radius * (Math.Sqrt(2 - Math.Sqrt(2)) / 2)) + center.X);
                    result.Y = (float)((radius * (Math.Sqrt(2 + Math.Sqrt(2)) / 2)) + center.Y);
                    break;
                case 14:
                    result.X = (float)((radius * (Math.Sqrt(2) / 2)) + center.X);
                    result.Y = (float)((radius * (Math.Sqrt(2) / 2)) + center.Y);
                    break;
                case 15:
                    result.X = (float)((radius * (Math.Sqrt(2 + Math.Sqrt(2)) / 2)) + center.X);
                    result.Y = (float)((radius * (Math.Sqrt(2 - Math.Sqrt(2)) / 2)) + center.Y);
                    break;
            }

            return result;
        }

    }
}
