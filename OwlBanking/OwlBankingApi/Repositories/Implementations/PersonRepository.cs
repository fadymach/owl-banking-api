using Microsoft.EntityFrameworkCore;
using OwlBankingApi.Database;
using OwlBankingApi.Models;
using OwlBankingApi.Repositories.Interfaces;

namespace OwlBankingApi.Repositories.Implementations;

public class PersonRepository : IPersonRepository
{
    private OwlBankingDbContext _dbContext;

    public PersonRepository(OwlBankingDbContext dbContext)
    {
        this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<IList<Person>> GetPersonsAsync()
    {
        return await this._dbContext.Persons.ToListAsync();
    }

    public async Task<Person> AddPersonAsync(Person person)
    {
        await this._dbContext.Persons.AddAsync(person);
        return person;
    }
    
    public async Task<Task> AddPersonsAsync(ICollection<Person> persons)
    {
        await this._dbContext.Persons.AddRangeAsync(persons);
        return Task.CompletedTask;
    }
    
    public async Task<Person?> GetPersonByIdAsync(Guid personId)
    {
        Person? person = await this._dbContext.Persons.Include(x => x.Accounts).FirstOrDefaultAsync(x => x.PersonId== personId);
        return person ?? null;
    }
    
    public Task SaveAsync()
    {
        try
        {
            this._dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Task.FromException(ex);

        }

        return Task.CompletedTask;
    }
    
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}