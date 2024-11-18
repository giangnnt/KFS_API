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

        public async Task<ResponseDto> CreateBatch(BatchCreate batch)
        {
            var response = new ResponseDto();
            try
            {
                var productList = new List<Product>();
                if (batch.ProductIds != null && batch.ProductIds.Count != 0)
                {
                    foreach (var productId in batch.ProductIds)
                    {
                        var product = await _productRepository.GetProductById(productId);
                        if (product == null)
                        {
                            response.StatusCode = 400;
                            response.Message = "Product not found";
                            response.IsSuccess = false;
                            return response;
                        }
                        if (product.Status != ProductStatusEnum.Active && product.Status != ProductStatusEnum.Deactive && product.Status != ProductStatusEnum.Consignment)
                        {
                            response.StatusCode = 400;
                            response.Message = "Can not add product";
                            response.IsSuccess = false;
                            return response;
                        }
                        productList.Add(product);
                    }
                }

                // create batch
                var mappedBatch = _mapper.Map<Batch>(batch);
                mappedBatch.IsForSell = true;
                mappedBatch.Products = productList;

                // update product status and batch weight
                foreach (var product in mappedBatch.Products)
                {
                    product.Status = ProductStatusEnum.InBatch;
                    await _productRepository.UpdateProduct(product);
                    mappedBatch.Weight += product.Weight;
                }

                var result = await _batchRepository.CreateBatch(mappedBatch);
                if (result)
                {
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
                var batchToDelete = await _batchRepository.GetBatchById(id);
                // update product status
                foreach (var product in batchToDelete.Products)
                {
                    product.Status = ProductStatusEnum.Deactive;
                    await _productRepository.UpdateProduct(product);
                }
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
                // get list of products
                var productList = new List<Product>();
                if (req.ProductIds != null && req.ProductIds.Count > 0)
                {
                    foreach (var productId in req.ProductIds)
                    {
                        productList.Add(await _productRepository.GetProductById(productId));
                    }

                    // update product status deactive and remove old product from batch
                    for (var i = batchToUpdate.Products.Count - 1; i >= 0; i--)
                    {
                        if (!productList.Contains(batchToUpdate.Products[i]))
                        {
                            batchToUpdate.Products[i].Status = ProductStatusEnum.Deactive;
                            await _productRepository.UpdateProduct(batchToUpdate.Products[i]);
                            batchToUpdate.Weight -= batchToUpdate.Products[i].Weight;
                            batchToUpdate.Products.RemoveAt(i);
                        }
                    }

                    // update product status active and add new product to batch
                    foreach (var product in productList)
                    {
                        if (batchToUpdate.Products.Contains(product) == false)
                        {
                            product.Status = ProductStatusEnum.InBatch;
                            await _productRepository.UpdateProduct(product);
                            batchToUpdate.Products.Add(product);
                            batchToUpdate.Weight += product.Weight;
                        }
                    }
                }

                // update batch
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

        public async Task<ResponseDto> UpdateBatchIsActive(bool isActive, Guid id)
        {
            var response = new ResponseDto();
            try
            {
                // get batch
                var batch = await _batchRepository.GetBatchById(id);
                batch.Status = isActive ? ProductStatusEnum.Active : ProductStatusEnum.Deactive;
                var result = await _batchRepository.UpdateBatch(batch);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Batch status updated successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Batch status update failed";
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