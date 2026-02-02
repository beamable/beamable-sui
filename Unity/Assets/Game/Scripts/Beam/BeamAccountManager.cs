using System;
using System.Threading;
using Beamable;
using Beamable.Player;
using Beamable.Server.Clients;
using Cysharp.Threading.Tasks;
using MoeBeam.Game.Scripts.Sui;
using SuiFederationCommon;
using UnityEngine;

namespace MoeBeam.Game.Scripts.Beam
{
    public class BeamAccountManager : BeamManagerBase<BeamAccountManager>
    {
        #region EXPOSED_VARIABLES

        #endregion

        #region PRIVATE_VARIABLES

        private BeamContext _beamContext;
        private bool _switchingAccounts = false;

        #endregion

        #region PUBLIC_VARIABLES

        public bool IsReady { get; private set; } = false;
        public bool IsNewAccountCreated { get; private set; }
        public long PlayerId { get; private set; }
        public PlayerAccount CurrentAccount { get; private set; }

        #endregion

        #region Actions

        public static event Action<PlayerAccount> OnSetCurrentAccount;

        #endregion

        #region UNITY_CALLS

        public override async UniTask InitAsync(CancellationToken ct)
        {
            try
            {
                _beamContext = BeamManager.BeamContext;
                await _beamContext.Accounts.OnReady;
                UpdateCurrentAccount(_beamContext.Accounts.Current);
                await UniTask.WaitUntil(CheckGamerTag, cancellationToken: ct);
                IsReady = true;
                Debug.Log($"Initialized BeamAccountManager with account: {CurrentAccount.GamerTag}");
            }
            catch (Exception e)
            {
                Debug.LogError($"AccountSignIn Start error: {e.Message}");
            }
        }

        public override async UniTask ResetAsync(CancellationToken ct)
        {
            await UniTask.Yield();
            IsReady = false;
            IsNewAccountCreated = false;
        }

        private bool CheckGamerTag() => CurrentAccount?.GamerTag != 0;


        #endregion

        #region PRIVATE_METHODS

        public async UniTask SwitchAccount(PlayerAccount newAccount)
        {
            if (_switchingAccounts) return;
            _switchingAccounts = true;
            
            await _beamContext.Accounts.SwitchToAccount(newAccount);
            if (newAccount is {HasDeviceId: false})
            {
                await _beamContext.Accounts.AddDeviceId(newAccount);
            }

            UpdateCurrentAccount(newAccount);
            _switchingAccounts = false;
            Debug.Log($"Switched to account: {newAccount.GamerTag}");
        }

        private void UpdateCurrentAccount(PlayerAccount newAccount)
        {
            CurrentAccount = newAccount;
            PlayerId = CurrentAccount?.GamerTag ?? 0;
            
            OnSetCurrentAccount?.Invoke(newAccount);
        }

        #endregion

        #region PUBLIC_METHODS
        
        public void ForceSetNewAccountCreated(bool value)
        {
            IsNewAccountCreated = value;
        }
        
        public async UniTask ChangeAlias(string alias)
        {
            try
            {
                await _beamContext.Accounts.SetAlias(alias, CurrentAccount);
                UpdateCurrentAccount(CurrentAccount);
                IsNewAccountCreated = true;
            }
            catch (Exception e)
            {
                Debug.LogError($"ChangeName {alias} error: {e.Message}");
            }
        }

        public async UniTask CreateNewAccount()
        {
            try
            {
                var newAccount = await _beamContext.Accounts.CreateNewAccount();
                await SwitchAccount(newAccount);
                await _beamContext.Accounts.AddExternalIdentity<SuiWeb3Identity, SuiFederationClient>("", 
                    (AsyncChallengeHandler) null, newAccount);
                Debug.Log($"New account {newAccount.GamerTag} has been created.");
            }
            catch (Exception e)
            {
                Debug.LogError($"Create New Account error: {e.Message}");
            }
        }
        
        public async UniTask<RegistrationResult> AddStashedExternalIdentity(string token, AsyncChallengeHandler challengeHandler)
        {
            try
            {
                var result = await _beamContext.Accounts.AddExternalIdentity<SuiWeb3ExternalIdentity, SuiFederationClient>
                    (token, challengeHandler, CurrentAccount);
                return result;
            }
            catch (Exception e)
            {
                Debug.LogError($"AddStashedExternalIdentity error: {e.Message}");
                return null;
            }
        }

        #endregion
        
    }
}