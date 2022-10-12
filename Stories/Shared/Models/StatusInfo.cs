using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stories.Shared.Models
{
    /// <summary>
    /// Class for displaying error messages on razor pages.
    /// </summary>
    public class StatusInfo
    {
        public string BootstrapAlertType { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
