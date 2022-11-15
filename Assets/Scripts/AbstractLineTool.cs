using UnityEngine;
using VRSketchingGeometry;
using VRSketchingGeometry.Commands;
using VRSketchingGeometry.Meshing;
using VRSketchingGeometry.Serialization;
using VRSketchingGeometry.SketchObjectManagement;

namespace MRTK3SketchingGeometry
{
    public abstract class AbstractLineTool : MonoBehaviour
    {
        /// The LineSketchObject which will form the reference for all brushes
        private static LineSketchObject _brushReference;
        
        protected LineSketchObject CurrentLineSketchObject;
        
        private DefaultReferences _defaults;
        
        /// This invoker is for tracking all segments of the latest drawn line of a tool
        /// Gets cleared when a new line is created
        protected CommandInvoker CurrentLocalLineInvoker = new CommandInvoker();
        
        
        public abstract void DrawLinePointAt(Vector3 position);
        
        /// <summary>
        /// Initializes the tool, necessary since constructor can't be called on objects inheriting from MonoBehaviour. 
        /// Must be called before the line tool can be used.
        /// </summary>
        /// <param name="defaults"></param>
        public virtual void Initialize(DefaultReferences defaults)
        {
            _defaults = defaults;

            if (_brushReference == null)
            {
                //Creating the LineSketchObject that will act as the default reference for new brushes.
                //Deactivation of the reference prevents it from rendering in the scene itself
                _brushReference = Instantiate(_defaults.LineSketchObjectPrefab).GetComponent<LineSketchObject>();
                _brushReference.name = "BrushReferenceObject";
                _brushReference.gameObject.SetActive(false);
            }
        }
        
        /// <summary>
        /// Creates a new LineSketchObject and adds its creation to the command stack of the provided CommandInvoker.
        /// </summary>
        /// <param name="commandInvoker"></param>
        /// <param name="sketchWorld"></param>
        public virtual void InstantiateLine(CommandInvoker commandInvoker, SketchWorld sketchWorld)
        {
            //refreshing the invoker for the next line
            CurrentLocalLineInvoker = new CommandInvoker();

            CurrentLineSketchObject = Instantiate(_defaults.LineSketchObjectPrefab).GetComponent<LineSketchObject>();
            
            commandInvoker.ExecuteCommand(new AddObjectToSketchWorldRootCommand(CurrentLineSketchObject, sketchWorld));
        }

        
        /// <summary>
        /// Creates a LineBrush that can be used to modify the appearance of drawn lines
        /// </summary>
        protected LineBrush CreateLineBrush(int resolution, float scale, int interpolationSteps)
        {
            //A LineSketchObject is necessary to get the default values for the brush.
            //Initialising the brush by obtaining a brush object from from the LineSketchObject. 
            LineBrush ret = _brushReference.GetBrush() as LineBrush;

            //Defining the values of the new brush
            ret.CrossSectionVertices = CircularCrossSection.GenerateVertices(resolution, scale);
            ret.CrossSectionNormals = CircularCrossSection.GenerateVertices(resolution);
            ret.InterpolationSteps = interpolationSteps;

            return ret;
        }
        
        /// <summary>
        /// Changes the material of a given LineSketchObject.
        /// </summary>
        /// <param name="material"></param>
        /// <param name="lineSketchObject"></param>
        protected void ChangeLineMaterialTo(Material material, LineSketchObject lineSketchObject)
        {
            lineSketchObject.GetComponent<Renderer>().material = material;
        }
        
        /// <summary>
        /// Changes the color of the material of a given LineSketchObject.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="lineSketchObject"></param>
        protected static void ChangeLineMaterialColorTo(Color color, LineSketchObject lineSketchObject)
        {
            lineSketchObject.GetComponent<Renderer>().material.color = color;
        }
    }
}