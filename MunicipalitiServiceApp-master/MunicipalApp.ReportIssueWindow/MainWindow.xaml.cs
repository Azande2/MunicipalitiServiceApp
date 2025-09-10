using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ProgPart17312.DataStructures;

namespace ProgPart17312
{
    public partial class MainWindow : Window
    {
        private string attachedFilePath = "";
        private ChatMessageQueue chatQueue = new ChatMessageQueue();
        private StackLogHistory logHistory = new StackLogHistory();


        public MainWindow()
        {
            InitializeComponent();
            this.PreviewMouseDown += Window_PreviewMouseDown;
        }

        private void AttachFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Title = "Select a file to attach"
            };
            if (fileDialog.ShowDialog() == true)
            {
                attachedFilePath = Path.GetFileName(fileDialog.FileName);
                lblFileName.Text = attachedFilePath;
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
            reportProgress.Value = 100;
            lblStatus.Content = "✔️ Report submitted successfully!";

            // ✅ Summary string
            string summary = $"📍 Location: {location}\n" +
                             $"📂 Category: {category}\n" +
                             $"📝 Description: {description}\n" +
                             $"📅 Date: {date}\n" +
                             $"📎 File: {attachedFilePath}";

            logHistory.Push($"[REPORT LOGGED] - {DateTime.Now}: {location}, {category}");

            // ✅ Step 1: Show user-submitted info
            MessageBoxResult result = MessageBox.Show(summary, "✅ Report Submitted", MessageBoxButton.OK, MessageBoxImage.Information);

            if (result == MessageBoxResult.OK)
            {
                // ✅ Step 2: Report assigned
                MessageBox.Show("✅ Your report has been received and assigned to a team member.", "Report Assigned", MessageBoxButton.OK, MessageBoxImage.Information);

                // ✅ Step 3: Technician will arrive
                DispatcherTimer delayTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(3)
                };
                delayTimer.Tick += (s, args) =>
                {
                    delayTimer.Stop();
                    MessageBox.Show(
                        $"{summary}\n\n🛠️ A technician will arrive shortly to resolve the issue.",
                        "Team Dispatched",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                };
                delayTimer.Start();

                // ✅ Step 4: Clear fields
                txtLocation.Clear();
                cbCategory.SelectedIndex = -1;
                rtbDescription.Document.Blocks.Clear();
                datePicker.SelectedDate = null;
                attachedFilePath = "";
                lblFileName.Text = "";
                reportProgress.Value = 0;
                lblStatus.Content = "";
            }
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
                Foreground = isUser ? Brushes.DarkSlateBlue : Brushes.Black
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

            return "Your query has been received. A support assistant will follow up if needed. You can ask about water, waste, roads, electricity, and more!";
        }

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ChatbotPanel.Visibility == Visibility.Visible)
            {
                var clickedElement = Mouse.DirectlyOver as DependencyObject;

                if (clickedElement != null && !IsDescendantOf(clickedElement, ChatbotPanel))
                {
                    ChatbotPanel.Visibility = Visibility.Collapsed;
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
