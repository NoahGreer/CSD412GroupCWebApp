using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToTwitter;

namespace CSD412GroupCWebApp
{
    public class TwitterTest
    {
        private static string url = "";

        void SetLink(string newUrl) {
            url = newUrl;
        }


        
        static async Task<Status> SendTweet()
        {
            var auth = new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = "ZHYa7RSBFLr4prXkGPGKUYlo1",
                    ConsumerSecret = "ZvOBubfKo3zgQXkxB9NHgufIO5GSkka43VBW16a4MUbAbZFlkC",
                    AccessToken = "1198377505696010240-Pf54zkzIwJsYpxcAdRc6m4GYsqQ3Gh",
                    AccessTokenSecret = "VGsWAk5msP2o16EalQhbWRyJB9fGmZa5IS9AcggOerFQl"
                }
            };
            var context = new TwitterContext(auth);

            var status = await context.TweetAsync(url);
            return status;

        }

    }
}
