using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.ResponseDtos;

namespace KFS.src.Domain.IService
{
    public interface IMediaService
    {
        Task<ResponseDto> UploadMedia(IFormFile file, string type);
    }
}