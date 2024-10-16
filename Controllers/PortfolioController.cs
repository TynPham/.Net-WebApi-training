using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;
using WebApi.Model;
using WebApi.Extensions;

namespace WebApi.Controllers;
[Route("/api/portfolios")]
[ApiController]
[Authorize]
public class PortfolioController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IStockRepository _stockRepo;
    private readonly IPortfolioRepository _portfolioRepo;
    public PortfolioController(UserManager<AppUser> userManager,
        IStockRepository stockRepo, IPortfolioRepository portfolioRepo)
    {
        _userManager = userManager;
        _stockRepo = stockRepo;
        _portfolioRepo = portfolioRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserPortfolios()
    {
        var username = User.GetUserName();
        var appUser = await _userManager.FindByNameAsync(username);
        var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
        return Ok(userPortfolio);
    }

    [HttpPost]
    public async Task<IActionResult> AddPortfolio(string symbol)
    {
        var username = User.GetUserName();
        var appUser = await _userManager.FindByNameAsync(username);
        var stock = await _stockRepo.GetStockBySymbolAsync(symbol);
        if (stock == null)
        {
            return NotFound("Stock not found");
        }
        var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
        if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower()))
        {
            return BadRequest("Cannot add same stock to portfolio");
        }
        var portfolioModel = new Portfolio
        {
            StockId = stock.Id,
            AppUserId = appUser.Id
        };
        await _portfolioRepo.CreateAsync(portfolioModel);
        if (portfolioModel == null)
        {
            return StatusCode(500, "Could not create");
        }
        else
        {
            return Ok("Portfolio created successfully");
        }
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeletePortfolio(string symbol)
    {
        var username = User.GetUserName();
        var appUser = await _userManager.FindByNameAsync(username);
        var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
        var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();
        if (filteredStock.Count() == 1)
        {
            await _portfolioRepo.DeletePortfolio(appUser, symbol);
        }
        else
        {
            return NotFound("Stock not in your portfolio");
        }
        return Ok();
    }
}