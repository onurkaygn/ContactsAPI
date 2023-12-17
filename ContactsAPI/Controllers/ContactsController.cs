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

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if(contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
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

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
        {
           var contact = await dbContext.Contacts.FindAsync(id);

            if(contact != null)
            {
               contact.FullName = updateContactRequest.FullName;
                contact.Phone = updateContactRequest.Phone;
                contact.Email = updateContactRequest.Email;
                contact.Address = updateContactRequest.Address;

                await dbContext.SaveChangesAsync();

                return Ok(contact);
            }

            return NotFound();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if(contact == null)
            {
                return NotFound();
            }
            dbContext.Contacts.Remove(contact);
            await dbContext.SaveChangesAsync();
            return Ok(contact);

        }
    }
}
