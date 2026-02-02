using Beamable.SuiFederation.Features.SuiApi;
using Beamable.SuiFederation.Features.Transactions;

namespace Beamable.SuiFederation.Features.Kiosk;

public class KioskService : IService
{
    private readonly SuiApiService _suiApiService;
    private readonly TransactionManagerFactory _transactionManagerFactory;
    public KioskService(SuiApiService suiApiService, TransactionManagerFactory transactionManagerFactory)
    {
        _suiApiService = suiApiService;
        _transactionManagerFactory = transactionManagerFactory;
    }
}