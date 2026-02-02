using Beamable.Common;
using Beamable.Server;
using Beamable.SuiFederation.Endpoints.Kiosk;

namespace Beamable.SuiFederation;

public partial class SuiFederation
{
    [ClientCallable]
    public async Promise PersonalKioskCreate()
    {
        await Provider.GetService<PersonalKioskCreateEndpoint>().PersonalKioskCreate();
    }

    [ClientCallable]
    public async Promise PersonalKioskList(long itemId, long price, long exclusiveBuyer)
    {
        await Provider.GetService<PersonalKioskListEndpoint>().PersonalKioskList(itemId, price, exclusiveBuyer);
    }

    [ClientCallable]
    public async Promise PersonalKioskDelist(string listingId, bool remove)
    {
        await Provider.GetService<PersonalKioskDelistEndpoint>().PersonalKioskDelist(listingId, remove);
    }

    [ClientCallable]
    public async Promise PersonalKioskTake(string listingId)
    {
        await Provider.GetService<PersonalKioskTakeEndpoint>().PersonalKioskTake(listingId);
    }

    [ClientCallable]
    public async Promise PersonalKioskDeclinePurchase(string listingId)
    {
        await Provider.GetService<PersonalKioskDeclinePurchaseEndpoint>().PersonalKioskDeclinePurchase(listingId);
    }

    [ClientCallable]
    public async Promise PersonalKioskCancelExclusive(string listingId)
    {
        await Provider.GetService<PersonalKioskCancelExclusiveEndpoint>().PersonalKioskCancelExclusive(listingId);
    }

    [ClientCallable]
    public async Promise PersonalKioskPurchase(string listingId)
    {
        await Provider.GetService<PersonalKioskPurchaseEndpoint>().PersonalKioskPurchase(listingId);
    }

    [ClientCallable]
    public async Promise PersonalKioskWithDraw(long amount)
    {
        await Provider.GetService<PersonalKioskWithdrawEndpoint>().PersonalKioskWithdraw(amount);
    }
}