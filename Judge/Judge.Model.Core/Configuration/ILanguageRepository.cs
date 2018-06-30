using System.Collections.Generic;
using Judge.Model.Core.Entities;

namespace Judge.Model.Core.Configuration
{
    public interface ILanguageRepository
    {
        IEnumerable<Language> GetLanguages();
        Language Get(int id);
        void Add(Language language);
        void Delete(Language language);
    }
}
