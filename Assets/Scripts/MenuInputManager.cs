using System;
using UnityEngine;

public class MenuInputManager : MonoBehaviour
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
