/* File: IScreenFactory.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace CIS598_Senior_Project.StateManagement
{
    /// <summary>
    /// Defines an object that can create a screen when given its type.
    /// </summary>
    interface IScreenFactory
    {

        GameScreen CreateScreen(Type screenType);

    }
}
