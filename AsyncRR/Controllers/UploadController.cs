using AsyncRR.Models;
using AsyncRR.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncRR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private IUploadRepository _uploadRespository;
        private IMemoryCache _cache;

        public UploadController(IMemoryCache memoryCache, IUploadRepository uploadRespository)
        {
            _cache = memoryCache;
            _uploadRespository = uploadRespository;
        }

        // GET api/<UploadController>/5
        [HttpGet("/Spots/SaveStatus")]
        public async Task<ActionResult> GetStatus()
        {
            bool spotsSaved;
            bool fileExists = _cache.TryGetValue("SavedSpots", out spotsSaved);

            if (!spotsSaved)
            {
                // return Accepted("Spots save still in progress");
                return Accepted(new UploadSpotsModel { uploadSpotsUID = null, savedStatus = "Spot save accepted and in progress" });
            }
            else
            {
                return Ok(new UploadSpotsModel { uploadSpotsUID=11111, savedStatus = "Saved to Cache"});
            }

        }

        // POST api/<UploadController>
        [HttpPost]
        public async Task<ActionResult> Create(IFormFile spots)
        {

            // Validate spots file
            if (spots.Length == 0)
            {
                return BadRequest();
            }
            else
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60));
                _cache.Set("FormFile", spots, cacheEntryOptions);

                ThreadPool.QueueUserWorkItem(
                    (obj) =>
                    {
                        _uploadRespository.UploadSpots(spots);
                    }

                );

                return Accepted("Spots save validated and started");
            }

        }

        [HttpPost("/Spots/ClearSave")]
        public void ClearCache()
        {
            _uploadRespository.Remove("SavedSpots");
        }
    }
}
