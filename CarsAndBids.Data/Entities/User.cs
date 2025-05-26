using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;

namespace CarsAndBids.Data.Entities
{
    public class User : IdentityUser<int>
    {
        public string? ProfilePictureUrl { get; set; }

        public List<Auction> Auctions { get; set; } = new List<Auction>();
        public List<Bid> Bids { get; set; } = new List<Bid>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
