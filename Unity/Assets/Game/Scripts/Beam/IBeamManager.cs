using System.Threading;
using Cysharp.Threading.Tasks;
using MoeBeam.Game.Scripts.Sui;
using UnityEngine;

namespace MoeBeam.Game.Scripts.Beam
{
    public interface IBeamManager
    {
        UniTask InitAsync(CancellationToken ct);
        UniTask ResetAsync(CancellationToken ct);
    }
    
    public abstract class BeamManagerBase<T> : GenericSingleton<T>, IBeamManager
        where T : BeamManagerBase<T>
    {
        public abstract UniTask InitAsync(CancellationToken ct);
        public virtual UniTask ResetAsync(CancellationToken ct) => UniTask.CompletedTask;
    }
}