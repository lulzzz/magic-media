using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagicMedia.Api.Controllers
{
    [Authorize("ApiAccess")]
    [Route("api/video")]
    public class VideoController : Controller
    {
        private readonly IVideoPlayerService _videoPlayerService;

        public VideoController(IVideoPlayerService videoPlayerService)
        {
            _videoPlayerService = videoPlayerService;
        }

        [Route("preview/{id}")]
        [HttpGet]
        public IActionResult Preview(Guid id, CancellationToken cancellationToken)
        {
            MediaStream? mediaStream = _videoPlayerService.GetVideoPreview(id, cancellationToken);
            return new FileStreamResult(mediaStream.Stream, mediaStream.MimeType)
                { EnableRangeProcessing = true };
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> VideoAsync(Guid id, CancellationToken cancellationToken)
        {
            MediaStream? mediaStream = await _videoPlayerService.GetVideoAsync(id, cancellationToken);

            return new FileStreamResult(mediaStream.Stream, $"video/{mediaStream.MimeType}")
                { EnableRangeProcessing = true };
        }
    }
}
