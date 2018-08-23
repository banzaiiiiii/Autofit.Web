using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoFit.Web.Data;
using AutoFit.Web.Data.Abstractions;

namespace AutoFit.Web.Services
{
    public class ContactService : BaseService, IContact
    {
	    private readonly WebsiteDbContext _dbContext;

	    public ContactService(WebsiteDbContext dbContext)
	    {
		    _dbContext = dbContext;
	    }

		public Task AddAsync(Contact contact)
		{
			_dbContext.Contacts.Add(contact);
			return _dbContext.SaveChangesAsync();
		}
	}
}
