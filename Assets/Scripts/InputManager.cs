using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Subsystems;
using UnityEngine;
using UnityEngine.XR;

public class InputManager : MonoBehaviour
{
    private bool _jointIsValid;

    private HandsAggregatorSubsystem _aggregator;

    public event Action<Vector3> OnRightHandPinched = delegate {  };
    public event Action OnRightStartedPinching = delegate {  };

    private bool _wasRightHandPinchedLastFrame;

    public void Start()
    {
        _aggregator = XRSubsystemHelpers.GetFirstRunningSubsystem<HandsAggregatorSubsystem>();
    }

    public void Update()
    {
        _jointIsValid = _aggregator.TryGetJoint(TrackedHandJoint.IndexTip, XRNode.RightHand, out HandJointPose jointPose);

        if (_jointIsValid && IsRightHandPinched())
        {
            if (!_wasRightHandPinchedLastFrame)
            {
                OnRightStartedPinching();
            }
            
            OnRightHandPinched(jointPose.Position);
        }
        PrepareNextFrame();
    }

    private bool IsRightHandPinched()
    {
        // Query pinch characteristics from the left hand.
        // pinchAmount is [0,1], normalized to the open/closed thresholds specified in the Aggregator configuration.
        // "isReadyToPinch" is adjusted with the HandRaiseCameraFOV and HandFacingAwayTolerance settings in the configuration.
        _aggregator.TryGetPinchProgress(XRNode.RightHand, out bool isReadyToPinch,
            out bool isPinching, out float pinchAmount);
        
        //returns true if the right hand is fully pinched
        return (isPinching && pinchAmount >= 1f);
    }

    private void PrepareNextFrame()
    {
        _wasRightHandPinchedLastFrame = IsRightHandPinched();
    }
    
}   
