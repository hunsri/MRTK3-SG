using System;
using UnityEngine;

namespace MRTK3SketchingGeometry
{
    public class MenuInput : MonoBehaviour
    {
        public event Action OnUndoEvent = delegate {  };
        public event Action OnRedoEvent = delegate {  };
    
        public void OnUndoButtonPressed()
        {
            OnUndoEvent();
        }
    
        public void OnRedoButtonPressed()
        {
            OnRedoEvent();
        }
    }

}