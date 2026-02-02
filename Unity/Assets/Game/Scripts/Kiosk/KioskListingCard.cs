using MoeBeam.Game.Scripts.Beam;
using SuiFederationCommon.Models.Kiosk;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Kiosk
{
    public class KioskListingCard : MonoBehaviour
    {
        #region EXPOSED_VARIABLES
        
        [SerializeField] private Image itemIcon;
        [SerializeField] private Image currencyIcon;
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemLevelText;
        [SerializeField] private TextMeshProUGUI itemPriceText;

        #endregion

        #region PRIVATE_VARIABLES

        private readonly int _normalizedHash = Animator.StringToHash("Normal");
        private Animator _animator;
        private KioskManager _kioskManager;

        #endregion

        #region PUBLIC_VARIABLES
        
        public KioskListing CurrentListing { get; private set; }

        #endregion

        #region UNITY_CALLS

        #endregion

        #region PUBLIC_METHODS
        
        public void Init(KioskListing listing, WeaponInstance weapon, int itemLevel, KioskManager kioskManager)
        {
             _animator = GetComponent<Animator>();
            CurrentListing = listing;
            itemIcon.sprite = weapon.Icon;
            itemNameText.text = weapon.DisplayName;
            itemLevelText.text = itemLevel.ToString();
            currencyIcon.sprite = BeamInventoryManager.Instance.InGameCurrency.Icon;
            itemPriceText.text = listing.price.ToString();
            _kioskManager = kioskManager;
        }
        
        public void DisableIfOwned()
        {
            if(CurrentListing.gamerTag != BeamAccountManager.Instance.PlayerId) return;
            
            var button = GetComponent<Button>();
            if (button != null)
            {
                button.interactable = false;
            }
            itemPriceText.text = "Owned";
        }
        
        public void Deselect()
        {
            _animator.Play(_normalizedHash);
        }

        public void OnSelected()
        {
            _kioskManager.OnCurrentOwnListingSelected(this);
        }

        #endregion

        #region PRIVATE_METHODS

        #endregion

        
    }
}