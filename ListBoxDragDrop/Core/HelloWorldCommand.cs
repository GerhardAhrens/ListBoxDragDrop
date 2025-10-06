namespace ListBoxDragDrop.Core
{
    using System.Windows;

    using ListBoxDragDrop.Extension;

    public class HelloWorldCommand : CommandExtension<HelloWorldCommand>
    {
        public override void Execute(object parameter)
        {
            MessageBox.Show("Hello world.");
        }
    }
}
