using System;
using System.Collections.Generic;
using System.Text;
using Alex.API.Gui.Elements.Controls;
using Alex.GameStates.Gui.Common;

namespace Alex.GameStates
{
    public class GuiDebugState : GuiMenuStateBase
    {

        private GuiComboBox<string> _comboBox;

        public GuiDebugState()
        {
            var items = new string[64];

            for (int i = 0; i < items.Length; i++)
            {
                items[i] = $"Item #{i}";
            }
            AddGuiRow(_comboBox = new GuiComboBox<string>()
            {
                Items = items,
                SelectedItem = items[0]
            });
        }



    }
}
