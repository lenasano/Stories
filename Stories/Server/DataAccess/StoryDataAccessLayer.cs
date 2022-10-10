using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stories.Shared.Models;
using Google.Cloud.Firestore;
using System.Text.Json;

namespace Stories.Server.DataAccess
{
    // connects to Firestore, Google's Cloud-hosted database

    public class StoryDataAccessLayer
    {
        FirestoreDb fireStoreDb;
        string projectId;

        public StoryDataAccessLayer()
        {
            string filepath = 
                Path.Combine(
                    Directory.GetCurrentDirectory(), 
                    "stories-23368-a1d801226f17.json"
                );
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);

            projectId = "stories-23368";
            fireStoreDb = FirestoreDb.Create(projectId);
        }

        public async Task<List<StoryModel>> GetAllStoriesAsync()
        {
            try
            {
                Query storiesQuery = fireStoreDb.Collection("stories");
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

                        stories.Add(story);
                    }
                }

                return stories.OrderBy(x => x.DateCreated).ToList();
            }
            catch (Exception)
            {
                throw new Exception();      // reset stack trace for security purposes
            }
        }

        public async Task<string> AddStoryAsync(StoryModel story)
        {
            try
            {
                CollectionReference stories = fireStoreDb.Collection("stories");
                DocumentReference storyRef = await stories.AddAsync(story);
                return storyRef.Id;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task UpdateStoryPageViewAsync()
        {
            try
            {
                /*
                DocumentReference storyDocument = fireStoreDb.Collection("stories").Document(story.StoryId);
                await storyDocument.SetAsync(story, SetOptions.Overwrite);
                */

                await Task.Delay(1);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<StoryModel> GetStoryAsync(string storyId)
        {
            try
            {
                DocumentReference storyDocument = fireStoreDb.Collection("stories").Document(storyId);
                DocumentSnapshot storySnapshot = await storyDocument.GetSnapshotAsync();

                if (storySnapshot.Exists)
                {
                    StoryModel story = storySnapshot.ConvertTo<StoryModel>();
                    story.StoryId = storySnapshot.Id;

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

        /* deleting stories is not supported in our scenerio
        public async void DeleteStory(string storyId)
        {
            try
            {
                DocumentReference storyDocument = fireStoreDb.Collection("stories").Document(storyId);
                await storyDocument.DeleteAsync();
            }
            catch
            {
                throw;
            }
        }*/
    }
}
