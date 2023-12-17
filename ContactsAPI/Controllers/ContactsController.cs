using ContactsAPI.Data;
using Microsoft.AspNetCore.Mvc;
using ContactsAPI.Models;
namespace ContactsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactsAPIDbContext dbContext;
        public ContactsController(ContactsAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
           return Ok(dbContext.Contacts.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactRequest addContactRequest)
        {
            var newContact = new Contact()
            {
                Id = Guid.NewGuid(),
                Address = addContactRequest.Address,
                Email = addContactRequest.Email,
                FullName = addContactRequest.FullName,
                Phone = addContactRequest.Phone,
            };
            await dbContext.Contacts.AddAsync(newContact);
            await dbContext.SaveChangesAsync();
            return Ok(newContact);
        }
    }
}
