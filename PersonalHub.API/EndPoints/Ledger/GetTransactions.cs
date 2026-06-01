using Microsoft.AspNetCore.Mvc;
using PersonalHub.API.Dtos.Ledger;
using PersonalHub.API.Dtos.Pagination;
using PersonalHub.API.Helpers;
using PersonalHub.Application.Dtos.Ledger;
using PersonalHub.Application.Services;
using System.ComponentModel.DataAnnotations;

namespace PersonalHub.API.EndPoints.Ledger
{
    public static class GetTransactions
    {
        private const string ENDPOINT_NAME = "Get Transactions by Month and Year";
        public static RouteGroupBuilder MapGetTransactions(this RouteGroupBuilder endpointGroup)
        {
            endpointGroup.MapGet("",
                async (
                    HttpContext context,
                    LedgerService ledgerService,
                    [AsParameters] PaginationRequest request,
                    [FromQuery, Required] short month,
                    [FromQuery, Required] short year

                ) =>
                {
                    var transactions = ledgerService.GetTransactions(month, year);
                    var list = await PagedList<TransactionDto>.ToPagedList(transactions, request);
                    var response = new GetTransactionsResponse(list, list.Sum(x => x.Amount));
                    return Results.Ok(response);
                }
            )
            .HasApiVersion(ApiConstants.VERSION_1)
            .Produces<GetTransactionsResponse>(StatusCodes.Status200OK)
            .WithDescription("Return the list of transactions for the specified month and year and the total amount")
            .WithSummary(ENDPOINT_NAME)
            .WithName(ENDPOINT_NAME);
            return endpointGroup;
        }
    }
}
