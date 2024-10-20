using AutoMapper;
using KFS.src.Application.Constant;
using KFS.src.Application.Core;
using KFS.src.Application.Dto.CategoryDtos;
using KFS.src.Application.Dto.MediaDtos;
using KFS.src.Application.Dto.ProductDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;
using KFS.src.Infrastucture.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace KFS.src.Application.Service
{
    public class MediaService : IMediaService
    {
        private readonly IMediaRepository _repo;
        private readonly IMapper _mapper;
        private readonly IGCService _gcService;
        public MediaService(IMediaRepository repo, IMapper mapper,IGCService gcService)
        {
            _repo = repo;
            _mapper = mapper;
            _gcService = gcService;

        }

        public async Task<ResponseDto> Create(MediaCreate media)
        {
            var response = new ResponseDto();
            try
            {
                //map product
                var mappedMedia = _mapper.Map<Media>(media);
                //check if category id is empty
                mappedMedia.Id = Guid.NewGuid();
                mappedMedia.CreatedAt = DateTime.Now;


                //get category by id

                //map category

                var result = await _repo.Create(mappedMedia);
                if (result)
                {
                    response.StatusCode = 201;
                    response.Message = "Media created successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Media creation failed";
                    response.IsSuccess = false;
                    return response;
                }

            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseDto> Delete(Guid id)
        {
            var response = new ResponseDto();
            try
            {

                //check if category id is empty


                //get category by id

                //map category

                var result = await _repo.Delete(id);
                if (result)
                {
                    response.StatusCode = 201;
                    response.Message = "Media deleted successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Media delete failed";
                    response.IsSuccess = false;
                    return response;
                }

            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseDto> GetMedias()
        {
            var response = new ResponseDto();
            try
            {
                var media = await _repo.GetMedias();
                if (media != null && media.Any())
                {
                    var mappedMedia = _mapper.Map<List<MediaDto>>(media);
                    response.StatusCode = 200;
                    response.Message = "Media retrieved successfully";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedMedia
                    };
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "No media found";
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = $"An error occurred: {ex.Message}";
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<ResponseDto> GetMediaById(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var media = await _repo.GetMediaById(id);
                var mappedMedia = _mapper.Map<MediaDto>(media);
                if (mappedMedia != null)
                {
                    response.StatusCode = 200;
                    response.Message = "Media Found successfully";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedMedia
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "No Media found";
                    response.IsSuccess = false;
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








        public async Task<ResponseDto> Update(Guid id, MediaUpdate mediaUpdate)
        {
            var response = new ResponseDto();

            var Media = await _repo.GetMediaById(id);

            if (Media == null)
            {
                response.StatusCode = 404;
                response.Message = "Media not found";
                response.IsSuccess = false;
                return response;
            }

            var mediamap = _mapper.Map(mediaUpdate, Media);

            if (mediaUpdate.Url != null)
                mediamap.Url = mediaUpdate.Url;
            if (mediaUpdate.Type != null)
                Media.Type = mediaUpdate.Type;
            mediamap.UpdatedAt = DateTime.Now;





            var result = await _repo.Update(Media);

            if (result)
            {
                response.StatusCode = 200;
                response.Message = "Media updated successfully";
                response.IsSuccess = true;

            }
            else
            {
                response.StatusCode = 400;
                response.Message = "Media update failed";
                response.IsSuccess = false;
            }

            return response;
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