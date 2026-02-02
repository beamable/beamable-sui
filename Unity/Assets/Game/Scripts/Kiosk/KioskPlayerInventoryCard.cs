using System;
using MoeBeam.Game.Scripts.Beam;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Kiosk
{
    public class KioskPlayerInventoryCard : MonoBehaviour
    {
        #region EXPOSED_VARIABLES
        
        [SerializeField] private Image itemIcon;
        [SerializeField] private Image currencyIcon;
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemLevelText;
        [SerializeField] private TMP_InputField itemPriceInputField;
        [SerializeField] private Button listItemButton;

        #endregion

        #region PRIVATE_VARIABLES

        private string _kioskContentId;
        private WeaponInstance _currentWeapon;
        private KioskManager _kioskManager;

        #endregion

        private void Update()
        {
            listItemButton.interactable = int.TryParse(itemPriceInputField.text, out var price) && price > 0;
        }

        #region PUBLIC_VARIABLES

        #endregion

        public void Init(WeaponInstance weapon, string kioskContentId, KioskManager kioskManager)
        {
            _kioskManager = kioskManager;
            _kioskContentId = kioskContentId;
            currencyIcon.sprite = BeamInventoryManager.Instance.InGameCurrency.Icon;
            _currentWeapon = weapon;
            itemIcon.sprite = weapon.Icon;
            itemNameText.text = _currentWeapon.DisplayName;
            itemLevelText.text = _currentWeapon.MetaData.Level.ToString();
            listItemButton.onClick.RemoveAllListeners();
            listItemButton.onClick.AddListener(() => OnListItem(weapon));
        }

        
        private async void OnListItem(WeaponInstance weapon)
        {
            try
            {
                await BeamManager.SuiClient.ListForSale(weapon.InstanceId, long.Parse(itemPriceInputField.text), _kioskContentId);
                Destroy(gameObject);
            }
            catch (Exception e)
            {
                //TODO: Display error to user
                Debug.LogError("Error listing item for sale: " + e.Message);
            }
        }


    }
}