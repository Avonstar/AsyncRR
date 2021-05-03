using AsyncRR.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncRR.Repositories
{
    public class UploadRepository : IUploadRepository
    {
        private IMemoryCache _cache;

        public UploadRepository(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public async Task<UploadSpotsModel> UploadSpots(IFormFile spotsFile)
        {
            var content = string.Empty;
            try
            {
                // simulating reading file and updating database

                Thread.Sleep(20000);

                //write to cache to simulate saving to DB
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60));
                _cache.Set("SavedSpots", true, cacheEntryOptions);

                var spotModel = new UploadSpotsModel { uploadSpotsUID = 111 };

                return spotModel;

            }
            catch (Exception ex)
            {
                throw new Exception("Upoading Spots Error");
            }
        }

        public void Remove(object key)
        {
            _cache.Remove(key);
        }
    }
}
