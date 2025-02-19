using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;
using Scratchy.Persistence.Repositories;
using Scratchy.Services;
using System.Security.Claims;

namespace Scratchy.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CollectionController : Controller
    {
        private readonly ILibraryService _libService;
        private readonly ILibraryRepository _collectionRepository;
        private readonly ICollectionService _collectionService;
        private readonly IUserService _userService;

        public CollectionController(ILibraryService libService, ILibraryRepository libraryRepository, IUserService userService, ICollectionService collectionService)
        {
            _libService = libService;
            _collectionRepository = libraryRepository;
            _collectionService = collectionService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLibrary()
        {
            var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserID))
            {
                return Unauthorized(new { Message = "User ID not found in token." });
            }
            var currUser = await _userService.GetUserByFireBaseId(currentUserID);

            var collection = await _collectionService.GetCollectionByUserId(currUser.UserId);
            return Ok(collection);
        }
    }
}
