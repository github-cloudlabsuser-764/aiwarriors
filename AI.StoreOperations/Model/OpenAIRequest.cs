namespace AI.StoreOperations.Model
{
    public class Message
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    public class OpenAIRequest
    {
        public List<Message> messages { get; set; }
        public int max_tokens { get; set; }
        public double temperature { get; set; }
        public int frequency_penalty { get; set; }
        public int presence_penalty { get; set; }
        public double top_p { get; set; }
        public object stop { get; set; }
    }
}
