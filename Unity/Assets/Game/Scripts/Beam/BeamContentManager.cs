using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Game.Scripts.Creature;
using Game.Scripts.Helpers;
using MoeBeam.Game.Scripts.Data;
using MoeBeam.Game.Scripts.Sui;
using SuiFederationCommon.FederationContent;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MoeBeam.Game.Scripts.Beam
{
    public class BeamContentManager : BeamManagerBase<BeamContentManager>
    {
        #region EXPOSED_VARIABLES
        
        [SerializeField] List<WeaponItemRef> weaponsRefs;
        [SerializeField] List<CreatureItemRef> creatureRefs;
        [SerializeField] List<CreatureBiomeRef> biomeRefs;
        [SerializeField] List<CreatureDnaRef> dnaRefs;

        #endregion

        #region PRIVATE_VARIABLES
        
        private bool _isInitialized = false;

        #endregion

        #region PUBLIC_VARIABLES
        
        public List<WeaponInstance> WeaponContents { get; private set; }
        public List<CreatureInstance> CreatureContents { get; private set; }
        public List<BiomeInstance> BiomeContents { get; private set; }
        public List<DnaInstance> DnaContents { get; private set; }

        #endregion

        #region UNITY_CALLS

        public override async UniTask InitAsync(CancellationToken ct)
        {
            if(_isInitialized) return;
            await UniTask.WaitUntil(() => BeamAccountManager.Instance.IsReady, cancellationToken: ct);
            await ResolveWeaponContents();
            await ResolveCreatures();
            await ResolveBiomes();
            await ResolveDna();
            _isInitialized = true;
            Debug.Log("BeamContentManager initialized.");
        }

        public override async UniTask ResetAsync(CancellationToken ct)
        {
            //reset instance IDs and levels for all weapon, creature, biome, and dna instances
            foreach (var weapon in WeaponContents)
            {
                weapon.InstanceId = 0;
                weapon.MetaData.Level = 1;
            }
            
            foreach (var creature in CreatureContents)
            {
                creature.InstanceId = 0;
                if(creature.CurrentBiome != null)
                    creature.CurrentBiome.Level = 1;
                if(creature.CurrentDna != null)
                    creature.CurrentDna.Level = 1;
            }

            foreach (var biome in BiomeContents)
            {
                biome.InstanceId = 0;
                biome.Level = 1;
            }

            foreach (var dna in DnaContents)
            {
                dna.InstanceId = 0;
                dna.Level = 1;
            }

            await UniTask.Yield();
        }

        #endregion
        
        #region PUBLIC_METHODS

        #endregion

        #region Resolve Methods

        private async UniTask ResolveWeaponContents()
        {
            WeaponContents = new List<WeaponInstance>();
            foreach (var weapon in weaponsRefs)
            {
                var resolvedW = await weapon.Resolve();
                var icon = await Utilities.GetSpriteAsync(resolvedW.icon);
                Sprite bulletIcon = null;
                
                resolvedW.CustomProperties.TryGetValue(GameData.DamageKey, out var damageValue);
                int.TryParse(damageValue, out var damage);
                resolvedW.CustomProperties.TryGetValue(GameData.AttackSpeedKey, out var attackSpeedValue);
                float.TryParse(attackSpeedValue, out var attackSpeed);
                resolvedW.CustomProperties.TryGetValue(GameData.AttackTypeKey, out var attackTypeValue);
                int.TryParse(attackTypeValue, out var attackType);
                var type = GameData.ToAttackType(attackType);
                
                var metaData = new WeaponMetaData(0, 1, damage, attackSpeed);
                var weaponInstance = new WeaponInstance(icon, bulletIcon, 0, resolvedW.Id, resolvedW.name, 
                   resolvedW.Description, type, metaData);
                WeaponContents.Add(weaponInstance);
            }
        }
        
        private async UniTask ResolveCreatures()
        {
            CreatureContents = new List<CreatureInstance>();

            foreach (var creatureRef in creatureRefs)
            {
                var resolvedCreature = await creatureRef.Resolve();
                //var icon = await GetSpriteAsync(resolvedCreature.icon);
                var icon = await Utilities.DownloadImageFromUrl(resolvedCreature.Image);

                var creatureInstance = new CreatureInstance
                {
                    ContentId = resolvedCreature.Id,
                    CreatureName = resolvedCreature.Name,
                    Description = resolvedCreature.Description,
                    CreatureImage = icon,
                    CustomProperties = resolvedCreature.CustomProperties
                };
                
                CreatureContents.Add(creatureInstance);
            }
        }

        private async UniTask ResolveBiomes()
        {
            BiomeContents = new List<BiomeInstance>();
            foreach (var biomeRef in biomeRefs)
            {
                var resolvedBiome = await biomeRef.Resolve();
                //var icon = await GetSpriteAsync(resolvedBiome.icon);
                var icon = await Utilities.DownloadImageFromUrl(resolvedBiome.Image);
                
                var biomeInstance = new BiomeInstance
                {
                    ContentId = resolvedBiome.Id,
                    BiomeName = resolvedBiome.Name,
                    Description = resolvedBiome.Description,
                    BiomeImage = icon,
                    CustomProperties = resolvedBiome.CustomProperties
                };
                
                BiomeContents.Add(biomeInstance);
            }
        }

        private async UniTask ResolveDna()
        {
            DnaContents = new List<DnaInstance>();
            foreach (var dnaRef in dnaRefs)
            {
                var resolvedDna = await dnaRef.Resolve();
                //var icon = await GetSpriteAsync(resolvedDna.icon);
                var icon = await Utilities.DownloadImageFromUrl(resolvedDna.Image);

                var dnaInstance = new DnaInstance
                {
                    ContentId = resolvedDna.Id,
                    DnaName = resolvedDna.Name,
                    Description = resolvedDna.Description,
                    DnaImage = icon,
                    CustomProperties = resolvedDna.CustomProperties
                };
                
                DnaContents.Add(dnaInstance);
            }
        }

        #endregion

        public CreatureInstance GetCurrentCreature()
        {
            //return the 1st creature in the list for now since there is ONE creature
            if (CreatureContents.Count > 0)
            {
                return CreatureContents[0];
            }
            
            Debug.LogWarning("No creatures available.");
            return null;
        }

        public void LevelUpCreatureBiomeAndDna(int biomeLevel, int dnaLevel)
        {
            var creature = GetCurrentCreature();
            if (creature == null)
            {
                Debug.LogError("Creature instance is null.");
                return;
            }
            
            creature.CurrentBiome.Level = biomeLevel;
            creature.CurrentDna.Level = dnaLevel;
        }
        
        public Sprite GetWeaponIconByContentId(string contentId)
        {
            foreach (var weapon in WeaponContents)
            {
                if (weapon.ContentId == contentId)
                    return weapon.Icon;
            }

            Debug.LogWarning($"No weapon found with ContentId: {contentId}");
            return null;
        }

        public WeaponInstance GetWeaponByContentId(string listingItemContentId)
        {
            foreach (var weapon in WeaponContents)
            {
                if (weapon.ContentId == listingItemContentId)
                    return weapon;
            }

            Debug.LogWarning($"No weapon found with ContentId: {listingItemContentId}");
            return null;
        }
    }
}