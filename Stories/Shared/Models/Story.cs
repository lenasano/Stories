using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace Stories.Shared.Models
{
    [FirestoreData]
    public class Story
    {
        public string StoryId { get; set; } = String.Empty;
        public DateTime DateCreated { get; set; } = new DateTime();
        [FirestoreProperty]
        public string Title { get; set; } = String.Empty;
        [FirestoreProperty]
        public string FirstParagraph { get; set; } = String.Empty;
        [FirestoreProperty]
        public int NumberOfPageViews { get; set; } = 0;
        [FirestoreProperty]
        public int NumberOfDownloads { get; set; } = 0;

        public string Title2 { get; set; } = "Wonderful";

    }
}
