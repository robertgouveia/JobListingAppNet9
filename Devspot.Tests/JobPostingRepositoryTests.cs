using Devspot.Data;
using Devspot.Models;
using Devspot.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Devspot.Tests;

public class JobPostingRepositoryTests
{
    // Options
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions 
        = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("JobPostingDb").Options;

    // Context
    private ApplicationDbContext CreateDbContext() => new (_dbContextOptions);

    [Fact]
    public async Task AddAsync_ShouldAddJobPosting()
    {
        // Db Context
        var db = CreateDbContext();
        
        // Job Hosting Repository
        var repository = new JobPostingRepository(db);
        
        // Job Posting
        var jobPosting = new JobPosting
        {
            Title = "Test Title",
            Description = "Test Description",
            PostedDate = DateTime.Now,
            Company = "Company",
            Location = "Test Location",
            UserId = "Test User ID"
        };

        // Execute
        await repository.AddAsync(jobPosting);

        // Check Result
        var result = await db.JobPostings.FirstOrDefaultAsync(x => x.Title == "Test Title");
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Title", result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnJobPosting()
    {
        var db = CreateDbContext();
        var repository = new JobPostingRepository(db);
        
        var jobPosting = new JobPosting
        {
            Title = "Test Title",
            Description = "Test Description",
            PostedDate = DateTime.Now,
            Company = "Company",
            Location = "Test Location",
            UserId = "Test User ID"
        };

        await db.JobPostings.AddAsync(jobPosting); // Not testing our repository for the add method
        await db.SaveChangesAsync();

        var result = await repository.GetByIdAsync(jobPosting.Id);

        Assert.NotNull(result);
        Assert.Equal("Test Title", result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowKeyNotFoundException()
    {
        var db = CreateDbContext();
        var repository = new JobPostingRepository(db);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => repository.GetByIdAsync(999));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllJobPostings()
    {
        var db = CreateDbContext();
        var repository = new JobPostingRepository(db);

        JobPosting[] jobPostings =
        [
            new()
            {
                Title = "Test Title",
                Description = "Test Description",
                PostedDate = DateTime.Now,
                Company = "Company",
                Location = "Test Location",
                UserId = "Test User ID"
            },
            new()
            {
                Title = "Test Title 2",
                Description = "Test Description 2",
                PostedDate = DateTime.Now,
                Company = "Company 2",
                Location = "Test Location 2",
                UserId = "Test User ID 2"
            }
        ];

        await db.JobPostings.AddRangeAsync(jobPostings);
        await db.SaveChangesAsync();

        var result = await repository.GetAllAsync();

        Assert.NotNull(result);
        Assert.True(result.Count() >= 2);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateJobPosting()
    {
        var db = CreateDbContext();
        var repository = new JobPostingRepository(db);
        
        var jobPosting = new JobPosting
        {
            Title = "Test Title",
            Description = "Test Description",
            PostedDate = DateTime.Now,
            Company = "Company",
            Location = "Test Location",
            UserId = "Test User ID"
        };

        await db.JobPostings.AddAsync(jobPosting); // Not testing our repository for the add method
        await db.SaveChangesAsync();

        jobPosting.Description = "Updated Description";
        await repository.UpdateAsync(jobPosting);

        var result = await db.JobPostings.FindAsync(jobPosting.Id);

        Assert.NotNull(result);
        Assert.Equal("Updated Description", result.Description);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveJobPosting()
    {
        var db = CreateDbContext();
        var repository = new JobPostingRepository(db);
        
        var jobPosting = new JobPosting
        {
            Title = "Test Title",
            Description = "Test Description",
            PostedDate = DateTime.Now,
            Company = "Company",
            Location = "Test Location",
            UserId = "Test User ID"
        };

        await db.JobPostings.AddAsync(jobPosting);
        await db.SaveChangesAsync();

        await repository.DeleteAsync(jobPosting.Id);
        var result = await db.JobPostings.FindAsync(jobPosting.Id);
        
        Assert.Null(result);
    }
}