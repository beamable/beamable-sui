using System;
using System.Collections.Generic;
using Beamable.Api.Inventory;
using Beamable.Common.Api;
using Beamable.Common.Api.Inventory;
using Beamable.Common.Content;
using Beamable.Server.Clients;
using Cysharp.Threading.Tasks;
using Game.Scripts.Creature;
using MoeBeam.Game.Scripts.Beam;
using MoeBeam.Game.Scripts.Data;
using MoeBeam.Game.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class UiCreaturePanel : MonoBehaviour
    {
        #region EXPOSED_VARIABLES
        
        [Header("Creature Panel")]
        [SerializeField] private CreatureCard creatureCardPrefab;
        [SerializeField] private Transform biomesContainer;
        [SerializeField] private Transform dnaContainer;
        [SerializeField] private Button cardSelectButton;
        
        [Header("Main Creature")]
        [SerializeField] private TextMeshProUGUI creatureNameText;
        [SerializeField] private TextMeshProUGUI creatureDescriptionText;
        [SerializeField] private Image creatureImage;

        #endregion

        #region PRIVATE_VARIABLES
        
        private UiMainMenuManager _mainMenuManager;
        private CreatureInstance _currentCreature = null;
        private BiomeInstance _selectedBiome = null;
        private DnaInstance _selectedDna = null;
        
        private List<CreatureCard> _allBiomeCards = new List<CreatureCard>();
        private List<CreatureCard> _allDnaCards = new List<CreatureCard>();

        #endregion

        #region PUBLIC_VARIABLES

        #endregion

        #region UNITY_CALLS

        #endregion

        #region PUBLIC_METHODS
        
        public async UniTask InitializeCreaturePanel(UiMainMenuManager mainMenuManager)
        {
            _mainMenuManager = mainMenuManager;
            // Clear previous content
            foreach (Transform child in biomesContainer)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in dnaContainer)
            {
                Destroy(child.gameObject);
            }

            _allBiomeCards.Clear();
            _allDnaCards.Clear();
            
            // Create and initialize creature card
            _currentCreature = BeamContentManager.Instance.GetCurrentCreature();
            
            //Set main creature details
            creatureNameText.text = _currentCreature.CreatureName;
            //creatureDescriptionText.text = _currentCreature.Description;
            creatureImage.sprite = _currentCreature.CreatureImage;

            try
            {
                if(_currentCreature.InstanceId == 0)
                    await BeamManager.BeamContext.Api.InventoryService.AddItem(_currentCreature.ContentId);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to add creature to inventory: {e}");
            }
            
            //Populate biomes and DNA
            foreach (var biome in BeamContentManager.Instance.BiomeContents)
            {
                var biomeCard = Instantiate(creatureCardPrefab, biomesContainer);
                biomeCard.InitializeWithBiome(biome, this);
                _allBiomeCards.Add(biomeCard);
            }
            
            foreach (var dna in BeamContentManager.Instance.DnaContents)
            {
                var dnaCard = Instantiate(creatureCardPrefab, dnaContainer);
                dnaCard.InitializeWithDna(dna, this);
                _allDnaCards.Add(dnaCard);
            }
            
            cardSelectButton.interactable = false;
        }
        
        public void OnBiomeSelected(BiomeInstance biome)
        {
            if (biome == null) return;

            DeselectBiomeCards();
            
            _selectedBiome = biome;
            _currentCreature.CurrentBiome = _selectedBiome;
            
            EnableCardSelectButton();
        }

        public void OnDnaSelected(DnaInstance dna)
        {
            if (dna == null) return;

            DeselectDnaCards();
            
            _selectedDna = dna;
            _currentCreature.CurrentDna = _selectedDna;
            
            EnableCardSelectButton();
        }

        public async void OnCreatureSelected()
        {
            try
            {
                var updatedBuilder = new InventoryUpdateBuilder();

                var attachProperties = new Dictionary<string, string>
                {
                    {"$attach", _currentCreature.CurrentBiome.ContentId},
                };
                updatedBuilder.UpdateItem(_currentCreature.ContentId, _currentCreature.InstanceId, attachProperties);
                await BeamManager.BeamContext.Api.InventoryService.Update(updatedBuilder);
                attachProperties = new Dictionary<string, string>
                {
                    {"$attach", _currentCreature.CurrentDna.ContentId}
                };
                updatedBuilder.UpdateItem(_currentCreature.ContentId, _currentCreature.InstanceId, attachProperties);
                await BeamManager.BeamContext.Api.InventoryService.Update(updatedBuilder);
                await _mainMenuManager.SetFinalId();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to update creature in inventory: {e}");
            }
        }

        #endregion
        
        #region PRIVATE_METHODS

        private void DeselectBiomeCards()
        {
            foreach (var card in _allBiomeCards)
            {
                card.Deselect();
            }
        }
        
        private void DeselectDnaCards()
        {
            foreach (var card in _allDnaCards)
            {
                card.Deselect();
            }
        }
        
        private void EnableCardSelectButton()
        {
            if(_selectedBiome != null && _selectedDna != null)
            {
                cardSelectButton.interactable = true;
            }
        }
        
        #endregion

        
    }
}