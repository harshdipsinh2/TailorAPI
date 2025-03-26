using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TailorAPI.DTO.RequestDTO;
using TailorAPI.DTOs.Request;
using TailorAPI.Services;
using TailorAPI.Services.Interface;


namespace TailorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Manager,Tailor")]
    public class ManagerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMeasurementService _measurementService;
        private readonly IProductService _productService;


        public ManagerController(ICustomerService customerService,
                               IMeasurementService measurementService,
                               IProductService productService)


        {
            _customerService = customerService;
            _measurementService = measurementService;
            _productService = productService;
        }


    }
}
