﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SKYNETAPI.DTOs;
using SKYNETCORE.Entities;
using System.Security.Claims;

namespace SKYNETAPI.Controllers;


public class BuggyController : BaseApiController
{
    [HttpGet("unauthorized")]
    public IActionResult GetUnauthorized()
    {
        return Unauthorized();
    }

    [HttpGet("badrequest")]
    public IActionResult GetBadRequest()
    {
        return BadRequest("Not a good request");
    }

    [HttpGet("notfound")]
    public IActionResult GetNotFound()
    {
        return NotFound();
    }

    [HttpGet("internalerror")]
    public IActionResult GetInternalError()
    {
        throw new Exception("This is a test exception.");
    }

    [HttpPost("validationerror")]
    public IActionResult GetValidationError(CreateProductDTO product)
    {
        return Ok();
    }

    [Authorize]
    [HttpGet("secret")]
    public IActionResult GetSecret()
    {
        var name = User.FindFirst(ClaimTypes.Name)?.Value;

        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


        return Ok($"Hello {name} with the id of {id}");
    }
}
