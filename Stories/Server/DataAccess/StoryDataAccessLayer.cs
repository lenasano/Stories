using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stories.Shared.Models;
using Google.Cloud.Firestore;
using System.Text.Json;
using Stories.Client.Pages;

namespace Stories.Server.DataAccess
{
    // connects to Firestore, Google's Cloud-hosted database
    public enum StoryIncrementValues
    {
        NumberOfViews,
        NumberOfDownloads
    }

    public class StoryDataAccessLayer
    {
        FirestoreDb firestoreDb;
        string projectId;

        public StoryDataAccessLayer()
        {
            string filepath = 
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "PUT YOUR PROJECT SERVICE ACCOUNT KEY JSON FILE HERE" // https://cloud.google.com/iam/docs/creating-managing-service-account-keys
                );
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);

            projectId = "PUT YOUR PROJECT ID HERE"; // you will see your Project ID at: Firebase console | click on your project | Project Overview | Project settings https://console.firebase.google.com/u/1/
            firestoreDb = FirestoreDb.Create(projectId);
        }

        #region functions to call retrieve, set, or update data in firebase

        public async Task<List<StoryModel>> GetAllStoriesAsync()
        {
            try
            {
                Query storiesQuery = firestoreDb.Collection("stories");
                QuerySnapshot storiesQuerySnapshot = await storiesQuery.GetSnapshotAsync();
                 
                List<StoryModel> stories = new List<StoryModel>();

                foreach (DocumentSnapshot storySnapshot in storiesQuerySnapshot.Documents)
                {
                    if (storySnapshot.Exists)
                    {
                        Dictionary<string, object> storyDictionary = storySnapshot.ToDictionary();
                        string storyJson  = JsonSerializer.Serialize(storyDictionary);
                        StoryModel? story = JsonSerializer.Deserialize<StoryModel>(storyJson);

                        if (story is null) throw new NullReferenceException();

                        story.StoryId = storySnapshot.Id;
                        story.DateCreated = storySnapshot.CreateTime?.ToDateTime() ?? new DateTime();

                        stories.Add(story);     // check that date created and storyid are populated
                    }
                }

                return stories.OrderBy(x => x.DateCreated).ToList();
            }
            catch (Exception)
            {
                throw new Exception();      // reset stack trace for security purposes
            }
        }

        public async Task<StoryModel> GetStoryAsync(string storyId)
        {
            try
            {
                DocumentReference storyDocument = firestoreDb.Collection("stories").Document(storyId);
                DocumentSnapshot storySnapshot = await storyDocument.GetSnapshotAsync();

                if (storySnapshot.Exists)
                {
                    StoryModel story = storySnapshot.ConvertTo<StoryModel>();

                    story.StoryId = storySnapshot.Id;
                    story.DateCreated = storySnapshot.CreateTime?.ToDateTime() ?? new DateTime();

                    return story;
                }
                else
                {
                    return new StoryModel();
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<string> AddStoryAsync(StoryModel story)
        {
            try
            {
                CollectionReference stories = firestoreDb.Collection("stories");
                DocumentReference storyRef = await stories.AddAsync(story);

                return storyRef.Id;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task IncrementStoryStatisticsAsync(string storyId, StoryIncrementValues incrementField)
        {
            try
            {
                string? today = DateOnly.FromDateTime(DateTime.Today).ToString();

                // get or create a document for today's views and downloads
                DocumentReference viewsDownloadsTodayRef = 
                   firestoreDb.Collection("stories").Document(storyId).Collection("ViewsDownloads").Document(today);

                // create or update the numberOfViews field in this document
                WriteResult incrementViewsResult = await
                    viewsDownloadsTodayRef.SetAsync(
                        new Dictionary<string, object> {
                            { incrementField.ToString(), FieldValue.Increment(1) },
                            { "Date", today }
                        }, 
                        SetOptions.MergeAll
                    );
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<StoryViewsDownloadsInfo> GetStoryStatisticsAsync(string storyId)
        {
            StoryViewsDownloadsInfo statistics = new();

            Query storyStatisticsQuery = firestoreDb.Collection($"stories/{storyId}/ViewsDownloads").OrderBy("Date");   // no .Where() filter, so get all documents in the ViewDownloads collection
            QuerySnapshot storyStatisticsSnapshot = await storyStatisticsQuery.GetSnapshotAsync();

            foreach(DocumentSnapshot daysStatistics in storyStatisticsSnapshot.Documents)
            {
                double views = 0;
                double downloads = 0;
                string date = string.Empty;

                daysStatistics.TryGetValue<double>(StoryIncrementValues.NumberOfViews.ToString(), out views);
                daysStatistics.TryGetValue<double>(StoryIncrementValues.NumberOfDownloads.ToString(), out downloads);
                daysStatistics.TryGetValue<string>("Date", out date);


                statistics.NumberOfViews.Add(views);
                statistics.NumberOfDownloads.Add(downloads);
                statistics.DateStrings.Add(DateTime.Parse(date).ToString("MMM d"));
            }

            return statistics;
        }
        #endregion functions to call retrieve, set, or update data in firebase
    }
}
