using AutoMapper;
using Core.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.APi.Entities;
using Model.APi.Model;
using System.Data;
using Core.API.Services;
using DATA.API;
using System.Security.Principal;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace ContactAPIUPDate.Controllers
{
    [Route("user")]
    [ApiController]
    public class ContactController : ControllerBase
    {

        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;
        private readonly ContactAPIContext _contactApiContext;
        private readonly IConfiguration _configuration;

        public ContactController(IContactRepository contactRepository, IMapper mapper, ContactAPIContext contactApiContext, IConfiguration configuration)
        {

            _contactRepository = contactRepository ?? throw new ArgumentNullException();
            _mapper = mapper ?? throw new ArgumentNullException();


            _contactApiContext = contactApiContext;
            _configuration = configuration;
        }
        [Authorize(Roles = "ADMIN, USER")]
        [HttpGet("contact")]
        public async Task<ActionResult<IEnumerable<ContactDTO>>> GetAllContact(int pageNumber, int perPageSize)
        {
            var getAllContact = await _contactRepository.GetAllContactAsync();
            if (getAllContact != null && getAllContact.Count() > 0)
            {
                var paged = _contactRepository.PaginatedAsync(getAllContact.ToList(), pageNumber, perPageSize);
                return Ok(paged);
            }
            return NotFound("There is nothing to be displayed");
        }


        [Authorize(Roles = "ADMIN, USER")]
        [HttpGet("contact/{id}")]
        public async Task<ActionResult<ContactDTO>> GetSingleContactById(int id)
        {
            var singleContactBy = await _contactRepository.GetContactAsync(id);
            if (singleContactBy == null)
            {
                return NotFound(singleContactBy);
            }
            return Ok(singleContactBy);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("contact/search")]
        public async Task<ActionResult<IEnumerable<ContactDTO>>> SearchContact(string? firstName, string? lastName)
        {
            var search = await _contactRepository.SearchContactAsync(firstName, lastName);
            if (search == null) { return NotFound(search); }
            return Ok(search);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<ActionResult<Contacts>> AddContact(CreateContactDTO createContact)
        {
            var mapItem = new Contacts()
            {
                FirstName = createContact.FirstName,
                LastName = createContact.LastName,
                Email = createContact.Emails,
                Address = createContact.Address,
                PhoneNumbers = createContact.PhoneNumber,
                WebsiteUrl = createContact.WebSiteUrl,
                //Image = createContact.Image,
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var contactAdd = await _contactRepository.AddContactAsync(mapItem);
            return Ok(contactAdd);

        }
        [Authorize(Roles = "USER")]
        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateContact(ContactUpdateDTO contactUpdateDtO, int id)
        {
            var mapUpdateItem = new Contacts()
            {
                Id = id,
                FirstName = contactUpdateDtO.FirstName,
                LastName = contactUpdateDtO.LastName,
                Email = contactUpdateDtO.Emails,
                Address = contactUpdateDtO.Address,
                PhoneNumbers = contactUpdateDtO.PhoneNumber,
                WebsiteUrl = contactUpdateDtO.WebSiteUrl,
                Image = contactUpdateDtO.Image,
            };
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var updateItem = await _contactRepository.UpdateContactAsync(mapUpdateItem);
            if (updateItem > 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        //

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteContact(int id)
        {
            if (id <= 0)
            {
                return BadRequest("This is not a valid id");
            }
            else
            {
                var delete = await _contactRepository.DeleteContactAsync(id);
                if (delete > 0)
                {
                    return NoContent();
                }
                else { return NotFound($"Contact with the {id} not found in the Database"); }
            }
        }
        [HttpPatch("photos")]
        public async Task<IActionResult> UploadPhoto2(IFormFile file, int id)
        {
            var findContact = await _contactApiContext.Contacts.FindAsync(id);
            if (findContact == null)
            {
                return NotFound("Contact to upload picture to not available");
            }
            if (file == null || file.Length == 0)
            {
                return BadRequest(new
                {
                    Status = "No Image Uploaded"
                });
            }
            var cloudinary = new Cloudinary(new Account("djjpgdixl", "419476571161995", "w-2fadYssBTqg1Ct7_qCJ3o2i-Q"));
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                PublicId = $"{id}"
            };
            var result = cloudinary.Upload(uploadParams);
            if (result == null)
            {
                return BadRequest(new
                {
                    Status = "Image not upload successfully"
                });
            }
            findContact.Image = result.Url.ToString();
            _contactApiContext.Contacts.Update(findContact);
            await _contactApiContext.SaveChangesAsync();
            return Ok(new
            {
                PublicId = result.PublicId,
                Url = result.SecureUrl.ToString(),
                Status = "Uploaded Successfully"
            });
        }

    }
}
