using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Interfaces;
using Steria.Core.Specification.СommonSpec;

namespace Steria.Data.Persistence.Seed;

public class CommentSeeder(IGenericRepository<Auction> auctionRepository,
                           IGenericRepository<Comment> commentRepository,
                           UserManager<User> userManager)       
{
    public async Task SeedAsync()
    {
        var existing = await commentRepository.GetItemBySpec(new FirstRecordSpec<Comment>());
        if(existing is not null) return;
        
        var allUsers = await userManager.Users.ToListAsync();
        var auctions = await auctionRepository.GetAsync();
        
        var random = new Random();
        var adjectives = new[] { "great", "amazing", "cool", "awesome", "nice", "bad", "poor", "terrible" };
        var nouns = new[] { "car", "deal", "price", "condition", "engine", "design", "speed", "look" };
        var verbs = new[] { "love", "hate", "like", "dislike", "want", "need", "check", "see" };
        
        foreach (var auction in auctions)
        {
            int numComments = random.Next(25, 35);
            DateTime start = (DateTime)auction.StartTime;
            DateTime end = (DateTime)(auction.Status == AuctionStatus.Active ? DateTime.UtcNow : auction.EndTime);
            TimeSpan duration = end - start;

            var rootComments = new List<Comment>();
            
            // Creating root-comments
            for (int i = 0; i < numComments; i++)
            {
                var user = allUsers[random.Next(allUsers.Count)];
                double progress = (double)i / numComments;
                DateTime createdAt = start + TimeSpan.FromTicks((long)(duration.Ticks * progress));

                string text = $"{verbs[random.Next(verbs.Length)]} this {adjectives[random.Next(adjectives.Length)]} {nouns[random.Next(nouns.Length)]}!";
                if (random.Next(2) == 0) text += $" {verbs[random.Next(verbs.Length)]} it.";

                var comment = new Comment
                {
                    AuctionId = auction.Id,
                    UserId = user.Id,
                    Text = text,
                    CreatedAt = createdAt
                };

                rootComments.Add(comment);
            }
            
            auction.Comments = rootComments;
            await auctionRepository.UpdateAsync(auction);
            
            // Creating reply-comments
            var replies = new List<Comment>();
            var commentsToReply = rootComments.OrderBy(_ => random.Next()).Take(numComments / 3).ToList();
            foreach (var comment in commentsToReply)
            {
                int numReplies = random.Next(1, 4);
                for (int j = 0; j < numReplies; j++)
                {
                    var user = allUsers[random.Next(allUsers.Count)];
                    DateTime replyTime = comment.CreatedAt + TimeSpan.FromMinutes(random.Next(1, 60 * 24));

                    string replyText = $"Yes, I {verbs[random.Next(verbs.Length)]} that {nouns[random.Next(nouns.Length)]} too!";
                    if (random.Next(2) == 0) replyText += $" {adjectives[random.Next(adjectives.Length)]} choice.";

                    var reply = new Comment
                    {
                        AuctionId = auction.Id,
                        UserId = user.Id,
                        ReplyId = comment.Id,
                        Text = replyText,
                        CreatedAt = replyTime
                    };

                    replies.Add(reply);
                }
            }

            foreach (var reply in replies)
            {
                auction.Comments.Add(reply);
            }
            await auctionRepository.UpdateAsync(auction);
        }
    }
}