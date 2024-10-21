using KFS.src.Domain.Entities;

namespace KFS.src.Domain.IRepository
{
    public interface IFeedbackRepository
    {
        public Task<IEnumerable<Feedback>> GetFeedback();
        public Task<bool> Create(Feedback media);
        public Task<bool> Update(Feedback media);
        public Task<bool> Delete(Guid id);
        public Task<Feedback> GetFeedbackById(Guid id);
    }
}
