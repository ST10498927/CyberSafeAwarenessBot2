
using System;
using System.Windows;

namespace CyberSafeAwarenessBot2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = UserInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show("Please enter a message.",
                    "Empty Message",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            ChatHistory.Text += Environment.NewLine
                + "You: " + message
                + Environment.NewLine
                + "Bot: Thank you for your message. I will help you with cybersecurity questions."
                + Environment.NewLine;

            UserInput.Clear();
        }
    }
}
