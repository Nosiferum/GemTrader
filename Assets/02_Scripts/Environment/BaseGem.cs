using UnityEngine;

namespace GemTrader.Environment
{
    //an abstraction of gem concept to future-proof the extensibility 
    public abstract class BaseGem : MonoBehaviour
    { 
       [field: SerializeField] public string Name { get; private set; }
       [field: SerializeField] public int BasePrice { get; private set; }
       [field: SerializeField] public Texture2D Icon { get; private set; }
       [field: SerializeField] public Mesh Model { get; private set; }

       public int CellCoordinateX { get; set; }
       public int CellCoordinateY { get; set; }
       public float PickedUpScaleValue { private get; set; }
       public bool IsReadyToHarvest { get; set; } = false;
       
       public int GetSellPrice()
       {
           return (int) (BasePrice + PickedUpScaleValue * 100);
       }
    }    
}

