using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoFit.Web.Data.Abstractions;


namespace AutoFit.Web.Data
{
    public class ContactService : IContact
    {
	    private readonly WebsiteDbContext _dbContext;

	    public ContactService(WebsiteDbContext dbContext)
	    {
		    _dbContext = dbContext;
	    }

		public void AddAsync(Contact contact)
		{
			if (contact == null)
			{
				throw new ArgumentNullException(nameof(contact));
			}
			_dbContext.Contacts.Add(contact);
		}

	    public async Task SaveAsync()
	    {
		    await _dbContext.SaveChangesAsync();
	    }
    }
}
