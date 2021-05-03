using AsyncRR.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncRR.Repositories
{
    public interface IUploadRepository
    {
        Task<UploadSpotsModel> UploadSpots(IFormFile spotsFile);
        void Remove(object key);
    }
}
