using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Judge.Application.Core;
using Microsoft.AspNetCore.Http;

namespace Judge.Application.ViewModels.Submit
{
    public sealed class SubmitSolutionViewModel
    {
        public IEnumerable<LanguageViewModel> Languages { get; set; }
        public int SelectedLanguage { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "SelectFile")]
        [FileValidation]
        [MaxFileSize(100 * 1024)]
        public IFormFile File { get; set; }

        public bool Success { get; set; }
        public long ProblemId { get; set; }
    }
}
