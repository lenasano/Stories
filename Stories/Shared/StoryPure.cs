using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace Stories.Shared
{
    public class StoryPure
    {
        public string StoryId { get; set; } = String.Empty;
        public DateTime DateCreated { get; set; } = new DateTime();

        public string Title { get; set; } = "Hello";

        public string FirstParagraph { get; set; } = "How do you like this first paragraph? It is the best paragraph ever. Never seen one like it before.";

        public int NumberOfPageViews { get; set; } = 0;

        public int NumberOfDownloads { get; set; } = 0;

        public string Title2 { get; set; } = "Wonderful";
    }
}
