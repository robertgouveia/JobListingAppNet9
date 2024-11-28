using Devspot.Models;
using Devspot.Repositories;
using Devspot.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Devspot.Controllers;

public class JobPostingsController : Controller
{
    private readonly IRepository<JobPosting> _repository;
    private readonly UserManager<IdentityUser> _userManager;

    public JobPostingsController(
        IRepository<JobPosting> repository,
        UserManager<IdentityUser> userManager
    )
    {
        _repository = repository;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var jobPostings = await _repository.GetAllAsync();
        return View(jobPostings);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(JobPostingViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var posting = new JobPosting
            {
                Title = vm.Title,
                Description = vm.Description,
                Company = vm.Company,
                Location = vm.Location,
                UserId = _userManager.GetUserId(User)!, // User is from the Identity Package
            };
            await _repository.AddAsync(posting);
            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }
}
