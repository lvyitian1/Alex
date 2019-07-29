using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RocketUI
{
    public abstract class GuiItemsControl<TValue> : GuiControl, IItemsControl<TValue>, IValuedControl<TValue> where TValue : IEquatable<TValue>
    {
        public event EventHandler<TValue> ValueChanged;
        public event EventHandler ItemsChanged;

        private TValue _selectedItem;
        private int _selectedIndex;
        private IEnumerable<TValue> _items;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => _selectedIndex = value;
        }

        public TValue SelectedItem
        {
            get => _selectedItem;
            set
            {
                var comparer = EqualityComparer<TValue>.Default;
                if (!comparer.Equals(_selectedItem, value))
                {
                    _selectedItem = value;
                    SelectedIndex = Array.IndexOf(Items.ToArray(), _selectedItem);
                    OnSelectedItemChanged();
                    ValueChanged?.Invoke(this, value);
                }
            }
        }

        public IEnumerable<TValue> Items
        {
            get => _items;
            set
            {
                if (!Equals(_items, value))
                {
                    _items = value;
                    OnItemsChanged();
                    ItemsChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        object IItemsControl.SelectedItem
        {
            get => Value;
            set => Value = (TValue) value;
        }

        IEnumerable IItemsControl.Items
        {
            get => Items;
            set => Items = value?.Cast<TValue>();
        }

        public TValue Value
        {
            get { return SelectedItem; }
            set { SelectedItem = value; }
        }

        public string DisplayFormat { get; set; }

        protected GuiItemsControl()
        {

        }


        protected virtual void OnSelectedItemChanged() { }
        protected virtual void OnItemsChanged() { }
    }
}
