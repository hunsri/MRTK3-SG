using UnityEngine;
using VRSketchingGeometry;
using VRSketchingGeometry.Commands;
using VRSketchingGeometry.SketchObjectManagement;

namespace MRTK3SketchingGeometry
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] private bool lineCreationActive = true;
    
        [SerializeField] private DefaultReferences defaults;
        [SerializeField] private AbstractLineTool leftHandLineTool;
        [SerializeField] private AbstractLineTool rightHandLineTool;
        
        //This invoker is for tracking all lines, individual segments won't be tracked
        //It also solely tracks all actions affected by undo and redo commands
        private static readonly CommandInvoker GlobalInvoker = new CommandInvoker();
        
        //Create a SketchWorld, many commands require a SketchWorld to be present
        private static SketchWorld _sketchWorld;
        
        //private static SketchObjectGroup _sketchObjectGroup;
        
        private void Awake()
        {
            GetComponent<PoseInput>().OnRightStartedPinching += HandleStartDrawingRightRequest;
            GetComponent<PoseInput>().OnRightHandPinched += HandleDrawRightRequest;
            GetComponent<PoseInput>().OnRightStoppedPinching += HandleStopDrawingRightRequest;
            
            GetComponent<PoseInput>().OnLeftStartedPinching += HandleStartDrawingLeftRequest;
            GetComponent<PoseInput>().OnLeftHandPinched += HandleDrawLeftRequest;
            GetComponent<PoseInput>().OnLeftStoppedPinching += HandleStopDrawingLeftRequest;

            GetComponent<MenuInput>().OnUndoEvent += HandleUndoRequest;
            GetComponent<MenuInput>().OnRedoEvent += HandleRedoRequest;
        }

        private void Start()
        {
            //Create a SketchWorld, many commands require a SketchWorld to be present
            _sketchWorld = Instantiate(defaults.SketchWorldPrefab).GetComponent<SketchWorld>();

            //_sketchObjectGroup = _sketchWorld.GetComponent<SketchObjectGroup>();

            leftHandLineTool.Initialize(defaults);
            rightHandLineTool.Initialize(defaults);
        }
        
        private void HandleStartDrawingRightRequest()
        {
            if(lineCreationActive)
                rightHandLineTool.InstantiateLine(GlobalInvoker, _sketchWorld);
        }
        
        private void HandleStartDrawingLeftRequest()
        {
            if(lineCreationActive)
                leftHandLineTool.InstantiateLine(GlobalInvoker, _sketchWorld);
        }

        private void HandleStopDrawingRightRequest()
        {
            //this method can be used for cleanup in the future, such as mesh optimization of the drawn line
            //it can be also used to signal that line creation has stopped
            
            rightHandLineTool.FinalizeLine();
        }
        
        private void HandleStopDrawingLeftRequest()
        {
            //this method can be used for cleanup in the future, such as mesh optimization of the drawn line
            //it can be also used to signal that line creation has stopped
            
            leftHandLineTool.FinalizeLine();
        }
        
        private void HandleDrawRightRequest(Vector3 position)
        {
            if(lineCreationActive)
                rightHandLineTool.DrawLinePointAt(position);
        }
        
        private void HandleDrawLeftRequest(Vector3 position)
        {
            if(lineCreationActive)
                leftHandLineTool.DrawLinePointAt(position);
        }

        private void HandleUndoRequest()
        {
            GlobalInvoker.Undo();
        }

        private void HandleRedoRequest()
        {
            GlobalInvoker.Redo();
        }
    }
}