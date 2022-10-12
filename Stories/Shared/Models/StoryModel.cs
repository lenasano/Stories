using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Google.Cloud.Firestore;

namespace Stories.Shared.Models
{
    public class StoryViewsDownloadsInfo
    {
        public List<string> DateStrings { get; set; } = new();

        public List<double> NumberOfDownloads { get; set; } = new();

        public List<double> NumberOfViews { get; set; } = new();
    }

    [FirestoreData]
    public class StoryModel
    {
        const int FIRST_PARAGRAPH_MAX_LENGTH = 300;

        #region public properties

        public string StoryId { get; set; } = string.Empty;

        public DateTime DateCreated { get; set; } = new DateTime();


        [FirestoreProperty] 
        public string Title { get; set; } = string.Empty;

        [FirestoreProperty] 
        public string FirstParagraph { get; set; } = string.Empty;

        public string FullText { get; set; } = string.Empty;

        #endregion public properties

        public void SetFirstParagraph()
        {
            if (!string.IsNullOrWhiteSpace(FirstParagraph)) return;

            FirstParagraph = FullText.Length < FIRST_PARAGRAPH_MAX_LENGTH  ? FullText       : FullText.Substring(0, FIRST_PARAGRAPH_MAX_LENGTH);
            FirstParagraph = !FirstParagraph.Contains(Environment.NewLine) ? FirstParagraph : FirstParagraph.Substring(0, FirstParagraph.IndexOf(Environment.NewLine));
            FirstParagraph = FirstParagraph.Trim();
        }

        /// <summary>
        /// Replaces the schema placeholders with this instance's property values.
        /// </summary>
        /// <remarks>
        /// If using Adaptive Cards with Blazor Server, then this can be done by the Blazor component itself, using the <b>Models</b> property.
        /// </remarks>
        /// <param name="schemaTemplate"></param>
        public string PopulateSchema(string schemaTemplate)
        {
            typeof(StoryModel)
                .GetProperties()
                .ToList()
                .ForEach(
                    p =>
                    {
                        schemaTemplate = schemaTemplate.Replace(
                            $"{{{{{p.Name}}}}}",
                            p.PropertyType == typeof(DateTime) ? (p.GetValue(this) as DateTime?)?.ToString("dddd, MMMM d, yyyy") : p.GetValue(this)?.ToString()
                            );
                    }
                );

            return schemaTemplate;
        }
    }
}
