using Mapster;
using Microsoft.EntityFrameworkCore;
using WebApi.Context;
using WebApi.Dtos;
using WebApi.Dtos.Comment;
using WebApi.Extensions;
using WebApi.Helpers;
using WebApi.Interfaces;
using WebApi.Model;

namespace WebApi.Repositories;

public class StockRepository : IStockRepository
{
    private readonly AppDbContext _context;

    public StockRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PaginationResults<StockDto>> GetStocksAsync(QueryObject query)
    {
        var stocks = _context.Stocks.AsQueryable();
        if (!string.IsNullOrWhiteSpace(query.CompanyName))
        {
            stocks = stocks.Where(x => x.CompanyName == query.CompanyName);
        }

        var take = query.limit;
        var skip = (query.page - 1) * take;
        var total = await stocks.CountAsync();
        var response = await stocks.Skip(skip).Take(take).Include(x => x.Comments).ThenInclude(a => a.AppUser).ToListAsync();
        
        TypeAdapterConfig<Comment, CommentStockDto>.NewConfig()
            .Map(dest => dest.createdBy, src => src.AppUser.UserName);
        
        var items  = response.Adapt<List<StockDto>>();
        
        var results = new PaginationResults<StockDto>(items, total, query.page, query.limit);
        
        return results;
    }

    public async Task<Stock?> GetStockByIdAsync(int id)
    {
        return await _context.Stocks.Include(x => x.Comments).ThenInclude(a => a.AppUser).FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<Stock?> GetStockBySymbolAsync(string symbol)
    {
        return await _context.Stocks.FirstOrDefaultAsync(x => x.Symbol == symbol);
    }

    public async Task<Stock> CreateStockAsync(CreateStockRequestDto stock)
    {
        var newStock = new Stock
        {
            Symbol = stock.Symbol,
            CompanyName = stock.CompanyName,
            Purchase = stock.Purchase,
            LastDiv = stock.LastDiv,
            Industry = stock.Industry,
            MarketCap = stock.MarketCap
        };

        _context.Stocks.Add(newStock);
        await _context.SaveChangesAsync();

        return newStock;
    }

    public async Task<Stock> UpdateStockAsync(UpdateStockRequestDto stock, int id)
    {
        var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

        if (existingStock == null)
        {
            throw new Exception("Stock not found");
        }

        existingStock.Symbol = stock.Symbol;
        existingStock.CompanyName = stock.CompanyName;
        existingStock.Purchase = stock.Purchase;
        existingStock.LastDiv = stock.LastDiv;
        existingStock.Industry = stock.Industry;
        existingStock.MarketCap = stock.MarketCap;

        await _context.SaveChangesAsync();

        return existingStock;
    }

    public async Task DeleteStockAsync(int id)
    {
        var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

        if (stock == null)
        {
            throw new Exception("Stock not found");
        }

        _context.Stocks.Remove(stock);
        await _context.SaveChangesAsync();
    }
}