using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beamable;
using Beamable.Common.Api.Inventory;
using Beamable.Server.Clients;
using Cysharp.Threading.Tasks;
using MoeBeam.Game.Scripts.Beam;
using MoeBeam.Game.Scripts.Helpers;
using SuiFederationCommon.FederationContent;
using SuiFederationCommon.Models.Kiosk;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace Game.Scripts.Kiosk
{
    public class Kiosk
    {
        public string DisplayName;
        public string ContentId;
        public string SoldItemContentId;
        public string CurrencyContentId;
    }
    
    public class KioskManager : MonoBehaviour
    {
        #region EXPOSED_VARIABLES

        [SerializeField] private KioskItemRef[] kioskWeaponRefs;
        [SerializeField] private ScrollRect inventoryScrollView;
        [SerializeField] private GameObject refreshingPanel;
        
        
        [Header("Currency")]
        [SerializeField] private TextMeshProUGUI playerCurrencyText;
        [SerializeField] private Image playerCurrencyIcon;

        [Header("Kiosk")] 
        [SerializeField] private GameObject noListingsText;
        [SerializeField] private ScrollRect kioskScrollView;
        [SerializeField] private BeamButton kioskButton;
        
        [Header("Kiosk Tabs")]
        [SerializeField] private Transform tabsParent;
        
        [Header("Prefabs")]
        [SerializeField] private KioskPlayerInventoryCard inventoryCardPrefab;
        [SerializeField] private KioskListingCard kioskListingCardPrefab;
        [SerializeField] private KioskTab kioskTabPrefab;

        #endregion

        #region PRIVATE_VARIABLES

        private bool _isOwnListingsTab = true;
        private KioskListingCard _selectedListingCard;
        private Kiosk _currentKiosk;
        private List<KioskListingCard> _currentListings = new List<KioskListingCard>();
        private List<Kiosk> _kiosksList = new List<Kiosk>();
        private List<KioskTab> _kioskTabs = new List<KioskTab>();
        private SuiFederationClient _client;

        #endregion

        #region PUBLIC_VARIABLES

        #endregion

        #region UNITY_CALLS
        
        private async void Start()
        {
            await UniTask.WaitUntil(() => BeamInventoryManager.Instance.IsReady);
            _client = BeamManager.SuiClient;

            await UniTask.WaitUntil(()=> _client != null);
            kioskButton.SetInteractable(false);
            await ResolveKiosk();
            PopulateTabs();
            await RefreshKioskPanel();
            LoadPlayerInventory().Forget(); 
            GetKioskListings(true).Forget(); //first tab is own listings
            await _client.PersonalKioskCreate();
        }

        private async void OnEnable()
        {
            RefreshKioskPanel().Forget();
            BeamInventoryManager.OnInventoryRefreshed += OnInventoryUpdate;
        }

        private void OnDisable()
        {
            BeamInventoryManager.OnInventoryRefreshed -= OnInventoryUpdate;
        }

        #endregion

        #region PUBLIC_METHODS
        
        [ContextMenu("Load Player Inventory")]
        public async UniTask LoadPlayerInventory()
        {
            Debug.Log($"Loading player inventory for kiosk");
            foreach (Transform child in inventoryScrollView.content)
            {
                Destroy(child.gameObject);
            }
            var weapons = BeamInventoryManager.Instance.PlayerWeapons;
            if(weapons == null || weapons.Count < 1) return;
            foreach (var weapon in weapons)
            {
                var card = Instantiate(inventoryCardPrefab, inventoryScrollView.content);
                card.Init(weapon, GetKioskIdToSellOn(weapon.ContentId), this);
                card.gameObject.SetActive(true);
            }
            await UniTask.Yield();
        }

        public void OnCurrentOwnListingSelected(KioskListingCard listingCard)
        {
            foreach (var card in _currentListings.Where(card => card != listingCard))
            {
                card.Deselect();
            }
            
            _selectedListingCard = listingCard;
            kioskButton.SetInteractable(true);
            kioskButton.RemoveAllListeners();
            if (_isOwnListingsTab)
            {
                kioskButton.AddListener(OnRemoveOwnListing);
            }
            else
            {
                kioskButton.AddListener(OnPurchaseListing);
            }
        }
        
        public void OnTabSelected(KioskTab selectedTab, Kiosk kiosk, bool ownListings)
        {
            foreach (var tab in _kioskTabs.Where(tab => tab != selectedTab))
            {
                tab.Deselect();
            }

            _isOwnListingsTab = ownListings;
            _currentKiosk = kiosk;
            var contentId = kiosk?.ContentId ?? "";
            GetKioskListings(ownListings, contentId).Forget();
        }

        [ContextMenu("Get All Kiosks")]
        public async void GetAllKiosk()
        {
            var kiosks = await _client.KioskList();
            Debug.Log(kiosks);
        }
        
        #endregion

        #region PRIVATE_METHODS
        
        private void PopulateTabs()
        {
            var playerTab = Instantiate(kioskTabPrefab, tabsParent);
            playerTab.Init("Player Listings", null, this);
            playerTab.Select();
            _kioskTabs.Add(playerTab);
            
            _isOwnListingsTab = true;
            foreach (var kiosk in _kiosksList)
            {
                var tab = Instantiate(kioskTabPrefab, tabsParent);
                //Knwoing we will only have one weapon kiosk
                tab.Init(kiosk.DisplayName, kiosk, this);
                _kioskTabs.Add(tab);
            }
        }
        
        private string GetKioskIdToSellOn(string itemContentId)
        {
            foreach (var kiosk in _kiosksList)
            {
                if (itemContentId.Contains(kiosk.SoldItemContentId))
                    return kiosk.ContentId;
            }
            return null;
        }
        
        private async UniTask ResolveKiosk()
        {
            _kiosksList = new List<Kiosk>();
            foreach (var kioskWeaponRef in kioskWeaponRefs)
            {
                var resolvedKiosk = await kioskWeaponRef.Resolve();
                if (resolvedKiosk != null)
                {
                    _kiosksList.Add(new Kiosk
                    {
                        ContentId = resolvedKiosk.Id,
                        DisplayName = resolvedKiosk.KioskName,
                        SoldItemContentId = resolvedKiosk.ItemType,
                        CurrencyContentId = resolvedKiosk.CurrencySymbol
                    });
                }
            }

            await UniTask.Yield();
        }

        [ContextMenu("Refresh Kiosk Panel")]
        private void OnInventoryUpdate()
        {
            RefreshKioskPanel().Forget();
        }

        private async UniTask RefreshKioskPanel()
        {
            Debug.LogWarning($"Refreshing Kiosk Panel due to inventory update");
            refreshingPanel.SetActive(true);
            var currency = BeamInventoryManager.Instance.InGameCurrency;
            if (currency != null)
            {
                playerCurrencyText.text = currency.Amount.ToString();
                playerCurrencyIcon.sprite = currency.Icon;
            }
            else
            {
                playerCurrencyText.text = "0";
                playerCurrencyIcon.sprite = null;
            }

            await LoadPlayerInventory();
            if (_isOwnListingsTab) await GetKioskListings(true);
            if (_currentKiosk != null) await GetKioskListings(false, _currentKiosk.ContentId);
            
            refreshingPanel.SetActive(false);

            await UniTask.Yield();
        }

        public async void OnBackClicked()
        {
            if (!BeamInventoryManager.Instance.HasMeleeAndRangedWeapons(out var hasMelee, out var hasRanged))
            {
                if (!hasMelee)
                    await BeamInventoryManager.Instance.AddWeaponToInventory(
                        BeamInventoryManager.Instance.SelectedMeleeWeapon);
                if (!hasRanged)
                    await BeamInventoryManager.Instance.AddWeaponToInventory(
                        BeamInventoryManager.Instance.SelectedRangedWeapon);
            }

            await UniTask.Yield();
            gameObject.SetActive(false);
        }

        private async UniTask GetKioskListings(bool ownListings, string kioskId = "")
        {
            if(_client == null) return;
            _currentListings.Clear();
            foreach (Transform child in kioskScrollView.content)
            {
                Destroy(child.gameObject);
            }
            noListingsText.SetActive(true);
            
            kioskButton.SetInteractable(false);
            var buttonText = ownListings ? "Remove Listing" : "Purchase Listing";
            kioskButton.SwitchText(false, buttonText);
            
            List<KioskListing> listings;
            if (ownListings)
            {
                var listingsResponse = await _client.OwnedListings(kioskId);
                if(listingsResponse == null || listingsResponse.listings.Count < 1) return;
                listings = listingsResponse.listings;
            }
            else
            {
                var listingsResponse = await _client.AllListings("", 1, 50);
                if(listingsResponse == null || listingsResponse.listings.Count < 1) return;
                listings = listingsResponse.listings;
            }
            
            noListingsText.gameObject.SetActive(false);
            
            foreach (var listing in listings)
            {
                if(string.IsNullOrEmpty(listing.itemProxyId)) continue;
                if(listing.status == "Sold") continue;
                var level = 1;
                var listingCard = Instantiate(kioskListingCardPrefab, kioskScrollView.content);
                var weaponContent = BeamContentManager.Instance.GetWeaponByContentId(listing.itemContentId);
                foreach (var property in listing.properties)
                {
                    if (property.name != "Level") continue;
                    level = int.Parse(property.value);
                    break;
                }
                listingCard.Init(listing, weaponContent, level, this);
                if(!ownListings) listingCard.DisableIfOwned();
                _currentListings.Add(listingCard);
            }
        }
        
        private async void OnRemoveOwnListing()
        {
            try
            {
                await _client.DelistFromSale(_selectedListingCard.CurrentListing.listingId);
                kioskButton.SetInteractable(false);
                Destroy(_selectedListingCard.gameObject);
                _selectedListingCard = null;
            }
            catch (Exception e)
            {
                Debug.Log($"Failed to delist item: {e.Message}");
            }
        }
        
        private async void OnPurchaseListing()
        {
            try
            {
                await _client.KioskPurchase(_selectedListingCard.CurrentListing.listingId);
                kioskButton.SetInteractable(false);
                Destroy(_selectedListingCard.gameObject);
                _selectedListingCard = null;
            }
            catch (Exception e)
            {
                Debug.Log($"Failed to purchase item: {e.Message}");
            }
        }
        
        
        #endregion


    }
}