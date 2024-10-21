using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastucture.Context;
using Microsoft.EntityFrameworkCore;

namespace KFS.src.Infrastucture.Repository
{
   
        public class FeedbackRepository : IFeedbackRepository
        {
            private readonly KFSContext _context;
            public FeedbackRepository(KFSContext context) 
            {
                _context = context;
            }

            public async Task<bool> Delete(Guid id)
            {
                var feedback = await _context.Feedbacks.FirstOrDefaultAsync(x => x.Id == id); 
                if (feedback == null) throw new Exception("Feedback not found"); 
                _context.Feedbacks.Remove(feedback);
                int result = await _context.SaveChangesAsync();
                return result > 0;
            }

            public async Task<IEnumerable<Feedback>> GetFeedback() 
            {
                return await _context.Feedbacks.ToListAsync(); 
            }

            public async Task<bool> Create(Feedback feedback)
            {
                await _context.Feedbacks.AddAsync(feedback); 
                int result = await _context.SaveChangesAsync();
                return result > 0;
            }

            public async Task<Feedback> GetFeedbackById(Guid id) 
            {
                return await _context.Feedbacks.FirstOrDefaultAsync(x => x.Id == id) ?? throw new("Feedback not found"); 
            }

            public async Task<bool> Update(Feedback feedback)
            {
                _context.Feedbacks.Update(feedback); 
                int result = await _context.SaveChangesAsync();
                return result > 0;
            }
        }
    }

