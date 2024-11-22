using AutoMapper;
using KFS.src.Application.Core.Jwt;
using KFS.src.Application.Dto.FeedbackDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;
using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Application.Service
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPaymentRepository _paymentRepository;
        private readonly HttpContext _httpContext;
        private readonly IOwnerService _ownerService;
        public FeedbackService(IFeedbackRepository feedbackRepository, IMapper mapper, IProductRepository productRepository, IHttpContextAccessor httpContextAccessor, IPaymentRepository paymentRepository, IOwnerService ownerService)
        {
            _feedbackRepository = feedbackRepository;
            _mapper = mapper;
            _productRepository = productRepository;
            _paymentRepository = paymentRepository;
            _ownerService = ownerService;

            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _httpContext = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("Http context is required.");

        }
        public async Task<ResponseDto> CreateFeedback(Guid id, FeedbackCreate req)
        {
            var response = new ResponseDto();
            try
            {
                var product = await _productRepository.GetProductById(id);
                if (product == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Product not found";
                    response.IsSuccess = false;
                    return response;
                }
                var httpContext = _httpContextAccessor.HttpContext;
                // check httpContext is null
                if (httpContext == null)
                {
                    response.StatusCode = 401;
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return response;
                }
                var payload = httpContext.Items["payload"] as Payload;
                // check Payload is null
                if (payload == null)
                {
                    response.StatusCode = 401;
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return response;
                }
                var userId = payload.UserId;
                // check if user buy the product
                var checkUserBoughtProduct = await _feedbackRepository.CheckUserBoughtProduct(userId, id);
                if (!checkUserBoughtProduct)
                {
                    response.StatusCode = 400;
                    response.Message = "You have not bought this product";
                    response.IsSuccess = false;
                    return response;
                }
                // check if user already feedback
                var haveUserFeedbackOnProduct = await _feedbackRepository.HaveUserFeedbackOnProduct(userId, id);
                if (haveUserFeedbackOnProduct)
                {
                    response.StatusCode = 400;
                    response.Message = "You have already feedback on this product";
                    response.IsSuccess = false;
                    return response;
                }
                var mappedFeedback = _mapper.Map<Feedback>(req);
                // check map user
                mappedFeedback.ProductId = id;
                mappedFeedback.UserId = userId;
                var result = await _feedbackRepository.CreateFeedback(mappedFeedback);
                if (result)
                {
                    response.StatusCode = 201;
                    response.Message = "Feedback created successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Feedback not created";
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

        public async Task<ResponseDto> DeleteFeedback(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var feedback = await _feedbackRepository.GetFeedbackById(id);
                if (feedback == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Feedback not found";
                    response.IsSuccess = false;
                    return response;
                }
                var httpContext = _httpContextAccessor.HttpContext;
                // check httpContext is null
                if (httpContext == null)
                {
                    response.StatusCode = 401;
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return response;
                }
                var payload = httpContext.Items["payload"] as Payload;
                // check Payload is null
                if (payload == null)
                {
                    response.StatusCode = 401;
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return response;
                }
                var userId = payload.UserId;
                if (feedback.UserId != userId)
                {
                    response.StatusCode = 401;
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return response;
                }
                var result = await _feedbackRepository.DeleteFeedback(id);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Feedback deleted successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Feedback not deleted";
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

        public async Task<ResponseDto> GetAverageRating(Guid productId)
        {
            var response = new ResponseDto();
            try
            {
                var feedbackList = await _feedbackRepository.GetFeedbacks(new FeedbackQuery { ProductId = productId });
                var averageRating = feedbackList.List.Average(x => x.Rating);
                if (feedbackList != null && feedbackList.List.Count() > 0)
                {
                    response.StatusCode = 201;
                    response.Message = "Average rating found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = averageRating
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Average rating not found";
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

        public async Task<ResponseDto> GetFeedbackByUserId(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var feedbackList = await _feedbackRepository.GetFeedbackByUserId(id);
                var mappedFeedbacks = _mapper.Map<List<FeedbackDto>>(feedbackList);
                if (feedbackList != null && feedbackList.Count() > 0)
                {
                    response.StatusCode = 201;
                    response.Message = "Feedback found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedFeedbacks
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Feedback not found";
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

        public async Task<ResponseDto> GetFeedbacks(FeedbackQuery feedbackQuery)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _feedbackRepository.GetFeedbacks(feedbackQuery);
                var mappedFeedbacks = _mapper.Map<List<FeedbackDto>>(result.List);
                if (result != null && result.List.Count() > 0)
                {
                    response.StatusCode = 201;
                    response.Message = "Feedback found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedFeedbacks,
                        PaginationResp = new PaginationResp
                        {
                            Page = feedbackQuery.Page,
                            PageSize = feedbackQuery.PageSize,
                            Total = result.Total
                        }
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Feedback not found";
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

        public async Task<ResponseDto> UpdateFeedback(Guid id, FeedbackUpdate req)
        {
            var response = new ResponseDto();
            try
            {
                var feedback = await _feedbackRepository.GetFeedbackById(id);
                if (feedback == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Feedback not found";
                    response.IsSuccess = false;
                    return response;
                }
                var httpContext = _httpContextAccessor.HttpContext;
                // check httpContext is null
                if (httpContext == null)
                {
                    response.StatusCode = 401;
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return response;
                }
                var payload = httpContext.Items["payload"] as Payload;
                // check Payload is null
                if (payload == null)
                {
                    response.StatusCode = 401;
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return response;
                }
                var userId = payload.UserId;
                if (feedback.UserId != userId)
                {
                    response.StatusCode = 401;
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return response;
                }
                var mappedFeedback = _mapper.Map(req, feedback);
                var result = await _feedbackRepository.UpdateFeedback(mappedFeedback);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Feedback updated successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Feedback not updated";
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
    }
}