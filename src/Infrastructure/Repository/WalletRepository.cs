using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace KFS.src.Infrastructure.Repository
{
    public class WalletRepository : IWalletRepository
    {
        private readonly KFSContext _context;
        public WalletRepository(KFSContext context)
        {
            _context = context;
        }
        public async Task<bool> AddPoint(Guid walletId, int point)
        {
            var wallet = _context.Wallets.FirstOrDefault(x => x.Id == walletId) ?? throw new Exception("Wallet not found");
            wallet.Point += point;
            _context.Wallets.Update(wallet);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> CreateWallet(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteWallet(Guid walletId)
        {
            var wallet = _context.Wallets.FirstOrDefault(x => x.Id == walletId) ?? throw new Exception("Wallet not found");
            _context.Wallets.Remove(wallet);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Wallet> GetWalletById(Guid walletId)
        {
            return await _context.Wallets.FirstOrDefaultAsync(x => x.Id == walletId) ?? throw new Exception("Wallet not found");
        }

        public async Task<Wallet> GetWalletByUserId(Guid userId)
        {
            return await _context.Wallets.FirstOrDefaultAsync(x => x.UserId == userId) ?? throw new Exception("Wallet not found");
        }

        public async Task<IEnumerable<Wallet>> GetWallets()
        {
            return await _context.Wallets.ToListAsync();
        }

        public async Task<bool> UpdateWallet(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UsePoint(Guid walletId, int point)
        {
            var wallet = _context.Wallets.FirstOrDefault(x => x.Id == walletId) ?? throw new Exception("Wallet not found");
            wallet.Point -= point;
            _context.Wallets.Update(wallet);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}