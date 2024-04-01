using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TripleSix.Core.Types
{
    /// <summary>
    /// Form file upload.
    /// </summary>
    public class FileInput
    {
        /// <summary>
        /// Id định danh.
        /// </summary>
        [Required]
        [DisplayName("File cần upload")]
        [FromForm]
        public IFormFile File { get; set; }
    }
}
