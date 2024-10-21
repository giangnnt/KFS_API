using AutoMapper;
using KFS.src.Application.Dto.FeedbackDtos; // Ensure you have the correct DTOs for Feedback
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Application.Service
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _repo;
        private readonly IMapper _mapper;

        public FeedbackService(IFeedbackRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ResponseDto> Create(FeedbackCreate feedback)
        {
            var response = new ResponseDto();
            try
            {
                var mappedFeedback = _mapper.Map<Feedback>(feedback);
                mappedFeedback.Id = Guid.NewGuid();


                var result = await _repo.Create(mappedFeedback);
                if (result)
                {
                    response.StatusCode = 201;
                    response.Message = "Feedback created successfully";
                    response.IsSuccess = true;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Feedback creation failed";
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

        public async Task<ResponseDto> Delete(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _repo.Delete(id);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Feedback deleted successfully";
                    response.IsSuccess = true;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Feedback delete failed";
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

        public async Task<ResponseDto> GetAll()
        {
            var response = new ResponseDto();
            try
            {
                var feedbacks = await _repo.GetFeedback();
                if (feedbacks != null && feedbacks.Any())
                {
                    var mappedFeedbacks = _mapper.Map<List<FeedbackDto>>(feedbacks);
                    response.StatusCode = 200;
                    response.Message = "Feedback retrieved successfully";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedFeedbacks
                    };
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "No feedback found";
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

        public async Task<ResponseDto> GetFeedbackById(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var feedback = await _repo.GetFeedbackById(id);
                var mappedFeedback = _mapper.Map<FeedbackDto>(feedback);
                if (mappedFeedback != null)
                {
                    response.StatusCode = 200;
                    response.Message = "Feedback found successfully";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedFeedback
                    };
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "No feedback found";
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

        public async Task<ResponseDto> Update(Guid id, FeedbackUpdate feedbackUpdate)
        {
            var response = new ResponseDto();
            var feedback = await _repo.GetFeedbackById(id);

            if (feedback == null)
            {
                response.StatusCode = 404;
                response.Message = "Feedback not found";
                response.IsSuccess = false;
                return response;
            }


            var feedbackMap = _mapper.Map(feedbackUpdate, feedback);

            if (feedbackMap.Description != null)
            {
                feedbackMap.Description = feedback.Description;
            }

            if (feedbackMap.Rating != null)
            {
                feedbackMap.Rating = feedback.Rating;
            }

            if (feedbackMap.ProductId != null)
            {
                feedbackMap.ProductId = feedback.ProductId;
            }

            if (feedbackMap.UserId != null)
            {
                feedbackMap.UserId = feedback.UserId;
            }
                var result = await _repo.Update(feedback);



                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Feedback updated successfully";
                    response.IsSuccess = true;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Feedback update failed";
                    response.IsSuccess = false;
                }

                return response;
            }
        }
    }
