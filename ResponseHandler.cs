
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

        // Delegate used to process messages.
        private readonly ResponseMethod _responseMethod;

        private readonly Dictionary<string, List<string>> _responses =
            new Dictionary<string, List<string>>
            {
                ["password"] = new List<string>
                {
                    "Use long, unique passwords for every account.",
                    "Use a password manager to help protect your passwords.",
                    "Enable multi-factor authentication wherever possible."
                },
                ["phishing"] = new List<string>
                {
                    "Check the sender's address and avoid suspicious links.",
                    "Never share passwords or verification codes through unexpected messages.",
                    "Verify suspicious messages through the organisation's official website."
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
                    "Avoid sharing sensitive personal information online.",
                    "Check app permissions and disable access you do not need."
                }
            };

        public ResponseHandler()
        {
            _responseMethod = GenerateResponse;
        }

        public string GetResponse(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return "Please enter a message so I can help you.";

            try
            {
                return _responseMethod(message.Trim());
            }
            catch (Exception)
            {
                return "Sorry, something went wrong. Please try asking your question again.";
            }
        }

        private string GenerateResponse(string message)
        {
            string input = message.Trim();
            string text = input.ToLowerInvariant();

            // Remember the user's name.
            if (text.StartsWith("my name is "))
            {
                _userName = input.Substring(11).Trim();

                return _userName.Length == 0
                    ? "Please tell me your name."
                    : $"Nice to meet you, {_userName}! I'll remember your name during this conversation.";
            }

            // Remember a favourite topic.
            if (text.StartsWith("my favourite topic is "))
            {
                _favouriteTopic = input.Substring(22).Trim();

                return _favouriteTopic.Length == 0
                    ? "Please tell me your favourite topic."
                    : $"I'll remember that your favourite topic is {_favouriteTopic}.";
            }

            // Remember interests expressed naturally.
            if (text.Contains("interested in"))
            {
                foreach (string topic in _responses.Keys)
                {
                    if (text.Contains(topic))
                    {
                        _favouriteTopic = topic;
                        _lastTopic = topic;

                        return $"Great! I'll remember that you're interested in {topic}. " +
                               GetRandomTip(topic);
                    }
                }
            }

            // Recall personal information.
            if (text.Contains("what is my name") ||
                text.Contains("what's my name"))
            {
                return string.IsNullOrEmpty(_userName)
                    ? "You haven't told me your name yet. Say 'My name is ...'."
                    : $"Your name is {_userName}.";
            }

            if (text.Contains("favourite topic") &&
                (text.Contains("what is") || text.Contains("what's") ||
                 text.Contains("remember")))
            {
                return string.IsNullOrEmpty(_favouriteTopic)
                    ? "You haven't told me your favourite topic yet."
                    : $"Your favourite topic is {_favouriteTopic}.";
            }

            // Detect sentiment and immediately provide relevant guidance.
            bool worried = ContainsAny(text, "worried", "scared", "afraid", "anxious");
            bool frustrated = ContainsAny(text, "frustrated", "confused", "overwhelmed");
            bool curious = text.Contains("curious");

            if (worried || frustrated || curious)
            {
                string topic = FindTopic(text);

                if (topic == "")
                    topic = _lastTopic;

                if (topic == "")
                    topic = _favouriteTopic;

                string acknowledgement = worried
                    ? "It's understandable to feel worried. "
                    : frustrated
                        ? "Cybersecurity can be confusing, but you can learn it step by step. "
                        : "It's good to be curious and learn how to stay safe. ";

                if (_responses.ContainsKey(topic))
                {
                    _lastTopic = topic;
                    return acknowledgement + GetRandomTip(topic);
                }

                return acknowledgement +
                       "You can ask me about passwords, phishing, scams or privacy.";
            }

            // Continue the current topic.
            if (ContainsAny(text, "another tip", "tell me more",
                "explain more", "more tips", "explain further"))
            {
                string topic = _lastTopic;

                if (topic == "")
                    topic = _favouriteTopic;

                return _responses.ContainsKey(topic)
                    ? GetRandomTip(topic)
                    : "Which topic interests you: passwords, phishing, scams or privacy?";
            }

            // Recognise cybersecurity keywords.
            string recognisedTopic = FindTopic(text);

            if (recognisedTopic != "")
            {
                _lastTopic = recognisedTopic;
                string tip = GetRandomTip(recognisedTopic);

                return string.IsNullOrEmpty(_userName)
                    ? tip
                    : $"{_userName}, {char.ToLowerInvariant(tip[0])}{tip.Substring(1)}";
            }

            if (ContainsAny(text, "hello", "hi", "hey"))
            {
                return string.IsNullOrEmpty(_userName)
                    ? "Hello! Ask me about passwords, phishing, scams or privacy."
                    : $"Hello, {_userName}! What would you like to learn about today?";
            }

            if (text.Contains("thank"))
                return "You're welcome! Staying informed is an important step towards online safety.";

            return "I'm not sure I understand. Please try rephrasing, or ask about passwords, phishing, scams or privacy.";
        }

        private string FindTopic(string text)
        {
            foreach (string topic in _responses.Keys)
            {
                if (text.Contains(topic))
                    return topic;
            }

            return "";
        }

        private string GetRandomTip(string topic)
        {
            List<string> tips = _responses[topic];
            return tips[_random.Next(tips.Count)];
        }

        private bool ContainsAny(string text, params string[] words)
        {
            return words.Any(word => text.Contains(word));
        }
    }
}
