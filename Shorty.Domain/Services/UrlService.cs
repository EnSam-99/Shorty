using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Data;
using Shorty.Dal.Entities;
using Shorty.Domain.Interfaces;

namespace Shorty.Domain.Services
{
    public class UrlService : IUrlService
    {
        private readonly ApplicationDbContext _context;

        public UrlService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UrlMapping> CreateShortUrlAsync(Guid userId, string originalUrl)
        {
            var existing = await _context.UrlMappings
                .FirstOrDefaultAsync(u => u.OriginalURL == originalUrl && u.UserId == userId);

            if (existing != null)
                return existing;

            string shortUrl;

            do
            {
                shortUrl = GenerateShortUrl();
            } while (await _context.UrlMappings.AnyAsync(u => u.ShortURL == shortUrl));

            var mapping = new UrlMapping
            {
                UserId = userId,
                OriginalURL = originalUrl,
                ShortURL = shortUrl
            };

            await _context.UrlMappings.AddAsync(mapping);
            await _context.SaveChangesAsync();

            return mapping;
        }

        public async Task<string?> GetOriginalUrlAsync(string shortUrl)
        {
            var mapping = await _context.UrlMappings
                .FirstOrDefaultAsync(u => u.ShortURL == shortUrl);

            return mapping?.OriginalURL;
        }


        public async Task<IEnumerable<UrlMapping>> GetUserUrlsAsync(Guid userId)
        {
            return await _context.UrlMappings
                .Where(u => u.UserId == userId)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
        }

        //private static string GenerateShortUrl()
        //{
        //    const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        //    return new string(Enumerable.Range(0, 8)
        //        .Select(_ => chars[new Random().Next(chars.Length)]).ToArray());
        //}

        private static string GenerateShortUrl()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var bytes = new byte[8];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(bytes);

            var result = new StringBuilder(8);
            foreach (var b in bytes)
                result.Append(chars[b % chars.Length]);

            return result.ToString();
        }
    }
}
