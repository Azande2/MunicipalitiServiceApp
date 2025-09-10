using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ProgPart17312.DataStructures;

namespace ProgPart17312
{
    public partial class HomePage : Page
    {
        private ChatMessageQueue chatQueue = new ChatMessageQueue();

        public HomePage()
        {
            InitializeComponent();
            this.PreviewMouseDown += Page_PreviewMouseDown; // ✅ Attach click detection
        }

        private void ReportIssue_Click(object sender, RoutedEventArgs e)
        {
            MainWindow reportWindow = new MainWindow();
            reportWindow.Show();

            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.Close();
            }
        }

        private void LearnMore_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "This application allows you to report municipal issues such as water, electricity, roads, and sanitation. A team member will be assigned to address your report as soon as possible.",
                "About Municipal Services",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }

        private void ToggleChatbotPanel(object sender, RoutedEventArgs e)
        {
            ChatPanel.Visibility = ChatPanel.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        private void ChatbotSend_Click(object sender, RoutedEventArgs e)
        {
            string userMessage = txtChatInput.Text.Trim();
            if (string.IsNullOrEmpty(userMessage)) return;

            string formattedUserMessage = $"🧍 You: {userMessage}";
            chatQueue.Enqueue(formattedUserMessage);
            AddChatMessage(formattedUserMessage, isUser: true);
            txtChatInput.Clear();

            string botResponse = GetChatbotResponse(userMessage);
            string formattedBotMessage = $"🤖 Bot: {botResponse}";
            chatQueue.Enqueue(formattedBotMessage);
            AddChatMessage(formattedBotMessage, isUser: false);
        }

        private void AddChatMessage(string message, bool isUser)
        {
            var msgBlock = new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
                Background = isUser ? Brushes.LightBlue : Brushes.LightGray,
                Padding = new Thickness(8),
                Margin = new Thickness(0, 4, 0, 4),
                HorizontalAlignment = isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                MaxWidth = 250
            };

            ChatHistory.Children.Add(msgBlock);
        }

        private string GetChatbotResponse(string input)
        {
            input = input.ToLower();

            if (input.Contains("hello") || input.Contains("hi"))
                return "Hi there! 👋 How can I assist you today?";

            if (input.Contains("report"))
                return "To report an issue, click '📋 Report an Issue' on the main screen.";

            if (input.Contains("category"))
                return "Available categories: Water, Electricity, Roads, Sanitation.";

            if (input.Contains("location"))
                return "Please provide the exact location for accurate issue resolution.";

            if (input.Contains("file") || input.Contains("attach"))
                return "You can attach photos or files during issue reporting.";

            if (input.Contains("goodbye") || input.Contains("bye"))
                return "Goodbye! 👋 Stay safe!";

            return "I'm still learning. Try asking about reports, categories, or help with navigation! 😊";
        }

        private void txtChatInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift))
            {
                e.Handled = true;
                ChatbotSend_Click(this, new RoutedEventArgs());
            }
        }

        // ✅ Automatically close the chatbot if clicking outside of it
        private void Page_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ChatPanel.Visibility == Visibility.Visible)
            {
                var clickedElement = Mouse.DirectlyOver as DependencyObject;

                if (clickedElement != null && !IsDescendantOf(clickedElement, ChatPanel))
                {
                    ChatPanel.Visibility = Visibility.Collapsed;
                }
            }
        }

        private bool IsDescendantOf(DependencyObject child, DependencyObject parent)
        {
            while (child != null)
            {
                if (child == parent)
                    return true;

                child = VisualTreeHelper.GetParent(child);
            }
            return false;
        }
    }
}
