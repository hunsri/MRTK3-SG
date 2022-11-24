using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SpatialManipulation;
using Unity.VisualScripting;
using UnityEngine;
using VRSketchingGeometry;
using VRSketchingGeometry.Commands;
using VRSketchingGeometry.Commands.Group;
using VRSketchingGeometry.Commands.Line;
using VRSketchingGeometry.Commands.Selection;
using VRSketchingGeometry.Serialization;
using VRSketchingGeometry.SketchObjectManagement;

namespace MRTK3SketchingGeometry
{
    /// <summary>
    /// Based on MaterialLineTool
    /// </summary>
    public class DebugLineTool : AbstractLineTool
    {
        [SerializeField] private float brushScale = 0.01f;

        private LineBrush _customBrush;

        [SerializeField] private Material customMaterial;

        private static SketchObjectGroup _sketchObjectGroup;

        private static SketchObjectSelection _sketchObjectSelection;

        private static bool _isPositionSet = false;

        private static bool _isSelectionPositionSet = false;
        
        public override void Initialize(DefaultReferences defaults)
        {
            base.Initialize(defaults);

            //setting a default scale of one, size changes can be done later by changing the diameter
            _customBrush = CreateLineBrush(8, 1, 2);
            
            if (_sketchObjectGroup == null)
            {
                //Vector3 vec3 = new Vector3(0, 2, 0);
                //Quaternion q = Quaternion.identity;
                
                _sketchObjectGroup = Instantiate(defaults.SketchObjectGroupPrefab).GetComponent<SketchObjectGroup>();
            }

            if (_sketchObjectSelection == null)
            {
                _sketchObjectSelection = Instantiate(defaults.SketchObjectSelectionPrefab).GetComponent<SketchObjectSelection>();
            }
        }
        
        public override void InstantiateLine(CommandInvoker commandInvoker, SketchWorld sketchWorld)
        {
            base.InstantiateLine(commandInvoker, sketchWorld);
            _isPositionSet = false;
            
            CurrentLocalLineInvoker.ExecuteCommand(new AddToGroupCommand(_sketchObjectGroup, CurrentLineSketchObject));
            CurrentLocalLineInvoker.ExecuteCommand(new AddObjectToSketchWorldRootCommand(_sketchObjectGroup, sketchWorld));
            
            //CurrentLocalLineInvoker.ExecuteCommand(new AddToSelectionAndHighlightCommand(_sketchObjectSelection, _sketchObjectGroup));
            //CurrentLocalLineInvoker.ExecuteCommand(new AddToSelectionAndHighlightCommand(_sketchObjectSelection, CurrentLineSketchObject));

            //changing the appearance of the line
            CurrentLineSketchObject.SetBrush(_customBrush);
            CurrentLineSketchObject.SetLineDiameter(brushScale);
            ChangeLineMaterialTo(customMaterial, CurrentLineSketchObject);
        }

        public override void DrawLinePointAt(Vector3 position)
        {
            
            if (!_isPositionSet)
            {
                CurrentLineSketchObject.transform.position = position;
                //_sketchObjectGroup.transform.position = position;
                //_sketchObjectSelection.transform.position = position;
                _isPositionSet = true;
            }

            if (!_isSelectionPositionSet)
            {
                _sketchObjectSelection.transform.position = position;
                _isSelectionPositionSet = true;
            }
            

            CurrentLocalLineInvoker.ExecuteCommand(new AddControlPointContinuousCommand(CurrentLineSketchObject, position));
            
            //CurrentLineSketchObject.GetComponent<MeshFilter>().mesh.RecalculateBounds();
            //CurrentLineSketchObject.GetComponent<MeshRenderer>().bounds =
            //    CurrentLineSketchObject.GetComponent<MeshFilter>().mesh.bounds;
            
            //CurrentLocalLineInvoker.ExecuteCommand(new AddToSelectionAndHighlightCommand(_sketchObjectSelection, CurrentLineSketchObject));
            //CurrentLocalLineInvoker.ExecuteCommand(new AddToSelectionAndHighlightCommand(_sketchObjectSelection, _sketchObjectGroup));
            
            //CurrentLocalLineInvoker.ExecuteCommand(new ActivateSelectionCommand(_sketchObjectSelection));
        }

        public override void FinalizeLine()
        {
            CurrentLocalLineInvoker.ExecuteCommand(new AddToSelectionAndHighlightCommand(_sketchObjectSelection, CurrentLineSketchObject));
            
            CurrentLocalLineInvoker.ExecuteCommand(new ActivateSelectionCommand(_sketchObjectSelection));

            BoundsControl bc = _sketchObjectSelection.GetComponent<BoundsControl>();
            if (bc != null)
            {
                bc.RecomputeBounds();
            }
        }
    }
}