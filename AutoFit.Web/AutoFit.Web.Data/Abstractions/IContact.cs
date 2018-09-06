using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutoFit.Web.Data.Abstractions
{
    public interface IContact
    {
	    void Add(Contact contact);
	    Task SaveAsync();
    }
}
