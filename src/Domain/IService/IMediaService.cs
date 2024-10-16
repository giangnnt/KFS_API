using KFS.src.Application.Dto.MediaDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Domain.Entities;
using System.Runtime.CompilerServices;

namespace KFS.src.Domain.IService
{
    public interface IMediaService
    {
        public Task<ResponseDto> getallmedia();
        public Task<ResponseDto> getmediabyid(Guid id);
        public Task<ResponseDto> update(Guid id, MediaUpdate media);
        public Task<ResponseDto> deletemedia(Guid id);
        public Task<ResponseDto> create(MediaCreate media);
        public Task<ResponseDto> getproductbymediaid(Guid id);
       






    }
}
