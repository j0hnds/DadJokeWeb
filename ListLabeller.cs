using System.Collections.Generic;

namespace DadJokeWeb
{
    public class ListLabeller
    {
        public string Label { get; set; }
        public List<string> Jokes { get; set; }

        public ListLabeller(string label, List<string> jokeList)
        {
            Label = label;
            Jokes = jokeList;
        }
    }
}
