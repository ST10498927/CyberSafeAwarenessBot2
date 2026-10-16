
using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSafeAwarenessBot2
{
    public delegate string ResponseMethod(string message);

    public class ResponseHandler
    {
        private readonly Random _random = new Random();
        private string _lastTopic = "";
        private string _userName = "";
        private string _favouriteTopic = "";

        private readonly Dictionary<string, List<string>> _responses =
            new Dictionary<string, List<string>>
            {
                ["password"] = new List<string>
                {
                    "Use a long, unique password for every account.",
                    "Consider using a password manager to store strong passwords.",
                    "Enable multi-factor authentication wherever possible."
                },
                ["phishing"] = new List<string>
                {
                    "Check the sender's address and avoid suspicious links.",
                    "Never share passwords or verification codes through unexpected messages.",
                    "If a message creates urgency, verify it through the organisation's official website."
                },
                ["scam"] = new List<string>
                {
                    "Be cautious of offers that promise easy money or guaranteed prizes.",
                    "Never send money or personal information to an unverified contact.",
                    "Verify payment requests independently before acting."
                },
                ["privacy"] = new List<string>
                {
                    "Review your privacy settings on social media.",
                    "Avoid sharing your address, passwords or other sensitive information online.",
                    "Check app permissions and disable access you do not need."
                }
            };

        public string GetResponse(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return "Please enter a message so I can help you.";

            try
            {
                string input = message.Trim();
                string lowerInput = input.ToLowerInvariant();

                // Remember the user's name.
                if (lowerInput.StartsWith("my name is "))
                {
                    _userName = input.Substring(11).Trim();

                    if (_userName.Length == 0)
                        return "Please tell me your name after 'My name is'.";

                    return $"Nice to meet you, {_userName}! I'll remember your name during this conversation.";
                }

                // Remember the user's favourite topic.
                if (lowerInput.StartsWith("my favourite topic is "))
                {
                    _favouriteTopic = input.Substring(22).Trim();

                    if (_favouriteTopic.Length == 0)
                        return "Please tell me your favourite topic.";

                    return $"I'll remember that your favourite topic is {_favouriteTopic}.";
                }

                if (lowerInput.Contains("what is my name") ||
                    lowerInput.Contains("what's my name"))
                {
                    return string.IsNullOrEmpty(_userName)
                        ? "You haven't told me your name yet. Try saying 'My name is ...'."
                        : $"Your name is {_userName}.";
                }

                if (lowerInput.Contains("favourite topic") &&
                    (lowerInput.Contains("remember") ||
                     lowerInput.Contains("what is") ||
                     lowerInput.Contains("what's")))
                {
                    return string.IsNullOrEmpty(_favouriteTopic)
                        ? "You haven't told me your favourite topic yet."
                        : $"Your favourite topic is {_favouriteTopic}.";
                }

                // Detect feelings and provide useful guidance.
                if (lowerInput.Contains("worried") ||
                    lowerInput.Contains("scared") ||
                    lowerInput.Contains("frustrated"))
                {
                    if (lowerInput.Contains("scam") ||
                        lowerInput.Contains("phishing"))
                    {
                        _lastTopic = lowerInput.Contains("phishing")
                            ? "phishing" : "scam";

                        return "It's understandable to feel concerned. Don't click suspicious links or share personal details. " +
                               _responses[_lastTopic][_random.Next(_responses[_lastTopic].Count)];
                    }

                    return "I understand that this can feel stressful. Take your time, and I can help you learn how to stay safe online.";
                }

                if (lowerInput.Contains("curious"))
                {
                    return "It's good to be curious! Ask me about passwords, phishing, scams or privacy.";
                }

                // Handle requests for another tip or more information.
                if (lowerInput.Contains("another tip") ||
                    lowerInput.Contains("tell me more") ||
                    lowerInput.Contains("explain more") ||
                    lowerInput.Contains("more tips"))
                {
                    if (_responses.ContainsKey(_lastTopic))
                        return _responses[_lastTopic][
                            _random.Next(_responses[_lastTopic].Count)];

                    return "Which topic would you like to explore: passwords, phishing, scams or privacy?";
                }

                // Recognise cybersecurity topics.
                foreach (var topic in _responses.Keys)
                {
                    if (lowerInput.Contains(topic))
                    {
                        _lastTopic = topic;
                        string response = _responses[topic][
                            _random.Next(_responses[topic].Count)];

                        return string.IsNullOrEmpty(_userName)
                            ? response
                            : $"{_userName}, {char.ToLower(response[0])}{response.Substring(1)}";
                    }
                }

                if (lowerInput == "hi" || lowerInput == "hello" ||
                    lowerInput.StartsWith("hi ") || lowerInput.StartsWith("hello "))
                {
                    return string.IsNullOrEmpty(_userName)
                        ? "Hello! Ask me about passwords, phishing, scams or privacy. You can also tell me your name."
                        : $"Hello, {_userName}! What would you like to learn about today?";
                }

                return "I'm not sure I understand. Try asking about passwords, phishing, scams or privacy.";
            }
            catch (Exception)
            {
                return "Sorry, something went wrong while processing your message. Please try again.";
            }
        }
    }
}
