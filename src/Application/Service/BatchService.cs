using AutoMapper;
using KFS.src.Application.Dto.BatchDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;

namespace KFS.src.Application.Service
{
    public class BatchService : IBatchService
    {
        private readonly IBatchRepository _batchRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public BatchService(IBatchRepository batchRepository, IMapper mapper, IProductRepository productRepository)
        {
            _batchRepository = batchRepository;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<ResponseDto> CreateBatchFromProduct(BatchCreate batch, Guid productId)
        {
            var response = new ResponseDto();
            try
            {
                var product = await _productRepository.GetProductById(productId);
                if (product == null)
                {
                    response.StatusCode = 400;
                    response.Message = "Product not found";
                    response.IsSuccess = false;
                    return response;
                }
                var result = false;
                // check if batch exists
                var batchExists = product.Batches.Where(b => b.Quantity == batch.Quantity).FirstOrDefault();
                if (batchExists == null)
                {
                    if (batch.Quantity == 0)
                    {
                        response.StatusCode = 400;
                        response.Message = "Batch quantity is required";
                        response.IsSuccess = false;
                        return response;
                    }
                    if (batch.Inventory == 0)
                    {
                        response.StatusCode = 400;
                        response.Message = "Batch inventory is required";
                        response.IsSuccess = false;
                        return response;
                    }
                    if (batch.Inventory * batch.Quantity > product.Inventory)
                    {
                        response.StatusCode = 400;
                        response.Message = "Batch quantity is greater than product quantity";
                        response.IsSuccess = false;
                        return response;
                    }

                    var mappedBatch = _mapper.Map<Batch>(batch);
                    mappedBatch.Weight = product.Weight * batch.Quantity;
                    mappedBatch.ProductId = productId;
                    mappedBatch.Status = ProductStatusEnum.Deactive;
                    mappedBatch.IsForSell = false;

                    result = await _batchRepository.CreateBatch(mappedBatch);
                }
                else
                {
                    batchExists.Inventory += batch.Inventory;
                    result = await _batchRepository.UpdateBatch(batchExists);
                }

                if (result)
                {
                    // update product inventory
                    product.Inventory -= batch.Inventory * batch.Quantity;
                    await _productRepository.UpdateProduct(product);

                    response.StatusCode = 201;
                    response.Message = "Batch created successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Batch creation failed";
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

        public async Task<ResponseDto> DeleteBatch(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _batchRepository.DeleteBatch(id);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Batch deleted successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Batch deletion failed";
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

        public async Task<ResponseDto> GetBatchById(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var batch = await _batchRepository.GetBatchById(id);
                var mappedBatch = _mapper.Map<BatchDto>(batch);
                if (mappedBatch != null)
                {
                    response.StatusCode = 200;
                    response.Message = "Batch found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedBatch
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Batch not found";
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

        public async Task<ResponseDto> GetBatches()
        {
            var response = new ResponseDto();
            try
            {
                var batches = await _batchRepository.GetAllBatches();
                var mappedBatches = _mapper.Map<List<BatchDto>>(batches);
                if (mappedBatches != null && mappedBatches.Count > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Batches found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedBatches
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Batches not found";
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

        public async Task<ResponseDto> UpdateBatch(BatchUpdate req, Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var batchToUpdate = await _batchRepository.GetBatchById(id);
                if (batchToUpdate == null)
                {
                    response.StatusCode = 400;
                    response.Message = "Batch not found";
                    response.IsSuccess = false;
                    return response;
                }

                var mappedBatch = _mapper.Map(req, batchToUpdate);
                var result = await _batchRepository.UpdateBatch(mappedBatch);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Batch updated successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Batch update failed";
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