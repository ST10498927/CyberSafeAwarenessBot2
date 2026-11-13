
using System;
using CyberSafeAwarenessBot2.Repositories;

namespace CyberSafeAwarenessBot2.Services
{
    public class IntentProcessor
    {
        public string DetectIntent(string message)
        {
            string input = message.ToLowerInvariant();

            if (ContainsAny(input, "show tasks", "view tasks",
                "list tasks", "my tasks"))
                return "VIEW_TASKS";

            if (ContainsAny(input, "add task", "create task",
                "new task", "remind me"))
                return "ADD_TASK";

            if (ContainsAny(input, "start quiz", "cyber quiz",
                "take quiz", "test my knowledge"))
                return "START_QUIZ";

            if (ContainsAny(input, "phishing", "suspicious email"))
                return "PHISHING";

            if (ContainsAny(input, "password", "strong password"))
                return "PASSWORD";

            if (ContainsAny(input, "privacy", "personal information"))
                return "PRIVACY";

            return "GENERAL";
        }

        private bool ContainsAny(string input, params string[] phrases)
        {
            foreach (string phrase in phrases)
            {
                if (input.Contains(phrase))
                    return true;
            }

            return false;
        }
    }
}
