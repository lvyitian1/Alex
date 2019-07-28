using System.Collections;
using System.Collections.Generic;

namespace Alex.API.Gui
{
    public interface IItemsControl
    {
        int SelectedIndex { get; set; }

        object SelectedItem { get; set; }

        IEnumerable Items { get; set; }

    }

    public interface IItemsControl<T> : IItemsControl
    {
        new T SelectedItem { get; set; }
        new IEnumerable<T> Items { get; set; }
    }
}
