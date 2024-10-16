using AutoMapper;
using KFS.src.Application.Dto.CategoryDtos;
using KFS.src.Application.Dto.MediaDtos;
using KFS.src.Application.Dto.ProductDtos;
using KFS.src.Application.Dto.ResponseDtos;
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
        public MediaService(IMediaRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ResponseDto> create(MediaCreate media)
        {
            var response = new ResponseDto();
            try
            {
                //map product
                var mappedMedia = _mapper.Map<Media>(media);
                //check if category id is empty
                mappedMedia.Id = Guid.NewGuid();

                //get category by id

                //map category

                var result = await _repo.CreateMedia(mappedMedia);
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

        public async Task<ResponseDto> deletemedia(Guid id)
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

        public async Task<ResponseDto> getallmedia()
        {
            var response = new ResponseDto();
            try
            {
                var media = await _repo.GetAllMedia();
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

        public async Task<ResponseDto> getmediabyid(Guid id)
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



        public Task<ResponseDto> getproductbymediaid(Guid id)
        {
            throw new NotImplementedException();
        }

      

        

        public async Task<ResponseDto> update(Guid id, MediaUpdate mediaUpdate)
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

            _mapper.Map(mediaUpdate, Media);

            if (mediaUpdate.Url != null)
                Media.Url = mediaUpdate.Url;
            if (mediaUpdate.ProductId.HasValue)
                Media.ProductId = mediaUpdate.ProductId.Value;

           

            var result = await _repo.UpdateMedia(Media);

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
    }
}
