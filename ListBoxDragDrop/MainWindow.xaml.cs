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
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ListBox dragSource = new ListBox();

        public MainWindow()
        {
            this.InitializeComponent();
            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

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
                    DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);
                }
            }
            else if (parent.Name == this.LbInArbeit.Name)
            {
                dragSource = parent;
                object data = GetDataFromListBox(dragSource, e.GetPosition(parent));

                if (data != null)
                {
                    DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);
                }
            }
            else if (parent.Name == this.LbFertig.Name)
            {
                dragSource = parent;
                object data = GetDataFromListBox(dragSource, e.GetPosition(parent));

                if (data != null)
                {
                    DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);
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