﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Security.Principal;
using System.Web;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels;
using Judge.Application.ViewModels.Problems.Solution;
using Judge.Application.ViewModels.Submit;
using Judge.Data;
using Judge.Model.CheckSolution;
using Judge.Model.Configuration;
using Judge.Model.SubmitSolution;

namespace Judge.Application
{
    internal sealed class SubmitSolutionService : ISubmitSolutionService
    {
        private readonly IUnitOfWorkFactory _factory;
        private readonly IPrincipal _principal;

        public SubmitSolutionService(IUnitOfWorkFactory factory, IPrincipal principal)
        {
            _factory = factory;
            _principal = principal;
        }

        public IEnumerable<LanguageViewModel> GetLanguages()
        {
            using (var uow = _factory.GetUnitOfWork())
            {
                var languageRepository = uow.LanguageRepository;

                return languageRepository.GetLanguages().Select(o => new LanguageViewModel
                {
                    Id = o.Id,
                    Name = o.Name
                }).ToArray();
            }
        }

        public void SubmitSolution(long problemId, int selectedLanguage, HttpPostedFileBase file, UserInfo userInfo)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            string sourceCode;
            using (var sr = new StreamReader(file.InputStream))
            {
                sourceCode = sr.ReadToEnd();
            }

            var submit = ProblemSubmit.Create();

            submit.ProblemId = problemId;
            submit.LanguageId = selectedLanguage;
            submit.UserId = userInfo.UserId;
            submit.FileName = Path.GetFileName(file.FileName);
            submit.SourceCode = sourceCode;
            submit.UserHost = userInfo.Host;
            submit.SessionId = userInfo.SessionId;

            using (var unitOfWork = _factory.GetUnitOfWork())
            {
                var submitRepository = unitOfWork.SubmitRepository;
                submitRepository.Add(submit);
                unitOfWork.Commit();
            }
        }

        public SolutionViewModel GetSolution(long submitResultId, long userId)
        {
            using (var unitOfWork = _factory.GetUnitOfWork())
            {
                var submitResultRepository = unitOfWork.SubmitResultRepository;
                var taskRepository = unitOfWork.TaskRepository;

                var result = submitResultRepository.Get(submitResultId);
                if (result == null)
                    return null;

                var hasPermission = _principal.IsInRole("admin");

                var submit = result.Submit;

                if (!hasPermission && submit.UserId != userId)
                {
                    throw new AuthenticationException();
                }

                var problem = taskRepository.Get(submit.ProblemId);

                var totalBytes = result.TotalBytes != null ? Math.Min(result.TotalBytes.Value, problem.MemoryLimitBytes) : (int?)null;
                var totalMilliseconds = result.TotalMilliseconds != null ? Math.Min(result.TotalMilliseconds.Value, problem.TimeLimitMilliseconds) : (int?)null;

                var submitViewModel = hasPermission ? new SubmitViewModel(totalBytes, totalMilliseconds)
                {
                    PassedTests = result.PassedTests,
                    Status = result.Status,
                    CompileOutput = result.CompileOutput,
                    RunDescription = result.RunDescription,
                    RunOutput = result.RunOutput,
                    UserHost = submit.UserHost,
                    SessionId = submit.SessionId
                } : null;
                return new SolutionViewModel
                {
                    ProblemId = submit.ProblemId,
                    SourceCode = submit.SourceCode,
                    ProblemName = problem.Name,
                    SubmitResults = submitViewModel
                };
            }
        }
    }
}
