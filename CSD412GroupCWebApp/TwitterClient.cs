using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToTwitter;

namespace CSD412GroupCWebApp
{
    public class TwitterClient
    {
        private TwitterContext context;

        private static string url;

        public TwitterClient(SingleUserInMemoryCredentialStore credentials)
        {
            var auth = new SingleUserAuthorizer
            {
                CredentialStore = credentials
            };

            context = new TwitterContext(auth);
        }

        public void SetLink(string newUrl)
        {
            url = newUrl;
        }

        public async Task<Status> SendTweet()
        {
            return await context.TweetAsync(url);
        }
    }
}