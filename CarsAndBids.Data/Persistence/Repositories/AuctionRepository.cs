using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using CarsAndBids.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarsAndBids.Data.Persistence.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly ApplicationDbContext _context;

        public AuctionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Auction> GetByIdAsync(int id)
        {
            var auction = await _context.Auctions
                .Include(a => a.Seller)
                .FirstOrDefaultAsync(a => a.Id == id);

            return auction ?? throw new KeyNotFoundException($"Auction with ID {id} was not found.");
        }


        public async Task<IEnumerable<Auction>> GetAllAsync()
        {
            return await _context.Auctions
                .Include(a => a.Seller)
                .ToListAsync();
        }

        public async Task<IEnumerable<Auction>> SearchAsync(string make, string model, int? year, decimal? maxPrice)
        {
            var query = _context.Auctions.AsQueryable();
            if (!string.IsNullOrEmpty(make))
                query = query.Where(a => a.Make.Contains(make));
            if (!string.IsNullOrEmpty(model))
                query = query.Where(a => a.Model.Contains(model));
            if (year.HasValue)
                query = query.Where(a => a.Year == year.Value);
            if (maxPrice.HasValue)
                query = query.Where(a => a.CurrentBid <= maxPrice.Value);
            return await query.ToListAsync();
        }

        public async Task AddAsync(Auction auction)
        {
            _context.Auctions.Add(auction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Auction auction)
        {
            _context.Auctions.Update(auction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var auction = await _context.Auctions.FindAsync(id);
            if (auction != null)
            {
                _context.Auctions.Remove(auction);
                await _context.SaveChangesAsync();
            }
        }
    }
}
