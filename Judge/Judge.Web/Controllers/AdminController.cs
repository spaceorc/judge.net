﻿using System.Collections.Generic;
using System.Web.Mvc;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Admin.Contests;
using Judge.Application.ViewModels.Admin.Languages;
using Judge.Application.ViewModels.Admin.Problems;
using Judge.Application.ViewModels.Admin.Users;

namespace Judge.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IProblemsService _problemsService;
        private readonly IContestsService _contestsService;
        private readonly ISecurityService securityService;

        public AdminController(IAdminService adminService, IProblemsService problemsService, IContestsService contestsService, ISecurityService securityService)
        {
            _adminService = adminService;
            _problemsService = problemsService;
            _contestsService = contestsService;
            this.securityService = securityService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Languages()
        {
            var model = _adminService.GetLanguages();

            return View(model);
        }

        [HttpPost]
        public ActionResult Languages(List<LanguageEditViewModel> languages)
        {
            languages.ForEach(o => TryValidateModel(o));
            if (!ModelState.IsValid)
            {
                return View(languages);
            }
            _adminService.SaveLanguages(languages);

            return RedirectToAction("Index");
        }

        public PartialViewResult Language()
        {
            return PartialView("Admin/Languages/_LanguageEditView", new LanguageEditViewModel());
        }

        public ActionResult Submits()
        {
            var model = _adminService.GetSubmitQueue();
            return View(model);
        }

        [HttpGet]
        public ActionResult EditProblem(long? id)
        {
            if (id == null)
            {
                return View(new EditProblemViewModel());
            }
            var model = _adminService.GetProblem(id.Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditProblem(EditProblemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var id = _adminService.SaveProblem(model);
                return RedirectToAction("EditProblem", new { id });
            }
            return View(model);
        }

        public ActionResult Problems(int? page)
        {
            var model = _problemsService.GetProblemsList(page ?? 1, 20, null, false);
            return View(model);
        }

        public ActionResult Contests()
        {
            var contests = _contestsService.GetContests(true);
            return View(contests);
        }

        public ActionResult EditContest(int? id)
        {
            var problems = _problemsService.GetAllProblems();
            ViewBag.Problems = problems;

            var model = id != null ? _adminService.GetContest(id.Value) : new EditContestViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult EditContest(EditContestViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = _adminService.SaveContest(model);
                return RedirectToAction("EditContest", new { id = model.Id });
            }
            var problems = _problemsService.GetAllProblems();
            ViewBag.Problems = problems;
            return View(model);
        }

        [HttpGet]
        public ActionResult Users()
        {
            var model = _adminService.GetUsers();
            return View(model);
        }

        [HttpGet]
        public ActionResult EditUser(long id)
        {
            var model = _adminService.GetUser(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        public ActionResult Edituser(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                securityService.UpdateUser(model);
                return RedirectToAction("Users");
            }

            return View(model);
        }

        public PartialViewResult Task()
        {
            var problems = _problemsService.GetAllProblems();
            ViewBag.Problems = problems;
            return PartialView("Admin/Contests/_TaskEditView", new TaskEditViewModel());
        }
    }
}