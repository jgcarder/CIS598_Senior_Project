using System;
using System.Collections.Generic;
using System.Text;
using CIS598_Senior_Project.StateManagement;

namespace CIS598_Senior_Project
{
    public class ScreenFactory : IScreenFactory
    {
        public GameScreen CreateScreen(Type screenType)
        {
            // All of our screens have empty constructors so we can just use Activator
            return Activator.CreateInstance(screenType) as GameScreen;
        }
    }
}
