using System.Collections.Generic;
using CyberSafeAwarenessBot2.Models;

namespace CyberSafeAwarenessBot2.Services
{
    public class QuizManager
    {
        private readonly List<QuizQuestion> _questions;
        private int _currentIndex;
        private int _score;

        public QuizManager()
        {
            _questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "What is phishing?",
                    Options = new[]
                    {
                        "A type of computer hardware",
                        "A fraudulent attempt to steal sensitive information",
                        "A method of backing up files",
                        "A secure login process"
                    },
                    CorrectAnswerIndex = 1,
                    Explanation = "Phishing uses deceptive messages or websites to steal personal information."
                },
                new QuizQuestion
                {
                    Question = "Which password is the strongest?",
                    Options = new[]
                    {
                        "password123",
                        "12345678",
                        "Derz2026",
                        "T!m3$afe#92_River"
                    },
                    CorrectAnswerIndex = 3,
                    Explanation = "A strong password uses a combination of characters and is difficult to guess."
                },
                new QuizQuestion
                {
                    Question = "What should you do with a suspicious email attachment?",
                    Options = new[]
                    {
                        "Open it immediately",
                        "Forward it to everyone",
                        "Avoid opening it and report the email",
                        "Download it to check what it contains"
                    },
                    CorrectAnswerIndex = 2,
                    Explanation = "Suspicious attachments may contain malware. Do not open them."
                },
                new QuizQuestion
                {
                    Question = "What does MFA stand for?",
                    Options = new[]
                    {
                        "Multiple File Access",
                        "Multi-Factor Authentication",
                        "Main Firewall Application",
                        "Managed File Approval"
                    },
                    CorrectAnswerIndex = 1,
                    Explanation = "MFA requires more than one form of verification when accessing an account."
                },
                new QuizQuestion
                {
                    Question = "Why should you install security updates?",
                    Options = new[]
                    {
                        "To make your screen brighter",
                        "To remove all your files",
                        "To fix vulnerabilities and improve security",
                        "To disable your antivirus"
                    },
                    CorrectAnswerIndex = 2,
                    Explanation = "Updates often fix security weaknesses that attackers could exploit."
                },
                new QuizQuestion
                {
                    Question = "Is it safe to use the same password for every account?",
                    Options = new[]
                    {
                        "True",
                        "False"
                    },
                    CorrectAnswerIndex = 1,
                    Explanation = "Reusing passwords means one compromised account could expose your other accounts."
                },
                new QuizQuestion
                {
                    Question = "What is malware?",
                    Options = new[]
                    {
                        "Software designed to harm or exploit systems",
                        "A type of secure password",
                        "A computer maintenance tool",
                        "A networking cable"
                    },
                    CorrectAnswerIndex = 0,
                    Explanation = "Malware is malicious software such as viruses, spyware and ransomware."
                },
                new QuizQuestion
                {
                    Question = "What should you do when using public Wi-Fi?",
                    Options = new[]
                    {
                        "Access banking sites without protection",
                        "Share your passwords",
                        "Disable all security settings",
                        "Avoid sensitive transactions on unsecured networks"
                    },
                    CorrectAnswerIndex = 3,
                    Explanation = "Unsecured public networks can expose your information to attackers."
                },
                new QuizQuestion
                {
                    Question = "What is social engineering?",
                    Options = new[]
                    {
                        "Building social media websites",
                        "Manipulating people into revealing information or taking unsafe actions",
                        "Installing computer hardware",
                        "Creating strong passwords"
                    },
                    CorrectAnswerIndex = 1,
                    Explanation = "Social engineering exploits human trust rather than relying only on technical attacks."
                },
                new QuizQuestion
                {
                    Question = "Should you share your one-time password (OTP) with someone who calls you?",
                    Options = new[]
                    {
                        "True",
                        "False"
                    },
                    CorrectAnswerIndex = 1,
                    Explanation = "Never share an OTP. Legitimate service providers should not ask you to disclose it."
                },
                new QuizQuestion
                {
                    Question = "What is ransomware?",
                    Options = new[]
                    {
                        "Software that improves computer speed",
                        "A secure file-sharing service",
                        "Malware that locks or encrypts data and demands payment",
                        "A password recovery tool"
                    },
                    CorrectAnswerIndex = 2,
                    Explanation = "Ransomware blocks access to data and demands payment to restore access."
                },
                new QuizQuestion
                {
                    Question = "What is the safest response to an unexpected password-reset link?",
                    Options = new[]
                    {
                        "Click it immediately",
                        "Verify the request through the official website or app",
                        "Send your password to the sender",
                        "Forward it to your contacts"
                    },
                    CorrectAnswerIndex = 1,
                    Explanation = "Use the official website or app instead of trusting unexpected links."
                }
            };

            Start();
        }

        public QuizQuestion CurrentQuestion =>
            _questions[_currentIndex];

        public int CurrentQuestionNumber =>
            _currentIndex + 1;

        public int TotalQuestions =>
            _questions.Count;

        public int Score =>
            _score;

        public bool IsFinished =>
            _currentIndex >= _questions.Count;

        public bool HasAnswered { get; private set; }

        public bool LastAnswerCorrect { get; private set; }

        public void Start()
        {
            _currentIndex = 0;
            _score = 0;
            HasAnswered = false;
            LastAnswerCorrect = false;
        }

        public string SubmitAnswer(int selectedAnswerIndex)
        {
            if (IsFinished || HasAnswered)
                return "Please move to the next question.";

            if (selectedAnswerIndex < 0 ||
                selectedAnswerIndex >= CurrentQuestion.Options.Length)
            {
                return "Please select a valid answer.";
            }

            HasAnswered = true;

            LastAnswerCorrect =
                selectedAnswerIndex == CurrentQuestion.CorrectAnswerIndex;

            if (LastAnswerCorrect)
            {
                _score++;
                return "Correct! " + CurrentQuestion.Explanation;
            }

            string correctAnswer =
                CurrentQuestion.Options[CurrentQuestion.CorrectAnswerIndex];

            return "Incorrect. The correct answer is: "
                + correctAnswer + ". "
                + CurrentQuestion.Explanation;
        }

        public bool MoveNext()
        {
            if (!HasAnswered || IsFinished)
                return false;

            _currentIndex++;
            HasAnswered = false;
            LastAnswerCorrect = false;

            return !IsFinished;
        }

        public void Restart()
        {
            Start();
        }
    }
}