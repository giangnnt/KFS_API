using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Constant;
using KFS.src.Application.Core;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Application.Enum;
using KFS.src.Domain.IService;

namespace KFS.src.Application.Service
{
    public class MediaService : IMediaService
    {
        private readonly IGCService _gcService;
        public MediaService(IGCService gcService)
        {
            _gcService = gcService;
        }
        public async Task<ResponseDto> UploadMedia(IFormFile file, string type)
        {
            var response = new ResponseDto();
            try
            {
                if (file == null || file.Length == 0)
                {
                    response.StatusCode = 400;
                    response.Message = "File is empty";
                    response.IsSuccess = false;
                    return response;
                }
                var contentType = file.ContentType;
                if (type == MediaTypeEnum.Image.ToString())
                {
                    if (!FileConst.IMAGE_CONTENT_TYPES.Contains(contentType))
                    {
                        response.StatusCode = 400;
                        response.Message = "File is not an image";
                        response.IsSuccess = false;
                        return response;
                    }
                    if (file.Length > FileConst.MAX_IMAGE_SIZE)
                    {
                        response.StatusCode = 400;
                        response.Message = "File is too large";
                        response.IsSuccess = false;
                        return response;
                    }
                }
                else if (type == MediaTypeEnum.Video.ToString())
                {
                    if (!FileConst.VIDEO_CONTENT_TYPES.Contains(contentType))
                    {
                        response.StatusCode = 400;
                        response.Message = "File is not a video";
                        response.IsSuccess = false;
                        return response;
                    }
                    if (file.Length > FileConst.MAX_VIDEO_SIZE)
                    {
                        response.StatusCode = 400;
                        response.Message = "File is too large";
                        response.IsSuccess = false;
                        return response;
                    }
                }
                else if (type == MediaTypeEnum.Cert.ToString())
                {
                    if (!FileConst.CERT_CONTENT_TYPES.Contains(contentType))
                    {
                        response.StatusCode = 400;
                        response.Message = "File is not an certificate";
                        response.IsSuccess = false;
                        return response;
                    }
                    if (file.Length > FileConst.MAX_CERT_SIZE)
                    {
                        response.StatusCode = 400;
                        response.Message = "File is too large";
                        response.IsSuccess = false;
                        return response;
                    }
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Invalid media type";
                    response.IsSuccess = false;
                    return response;
                }
                // Upload file to Google Cloud Storage
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var fileStream = file.OpenReadStream();
                var result = await _gcService.UploadFileAsync(fileStream, fileName, contentType);
                if (result == null)
                {
                    response.StatusCode = 500;
                    response.Message = "Failed to upload file";
                    response.IsSuccess = false;
                    return response;
                }
                else
                {
                    response.StatusCode = 200;
                    response.Message = "File uploaded successfully";
                    response.Result = new ResultDto { Data = result };
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }
    }
}