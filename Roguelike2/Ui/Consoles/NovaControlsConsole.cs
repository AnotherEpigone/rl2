using Roguelike2.Ui.Controls;
using SadConsole;
using SadConsole.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Roguelike2.Ui.Consoles
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class NovaControlsConsole : Console
    {
        private Dictionary<NovaSelectionButton, System.Action> _selectionButtons;
        private NovaSelectionButton _lastFocusedButton;

        public NovaControlsConsole(int width, int height)
            : this(width, height, false)
        { }

        public NovaControlsConsole(int width, int height, bool clearBackground)
            : base(width, height)
        {
            Controls = new ControlHost
            {
                ClearOnAdded = clearBackground,
            };
            SadComponents.Add(Controls);
        }

        public ControlHost Controls { get; }

        public void SetupSelectionButtons(params NovaSelectionButton[] buttons)
        {
            SetupSelectionButtons(new Dictionary<NovaSelectionButton, System.Action>(buttons.Select(b => new KeyValuePair<NovaSelectionButton, System.Action>(b, () => { }))));
        }

        public void SetupSelectionButtons(Dictionary<NovaSelectionButton, System.Action> buttonSelectionActions)
        {
            _selectionButtons = new Dictionary<NovaSelectionButton, System.Action>(buttonSelectionActions);
            if (_selectionButtons.Count < 1)
            {
                return;
            }

            var buttons = buttonSelectionActions.Keys.ToArray();
            for (int i = 1; i < _selectionButtons.Count; i++)
            {
                buttons[i - 1].NextSelection = buttons[i];
                buttons[i].PreviousSelection = buttons[i - 1];
            }

            buttons[0].PreviousSelection = buttons[_selectionButtons.Count - 1];
            buttons[_selectionButtons.Count - 1].NextSelection = buttons[0];

            foreach (var button in buttons)
            {
                Controls.Add(button);
                button.MouseEnter += (_, __) =>
                {
                    Controls.FocusedControl = button;
                };
            }

            if (buttons[0].IsEnabled)
            {
                Controls.FocusedControl = buttons[0];
            }
            else
            {
                buttons[0].SelectNext();
            }
        }

        public override void Update(System.TimeSpan time)
        {
            if (!(Controls.FocusedControl is NovaSelectionButton focusedButton)
                || focusedButton == _lastFocusedButton)
            {
                base.Update(time);
                return;
            }

            _lastFocusedButton = focusedButton;
            _selectionButtons[focusedButton]();

            base.Update(time);
        }

        private string DebuggerDisplay
        {
            get
            {
                return string.Format($"{nameof(NovaControlsConsole)} ({Position.X}, {Position.Y})");
            }
        }
    }
}
