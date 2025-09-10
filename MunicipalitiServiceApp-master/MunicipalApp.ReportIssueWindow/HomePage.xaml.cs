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

        private string GetChatbotResponse(string userMessage)
        {
            userMessage = userMessage.ToLower();

            if (userMessage.Contains("hello") || userMessage.Contains("hi") || userMessage.Contains("hey"))
            {
                return "Hello! 👋 How can I assist you today?\n\nChoose an option:\n1️⃣ How to report an issue\n2️⃣ View service categories\n3️⃣ File attachments help\n4️⃣ Technician availability";
            }

            // Option-based replies
            if (userMessage == "1" || userMessage.Contains("how to report"))
                return "To report an issue, click 📋 'Report an Issue' and complete the form with location, category, and description.";

            if (userMessage == "2" || userMessage.Contains("categories") || userMessage.Contains("services"))
                return "Our services include: 🚰 Water, ⚡ Electricity, 🛣️ Roads, 🧼 Waste, 💡 Streetlights, 📡 Connectivity.";

            if (userMessage == "3" || userMessage.Contains("attach") || userMessage.Contains("file"))
                return "Click 📎 Attach File in the form to upload evidence (e.g., photo of the issue). Supported formats: JPG, PNG, PDF.";

            if (userMessage == "4" || userMessage.Contains("technician") || userMessage.Contains("coming"))
                return "After a report is submitted, a technician is dispatched within 2-4 hours depending on the severity.";

            // Existing intents
            if (userMessage.Contains("report") && userMessage.Contains("issue"))
                return "Click '📋 Report an Issue' and provide the required info: location, category, and description.";

            if (userMessage.Contains("waste") || userMessage.Contains("garbage") || userMessage.Contains("trash"))
                return "Waste collection happens every Monday and Thursday in most areas. Confirm your suburb for details.";

            if (userMessage.Contains("electricity") || userMessage.Contains("power"))
                return "Please check your DB board and nearby outages. If it's isolated, submit a report with meter number.";

            if (userMessage.Contains("water"))
                return "Specify if it's a leak, burst pipe, or outage. Include location for faster resolution.";

            if (userMessage.Contains("payment") || userMessage.Contains("pay"))
                return "Visit the municipal billing portal online or go to the nearest municipal office to make a payment.";

            if (userMessage.Contains("location"))
                return "Please include the street name and suburb so teams can locate the problem easily.";

            if (userMessage.Contains("about") || userMessage.Contains("info") || userMessage.Contains("learn"))
                return "This app helps citizens report service issues, attach evidence, and get real-time assistance.";

            if (userMessage.Contains("bye") || userMessage.Contains("thanks"))
                return "You're welcome! 👋 Stay safe and let us know if you need further assistance.";

            return "I'm here to help! Try sending 'hi' to get started or ask a question about services, files, or reporting.";
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
