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

           var mediamap= _mapper.Map(mediaUpdate, Media);

            if (mediaUpdate.Url != null)
               mediamap.Url = mediaUpdate.Url;
            if (mediaUpdate.Type !=null)
                Media.Type = mediaUpdate.Type;
            mediamap.UpdatedAt= DateTime.Now;



           

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
    }
}
