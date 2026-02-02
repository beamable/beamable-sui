using System.Threading.Tasks;
using Beamable.SuiFederation.Features.Kiosk.Models;

namespace Beamable.SuiFederation.Features.Kiosk.Handlers;

public interface IKioskHandler
{
    Task ListForSale(KioskListModel model);
    Task DelistFromSale(KioskDelistModel model);
    Task PurchaseFromSale(KioskPurchaseModel model);
}