using MoeBeam.Game.Scripts.Data;
using MoeBeam.Game.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MoeBeam.Game.Scripts.Items
{
    public class WeaponCard : MonoBehaviour
    {
        #region EXPOSED_VARIABLES

        [FormerlySerializedAs("selectedBG")]
        [Header("References")]
        [SerializeField] private GameObject selectedBg;
        [SerializeField] private Image weaponImage;
        [SerializeField] private Button weaponButton;
        
        [Header("Weapon Details")]
        [SerializeField] private TextMeshProUGUI weaponName;
        [SerializeField] private TextMeshProUGUI weaponDescription;
        [SerializeField] private TextMeshProUGUI weaponDamage;
        [SerializeField] private TextMeshProUGUI weaponAttackSpeed;
        [SerializeField] private TextMeshProUGUI weaponAttackType;
        [SerializeField] private TextMeshProUGUI weaponLevel;
        

        #endregion

        #region PRIVATE_VARIABLES

        private bool _canClick = true;
        private UiMainMenuWeaponChooser _weaponChooser;

        #endregion

        #region PUBLIC_VARIABLES
        
        public WeaponInstance CurrentWeapon { get; private set; }
        public bool IsMelee { get; private set; }
        public bool IsSelected { get; private set; }

        #endregion

        #region UNITY_CALLS

        private void OnEnable()
        {
            selectedBg.SetActive(false);
        }

        #endregion

        #region PUBLIC_METHODS
        
        public void SetWeaponCard(WeaponInstance weapon, UiMainMenuWeaponChooser weaponChooser, bool canClick = true)
        {
            _canClick = canClick;
            _weaponChooser = weaponChooser;
            CurrentWeapon = weapon;
            weaponImage.sprite = weapon.Icon;
            weaponName.text = weapon.DisplayName;
            //weaponDescription.text = weapon.Description;
            weaponDamage.text = weapon.MetaData.CurrentDamage.ToString();
            weaponAttackSpeed.text = weapon.MetaData.CurrentAttackSpeed.ToString();
            weaponAttackType.text = weapon.AttackType.ToString();
            weaponLevel.text = weapon.MetaData.Level.ToString();
            IsMelee = weapon.AttackType != GameData.AttackType.Shoot;
        }

        #endregion

        #region PRIVATE_METHODS
        
        public void SelectCard(bool status)
        {
            selectedBg.SetActive(status);
            IsSelected = status;
        }

        #endregion

        public void OnWeaponClicked()
        {
            if (!_canClick) return;
            if (IsSelected) return;
            if(IsMelee)
            {
                _weaponChooser.SetSelectedMelee(this);
            }
            else
            {
                _weaponChooser.SetSelectedRanged(this);
            }
            SelectCard(true);
        }
    }
}