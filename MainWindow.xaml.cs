
using System;
using System.IO;
using System.Media;
using System.Windows;

namespace CyberSafeAwarenessBot2
{
    public partial class MainWindow : Window
    {
        private readonly Chatbot _chatbot;
        private SoundPlayer? _greetingPlayer;

        public MainWindow()
        {
            InitializeComponent();
            _chatbot = new Chatbot();

            try
            {
                string audioPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Audio",
                    "greeting.wav");

                if (File.Exists(audioPath))
                {
                    _greetingPlayer = new SoundPlayer(audioPath);
                    _greetingPlayer.Play();
                }
                else
                {
                    MessageBox.Show(
                        "The greeting audio file could not be found.",
                        "Audio Notice",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "The greeting audio could not be played.",
                    "Audio Notice",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
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
