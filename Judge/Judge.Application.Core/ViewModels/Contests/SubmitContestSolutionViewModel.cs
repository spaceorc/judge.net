using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Judge.Application.Core;
using Judge.Application.ViewModels.Submit;
using Microsoft.AspNetCore.Http;

namespace Judge.Application.ViewModels.Contests
{
    public class SubmitContestSolutionViewModel
    {
        public IEnumerable<LanguageViewModel> Languages { get; set; }
        public int SelectedLanguage { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "SelectFile")]
        [FileValidation]
        [MaxFileSize(100 * 1024)]
        public IFormFile File { get; set; }

        public bool Success { get; set; }
        public string Label { get; set; }
        public int ContestId { get; set; }
    }
}