using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

using tripdini.accounts.Models;
using tripdini.accounts.Services.Interfaces;


namespace tripdini.accounts.Controllers
{
    [Authorize]
    [Route("v1/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IAccountService _accountService;

        public UsersController(IAccountService accountService) {
            _accountService = accountService;
        }

        [HttpGet("{id}/profile")]
        public async Task<ActionResult<Profile>> GetProfile (string id)
        {
            var profile = await _accountService.GetProfileById(id);

            if (profile == null) return NotFound();

            return profile;
        }

        [HttpGet("{id}/profile-image")]
        public async Task<ActionResult<ProfileImage>> GetProfileImage (string id)
        {
            var profileImage = await _accountService.GetProfileImageById(id);

            if (profileImage == null) return NotFound();

            return profileImage;
        }

        [AllowAnonymous]
        [HttpGet("{id}/profile-header")]
        public async Task<ActionResult<ProfileHeader>> GetProfileHeader (string id)
        {
            var profileHeader = await _accountService.GetProfileHeaderById(id);

            if (profileHeader == null) return NotFound();

            return profileHeader;
        }

        [HttpGet("{id}/profile-summary")]
        public async Task<ActionResult<ProfileSummary>> GetProfileSummary (string id)
        {
            var profileSummary = await _accountService.GetProfileSummaryById(id);

            if (profileSummary == null) return NotFound();

            return profileSummary;
        }
       
        [AllowAnonymous]
        [HttpGet("{id}/signature")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Signature>> GetSignature (string id)
        {
            var signature = await _accountService.GetSignatureById(id);

            if (signature == null) return NotFound();

            return signature;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Profile>> Register(Register register)
        {
            if ( ! ModelState.IsValid) return BadRequest(ModelState);

            await _accountService.Create(register);

            return StatusCode(201, register);
        }

        [HttpPatch("{id}/update-profile")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Profile>> UpdateProfile(string id, Profile profile)
        {
            if ( ! ModelState.IsValid) return BadRequest(ModelState);

            await _accountService.UpdateProfile(id, profile);

            return StatusCode(201, profile);
        }

        [HttpPatch("{id}/update-profile-image")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Profile>> UpdateProfileImage(string id, ProfileImage profileImage)
        {
            if ( ! ModelState.IsValid) return BadRequest(ModelState);

            await _accountService.UpdateProfileImage(id, profileImage);

            return StatusCode(201, profileImage);
        }
    }
}