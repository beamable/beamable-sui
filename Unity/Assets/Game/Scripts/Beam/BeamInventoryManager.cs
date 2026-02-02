using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Beamable;
using Beamable.Common.Api.Inventory;
using Beamable.Common.Docs;
using Beamable.Common.Inventory;
using Beamable.Player;
using Beamable.Server.Clients;
using Cysharp.Threading.Tasks;
using Game.Scripts.Helpers;
using MoeBeam.Game.Scripts.Data;
using MoeBeam.Game.Scripts.Sui;
using SuiFederationCommon.FederationContent;
using Unity.VisualScripting;
using UnityEngine;

namespace MoeBeam.Game.Scripts.Beam
{
    [Serializable]
    public class PlayerCoin
    {
        public string ContentId;
        public int Amount;
        public Sprite Icon;
        public GameData.CoinType CoinType;
        
        public PlayerCoin(string contentId, int amount, Sprite icon, GameData.CoinType coinType)
        {
            ContentId = contentId;
            Amount = amount;
            Icon = icon;
            CoinType = coinType;
        }
    }
    
    public class BeamInventoryManager : BeamManagerBase<BeamInventoryManager>
    {
        public static Action OnInventoryRefreshed;
        
        #region EXPOSED_VARIABLES
        
        [Header("Content IDs")]
        [SerializeField] private string weaponContentId = "items.weapon";
        [SerializeField] private string creatureContentId = "items.creature";
        [SerializeField] private string biomeContentId = "items.creature.biome";
        [SerializeField] private string dnaContentId = "items.creature.dna";
        
        
        [Header("Currency")]
        [SerializeField] private float coinUpdateInterval = 1f;
        [SerializeField] private List<CoinCurrencyRef> coinCurrencyRefs = new List<CoinCurrencyRef>();
        [SerializeField] private CoinCurrencyRef inGameCurrencyRef;

        #endregion

        #region PRIVATE_VARIABLES

        private float _nextCoinUpdate = 0f;
        private BeamContext _beamContext = BeamManager.BeamContext;
        private InventoryUpdateBuilder _inventoryUpdateBuilder;

        #endregion

        #region PUBLIC_VARIABLES
        
        public bool IsReady { get; private set; } = false;
        public WeaponInstance SelectedMeleeWeapon { get; set; }
        public WeaponInstance SelectedRangedWeapon { get; set; }
        public PlayerCoin InGameCurrency { get; private set; }
        public List<PlayerCoin> PlayerCoins { get; private set; }
        public List<WeaponInstance> PlayerWeapons { get; private set; }

        #endregion

        #region UNITY_CALLS

        public override async UniTask InitAsync(CancellationToken ct)
        {
            _beamContext = BeamManager.BeamContext;
            _inventoryUpdateBuilder ??= new InventoryUpdateBuilder();
            await SetupCoins();
            _beamContext.Api.InventoryService.Subscribe(OnRefresh);
            Debug.Log("BeamInventoryManager initialized.");
        }
        
        public override async UniTask ResetAsync(CancellationToken ct)
        {
            await UniTask.Yield();
            IsReady = false;    
        }


        #endregion

        #region PUBLIC_METHODS
        
        public async UniTask AddAnItemToInventory(string contentId, Dictionary<string, string> properties = null)
        {
            try
            {
                await BeamManager.BeamContext.Api.InventoryService.AddItem(contentId, properties);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to add ITEM to INVENTORY: {e}");
            }
        }
        
        public async UniTask AddWeaponToInventory(WeaponInstance weapon)
        {
            try
            {
                var add = BeamManager.BeamContext.Api.InventoryService.AddItem(weapon.ContentId,
                    weapon.MetaData.ToDictionary(true)).IsCompleted;
                Debug.LogWarning($"ADDING ITEM TO INVENTORY {add}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to add ITEM to INVENTORY: {e}");
            }
        }

