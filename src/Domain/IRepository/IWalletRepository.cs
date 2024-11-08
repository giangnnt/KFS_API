using KFS.src.Domain.Entities;

namespace KFS.src.Domain.IRepository
{
    public interface IWalletRepository
    {
        public Task<bool> AddPoint(Guid walletId, int point);
        public Task<bool> UsePoint(Guid walletId, int point);
        public Task<bool> CreateWallet(Wallet wallet);
        public Task<bool> DeleteWallet(Guid walletId);
        public Task<IEnumerable<Wallet>> GetWallets();
        public Task<Wallet> GetWalletById(Guid walletId);
        public Task<bool> UpdateWallet(Wallet wallet);
        public Task<Wallet> GetWalletByUserId(Guid userId);
    }
}