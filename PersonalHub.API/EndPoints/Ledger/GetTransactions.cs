using PersonalHub.API.Dtos.Pagination;
using PersonalHub.API.Helpers;

namespace PersonalHub.API.EndPoints.Ledger
{
    public static class GetTransactions
    {
        private const string ENDPOINT_NAME = "Obtener Transacciones por Mes y Año";
        public static RouteGroupBuilder MapGetTransactions(this RouteGroupBuilder endpointGroup)
        {
            endpointGroup.MapGet("",
                async (
                    HttpContext context,
                    CategoryService categoryService,
                    [AsParameters] PaginationRequest request,
                    [FromQuery, Required] short month,
                    [FromQuery, Required] short year

                ) =>
                {
                    var categories = await categoryService.GetCategories();
                    var response = await PagedList<GetCategoriesResponse>.ToPagedList(categories, request);
                    return Results.Ok(response);
                }
            )
            .HasApiVersion(ApiConstants.VERSION_1)
            .Produces<PagedList<GetCategoriesResponse>>(StatusCodes.Status200OK)
            .WithDescription("Devuelve el listado de categorías y subcategorías")
            .WithSummary(ENDPOINT_NAME)
            .WithName(ENDPOINT_NAME);
            return endpointGroup;
        }
    }
}
