
using System;
using System.Windows;

namespace CyberSafeAwarenessBot2
{
    public partial class MainWindow : Window
    {
        private readonly Chatbot _chatbot;

        public MainWindow()
        {
            InitializeComponent();
            _chatbot = new Chatbot();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string message = UserInput.Text.Trim();

                if (string.IsNullOrWhiteSpace(message))
                {
                    MessageBox.Show(
                        "Please enter a message.",
                        "Empty Message",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                string response = _chatbot.GetResponse(message);

                ChatHistory.Text += Environment.NewLine
                    + "You: " + message
                    + Environment.NewLine
                    + "Bot: " + response
                    + Environment.NewLine;

                UserInput.Clear();
                UserInput.Focus();
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Sorry, something went wrong. Please try again.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
