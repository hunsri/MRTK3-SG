using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using VRSketchingGeometry.Commands.Line;
using VRSketchingGeometry.SketchObjectManagement;
using VRSketchingGeometry;
using VRSketchingGeometry.Commands;
using VRSketchingGeometry.Meshing;
using VRSketchingGeometry.Serialization;

public class LineCreator : MonoBehaviour
{
    [SerializeField] private DefaultReferences defaults;
    private LineSketchObject _currentLineSketchObject;
    private SketchWorld _sketchWorld;

    [SerializeField] private float brushScale = 0.01f;

    private LineBrush _customBrush;
    
    [SerializeField] private Material customMaterial;
    
    //The LineSketchObject which will form the reference for all brushes
    private static LineSketchObject _brushReference;
    
    //Creates a CommandInvoker that is necessary to execute commands.
    private static readonly CommandInvoker Invoker = new CommandInvoker();

    private void Awake()
    {
        GetComponent<InputManager>().OnRightStartedPinching += HandleStartDrawingRequest;
        GetComponent<InputManager>().OnRightHandPinched += HandleDrawRequest;
    }
    
    private void Start()
    {
        //Creating the LineSketchObject that will act as the default reference for new brushes.
        //Deactivation of the reference will prevents it from rendering in the scene itself
        _brushReference = Instantiate(defaults.LineSketchObjectPrefab).GetComponent<LineSketchObject>();
        _brushReference.name = "BrushReferenceObject";
        _brushReference.gameObject.SetActive(false);
        
        //Create a SketchWorld, many commands require a SketchWorld to be present
        _sketchWorld = Instantiate(defaults.SketchWorldPrefab).GetComponent<SketchWorld>();

        //setting a default scale of one, size changes can be done later by changing the diameter
        _customBrush = CreateLineBrush(8, 1, 2);

        InstantiateLine();
    }
    
    
    /// <summary>
    /// Creates a LineBrush that can be used to modify the appearance of drawn lines
    /// </summary>
    private LineBrush CreateLineBrush(int resolution, float scale, int interpolationSteps)
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
    private static void ChangeLineMaterialTo(Material material, LineSketchObject lineSketchObject)
    {
        lineSketchObject.GetComponent<Renderer>().material = material;
    }
    
    private void Update()
    {
        
    }

    private void HandleStartDrawingRequest()
    {
        InstantiateLine();
    }

    private void HandleDrawRequest(Vector3 position)
    {
        DrawLinePointAt(position);
    }

    private void InstantiateLine()
    {
        _currentLineSketchObject = Instantiate(defaults.LineSketchObjectPrefab).GetComponent<LineSketchObject>();

        _currentLineSketchObject.SetBrush(_customBrush);
        
        _currentLineSketchObject.SetLineDiameter(brushScale);
        
        ChangeLineMaterialTo(customMaterial, _currentLineSketchObject);

        Invoker.ExecuteCommand(new AddObjectToSketchWorldRootCommand(_currentLineSketchObject, _sketchWorld));
    }
    
    private void DrawLinePointAt(Vector3 position)
    {
        Invoker.ExecuteCommand(new AddControlPointContinuousCommand(_currentLineSketchObject, position));
    }
}
