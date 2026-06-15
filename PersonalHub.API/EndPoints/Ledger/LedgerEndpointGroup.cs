namespace PersonalHub.API.EndPoints.Ledger
{
    public static class LedgerEndpointGroup
    {
        public static RouteGroupBuilder MapLedgerEndpoints(this RouteGroupBuilder appEndpoints)
        {
            var ledgerEndpointGroup = appEndpoints.MapGroup("ledger").WithTags("Ledger");
            ledgerEndpointGroup.MapEndpoints();
            return appEndpoints;
        }
        private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder endpointGroup)
        {
            endpointGroup.MapGetTransactions();
            endpointGroup.MapSaveTransaction();
            return endpointGroup;
        }
    }
}
