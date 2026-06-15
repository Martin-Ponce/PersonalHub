using Microsoft.AspNetCore.Mvc;
using PersonalHub.API.Dtos.Ledger;
using PersonalHub.API.Helpers;
using PersonalHub.Application.Dtos.Ledger;
using PersonalHub.Application.Services;

namespace PersonalHub.API.EndPoints.Ledger
{
    public static class SaveTransaction
    {
        private const string ENDPOINT_NAME = "Save Transaction";
        public static RouteGroupBuilder MapSaveTransaction(this RouteGroupBuilder endpointGroup)
        {
            endpointGroup.MapPost("transactions",
                async (
                    HttpContext context,
                    LedgerService ledgerService,
                    [FromBody] SaveTransactionRequest request
                ) =>
                {
                    var userId = Guid.Parse(context.User.FindFirst(ApiConstants.Claims.USER_ID)?.Value
                        ?? throw new UnauthorizedAccessException("User ID claim not found"));
                    var transactionRequest = new TransactionRequest(userId, request.Month, request.Year, request.Type, request.CategoryId, request.Description, request.Amount);
                    var response = await ledgerService.CreateTransaction(transactionRequest);
                    return Results.Created($"/ledger/{response.Transaction.Id}", response);
                }
            )
            .RequireAuthorization()
            .HasApiVersion(ApiConstants.VERSION_1)
            .Produces<TransactionResponse>(StatusCodes.Status201Created)
            .WithDescription("Creates a new transaction and returns the created transaction with the updated monthly total")
            .WithSummary(ENDPOINT_NAME)
            .WithName(ENDPOINT_NAME);
            return endpointGroup;
        }
    }
}
