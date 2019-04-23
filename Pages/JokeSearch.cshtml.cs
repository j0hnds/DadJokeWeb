using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DadJokeConsole;

namespace DadJokeWeb.Pages
{
    public class JokeSearchModel : PageModel
    {
        private ICanHazDadJokeAPI iCHDJApi = new ICanHazDadJokeAPI();
        private Regex validSearchRE = new Regex(@"^[\w'0-9]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public List<string> ShortJokes { get; private set; }
        public List<string> MediumJokes { get; private set; }
        public List<string> LongJokes { get; private set; }
        public string SearchTerm { get; private set; }

        private int CountWords(string str)
        {
            return Regex.Matches(str, @"(([\w'0-9]+(\s*)))").Count;
        }

        public void OnGet()
        {
            SearchTerm = Request.Query["term"];

            if (SearchTerm == null)
            {
                return;
            }

            // Make sure this is a 'valid' search term. I've decided that 'valid' means
            // that the search term is a single word that may contain alphanumeric characters
            // and an apostrophe.
            if (! validSearchRE.IsMatch(SearchTerm))
            {
                // For now, if the search term isn't 'valid', I'll just mash it 
                // down to an empty string. This situation should probably be
                // handled in the UI, but - at a minimum - I want to not pass a
                // bad search term along to the API.
                SearchTerm = "";
            }

            Task<JokeSearchResults> task = iCHDJApi.SearchForJokesAsync(SearchTerm);

            task.Wait();

            JokeSearchResults searchResults = task.Result;

            ShortJokes = new List<string>();
            MediumJokes = new List<string>();
            LongJokes = new List<string>();

            foreach (Joke j in searchResults.results)
            {
                int wordCount = CountWords(j.joke);
                if (wordCount < 10) 
                {
                    ShortJokes.Add(j.joke);
                } 
                else if (wordCount >= 10 && wordCount < 20)
                {
                    MediumJokes.Add(j.joke);
                }
                else if (wordCount >= 20)
                {
                    LongJokes.Add(j.joke);
                }
            }

        }
    }
}

