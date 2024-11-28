using Devspot.Constants;
using Devspot.Models;
using Devspot.Repositories;
using Devspot.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Devspot.Controllers;

[Authorize]
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

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var allJobPostings= await _repository.GetAllAsync();
        if (!User.IsInRole(Roles.Employer)) return View(allJobPostings);
        
        var filteredJobPostings = allJobPostings.Where(x => x.UserId == _userManager.GetUserId(User));
        return View(filteredJobPostings);
    }
    
    [Authorize(Roles = $"{Roles.Admin}, {Roles.Employer}")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(JobPostingViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        
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
    
    [Authorize(Roles = $"{Roles.Admin}, {Roles.Employer}")]
    public async Task<IActionResult> Delete(int id)
    {
        var jobPosting = await _repository.GetByIdAsync(id);
        if (jobPosting is null) return NotFound();

        if (!User.IsInRole(Roles.Admin) && jobPosting.UserId !=  _userManager.GetUserId(User)) return Forbid();
        
        await _repository.DeleteAsync(jobPosting.Id);
        return Ok();
    }
}