        public async UniTask UpdateCurrency(GameData.CoinType coinType, bool deduct = false)
        {
            //var inventoryBuilder = new InventoryUpdateBuilder();
            foreach (var coin in PlayerCoins.Where(coin => coin.CoinType == coinType))
            {
                if(deduct && coin.Amount == 0) {return;}
                Debug.Log($"updating Currency for {coin.ContentId} with deduct = {deduct}");
                var amount = deduct ? -1 : +1;
                coin.Amount += amount;
                _inventoryUpdateBuilder.CurrencyChange(coin.ContentId, amount);
                Dictionary<PlayerCoin, bool> updated = new Dictionary<PlayerCoin, bool> {{coin, deduct}};
                EventCenter.InvokeEvent(GameData.OnCoinCollectedEvent, updated);
                break;
            }
            await _beamContext.Inventory.Update(_inventoryUpdateBuilder);
        }
        
        public PlayerCoin GetCoinByType(GameData.CoinType coinType)
        {
            return PlayerCoins.FirstOrDefault(coin => coin.CoinType == coinType);
        }
        
        #endregion

        #region Getters

        public int GetOwnedWeaponsCount()
        {
            return PlayerWeapons.FindAll(w => w.IsOwned).Count;
        }
        
        public WeaponInstance GetItemByInstanceId(long instanceId)
        {
            return PlayerWeapons.Find(w => w.InstanceId == instanceId);
        }

        public WeaponInstance GetOwnedMeleeWeapon()
        {
            try
            {
                return PlayerWeapons.Find(w => (w.AttackType != GameData.AttackType.Shoot && w.IsOwned));
            }
            catch (Exception e)
            {
                return null;
            } 
        }
        
        public WeaponInstance GetOwnedRangedWeapon()
        {
            try
            {
                return PlayerWeapons.Find(w => (w.AttackType == GameData.AttackType.Shoot && w.IsOwned));
            }
            catch (Exception e)
            {
                return null;
            }
        }
        
        //check if PlayerWeapons have at least one melee and one ranged weapon
        public bool HasMeleeAndRangedWeapons(out bool hasMelee, out bool hasRanged)
        {
            hasMelee = false;
            hasRanged = false;
            //Debug.Log($"PlayerWeapons count: {PlayerWeapons.Count}");
            if (PlayerWeapons == null || PlayerWeapons.Count < 1) return false;
            hasMelee = PlayerWeapons.Exists(w => w.AttackType != GameData.AttackType.Shoot && w.IsOwned);
            hasRanged = PlayerWeapons.Exists(w => w.AttackType == GameData.AttackType.Shoot && w.IsOwned);
            return hasMelee && hasRanged;
        }

        #endregion

        #region PRIVATE_METHODS
        
        [ContextMenu("Refresh")]
        private void OnRefreshing()
        {
            RefreshInventory().Forget();
        }
        private void OnRefresh(InventoryView obj)
        {
            RefreshInventory().Forget();
        }

        private async UniTask RefreshInventory()
        {
            var inventoryItems = await _beamContext.Inventory.LoadItems();
            var weapons = inventoryItems.Where(item => item.ContentId.Contains(weaponContentId)).ToArray();
            var creatures = inventoryItems.Where(item => item.ContentId.Contains(creatureContentId)).ToArray();
            var biomes = inventoryItems.Where(item => item.ContentId.Contains(biomeContentId)).ToArray();
            var dna = inventoryItems.Where(item => item.ContentId.Contains(dnaContentId)).ToArray();
            var currencyGroup = await _beamContext.Inventory.LoadCurrencies();
            
            await RefreshWeapons(weapons);
            await RefreshCurrencies(currencyGroup);
            RefreshCreatures(creatures);
            RefreshBiomes(biomes);
            RefreshDna(dna);
            IsReady = true;
            await UniTask.Yield();
            OnInventoryRefreshed?.Invoke();
            Debug.Log($"Inventory Refreshed: {weapons.Length} weapons, {creatures.Length} creatures, {biomes.Length} biomes, {dna.Length} dna, {currencyGroup.ToList().Count} currencies");
        }

        private async UniTask RefreshCurrencies(PlayerCurrencyGroup currencyGroup)
        {
            await currencyGroup.OnReady;
            if (currencyGroup.ToList().Count < 1) return;
            if (InGameCurrency == null) return;
            if (PlayerCoins == null || PlayerCoins.Count < 1) return;
            var found = currencyGroup.ToList().Find(c => c.CurrencyId == InGameCurrency.ContentId);
            InGameCurrency.Amount = (int)found.Amount;
            
            foreach (var coin in PlayerCoins.Where(coin => coin.ContentId != InGameCurrency.ContentId))
            {
                var foundCoin = currencyGroup.ToList().Find(c => c.CurrencyId == coin.ContentId);
                if (foundCoin != null)
                {
                    coin.Amount = (int)foundCoin.Amount;
                }
            }
        }

