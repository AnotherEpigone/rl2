using Roguelike2.Text;
using SadConsole;
using SadConsole.UI;
using SadConsole.UI.Controls;
using SadRogue.Primitives;

namespace Roguelike2.Ui.Windows
{
    public class DeathWindow : Window
    {
        private readonly Label _deathLabel;

        public DeathWindow(IUiManager uiManager, IGameManager gameManager)
        : base(40, 5)
        {
            CloseOnEscKey = false;
            Center();
            Title = "You died";

            var background = new Console(Width, Height);
            background.Fill(null, ColorHelper.DarkGreyHighlight, null);

            Children.Add(background);

            _deathLabel = new Label(Width)
            {
                Position = new Point(2, 3)
            };
            var mainMenuButton = new Button(13)
            {
                Text = "Main Menu",
                Position = new Point((Width - 13) / 2, 2),
            };
            mainMenuButton.Click += (_, __) =>
            {
                Hide();
                uiManager.ShowMainMenu(gameManager);
            };

            //Controls.Add(_deathLabel);
            Controls.Add(mainMenuButton);
        }

        public void Show(string message)
        {
            _deathLabel.DisplayText = TextHelper.TruncateString(message, Width);
            base.Show(true);
        }
    }
}