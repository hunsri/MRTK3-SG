using UnityEngine;
using VRSketchingGeometry;
using VRSketchingGeometry.Commands;
using VRSketchingGeometry.SketchObjectManagement;

namespace MRTK3SketchingGeometry
{
    public class EmptyLineTool : AbstractLineTool
    {
        public override void DrawLinePointAt(Vector3 position)
        {
            //do nothing
        }

        public override void Initialize(DefaultReferences defaults)
        {
            //nothing needs to be initialized here
        }

        public override void InstantiateLine(CommandInvoker commandInvoker, SketchWorld sketchWorld)
        {
            //and nothing needs to be instantiated
        }
    }
}