using UnityEngine;
using VRSketchingGeometry;
using VRSketchingGeometry.Commands;
using VRSketchingGeometry.Commands.Line;
using VRSketchingGeometry.Serialization;
using VRSketchingGeometry.SketchObjectManagement;

namespace MRTK3SketchingGeometry
{
    public class MaterialLineTool : AbstractLineTool
    {
        [SerializeField] private float brushScale = 0.01f;

        private LineBrush _customBrush;

        [SerializeField] private Material customMaterial;
        
        public override void Initialize(DefaultReferences defaults)
        {
            base.Initialize(defaults);

            //setting a default scale of one, size changes can be done later by changing the diameter
            _customBrush = CreateLineBrush(8, 1, 2);
        }
        
        public override void InstantiateLine(CommandInvoker commandInvoker, SketchWorld sketchWorld)
        {
            base.InstantiateLine(commandInvoker, sketchWorld);
            
            //changing the appearance of the line
            CurrentLineSketchObject.SetBrush(_customBrush);
            CurrentLineSketchObject.SetLineDiameter(brushScale);
            ChangeLineMaterialTo(customMaterial, CurrentLineSketchObject);
        }

        public override void DrawLinePointAt(Vector3 position)
        {
            CurrentLocalLineInvoker.ExecuteCommand(new AddControlPointContinuousCommand(CurrentLineSketchObject, position));
        }
    }
}