using System.Windows;
using System.Windows.Interactivity;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace WindowsExplorerish.Views
{
    public class ShowMessageBoxAction : TriggerAction<FrameworkElement>
    {
        protected override void Invoke(object parameter)
        {
            InteractionRequestedEventArgs e = parameter as InteractionRequestedEventArgs;
            if (e != null)
            {
                var title = e.Context.Title;
                var message = e.Context.Content.ToString();

                if (e.Context is Confirmation)
                {
                    var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    ((Confirmation)e.Context).Confirmed = result == MessageBoxResult.Yes;
                }
                else
                {
                    MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                }

                e.Callback();
            }
        }
    }
}
