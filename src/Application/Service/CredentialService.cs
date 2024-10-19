using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KFS.src.Application.Dto.CredentialDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;

namespace KFS.src.Application.Service
{
    public class CredentialService : ICredentialService
    {
        private readonly ICredentialRepositoty _credentialRepositoty;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public CredentialService(ICredentialRepositoty credentialRepositoty, IProductRepository productRepository, IMapper mapper)
        {
            _credentialRepositoty = credentialRepositoty;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDto> CreateCredential(CreadentialCreate req)
        {
            var response = new ResponseDto();
            try
            {
                //check if product id is empty
                if (req.ProductId == Guid.Empty)
                {
                    response.StatusCode = 400;
                    response.Message = "Product Id is required";
                    response.IsSuccess = false;
                    return response;
                }
                //get product by id
                var product = await _productRepository.GetProductById(req.ProductId);
                if (product == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Product not found";
                    response.IsSuccess = false;
                    return response;
                }
                //map credential
                var mappedCredential = _mapper.Map<Credential>(req);
                mappedCredential.Product = product;

                var result = await _credentialRepositoty.CreateCredential(mappedCredential);
                if (result)
                {
                    response.StatusCode = 201;
                    response.Message = "Credential created successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Failed to create credential";
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

        public async Task<ResponseDto> DeleteCredential(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _credentialRepositoty.DeleteCredential(id);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Credential deleted successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Credential not found";
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

        public async Task<ResponseDto> GetCredentialById(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _credentialRepositoty.GetCredentialById(id);
                var mappedCredential = _mapper.Map<CredentialDto>(result);
                if (mappedCredential == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Credential not found";
                    response.IsSuccess = false;
                    return response;
                }
                else
                {
                    response.StatusCode = 200;
                    response.Message = "Credential found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedCredential
                    };
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

        public async Task<ResponseDto> GetCredentials()
        {
            var response = new ResponseDto();
            try
            {
                var result = await _credentialRepositoty.GetCredentials();
                var mappedCredentials = _mapper.Map<List<CredentialDto>>(result);
                if (result != null && result.Count > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Credentials found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedCredentials
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Credentials not found";
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

        public async Task<ResponseDto> GetCredentialsByProductId(Guid productId)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _credentialRepositoty.GetCredentialsByProductId(productId);
                var mappedCredentials = _mapper.Map<List<CredentialDto>>(result);
                if (result != null && result.Count > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Credentials found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedCredentials
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Credentials not found";
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

        public async Task<ResponseDto> UpdateCredential(CredentialUpdate req, Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var credential = await _credentialRepositoty.GetCredentialById(id);
                if (credential == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Credential not found";
                    response.IsSuccess = false;
                    return response;
                }
                var mappedCredential = _mapper.Map(req, credential);
                var result = await _credentialRepositoty.UpdateCredential(mappedCredential);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Credential updated successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Failed to update credential";
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