using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Google.Cloud.Firestore;

namespace Stories.Shared.Models
{
    [FirestoreData]
    public class StoryModel
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

        /// <summary>
        /// Replaces the schema placeholders with this instance's property values.
        /// </summary>
        /// <remarks>
        /// If using Adaptive Cards with Blazor Server, then this can be done by the Blazor component itself, using the <b>Models</b> property.
        /// </remarks>
        /// <param name="schemaTemplate"></param>
        public string PopulateSchema(string schemaTemplate)
        {
            /*foreach (PropertyInfo p in typeof(Story).GetProperties())
            {
                schemaTemplate = 
                    schemaTemplate.Replace(
                        $"{{{{{p.Name}}}}}", 
                        p.GetValue(this)?.ToString()
                );
            }*/

            typeof(StoryModel)
                .GetProperties()
                .ToList()
                .ForEach(
                    p =>
                    {
                        schemaTemplate = schemaTemplate.Replace(
                            $"{{{{{p.Name}}}}}",
                            p.GetValue(this)?.ToString()
                            );
                    }
                );

            return schemaTemplate;
        }
    }
}
