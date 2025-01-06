using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;
using Scratchy.Persistence.Repositories;
using System.Security.Claims;

namespace Scratchy.Controllers
{
    [ApiController]
    [Route("library")]
    [Authorize]
    public class LibraryController : Controller
    {
        private readonly ILibraryService _libService;
        private readonly ILibraryRepository _libRepository;

        public LibraryController(ILibraryService libService, ILibraryRepository libraryRepository)
        {
            _libService = libService;
            _libRepository = libraryRepository;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetLibrary()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var library = await _libRepository.GetByUserIdAsync(userId);
            return Ok(library);
        }
    }
}
