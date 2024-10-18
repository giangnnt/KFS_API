﻿using KFS.src.Application.Dto.MediaDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Domain.Entities;
using System.Runtime.CompilerServices;

namespace KFS.src.Domain.IService
{
    public interface IMediaService
    {
        public Task<ResponseDto> GetMedias();
        public Task<ResponseDto> GetMediaById(Guid id);
        public Task<ResponseDto> Update(Guid id, MediaUpdate media);
        public Task<ResponseDto> Delete(Guid id);
        public Task<ResponseDto> Create(MediaCreate media);
       
       






    }
}