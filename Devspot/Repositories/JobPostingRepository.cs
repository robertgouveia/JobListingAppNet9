using Devspot.Data;
using Devspot.Models;
using Microsoft.EntityFrameworkCore;

namespace Devspot.Repositories;

public class JobPostingRepository : IRepository<JobPosting>
{
    private readonly ApplicationDbContext _context;

    public JobPostingRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<JobPosting>> GetAllAsync()
        => (await _context.JobPostings.ToListAsync())!;

    public async Task<JobPosting?> GetByIdAsync(int id)
    {
        var job = await _context.JobPostings.FindAsync(id);
        return job ?? throw new KeyNotFoundException();
    }

    public async Task AddAsync(JobPosting entity)
    {
        await _context.JobPostings.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(JobPosting entity)
    {
        _context.JobPostings.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.JobPostings.FindAsync(id);
        if (entity is null) throw new NullReferenceException();
        _context.JobPostings.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
