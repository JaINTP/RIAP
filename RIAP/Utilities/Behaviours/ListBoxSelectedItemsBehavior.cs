// ***********************************************************************
// Assembly         : RIAP
// Author           : Jai Brown
// Created          : 14-11-2020
//
// Last Modified By : Jai Brown
// Last Modified On : 14-11-2020
// ***********************************************************************

#region Usings

using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

#endregion Usings

namespace JaINTP.RIAP.Utilities.Behaviours
{
    public class ListBoxSelectedItemsBehavior : Behavior<ListBox>
    {
        #region Overrides

        protected override void OnAttached()
        {
            AssociatedObject.SelectionChanged += AssociatedObjectSelectionChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SelectionChanged -= AssociatedObjectSelectionChanged;
        }

        #endregion Overrides

        #region Event Handlers

        private void AssociatedObjectSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var array = new object[AssociatedObject.SelectedItems.Count];
            AssociatedObject.SelectedItems.CopyTo(array, 0);
            SelectedItems = array;
        }

        #endregion Event Handlers

        #region Dependency Properties

        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(IEnumerable), typeof(ListBoxSelectedItemsBehavior),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion Dependency Properties

        #region Properties

        public IEnumerable SelectedItems
        {
            get => (IEnumerable)GetValue(SelectedItemsProperty);
            set => SetValue(SelectedItemsProperty, value);
        }

        #endregion Properties
    }
}