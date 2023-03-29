using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleWebApi.Models;

namespace SimpleWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ApplicationContext dbContext;

        public ContactsController(ApplicationContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Get all contacts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            return Ok(await dbContext.Contacts.ToListAsync());
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetContact([FromRoute] int id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if (contact == null) return NotFound(new { message = "The contact is not found" });

            return Ok(Results.Json(contact));
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(Contact contact)
        {
            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();
            return Ok(Results.Json(contact));
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateContact([FromRoute] int id, Contact contactData)
        {
            var contact = await dbContext.Contacts.FirstOrDefaultAsync(u => u.Id == id);
            if (contact == null) return NotFound(new { message = "The contact is not found" });

            contact.Name = contactData.Name;
            contact.Surname = contactData.Surname;
            contact.Email = contactData.Email;
            contact.DateOfBirth = contactData.DateOfBirth;
            contact.PhoneNumber = contactData.PhoneNumber;
            contact.Country = contactData.Country;
            contact.City = contactData.City;

            await dbContext.SaveChangesAsync();
            return Ok(Results.Json(contact));
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteContact([FromRoute] int id)
        {
            var contact = await dbContext.Contacts.FirstOrDefaultAsync(t => t.Id == id);
            if (contact == null) return NotFound(new { message = "The contact is not found" });

            dbContext.Contacts.Remove(contact);
            await dbContext.SaveChangesAsync();
            return Ok(Results.Json(contact));
        }
    }
}
