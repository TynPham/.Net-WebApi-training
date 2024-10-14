
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Context;
using WebApi.Dtos;
using WebApi.Extensions;
using WebApi.Helpers;
using WebApi.Interfaces;
using WebApi.Model;
using WebApi.Repositories;

namespace WebApi.Controllers;
[ApiController]
[Route("api/stocks")]
[Authorize]
public class StockController : ControllerBase
{
    private readonly IStockRepository _stockRepository;
    
    public StockController(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetStocks([FromQuery] QueryObject query)
    {
        var response = await _stockRepository.GetStocksAsync(query);
        return Ok(response);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStockById(int id)
    {
        var stock = await _stockRepository.GetStockByIdAsync(id);
        if (stock == null)
        {
            return NotFound();
        }
        return Ok(stock.Adapt<StockDto>());
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateStock([FromBody] CreateStockRequestDto request)
    {   
        var stock = await _stockRepository.CreateStockAsync(request);
        return CreatedAtAction(nameof(GetStockById), new { id = stock.Id }, stock.Adapt<StockDto>());
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStock(int id, [FromBody] UpdateStockRequestDto request)
    {
        var stock = await _stockRepository.UpdateStockAsync(request, id);
        return Ok(stock.Adapt<StockDto>());
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStock(int id)
    {
        await _stockRepository.DeleteStockAsync(id);
        return Ok(true);
    }
}