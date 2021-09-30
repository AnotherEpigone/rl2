using SadConsole.UI;
using SadConsole.UI.Controls;
using SadConsole.UI.Themes;
using SadRogue.Primitives;

namespace Roguelike2.Ui.Themes
{
    public class SimpleProgressBarTheme : ProgressBarTheme
    {
        private readonly Color _filled;
        private readonly Color _empty;
        private readonly Color _text;

        public SimpleProgressBarTheme(
            Color filled,
            Color empty,
            Color text)
        {
            _filled = filled;
            _empty = empty;
            _text = text;
        }

        public override void RefreshTheme(Colors themeColors, ControlBase control)
        {
            Background.SetForeground(_empty);
            Background.SetBackground(themeColors.ControlHostBackground);

            Foreground.SetForeground(_filled);
            Foreground.SetBackground(themeColors.ControlHostBackground);

            DisplayText.SetForeground(_text);
            DisplayText.SetBackground(Color.Transparent);
        }
    }
}
