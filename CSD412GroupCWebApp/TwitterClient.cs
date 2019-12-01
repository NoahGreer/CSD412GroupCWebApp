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

        private string url;
        private string message;

        public TwitterClient(SingleUserInMemoryCredentialStore credentials)
        {
            var auth = new SingleUserAuthorizer
            {
                CredentialStore = credentials
            };

            context = new TwitterContext(auth);
        }

        public void SetMessage(string newMessage)
        {
            message = newMessage;
        }

        public void SetLink(string newUrl)
        {
            url = newUrl;
        }

        public async Task<Status> SendTweet()
        {
            string status = message + " " + url;
            return await context.TweetAsync(status);
        }
    }
}