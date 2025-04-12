namespace Chat_Client
{
    public class Msg_server
    {
        public string Command { get; set; }
        public string Text { get; set; }
        public string Sender { get; set; }

        public Dictionary<string, string> People { get; set; }

        public Msg_server(string command, string text, string sender, Dictionary<string, string> people)
        {

            Command = command;
            Text = text;
            Sender = sender;
            People = people;
        }
    }

    public class Msg_server_c
    {
        public string Command { get; set; }
        public string Text { get; set; }
        public Dictionary<string, string> People { get; set; }

        public Msg_server_c(string command, string text, Dictionary<string, string> people)
        {
            Command = command;
            Text = text;
            People = people;
        }
    }
}

