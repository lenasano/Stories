using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stories.Shared
{
    public class UploadedStory
    {
        public bool Uploaded { get; set; }
        public string FileName { get; set; }
        public string StoredFileName { get; set; }
        public int ErrorCode { get; set; }

        // id, title, text (first paragraph text), imagefilename.png, number of page views, number of downloads
    }
}
