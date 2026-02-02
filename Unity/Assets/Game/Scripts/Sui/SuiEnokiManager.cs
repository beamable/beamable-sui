using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Beamable;
using Beamable.Common;
using Beamable.Common.Api.Auth;
using Beamable.Player;
using Beamable.Server.Clients;
using Cysharp.Threading.Tasks;
using Game.Scripts.Helpers;
using MoeBeam.Game.Scripts.Beam;
using Newtonsoft.Json;
using SuiFederationCommon;
using SuiFederationCommon.Models;
using SuiFederationCommon.Models.Notifications;
using SuiFederationCommon.Models.Oauth;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Sui
{
    public class SuiEnokiManager : MonoBehaviour
    {
        #region EXPOSED_VARIABLES
        
        [SerializeField] private Button signInButton;
        [SerializeField] private TextMeshProUGUI signInButtonText;
        [SerializeField] private TextMeshProUGUI walletIdText;
        [SerializeField] private TMP_Dropdown oAuthProviderDropDown;

        #endregion

        #region PRIVATE_VARIABLES
        
        private const string SignInText = "Sign In";
        private const string WaitingText = "Waiting for server response...";
        private const string AttachText = "Attach Enoki Wallet...";
        private const string AttachSuccessText = "Enoki Wallet Attached";
        private const string NoEnokiAttached = "No Enoki Wallet Attached";

        private string _oAuthProvider = "google";
        private bool _hasEnokiWalletAttached = false;
        ChallengeSolution _challengeSolution = null;
        
        #endregion

        #region PUBLIC_VARIABLES

        #endregion

        #region UNITY_CALLS

        private async void Start()
        {
            signInButton.onClick.AddListener(OnSignInButtonClicked);
            signInButtonText.text = SignInText;
            
            //Populate the dropdown with OAuth providers
            InitializeOAuthProviderDropdown();
            
            //Wait for BeamAccountManager to be ready
            await UniTask.WaitUntil(() => BeamAccountManager.Instance.IsReady);
            _challengeSolution = new ChallengeSolution();

            if (HasEnokiAttached(BeamAccountManager.Instance.CurrentAccount) == null)
            {
                _hasEnokiWalletAttached = false;
                return;
            }
            
            
            walletIdText.text = HasEnokiAttached(BeamAccountManager.Instance.CurrentAccount);
            if (walletIdText.text == NoEnokiAttached)
            {
                _hasEnokiWalletAttached = false;
                return;
            }
            
            //User has Enoki wallet attached
            _hasEnokiWalletAttached = true;
            signInButtonText.text = AttachSuccessText;
            signInButton.interactable = false;
        }

        private async void OnEnable()
        {
            BeamAccountManager.OnSetCurrentAccount += Reset;
            await UniTask.WaitUntil(() => BeamAccountManager.Instance.IsReady);
            BeamManager.BeamContext.Api.NotificationService.Subscribe(PlayerNotificationContext.OauthContext, OnNotificationServiceReceived);
        }
        
        private void OnDisable()
        {
            BeamAccountManager.OnSetCurrentAccount -= Reset;
            BeamManager.BeamContext.Api.NotificationService.Unsubscribe(PlayerNotificationContext.OauthContext, OnNotificationServiceReceived);
        }

        #endregion

        #region PUBLIC_METHODS
        
        
        public void OnProviderDropDownValueChanged(int value)
        {
            _oAuthProvider = value switch
            {
                0 => OauthProvider.Google,
                1 => OauthProvider.Twitch,
                _ => OauthProvider.Google,
            };
        }

        #endregion

        #region PRIVATE_METHODS
        
        private void InitializeOAuthProviderDropdown()
        {
            oAuthProviderDropDown.ClearOptions();
            var options = new List<string> { OauthProvider.Google.CapitalizeFirst(), OauthProvider.Twitch.CapitalizeFirst() };
            oAuthProviderDropDown.AddOptions(options);
            oAuthProviderDropDown.onValueChanged.AddListener(OnProviderDropDownValueChanged);
            // Set the default value to Google
            oAuthProviderDropDown.value = 0; // Google
            oAuthProviderDropDown.RefreshShownValue();
            _oAuthProvider = OauthProvider.Google;
        }
        
        private async void OnNotificationServiceReceived(object notification)
        {
            if (notification is not IDictionary<string, object> dict) return;
            if (!dict.TryGetValue("Id", out var idObj)) return;
            var idToken = idObj.ToString();
            if (string.IsNullOrEmpty(idToken))
            {
                _challengeSolution.challenge_token = null;
                Debug.Log($"Enoki login notification received with empty ID.");
                return;
            }

            _challengeSolution.solution = idToken;
            signInButtonText.text = AttachText;
            var playerAccount = BeamAccountManager.Instance.CurrentAccount;
            await BeamManager.BeamContext.Api.AuthService.AttachIdentity( OauthProvider.Google,
                    SuiFederationSettings.MicroserviceName, SuiFederationSettings.SuiEnokiIdentityName, _challengeSolution)
                .Then(t =>
                {
                    _hasEnokiWalletAttached = true;
                    signInButtonText.text = AttachSuccessText;
                    signInButton.interactable = false;
                });

            if (!_hasEnokiWalletAttached) return;
            await BeamAccountManager.Instance.SwitchAccount(playerAccount);
            walletIdText.text = HasEnokiAttached(BeamAccountManager.Instance.CurrentAccount);
            
        }
        
        private async void OnSignInButtonClicked()
        {
            if (_hasEnokiWalletAttached) return;
            
            //1st attach wallet
            try
            {
                string loginUrl = null;
                // Check if the Enoki wallet is already attached
                signInButtonText.text = WaitingText;   
                
                _challengeSolution.challenge_token = await GetUrlChallengeAsync();
                if (string.IsNullOrEmpty(_challengeSolution.challenge_token))
                {
                    Debug.LogError(" URL Challenge token is null or empty.");
                    return;
                }
                
                loginUrl = ParseChallengeToUrl(_challengeSolution.challenge_token);
                if (string.IsNullOrEmpty(loginUrl))
                {
                    Debug.LogError(" URL Challenge token is null or empty.");
                    _challengeSolution.challenge_token = null;
                    return;
                }
                
                Application.OpenURL(loginUrl);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to attach Enoki wallet: {e.Message}");
            }
            
        }

        private string ParseChallengeToUrl(string urlChallengeToken)
        {
            var parsedToken = BeamManager.BeamContext.Api.AuthService.ParseChallengeToken(urlChallengeToken);
            var challengeBytes = Convert.FromBase64String(parsedToken.challenge);
            return Encoding.UTF8.GetString(challengeBytes);
        }

        private async UniTask<string> GetUrlChallengeAsync()
        {
            _oAuthProvider = OauthProvider.Google;
            var result = await BeamManager.BeamContext.Api.AuthService
                .AttachIdentity(_oAuthProvider, SuiFederationSettings.MicroserviceName, 
                    SuiFederationSettings.SuiEnokiIdentityName);
            return result.challenge_token;
        }

        /// <summary>
        /// Returns the Enoki wallet userId if it is attached to the account.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        private string HasEnokiAttached(PlayerAccount account)
        {
            if(account.ExternalIdentities == null || account.ExternalIdentities.Length == 0)
                return NoEnokiAttached;
            foreach (var identity in account.ExternalIdentities)
            {
                if (identity.providerNamespace == SuiFederationSettings.SuiEnokiIdentityName)
                {
                    return identity.userId;
                }
            }

            return NoEnokiAttached;
        }
        
        private void Reset(PlayerAccount account)
        {
            _hasEnokiWalletAttached = string.IsNullOrEmpty(HasEnokiAttached(account));
            if (_hasEnokiWalletAttached)
            {
                signInButtonText.text = AttachSuccessText;
                signInButton.interactable = false;
                walletIdText.text = HasEnokiAttached(account);
                return;
            }
            _challengeSolution = new ChallengeSolution();
            signInButtonText.text = SignInText;
            signInButton.interactable = true;
            walletIdText.text = NoEnokiAttached;
        }

        #endregion

        
    }
}