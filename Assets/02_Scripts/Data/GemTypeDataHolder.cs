using GemTrader.Environment;
using UnityEngine;

namespace GemTrader.Data
{
    [CreateAssetMenu(fileName = "GemTypeDataHolder", menuName = "Data/GemTypeDataHolder")]
    public class GemTypeDataHolder : ScriptableObject
    { 
        public BaseGem[] gems;
    }
}
