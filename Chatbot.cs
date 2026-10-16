
namespace CyberSafeAwarenessBot2
{
    public class Chatbot
    {
        private readonly ResponseHandler _responseHandler;

        public Chatbot()
        {
            _responseHandler = new ResponseHandler();
        }

        public string GetResponse(string userMessage)
        {
            return _responseHandler.GetResponse(userMessage);
        }
    }
}
