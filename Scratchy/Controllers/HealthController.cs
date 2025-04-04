﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Scratchy.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetHealthAsync()
        {
            return Ok(true);
        }
    }
}
