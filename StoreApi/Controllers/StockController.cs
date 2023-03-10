using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApi.Data;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public StockController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        [Route("GetStock")]
        public async Task<ActionResult<List<Stock>>> GetStock()
        {
            return Ok(await _dataContext.Stocks.ToListAsync());
        }

        



        [HttpPost]
        [Route("AddStock")]
        public async Task<ActionResult<IEnumerable<List<Stock>>>> AddStock(Stock stock)
        {
            _dataContext.Stocks.Add(stock);
            await _dataContext.SaveChangesAsync();

            return Ok(stock);
        }

        [HttpPut]
        [Route("UpdateStock")]
        public async Task<ActionResult<List<Stock>>> UpdateStock(Stock request)
        {
            var dbStock = await _dataContext.Stocks.FindAsync(request.StockId);
            if (dbStock == null)
                return BadRequest("Stock not found.");

            dbStock.Product_id = request.Product_id;
            dbStock.CreateUserId = request.CreateUserId;
            dbStock.UpdateUserId = request.UpdateUserId;
            dbStock.Quantity = request.Quantity;
            dbStock.Status = request.Status;
            dbStock.CreatedDate = request.CreatedDate;
            dbStock.UpdatedDate = request.UpdatedDate;

            await _dataContext.SaveChangesAsync();

            return Ok(request);
        }
        [HttpDelete]
        [Route("DeleteStock/{id}")]
        public async Task<ActionResult<List<Stock>>> DeleteStock(int id)
        {
            var dbStock = await _dataContext.Stocks.FindAsync(id);
            if (dbStock == null)
                return BadRequest("Hero not found.");

            _dataContext.Stocks.Remove(dbStock);
            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.Stocks.ToListAsync());
        }
    }
}
