using DATA.API;
using Microsoft.EntityFrameworkCore;
using Model.APi.Entities;
using Core.API.Services;
using Model.API.Model;

namespace Core.API.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly ContactAPIContext _dbContext;

        public ContactRepository(ContactAPIContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException();
        }

        #region AddContactAsync
        public async Task<Contacts> AddContactAsync(Contacts newContactEntity)
        {
            await _dbContext.Contacts.AddAsync(newContactEntity);
            var result = await _dbContext.SaveChangesAsync();
            if (result > 0)
            {
                return newContactEntity;
            }
            else
            {
                throw new Exception("Contact was not Added Successfully");
            }
        }
        #endregion

        #region DeleteContactAsync
        public async Task<int> DeleteContactAsync(int id)
        {
            var contactToDelete = await _dbContext.Contacts.FindAsync(id);
            if (contactToDelete != null)
            {
                _dbContext.Contacts.Remove(contactToDelete);

                var result = await _dbContext.SaveChangesAsync();
                return result;
            }
            else
            {
                return 0;
            }
        }

        #endregion

        #region GetAllContactAsync
        public async Task<IEnumerable<Contacts>> GetAllContactAsync()
        {
            return await _dbContext.Contacts.ToListAsync();

        }
        #endregion

        #region GetContactAsync
        public async Task<Contacts> GetContactAsync(int contactId)
        {

            return await _dbContext.Contacts.FirstOrDefaultAsync(c => c.Id == contactId);
        }
        #endregion

        #region SearchContactAsync
        public async Task<IEnumerable<Contacts>> SearchContactAsync(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
            {
                return await GetAllContactAsync();
            }
            var loadSearchContact = await _dbContext.Contacts.Where(item =>
            item.FirstName.ToLower().Contains(firstName.ToLower().Trim()) &&
            item.LastName.ToLower().Contains(lastName.ToLower().Trim())).ToListAsync();
            return loadSearchContact;
        }
        #endregion

        #region UpdateContactAsync
        public async Task<int> UpdateContactAsync(Contacts updateContactEntity)
        {
            _dbContext.Contacts.Update(updateContactEntity);
            var affectedResult = await _dbContext.SaveChangesAsync();
            return affectedResult;
        }
        #endregion

        #region PaginatedContact
        public PaginatedContact PaginatedAsync(List<Contacts> contacts, int pageNumber, int perPageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            perPageSize = perPageSize < 1 ? 5 : perPageSize;
            var totalCount = contacts.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / perPageSize);
            var paginated = contacts.Skip((pageNumber - 1) * perPageSize).Take(perPageSize).ToList();
            var result = new PaginatedContact
            {
                CurrentPage = pageNumber,
                PageSize = perPageSize,
                TotalPages = totalPages,
                Contacts = paginated
            };
            return result;
        }

        #endregion

    }
}
