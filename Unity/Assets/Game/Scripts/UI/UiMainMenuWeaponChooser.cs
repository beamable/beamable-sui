using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MoeBeam.Game.Scripts.Beam;
using MoeBeam.Game.Scripts.Data;
using MoeBeam.Game.Scripts.Helpers;
using MoeBeam.Game.Scripts.Items;
using TMPro;
using UnityEngine;
// ReSharper disable All

namespace MoeBeam.Game.Scripts.UI
{
    public class UiMainMenuWeaponChooser : MonoBehaviour
    {
        #region EXPOSED_VARIABLES
        
        [SerializeField] private WeaponCard weaponCardPrefab;
        [SerializeField] private Transform meleeTransform;
        [SerializeField] private Transform rangedTransform;
        [SerializeField] private TextMeshProUGUI descriptionText;
        
        [Header("Buttons")]
        [SerializeField] private BeamButton selectButton;
        [SerializeField] private BeamButton chooseNewWeaponsButton;
        
        #endregion

        #region PRIVATE_VARIABLES

        private bool _ownsMeleeAndRanged;
        private WeaponCard _selectedMeleeCard;
        private WeaponCard _selectedRangedCard;
        private List<WeaponCard> _meleeCards = new List<WeaponCard>();
        private List<WeaponCard> _rangedCards = new List<WeaponCard>();

        #endregion

        #region PUBLIC_VARIABLES

        #endregion

        #region UNITY_CALLS

        private void Start()
        {
            chooseNewWeaponsButton.AddListener(PopulateWeaponContent);
            chooseNewWeaponsButton.gameObject.SetActive(false);
            selectButton.AddListener(OnSelectWeapons);
            selectButton.SetInteractable(false);
            _ownsMeleeAndRanged =
                BeamInventoryManager.Instance.HasMeleeAndRangedWeapons(out var hasMelee, out var hasRanged);
            
            if (_ownsMeleeAndRanged)
            {
                PopulateFromInventory();
            }
            else
            {
                PopulateWeaponContent();
            }
        }

        private void Update()
        {
            if (_selectedMeleeCard == null || _selectedRangedCard == null || 
                selectButton.ButtonCurrent.interactable) return;
            selectButton.SetInteractable(true);
        }

        #endregion

        #region PUBLIC_METHODS

        public void SetSelectedMelee(WeaponCard card)
        {
            _selectedMeleeCard = card;
            DeselectMeleeCards();
        }
        
        public void SetSelectedRanged(WeaponCard card)
        {
            _selectedRangedCard = card;
            DeselectRangedCards();
        }

        #endregion

        #region PRIVATE_METHODS
        
        private void PopulateFromInventory()
        {
            descriptionText.text = "Please choose weapons from your own inventory";
            CleanUpPrefabTransforms();

            foreach (var weapon in BeamInventoryManager.Instance.PlayerWeapons)
            {
                InstantiateWeapon(weapon);
            }
            chooseNewWeaponsButton.gameObject.SetActive(true);
        }
        

        private void PopulateWeaponContent()
        {
            if(chooseNewWeaponsButton.gameObject.activeInHierarchy) 
                chooseNewWeaponsButton.gameObject.SetActive(false);
            descriptionText.text = "Take your weapon of choice from the stash!";
            CleanUpPrefabTransforms();
                
            foreach (var weapon in BeamContentManager.Instance.WeaponContents)
            {
                InstantiateWeapon(weapon);
            }
        }
        
        private void CleanUpPrefabTransforms()
        {
            //Go thru children in rangedTransform and destory them
            foreach (Transform child in rangedTransform)
            {
                Destroy(child.gameObject);
            }

            //Go thru children in meleeTransform and destory them
            foreach (Transform child in meleeTransform)
            {
                Destroy(child.gameObject);
            }
        }

        private void InstantiateWeapon(WeaponInstance weapon)
        {
            var weaponCard = Instantiate(weaponCardPrefab, 
                weapon.AttackType == GameData.AttackType.Shoot ? rangedTransform : meleeTransform);
            weaponCard.SetWeaponCard(weapon, this);
            weaponCard.transform.name = weapon.DisplayName + "_Card";
            weaponCard.gameObject.SetActive(true);
            if(weapon.AttackType == GameData.AttackType.Shoot) 
                _rangedCards.Add(weaponCard);
            else 
                _meleeCards.Add(weaponCard);
        }

        private void DeselectMeleeCards()
        {
            foreach (var card in _meleeCards)
            {
                card.SelectCard(false);
            }
        }

        private void DeselectRangedCards()
        {
            foreach (var card in _rangedCards)
            {
                card.SelectCard(false);
            }
        }

        private async void OnSelectWeapons()
        {
            try
            {
                selectButton.SwitchText(false, "Adding weapons to your inventory...");
                BeamInventoryManager.Instance.SelectedMeleeWeapon = _selectedMeleeCard.CurrentWeapon;
                BeamInventoryManager.Instance.SelectedRangedWeapon = _selectedRangedCard.CurrentWeapon;
                //Add weapons to inventory
                if(!_ownsMeleeAndRanged)
                {
                    await BeamInventoryManager.Instance.AddWeaponToInventory(_selectedMeleeCard.CurrentWeapon);
                    await BeamInventoryManager.Instance.AddWeaponToInventory(_selectedRangedCard.CurrentWeapon);
                }

                await UniTask.Yield();
                UiMainMenuManager.Instance.ShowCreaturePanelAfterWeapons();
                selectButton.ButtonCurrent.interactable = false;
            }
            catch (Exception e)
            {
                selectButton.SwitchText(true);
                selectButton.ButtonCurrent.interactable = true;
                Debug.LogError($"On Select weapons failed: {e.Message}");
            }
        }


        #endregion


    }
}