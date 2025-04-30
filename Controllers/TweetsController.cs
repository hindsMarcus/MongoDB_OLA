using Microsoft.AspNetCore.Mvc;
using TweetApi.Models;
using TweetApi.Services;

namespace TweetApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TweetsController : ControllerBase
{
    private readonly TweetService _tweetService;

    public TweetsController(TweetService tweetService)
    {
        _tweetService = tweetService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Tweet>>> Get() =>
        await _tweetService.GetFirst10Async();

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Tweet tweet)
    {
        await _tweetService.CreateAsync(tweet);
        return CreatedAtAction(nameof(Post), new { id = tweet.Id }, tweet);
    }
}