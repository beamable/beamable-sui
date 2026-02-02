using Beamable.Common.Content;
using UnityEngine;

namespace Game.Scripts.Creature
{
    [System.Serializable]
    public class CreatureInstance
    {
        public long InstanceId;
        public string ContentId;
        public string CreatureName;
        public string Description;
        public Sprite CreatureImage;
        public BiomeInstance CurrentBiome;
        public DnaInstance CurrentDna;
        public SerializableDictionaryStringToString CustomProperties;
    }
    
    [System.Serializable]
    public class BiomeInstance
    {
        public long InstanceId;
        public string ContentId;
        public string BiomeName;
        public string Description;
        public Sprite BiomeImage;
        public SerializableDictionaryStringToString CustomProperties;

        public int Level = 1;
        //TODO: Add ability
    }
    
    [System.Serializable]
    public class DnaInstance
    {
        public long InstanceId;
        public string ContentId;
        public string DnaName;
        public string Description;
        public Sprite DnaImage;
        public SerializableDictionaryStringToString CustomProperties;

        public int Level = 1;
        //TODO: Add ability
    }
}