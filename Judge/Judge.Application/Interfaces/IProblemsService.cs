﻿using Judge.Application.ViewModels.Problems.ProblemsList;
using Judge.Application.ViewModels.Problems.Statement;

namespace Judge.Application.Interfaces
{
    public interface IProblemsService
    {
        /// <summary>
        /// Get problems list for one page
        /// </summary>
        /// <param name="page">Page number from 1 to N</param>
        /// <param name="pageSize">Page size from 1 to N</param>
        ProblemsListViewModel GetProblemsList(int page, int pageSize);

        StatementViewModel GetStatement(long id);
    }
}