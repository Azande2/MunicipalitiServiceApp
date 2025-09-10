using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using ProgPart17312.DataStructures;

namespace ProgPart17312
{
    public partial class MainWindow : Window
    {
        private string attachedFilePath = "";
        private ChatMessageQueue chatQueue = new ChatMessageQueue();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AttachFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Title = "Select a file to attach"
            };
            if (fileDialog.ShowDialog() == true)
            {
                attachedFilePath = fileDialog.FileName;
                lblFileName.Text = Path.GetFileName(attachedFilePath);
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string location = txtLocation.Text.Trim();
            string category = (cbCategory.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string description = new TextRange(rtbDescription.Document.ContentStart, rtbDescription.Document.ContentEnd).Text.Trim();
            string date = datePicker.SelectedDate.HasValue ? datePicker.SelectedDate.Value.ToShortDateString() : "Not Selected";

            if (string.IsNullOrEmpty(location) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Please fill in all the required fields.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            reportProgress.Visibility = Visibility.Visible;
            reportProgress.Value = 50;

            reportProgress.Value = 100;
            lblStatus.Content = "✔️ Report submitted successfully!";

            MessageBox.Show($"Report Submitted:\n\nLocation: {location}\nCategory: {category}\nDescription: {description}\nDate: {date}\nFile: {Path.GetFileName(attachedFilePath)}", "Success");
        }

        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            Window homeWindow = new Window
            {
                Title = "Municipal Services - Home",
                Content = new HomePage(),
                Width = 900,
                Height = 700,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            homeWindow.Show();
            this.Close();
        }

        private void btnHelpFloat_Click(object sender, RoutedEventArgs e)
        {
            ChatbotPanel.Visibility = ChatbotPanel.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        private void ChatbotSend_Click(object sender, RoutedEventArgs e)
        {
            string userMessage = txtChatInput.Text.Trim();
            if (!string.IsNullOrEmpty(userMessage))
            {
                chatQueue.Enqueue($"🧍 You: {userMessage}");
                AddChatMessage($"🧍 You: {userMessage}", isUser: true);

                string botResponse = GetChatbotResponse(userMessage);
                chatQueue.Enqueue($"🤖 Bot: {botResponse}");
                AddChatMessage($"🤖 Bot: {botResponse}", isUser: false);

                txtChatInput.Text = string.Empty;
            }
        }

        private void AddChatMessage(string message, bool isUser)
        {
            TextBlock txt = new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(5),
                FontWeight = isUser ? FontWeights.Bold : FontWeights.Normal,
                Foreground = isUser ? System.Windows.Media.Brushes.DarkSlateBlue : System.Windows.Media.Brushes.Black
            };
            ChatHistory.Children.Add(txt);
        }

        private string GetChatbotResponse(string userMessage)
        {
            userMessage = userMessage.ToLower();

            if (userMessage.Contains("hello") || userMessage.Contains("hi") || userMessage.Contains("hey"))
                return "Hello! 👋 How can I assist you with municipal services today?";

            if (userMessage.Contains("report") && userMessage.Contains("issue"))
                return "You can report an issue by clicking '📋 Report an Issue' and completing the required fields.";

            if (userMessage.Contains("waste") || userMessage.Contains("garbage") || userMessage.Contains("trash"))
                return "Waste collection occurs every Monday and Thursday. Please confirm your area for accuracy.";

            if (userMessage.Contains("electricity") || userMessage.Contains("power"))
                return "Please check your DB board first. If issues persist, report with your meter number.";

            if (userMessage.Contains("water"))
                return "Please include the street name and describe the problem (e.g., leak, outage).";

            if (userMessage.Contains("payment") || userMessage.Contains("pay"))
                return "Visit the municipal billing portal or call customer care to make a payment.";

            if (userMessage.Contains("attach") || userMessage.Contains("file"))
                return "Click 📎 Attach File in the report form to upload supporting documents.";

            if (userMessage.Contains("location"))
                return "Please enter the street name or suburb where the issue is occurring.";

            if (userMessage.Contains("learn more") || userMessage.Contains("info") || userMessage.Contains("about"))
                return "We provide services like water, waste, road repairs, and more. Click 'ℹ️ Learn More'.";

            return "🤔 I'm not sure how to help with that. Try asking about electricity, water, waste, payments, or reporting an issue.";
        }
    }
}
