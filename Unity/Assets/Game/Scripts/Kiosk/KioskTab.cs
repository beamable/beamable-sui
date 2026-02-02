using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Scripts.Kiosk
{
    public class KioskTab : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        #region EXPOSED_VARIABLES
        
        [SerializeField] private TextMeshProUGUI tabNameText;

        #endregion

        private readonly int _normalizedHash = Animator.StringToHash("Normal");
        private readonly int _selectedHash = Animator.StringToHash("Selected");
        private readonly int _hoverHash = Animator.StringToHash("Hover");
        private bool _selected;
        private KioskManager _kioskManager;
        private Kiosk _currentKiosk = null;
        private Animator _animator;
        private Button _tabButton;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _tabButton = GetComponent<Button>();
        }
        
        public void Init(string tabName, Kiosk kiosk, KioskManager kioskManager)
        {
            _kioskManager = kioskManager;
            tabNameText.text = tabName;
            _currentKiosk = kiosk;
        }
        
        public void Deselect()
        {
            _animator.Play(_normalizedHash);
            _selected = false;
        }
        
        public void Select()
        {
            _selected = true;
            _animator.Play(_selectedHash);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_selected) return;
            _animator.Play(_hoverHash);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_selected) return;
            _animator.Play(_normalizedHash);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var ownTab = _currentKiosk == null;
            _kioskManager.OnTabSelected(this, _currentKiosk, ownTab);
            Select();
        }
    }
}