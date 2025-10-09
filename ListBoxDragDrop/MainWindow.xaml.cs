//-----------------------------------------------------------------------
// <copyright file="MainWindow.cs" company="Lifeprojects.de">
//     Class: MainWindow
//     Copyright © Lifeprojects.de yyyy
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>dd.MM.yyyy</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ListBoxDragDrop
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ListBox dragSource = new ListBox();
        private Popup dragPopup;

        public MainWindow()
        {
            this.InitializeComponent();
            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.CreateDragPopup();

            this.WindowTitel = "ListBox mit Drag & Drop Funktion";
            this.DataContext = this;
            this.dragSource.ItemsSource = new ObservableCollection<string>() { string.Empty };
        }

        private string _WindowTitel;

        public string WindowTitel
        {
            get { return _WindowTitel; }
            set
            {
                if (this._WindowTitel != value)
                {
                    this._WindowTitel = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<KanbanItem> _KanbanItem;

        public ObservableCollection<KanbanItem> KanbanItem
        {
            get { return _KanbanItem; }
            set
            {
                if (this._KanbanItem != value)
                {
                    this._KanbanItem = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            WeakEventManager<ListBox, MouseButtonEventArgs>.AddHandler(this.LbWorkItem, "PreviewMouseLeftButtonDown", this.OnPreviewMouseLeftButtonDown);
            WeakEventManager<ListBox, DragEventArgs>.AddHandler(this.LbWorkItem, "Drop", this.OnDrop);

            WeakEventManager<ListBox, MouseButtonEventArgs>.AddHandler(this.LbInArbeit, "PreviewMouseLeftButtonDown", this.OnPreviewMouseLeftButtonDown);
            WeakEventManager<ListBox, DragEventArgs>.AddHandler(this.LbInArbeit, "Drop", this.OnDrop);

            WeakEventManager<ListBox, MouseButtonEventArgs>.AddHandler(this.LbFertig, "PreviewMouseLeftButtonDown", this.OnPreviewMouseLeftButtonDown);
            WeakEventManager<ListBox, DragEventArgs>.AddHandler(this.LbFertig, "Drop", this.OnDrop);

            this.ListBoxFrame.MouseMove += this.OnMouseMoveHandler;

            this.KanbanItem = new ObservableCollection<KanbanItem>()
            { 
                new KanbanItem("Aufgabe 1"),
                new KanbanItem("Aufgabe 2"),
                new KanbanItem("Aufgabe 3"),
                new KanbanItem("Aufgabe 4"),
                new KanbanItem("Aufgabe 5"),
            };

            this.LbWorkItem.ItemsSource = this.KanbanItem;
        }

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            if (parent.Name == this.LbWorkItem.Name)
            {
                dragSource = parent;
                object data = GetDataFromListBox(dragSource, e.GetPosition(parent));

                if (data != null)
                {
                    // Position des Popups aktualisieren
                    //this.ListBoxFrame.MouseMove += this.OnMouseMoveHandler;
                    dragPopup.IsOpen = true;

                    DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);

                    // Nach Drag & Drop schließen
                    dragPopup.IsOpen = false;
                    //this.ListBoxFrame.MouseMove -= this.OnMouseMoveHandler;
                }
            }
            else if (parent.Name == this.LbInArbeit.Name)
            {
                dragSource = parent;
                object data = GetDataFromListBox(dragSource, e.GetPosition(parent));

                if (data != null)
                {
                    // Position des Popups aktualisieren
                    //this.gridMain.MouseMove += this.OnMouseMoveHandler;
                    dragPopup.IsOpen = true;

                    DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);

                    // Nach Drag & Drop schließen
                    dragPopup.IsOpen = false;
                    //this.gridMain.MouseMove -= this.OnMouseMoveHandler;
                }
            }
            else if (parent.Name == this.LbFertig.Name)
            {
                dragSource = parent;
                object data = GetDataFromListBox(dragSource, e.GetPosition(parent));

                if (data != null)
                {
                    // Position des Popups aktualisieren
                    //this.gridMain.MouseMove += this.OnMouseMoveHandler;
                    dragPopup.IsOpen = true;

                    DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);

                    // Nach Drag & Drop schließen
                    dragPopup.IsOpen = false;
                    //this.gridMain.MouseMove -= this.OnMouseMoveHandler;
                }
            }
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            if (parent.Name == this.LbWorkItem.Name)
            {
                object data = e.Data.GetData(typeof(KanbanItem));
                if (dragSource != null)
                {
                    if (((IList)parent.Items).Contains(data) == false)
                    {
                        if (dragSource.ItemsSource != null)
                        {
                            ((IList)dragSource.ItemsSource).Remove(data);
                        }
                        else
                        {
                            ((IList)dragSource.Items).Remove(data);
                        }

                        ((KanbanItem)data).Status = "Neu";
                        ((KanbanItem)data).WorkDate = DateTime.Now;
                        ((IList)parent.ItemsSource).Add(data);
                    }
                }
            }
            else if (parent.Name == this.LbInArbeit.Name)
            {
                object data = e.Data.GetData(typeof(KanbanItem));
                if (dragSource != null)
                {
                    if (((IList)parent.Items).Contains(data) == false)
                    {
                        if (dragSource.ItemsSource != null)
                        {
                            ((IList)dragSource.ItemsSource).Remove(data);
                        }
                        else
                        {
                            ((IList)dragSource.Items).Remove(data);
                        }

                        ((KanbanItem)data).Status = "In Arbeit";
                        ((KanbanItem)data).WorkDate = DateTime.Now;
                        ((IList)parent.Items).Add(data);
                    }
                }
            }
            else if (parent.Name == this.LbFertig.Name)
            {
                object data = e.Data.GetData(typeof(KanbanItem));
                if (dragSource != null)
                {
                    if (((IList)parent.Items).Contains(data) == false)
                    {
                        if (dragSource.ItemsSource != null)
                        {
                            ((IList)dragSource.ItemsSource).Remove(data);
                        }
                        else
                        {
                            ((IList)dragSource.Items).Remove(data);
                        }

                        ((KanbanItem)data).Status = "Ferig";
                        ((KanbanItem)data).WorkDate = DateTime.Now;
                        ((IList)parent.Items).Add(data);
                    }
                }
            }
        }

        private void OnMouseMoveHandler(object sender, MouseEventArgs e)
        {
            Point position = e.GetPosition(this.ListBoxFrame);
            Debug.WriteLine($"{position.X};{position.Y};{e.LeftButton};{sender}");
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
                dragPopup.HorizontalOffset = position.X + 20;
                dragPopup.VerticalOffset = position.Y + 10;
            }

            e.Handled = true;
        }

        private static object GetDataFromListBox(ListBox source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);

                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = VisualTreeHelper.GetParent(element) as UIElement;
                    }

                    if (element == source)
                    {
                        return null;
                    }
                }

                if (data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
            }

            return null;
        }

        private void CreateDragPopup()
        {
            dragPopup = new Popup
            {
                AllowsTransparency = true,
                Placement = PlacementMode.Mouse,
                PopupAnimation = PopupAnimation.Fade,
                StaysOpen = true,
                IsOpen = false,
                Child = new Border
                {
                    Background = Brushes.LightYellow,
                    BorderBrush = Brushes.Gray,
                    BorderThickness = new Thickness(1),
                    Padding = new Thickness(5),
                    Child = new TextBlock
                    {
                        Text = "Tooltip beim Draggen",
                        Foreground = Brushes.Black
                    }
                }
            };
        }

        #region INotifyPropertyChanged implementierung
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler == null)
            {
                return;
            }

            var e = new PropertyChangedEventArgs(propertyName);
            handler(this, e);
        }
        #endregion INotifyPropertyChanged implementierung
    }

    public static class CursorHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(ref Win32Point pt);

        public static Point GetCurrentCursorPosition(Visual relativeTo)
        {
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return relativeTo.PointFromScreen(new Point(w32Mouse.X, w32Mouse.Y));
        }
    }

    public class KanbanItem
    {
        public KanbanItem(string titel)
        {
            this.Id = Guid.NewGuid();
            this.Titel = titel;
            this.Status = "Neu";
            this.WorkDate = DateTime.Now;
        }

        public Guid Id { get; set; }
        public string Titel { get; set; }
        public string Status { get; set; }
        public DateTime WorkDate { get; set; }
    }
}