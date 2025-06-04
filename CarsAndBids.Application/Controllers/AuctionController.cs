using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarsAndBids.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionsController : ControllerBase
    {
        private readonly IAuctionService _auctionService;

        public AuctionsController(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuctionDto>>> GetAll()
        {
            var auctions = await _auctionService.GetAllAsync();
            return Ok(auctions);
        }
    }
}
