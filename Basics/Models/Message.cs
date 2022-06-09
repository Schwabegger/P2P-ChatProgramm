namespace Basics.Models
{
    public class Message
    {
        public string Content { get; set; }
        public User Sender { get; set; }

        public Message(User sender, string content)
        {
            Content = content;
            Sender = sender;
        }
    }
}