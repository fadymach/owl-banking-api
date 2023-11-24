using OwlBankingApi.Models;

namespace OwlBankingApi.Repositories.Interfaces;

public interface IPersonRepository : IOwlRepository, IDisposable 
{
        Task<IList<Person>> GetPersonsAsync();
        Task<Person> AddPersonAsync(Person person);
        Task<Task> AddPersonsAsync(ICollection<Person> persons);
        Task<Person?> GetPersonByIdAsync(Guid personId);
}