        private void RefreshCreatures(PlayerItem[] creatures)
        {
            if (creatures.Length == 0) return;
            if(BeamContentManager.Instance.CreatureContents == null || BeamContentManager.Instance.CreatureContents.Count < 1) return;
            foreach (var item in creatures)
            {
                foreach (var creature in BeamContentManager.Instance.CreatureContents.Where(creature =>
                             creature.ContentId == item.ContentId))
                {
                    creature.InstanceId = item.ItemId;
                    creature.CustomProperties = item.Properties;
                    break;
                }
            }
        }

        private void RefreshBiomes(PlayerItem[] biomes)
        {
            if(biomes.Length == 0) return;
            foreach (var item in biomes)
            {
                foreach (var biome in BeamContentManager.Instance.BiomeContents.Where(biome =>
                             biome.ContentId == item.ContentId))
                {
                    biome.InstanceId = item.ItemId;
                    item.Properties.TryGetValue(GameData.CreatureLevelKey, out var levelValue);
                    int.TryParse(levelValue, out var level);
                    biome.Level = level;
                }
            }
        }

        private void RefreshDna(PlayerItem[] dnas)
        {
            if(dnas.Length < 1) return;
            if(BeamContentManager.Instance.DnaContents == null || BeamContentManager.Instance.DnaContents.Count < 1) return;
            foreach (var item in dnas)
            {
                foreach (var dna in BeamContentManager.Instance.DnaContents.Where(dnaInstance =>
                             dnaInstance.ContentId == item.ContentId))
                {
                    dna.InstanceId = item.ItemId;
                    item.Properties.TryGetValue(GameData.CreatureLevelKey, out var levelValue);
                    int.TryParse(levelValue, out var level);
                    dna.Level = level;
                }
            }
        }

        private async UniTask RefreshWeapons(PlayerItem[] weapons)
        {
            if(weapons.Length < 1) return;
            if(BeamContentManager.Instance.WeaponContents == null || BeamContentManager.Instance.WeaponContents.Count < 1) return;
            PlayerWeapons = new List<WeaponInstance>();
            PlayerWeapons.Clear();
            foreach (var item in weapons)
            {
                foreach (var weapon in BeamContentManager.Instance.WeaponContents.Where
                             (weapon => weapon.ContentId == item.ContentId))
                {
                    weapon.InstanceId = item.ItemId;
                    PlayerWeapons.Add(weapon);
                }
            }

            await UniTask.Yield();
        }
        
        private async UniTask SetupCoins()
        {
            PlayerCoins = new List<PlayerCoin>();
            foreach (var currencyRef in coinCurrencyRefs)
            {
                var resolved = await currencyRef.Resolve();
                var icon = await Utilities.GetSpriteAsync(resolved.icon);
                var coin = new PlayerCoin(currencyRef.Id, 0, icon, ParseCoinType(currencyRef.Id));
                PlayerCoins.Add(coin);
            }
            
            //As a start we give the player 10,000 gold for testing purposes
            var resolvedInGame = await inGameCurrencyRef.Resolve();
            var iconInGame = await Utilities.GetSpriteAsync(resolvedInGame.icon);
            InGameCurrency = new PlayerCoin(inGameCurrencyRef.Id, 1000, iconInGame, ParseCoinType(inGameCurrencyRef.Id));
            PlayerCoins.Add(InGameCurrency);
            
            await _beamContext.Api.InventoryService.AddCurrency(inGameCurrencyRef.Id, 1000).Then(e =>
            {
                Debug.Log("Added starting currency to player inventory");
            }).Error(err =>
            {
                Debug.LogError($"Failed to add starting currency to player inventory: {err}");
            });
        }

        private GameData.CoinType ParseCoinType(string contentId)
        {
            if (contentId.Contains("gold")) return GameData.CoinType.Gold;
            if (contentId.Contains("star")) return GameData.CoinType.Star;
            
            return GameData.CoinType.Beam;
        }
        

        #endregion


    }
}