using Microsoft.AspNetCore.Mvc;
using LoanCRUD.Models;
using LoanCRUD.Services;

namespace LoanCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly FirestoreService _firestoreService;

    public LoansController(FirestoreService firestoreService)
    {
        _firestoreService = firestoreService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateLoan([FromBody] Loan loan)
    {
        if (loan.Amount <= 0 || string.IsNullOrWhiteSpace(loan.Purpose) || loan.TermInMonths <= 0)
        {
            return BadRequest("Invalid loan data. Please provide a valid Amount, Purpose, and Term.");
        }

        if (string.IsNullOrWhiteSpace(loan.UserId))
        {
            return BadRequest("UserId is required.");
        }

        var id = await _firestoreService.CreateLoanAsync(loan);
        return CreatedAtAction(nameof(GetLoan), new { id }, id);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLoan(string id, [FromQuery] string userId)
    {
        var loan = await _firestoreService.GetLoanAsync(userId, id);
        if (loan == null)
        {
            return NotFound();
        }
        return Ok(loan);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLoans([FromQuery] string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return BadRequest("UserId query parameter is required.");
        }

        var loans = await _firestoreService.GetAllLoansForUserAsync(userId);
        return Ok(loans);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLoan(string id, [FromBody] Loan loan)
    {
        if (loan.Amount <= 0 || string.IsNullOrWhiteSpace(loan.Purpose) || loan.TermInMonths <= 0)
        {
            return BadRequest("Invalid loan data. Please provide a valid Amount, Purpose, and Term.");
        }

        if (string.IsNullOrWhiteSpace(loan.UserId))
        {
            return BadRequest("UserId is required.");
        }

        try
        {
            await _firestoreService.UpdateLoanAsync(loan.UserId, id, loan);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLoan(string id, [FromQuery] string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return BadRequest("UserId query parameter is required.");
        }

        try
        {
            await _firestoreService.DeleteLoanAsync(userId, id);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}

}

