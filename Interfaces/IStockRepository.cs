using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Extensions;
using WebApi.Helpers;
using WebApi.Model;

namespace WebApi.Interfaces;

public interface IStockRepository
{
    Task<PaginationResults<StockDto>> GetStocksAsync(QueryObject query);
    Task<Stock> GetStockByIdAsync(int id);
    Task<Stock> CreateStockAsync(CreateStockRequestDto stock);
    Task<Stock> UpdateStockAsync(UpdateStockRequestDto stock, int id);
    Task DeleteStockAsync(int id);
}