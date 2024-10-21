using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KFS.src.Application.Constant;
using KFS.src.Application.Core;
using KFS.src.Application.Dto.MediaDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;

namespace KFS.src.Application.Service
{
    public class MediaService : IMediaService
    {
        private readonly IGCService _gcService;
        private readonly IMediaRepository _mediaRepository;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        public MediaService(IGCService gcService, IMediaRepository mediaRepository, IMapper mapper, IProductRepository productRepository)
        {
            _gcService = gcService;
            _mediaRepository = mediaRepository;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public Task<ResponseDto> CreateMedia(MediaCreate mediaCreate)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto> DeleteMedia(Guid mediaId)
        {
            var response = new ResponseDto();
            try
            {
                var media = await _mediaRepository.GetMediaById(mediaId);
                // Delete media from Google Cloud Storage
                var resultCloud = await _gcService.DeleteFileAsync(media.Url);
                if (!resultCloud)
                {
                    response.StatusCode = 500;
                    response.Message = "Failed to delete media";
                    response.IsSuccess = false;
                    return response;
                }
                var result = await _mediaRepository.DeleteMedia(media);
                if (!result)
                {
                    response.StatusCode = 500;
                    response.Message = "Failed to delete media";
                    response.IsSuccess = false;
                    return response;
                }
                else
                {
                    response.StatusCode = 200;
                    response.Message = "Media deleted successfully";
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

        public async Task<ResponseDto> GetMediaByProductId(Guid productId)
        {
            var response = new ResponseDto();
            try
            {
                var medias = await _mediaRepository.GetAllMediaByProductId(productId);
                if (medias == null || !medias.Any())
                {
                    response.StatusCode = 404;
                    response.Message = "Media not found";
                    response.IsSuccess = false;
                    return response;
                }
                else
                {
                    response.StatusCode = 200;
                    response.Message = "Success";
                    response.Result = new ResultDto { Data = medias };
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

        public async Task<ResponseDto> UpdateMediaProduct(Guid productId, List<Guid> mediaIds)
        {
            var response = new ResponseDto();
            try
            {
                var product = await _productRepository.GetProductById(productId);
                var medias = new List<Media>();
                foreach (var mediaId in mediaIds)
                {
                    var media = await _mediaRepository.GetMediaById(mediaId);
                    if (media == null)
                    {
                        response.StatusCode = 404;
                        response.Message = "Media not found";
                        response.IsSuccess = false;
                        return response;
                    }
                    medias.Add(media);
                }
                var result = await _mediaRepository.UpdateMediaProduct(product, medias);
                if (!result)
                {
                    response.StatusCode = 500;
                    response.Message = "Failed to update media";
                    response.IsSuccess = false;
                    return response;
                }
                else
                {
                    response.StatusCode = 200;
                    response.Message = "Media updated successfully";
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
                var fileUrl = await _gcService.UploadFileAsync(fileStream, fileName, contentType);
                if (fileUrl == null)
                {
                    response.StatusCode = 500;
                    response.Message = "Failed to upload file";
                    response.IsSuccess = false;
                    return response;
                }
                var media = new MediaCreate
                {
                    Url = fileUrl,
                    Type = System.Enum.Parse<MediaTypeEnum>(type)
                };
                var mappedMedia = _mapper.Map<Media>(media);
                var result = await _mediaRepository.CreateMedia(mappedMedia);
                if (!result)
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
                    response.Result = new ResultDto { Data = fileUrl };
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