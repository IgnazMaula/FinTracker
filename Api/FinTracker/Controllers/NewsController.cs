using Microsoft.AspNetCore.Mvc;
using FinTracker.Domain.Interfaces;
using MediatR;
using FinTracker.Application.Services;
using FinTracker.Domain.Entities;

namespace FinTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NewsController : ControllerBase
{
    private readonly IAlphavantageNewsService _newsFeedService;

    public NewsController(IAlphavantageNewsService newsFeedService)
    {
        _newsFeedService = newsFeedService;
    }

    [HttpGet]
    public async Task<IActionResult> GetNewsFeedAsync()
    {
        try
        {
            // Call the NewsFeedService to fetch the news feed
            var newsFeed = await _newsFeedService.GetNewsFeed();

            // Return the news feed as a JSON response
            return Ok(newsFeed);
        }
        catch (Exception ex)
        {
            // Handle any errors (optional)
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}