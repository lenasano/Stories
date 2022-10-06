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

        public async Task<List<Story>> GetAllStories()
        {
            try
            {
                Query storiesQuery = fireStoreDb.Collection("stories");
                QuerySnapshot storiesQuerySnapshot = await storiesQuery.GetSnapshotAsync();
                 
                List<Story> stories = new List<Story>();

                foreach (DocumentSnapshot storySnapshot in storiesQuerySnapshot.Documents)
                {
                    if (storySnapshot.Exists)
                    {
                        Dictionary<string, object> storyDictionary = storySnapshot.ToDictionary();
                        string storyJson = JsonSerializer.Serialize(storyDictionary);
                        Story? story = JsonSerializer.Deserialize<Story>(storyJson);

                        if (story is null) throw new NullReferenceException();

                        story.StoryId = storySnapshot.Id;
                        story.DateCreated = storySnapshot.CreateTime?.ToDateTime() ?? new DateTime();

                        stories.Add(story);
                    }
                }

                return stories.OrderBy(x => x.DateCreated).ToList();
            }
            catch
            {
                throw;      // todo: fix
            }
        }

        public async void AddStory(Story story)
        {
            try
            {
                CollectionReference stories = fireStoreDb.Collection("stories");
                await stories.AddAsync(story);
            }
            catch
            {
                throw;      // todo: fix
            }
        }

        /*  modifying stories is not supported in our app
        public async void UpdateEmployee(Story story)
        {
            try
            {
                DocumentReference storyDocument = fireStoreDb.Collection("stories").Document(story.StoryId);
                await storyDocument.SetAsync(story, SetOptions.Overwrite);
            }
            catch
            {
                throw;
            }
        }*/

        public async Task<Story> GetStoryData(string storyId)
        {
            try
            {
                DocumentReference storyDocument = fireStoreDb.Collection("stories").Document(storyId);
                DocumentSnapshot storySnapshot = await storyDocument.GetSnapshotAsync();

                if (storySnapshot.Exists)
                {
                    Story story = storySnapshot.ConvertTo<Story>();
                    story.StoryId = storySnapshot.Id;
                    return story;
                }
                else
                {
                    return new Story();
                }
            }
            catch
            {
                throw;      // todo
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
