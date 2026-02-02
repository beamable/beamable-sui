using Game.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Scripts.Creature
{
    public class CreatureCard : MonoBehaviour
    {
        #region EXPOSED_VARIABLES
        
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI levelValueText;
        [SerializeField] private Image creatureImage;
        
        [Header("Selection")]
        [SerializeField] private Image selectionImage;

        #endregion

        private UiCreaturePanel _creaturePanel = null;
        private BiomeInstance _currentBiome = null;
        private DnaInstance _currentDna = null;

        public void InitializeWithBiome(BiomeInstance biome, UiCreaturePanel creaturePanel)
        {
            _creaturePanel = creaturePanel;
            nameText.text = biome.BiomeName;
            descriptionText.text = biome.Description;
            creatureImage.sprite = biome.BiomeImage;

            levelValueText.text = biome.Level.ToString();
            _currentBiome = biome;
            selectionImage.enabled = false;
        }
        
        public void InitializeWithDna(DnaInstance dna, UiCreaturePanel creaturePanel)
        {
            _creaturePanel = creaturePanel;
            nameText.text = dna.DnaName;
            descriptionText.text = dna.Description;
            creatureImage.sprite = dna.DnaImage;
            levelValueText.text = dna.Level.ToString();

            _currentDna = dna;
            selectionImage.enabled = false;
        }
        
        //TODO: Add abilities 
        //Biomes: player buffs basically
        //Dna: creature abilities
        
        public void OnCardClicked()
        {
            if (_currentDna != null)
            {
                _creaturePanel.OnDnaSelected(_currentDna);
            }
            else
            {
                _creaturePanel.OnBiomeSelected(_currentBiome);
            }
            
            selectionImage.enabled = true;
        }
        
        public void Deselect()
        {
            selectionImage.enabled = false;
        }

    }
}