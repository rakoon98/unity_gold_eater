using UnityEngine;

namespace GoldEater
{
    [CreateAssetMenu(fileName = "HitFlashData", menuName = "GoldEaterSO/Hit Flash")]
    public class HitFlashData : ScriptableObject
    {
        public Color hitColor = Color.red;
        public float flashDuration = 0.1f;
        public int flashCount = 2;
    }

}