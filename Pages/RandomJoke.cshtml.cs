using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DadJokeConsole;

namespace DadJokeWeb.Pages
{
    public class RandomJokeModel : PageModel
    {
        private ICanHazDadJokeAPI iCHDJApi = new ICanHazDadJokeAPI();
        public Joke RandomJoke { get; private set; }

        public void OnGet()
        {
            Task<Joke> task = iCHDJApi.GetRandomJokeAsync();

            task.Wait();

            RandomJoke = task.Result;
        }
    }
}

