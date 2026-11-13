
using System;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using CyberSafeAwarenessBot2.Models;
using CyberSafeAwarenessBot2.Repositories;
using CyberSafeAwarenessBot2.Services;

namespace CyberSafeAwarenessBot2
{
    public partial class MainWindow : Window
    {
        private readonly Chatbot _chatbot;
        private readonly TaskRepository _taskRepository;
        private readonly QuizManager _quizManager;

        private SoundPlayer? _greetingPlayer;
        private int? _selectedAnswer;

        public MainWindow()
        {
            InitializeComponent();

            _chatbot = new Chatbot();
            _taskRepository = new TaskRepository();
            _quizManager = new QuizManager();

            PlayGreeting();

            LoadTasks();
            LoadQuizQuestion();

            LogActivity("Application started.");
        }

        // -----------------------------------------
        // AUDIO GREETING
        // -----------------------------------------

        private void PlayGreeting()
        {
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

        // -----------------------------------------
        // CHATBOT
        // -----------------------------------------

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

                LogActivity("Chatbot interaction: " + message);

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

        // -----------------------------------------
        // TASK ASSISTANT
        // -----------------------------------------

        private void LoadTasks()
        {
            try
            {
                TasksGrid.ItemsSource = _taskRepository.GetTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not load tasks: " + ex.Message,
                    "Database Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TaskTitleInput.Text) ||
                string.IsNullOrWhiteSpace(TaskDescriptionInput.Text))
            {
                MessageBox.Show(
                    "Enter a task title and description.",
                    "Missing Information",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            try
            {
                CyberTask task = new CyberTask
                {
                    Title = TaskTitleInput.Text.Trim(),
                    Description = TaskDescriptionInput.Text.Trim(),
                    ReminderDate = TaskReminderInput.SelectedDate
                };

                _taskRepository.AddTask(task);

                LogActivity("Task created: " + task.Title);

                TaskTitleInput.Clear();
                TaskDescriptionInput.Clear();
                TaskReminderInput.SelectedDate = null;

                LoadTasks();

                MessageBox.Show("Task added successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not add task: " + ex.Message,
                    "Task Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void RefreshTasks_Click(object sender, RoutedEventArgs e)
        {
            LoadTasks();
            LogActivity("Task list refreshed.");
        }

        private void CompleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (TasksGrid.SelectedItem is not CyberTask task)
            {
                MessageBox.Show("Select a task first.");
                return;
            }

            try
            {
                _taskRepository.CompleteTask(task.TaskId);

                LogActivity("Task completed: " + task.Title);

                LoadTasks();

                MessageBox.Show("Task marked as completed.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not complete task: " + ex.Message);
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (TasksGrid.SelectedItem is not CyberTask task)
            {
                MessageBox.Show("Select a task first.");
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to delete this task?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                _taskRepository.DeleteTask(task.TaskId);

                LogActivity("Task deleted: " + task.Title);

                LoadTasks();

                MessageBox.Show("Task deleted successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not delete task: " + ex.Message);
            }
        }

        // -----------------------------------------
        // CYBERSECURITY QUIZ
        // -----------------------------------------

        private void LoadQuizQuestion()
        {
            if (_quizManager.IsFinished)
            {
                ShowQuizResults();
                return;
            }

            QuizQuestion question = _quizManager.CurrentQuestion;

            QuizProgressText.Text =
                $"Question {_quizManager.CurrentQuestionNumber} of {_quizManager.TotalQuestions}";

            QuizQuestionText.Text = question.Question;

            QuizOptionsPanel.Children.Clear();

            _selectedAnswer = null;

            for (int i = 0; i < question.Options.Length; i++)
            {
                RadioButton option = new RadioButton
                {
                    Content = question.Options[i],
                    Tag = i,
                    GroupName = "QuizAnswers",
                    FontSize = 14,
                    Margin = new Thickness(5, 10, 5, 10),
                    Padding = new Thickness(5)
                };

                option.Checked += QuizOption_Checked;

                QuizOptionsPanel.Children.Add(option);
            }

            QuizFeedbackText.Text = "Select an answer and submit.";

            QuizScoreText.Text = "Score: " + _quizManager.Score;

            SubmitAnswerButton.IsEnabled = true;
            NextQuestionButton.IsEnabled = false;
            NextQuestionButton.Content = "Next";
        }

        private void QuizOption_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton &&
                radioButton.Tag is int answerIndex)
            {
                _selectedAnswer = answerIndex;
            }
        }

        private void SubmitAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (!_selectedAnswer.HasValue)
            {
                MessageBox.Show("Please select an answer first.");
                return;
            }

            string feedback =
                _quizManager.SubmitAnswer(_selectedAnswer.Value);

            QuizFeedbackText.Text = feedback;

            QuizScoreText.Text = "Score: " + _quizManager.Score;

            SubmitAnswerButton.IsEnabled = false;
            NextQuestionButton.IsEnabled = true;

            foreach (RadioButton option in
                     QuizOptionsPanel.Children.OfType<RadioButton>())
            {
                option.IsEnabled = false;
            }

            if (_quizManager.LastAnswerCorrect)
            {
                QuizFeedbackText.Foreground =
                    System.Windows.Media.Brushes.Green;
            }
            else
            {
                QuizFeedbackText.Foreground =
                    System.Windows.Media.Brushes.Red;
            }

            LogActivity(
                _quizManager.LastAnswerCorrect
                    ? "Quiz question answered correctly."
                    : "Quiz question answered incorrectly.");
        }

        private void NextQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (_quizManager.IsFinished)
            {
                RestartQuiz();
                return;
            }

            bool hasNextQuestion = _quizManager.MoveNext();

            if (hasNextQuestion)
            {
                LoadQuizQuestion();
            }
            else
            {
                ShowQuizResults();
            }
        }

        private void ShowQuizResults()
        {
            QuizProgressText.Text = "Quiz completed!";

            QuizQuestionText.Text =
                "You have completed the cybersecurity awareness quiz.";

            QuizOptionsPanel.Children.Clear();

            double percentage =
                (double)_quizManager.Score / _quizManager.TotalQuestions * 100;

            QuizFeedbackText.Text =
                $"Final result: {_quizManager.Score} out of {_quizManager.TotalQuestions} correct.";

            QuizScoreText.Text =
                $"Final Score: {percentage:F0}%";

            QuizFeedbackText.Foreground =
                System.Windows.Media.Brushes.DarkGreen;

            SubmitAnswerButton.IsEnabled = false;

            NextQuestionButton.Content = "Restart Quiz";
            NextQuestionButton.IsEnabled = true;

            LogActivity(
                $"Cybersecurity quiz completed. Score: {_quizManager.Score}/{_quizManager.TotalQuestions}.");
        }

        private void RestartQuiz()
        {
            _quizManager.Restart();

            LogActivity("Cybersecurity quiz restarted.");

            LoadQuizQuestion();
        }

        // -----------------------------------------
        // ACTIVITY LOG
        // -----------------------------------------

        private void LogActivity(string activity)
        {
            string entry =
                $"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - {activity}";

            ActivityLogList.Items.Insert(0, entry);

            // Keep the most recent 10 activities.
            while (ActivityLogList.Items.Count > 10)
            {
                ActivityLogList.Items.RemoveAt(
                    ActivityLogList.Items.Count - 1);
            }
        }
    }
}
