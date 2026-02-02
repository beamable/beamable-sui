using System.Threading.Tasks;
using Beamable.SuiFederation.Features.Contract.Models;

namespace Beamable.SuiFederation.Features.Contract.Handlers;

public interface IContentContractHandler
{
    Task HandleContract(ContentContractsModel model);
    Task HandleExistingContract(ContentContractsModel model);
}