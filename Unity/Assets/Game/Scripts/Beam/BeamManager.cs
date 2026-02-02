using System;
using System.Collections.Generic;
using System.Threading;
using Beamable;
using Beamable.Server;
using Beamable.Server.Clients;
using Cysharp.Threading.Tasks;
using MoeBeam.Game.Scripts.Sui;
using Unity.VisualScripting;
using UnityEngine;

namespace MoeBeam.Game.Scripts.Beam
{
    public class BeamManager : GenericSingleton<BeamManager>
    {
        #region EXPOSED_VARIABLES
        
        [Header("Managers (init in this exact order)")]
        [SerializeField] private List<MonoBehaviour> beamManagers = new();

        #endregion

        #region PRIVATE_VARIABLES
        
        private CancellationTokenSource _cts;
        private readonly List<IBeamManager> _managers = new();

        #endregion

        #region PUBLIC_VARIABLES
        
        public static bool IsReady { get; private set; }
        public static BeamContext BeamContext { get; private set; }
        public static SuiFederationClient SuiClient { get; private set; }
        public IReadOnlyList<IBeamManager> Managers => _managers;
        
        public event Action Initialized;
        public event Action ResetStarted;
        public event Action ResetCompleted;


        #endregion

        #region UNITY_CALLS

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
            RebuildManagerCacheFromList();
        }

        private void Start()
        {
            InitAllAsync().Forget();
        }
        
        private void RebuildManagerCacheFromList()
        {
            _managers.Clear();
            foreach (var manager in beamManagers)
            {
                if (manager == null) continue;
                if (manager is IBeamManager gm) _managers.Add(gm);
                else Debug.LogError($"'{manager.name}' does not implement IGameManager.");
            }
        }
        

        #endregion

        #region PUBLIC_METHODS
        
        public async UniTask InitAllAsync(CancellationToken externalCt = default)
        {
            if (IsReady) return;

            _cts?.Cancel();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(externalCt);
            var ct = _cts.Token;

            await Init();

            // Forward order == exact Inspector order
            foreach (var m in _managers)
            {
                ct.ThrowIfCancellationRequested();
                await m.InitAsync(ct);
            }

            IsReady = true;
            Initialized?.Invoke();
        }

        public async UniTask ResetAllAsync(CancellationToken externalCt = default)
        {
            ResetStarted?.Invoke();

            _cts?.Cancel();
            using var localCts = CancellationTokenSource.CreateLinkedTokenSource(externalCt);
            var ct = localCts.Token;

            // Reverse order for safe teardown
            for (int i = _managers.Count - 1; i >= 0; i--)
            {
                ct.ThrowIfCancellationRequested();
                try { await _managers[i].ResetAsync(ct); }
                catch (Exception e) { Debug.LogException(e); /* best-effort cleanup */ }
            }

            IsReady = false;
            ResetCompleted?.Invoke();
        }

        public async UniTask ReinitializeAsync(CancellationToken ct = default)
        {
            await ResetAllAsync(ct);
            await InitAllAsync(ct);
        }

        #endregion

        #region PRIVATE_METHODS

        private async UniTask Init()
        {
            BeamContext ??= BeamContext.Default;
            await BeamContext.OnReady;
            SuiClient ??= new SuiFederationClient(BeamContext);
        }

        #endregion

        
    }
}