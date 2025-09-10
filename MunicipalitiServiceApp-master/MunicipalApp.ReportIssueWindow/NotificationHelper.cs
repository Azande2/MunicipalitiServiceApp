using System.Windows;


namespace ProgPart17312
{
    public static class NotificationHelper
    {
        public static void ShowNotification(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}