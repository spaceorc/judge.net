using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Judge.Application.ViewModels.Submit
{
    public sealed class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        public override bool IsValid(object value)
        {
            var file = value as IFormFile;
            return file?.Length <= _maxFileSize;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"Размер файла не может превышать {_maxFileSize / 1024.0} килобайт";
        }
    }
}
