using POC.Response;

namespace POC.Input
{
    public class ChatGPTInputModel
    {
        public ChatGPTInputModel()
        {
            messages = new List<Message>();
        }

        public string model { get; set; }
        public List<Message> messages { get; set; }
    }

    public class Message
    {
        public string role { get; set; }
        public string content { get; set; }
    }
}
