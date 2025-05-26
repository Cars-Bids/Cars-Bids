using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsAndBids.Data.Entities;

namespace CarsAndBids.Data.Interfaces
{
    public interface IAuctionRepository
    {
        Task<Auction> GetByIdAsync(int id);
        Task<IEnumerable<Auction>> GetAllAsync();
        Task<IEnumerable<Auction>> SearchAsync(string make, string model, int? year, decimal? maxPrice);
        Task AddAsync(Auction auction);
        Task UpdateAsync(Auction auction);
        Task DeleteAsync(int id);
    }
}
