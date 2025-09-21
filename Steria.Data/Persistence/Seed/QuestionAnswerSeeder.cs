using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Interfaces;
using Steria.Core.Specification.СommonSpec;

namespace Steria.Data.Persistence.Seed;

public class QuestionAnswerSeeder(IGenericRepository<Auction> auctionRepository,
                                  IGenericRepository<Answer> answerRepository,
                                  UserManager<User> userManager)   
{
    public async Task SeedAsync()
    {
        var existed = await answerRepository.GetItemBySpec(new FirstRecordSpec<Answer>());
        if (existed is not null) return;

        var allUsers = await userManager.Users.ToListAsync();
        var auctions = await auctionRepository.GetAsync();

        var random = new Random();
        var topics = new[] { "price", "condition", "delivery", "history", "maintenance", "performance" };
        var verbs = new[] { "is", "are", "can", "will", "does", "should" };
        var adjectives = new[] { "good", "bad", "fair", "great", "poor", "excellent" };

        foreach (var auction in auctions)
        {
            auction.Questions = new List<Question>();


            int numQuestions = random.Next(3, 5);

            DateTime start = (DateTime)auction.StartTime;
            DateTime end = (DateTime)(auction.Status == AuctionStatus.Active ? DateTime.UtcNow : auction.EndTime);
            TimeSpan duration = end - start;

            var questions = new List<Question>();

            for (int i = 0; i < numQuestions; i++)
            {
                var user = allUsers.Where(u => u.Id != auction.SellerId).OrderBy(_ => random.Next()).First();
                double progress = (double)i / numQuestions;
                DateTime createdAt = start + TimeSpan.FromTicks((long)(duration.Ticks * progress));

                string questionText =
                    $"What {verbs[random.Next(verbs.Length)]} the {adjectives[random.Next(adjectives.Length)]} {topics[random.Next(topics.Length)]}?";
                if (random.Next(2) == 0) questionText += $" Please clarify.";

                var question = new Question
                {
                    AuctionId = auction.Id,
                    UserId = user.Id,
                    QuestionText = questionText,
                    CreatedAt = createdAt
                };

                questions.Add(question);
                auction.Questions.Add(question);
                await auctionRepository.UpdateAsync(auction);
            }

            foreach (var question in questions)
            {
                double progress = questions.IndexOf(question) / (double)numQuestions + 0.1;
                DateTime answerTime = question.CreatedAt + TimeSpan.FromMinutes(random.Next(15, 1440));

                string answerText =
                    $"The {topics[random.Next(topics.Length)]} {verbs[random.Next(verbs.Length)]} {adjectives[random.Next(adjectives.Length)]}.";
                if (random.Next(2) == 0) answerText += $" More details will follow.";

                var answer = new Answer
                {
                    QuestionId = question.Id,
                    UserId = auction.SellerId,
                    AnswerText = answerText,
                    CreatedAt = answerTime
                };

                question.Answer = answer;
                await answerRepository.InsertAsync(answer);
            }
        }
    }
}