using SadConsole.UI.Controls;
using SadConsole.UI.Themes;

namespace Roguelike2.Ui
{
    public static class ThemeHelper
    {
        public static ButtonTheme ButtonThemeNoEnds()
        {
            var buttonTheme = (ButtonTheme)Library.Default.GetControlTheme(typeof(Button));
            buttonTheme.ShowEnds = false;
            return buttonTheme;
        }
    }
}
