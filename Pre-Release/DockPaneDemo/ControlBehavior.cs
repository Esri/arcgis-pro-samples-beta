//Copyright 2014 Esri

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace ProSDKSamples.DockPaneDemo
{
    public static class ControlViewBehavior
    {
        // Capture *left mouse* events 

        public static DependencyProperty LeftClickCommandProperty = DependencyProperty.RegisterAttached("LeftClick",
                    typeof(ICommand),
                    typeof(ControlViewBehavior),
                    new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ControlViewBehavior.LeftClickChanged)));

        public static void SetLeftClick(DependencyObject target, ICommand value)
        {
            target.SetValue(ControlViewBehavior.LeftClickCommandProperty, value);
        }

        public static ICommand GetLeftClick(DependencyObject target)
        {
            return (ICommand)target.GetValue(LeftClickCommandProperty);
        }

        private static void LeftClickChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            UIElement element = target as UIElement;
            if (element != null)
            {
                // If we're putting in a new command and there wasn't one already hook the event
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    element.MouseLeftButtonUp += element_MouseLeftButtonUp;
                }

                // If we're clearing the command and it wasn't already null unhook the event
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    element.MouseLeftButtonUp -= element_MouseLeftButtonUp;
                }
            }
        }

        static void element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UIElement element = (UIElement)sender;
            if (element == null)
                return;

            ExecuteCommand((ICommand)element.GetValue(ControlViewBehavior.LeftClickCommandProperty),
                           (DependencyObject)e.OriginalSource);
        }

        // Generic call to run command.
        static void ExecuteCommand(ICommand command, DependencyObject dep)
        {
            // Walk up the visual tree to find our view model. We can't use the selected item, b/c we
            // have a multi-selectable listview... and we want the current/new clicked item in the selection.
            while ((dep != null) && !(dep is System.Windows.Controls.ListViewItem ||
                                      dep is System.Windows.Controls.ListBoxItem ||
                                      dep is System.Windows.Controls.TreeViewItem))
                dep = System.Windows.Media.VisualTreeHelper.GetParent(dep);
            if (dep == null)
                return;

            System.Windows.FrameworkElement viewItem = dep as System.Windows.FrameworkElement;
            if (viewItem == null)
                return; // this should never happen. 

            command.Execute(viewItem.DataContext);
        }

    
    }
}
