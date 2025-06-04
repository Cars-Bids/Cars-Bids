using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsAndBids.Core.DTOs;

namespace CarsAndBids.Core.Interfaces
{
    public interface IAuctionService
    {
        Task<IEnumerable<AuctionDto>> GetAllAsync();
    }
}
