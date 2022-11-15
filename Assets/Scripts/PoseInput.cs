using System;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Subsystems;
using UnityEngine;
using UnityEngine.XR;

namespace MRTK3SketchingGeometry
{
    public class PoseInput : MonoBehaviour
    {
        private bool _jointIsValid;

        private HandsAggregatorSubsystem _aggregator;

        public event Action<Vector3> OnRightHandPinched = delegate {  };
        public event Action OnRightStartedPinching = delegate {  };
        public event Action OnRightStoppedPinching = delegate {  };
        public event Action<Vector3> OnLeftHandPinched = delegate {  };
        public event Action OnLeftStartedPinching = delegate {  };
        public event Action OnLeftStoppedPinching = delegate {  };

        private bool _wasRightHandPinchedLastFrame;
        private bool _wasLeftHandPinchedLastFrame;

        public void Start()
        {
            _aggregator = XRSubsystemHelpers.GetFirstRunningSubsystem<HandsAggregatorSubsystem>();
        }

        public void Update()
        {
            UpdateRightHand();
            UpdateLeftHand();
            
            PrepareNextFrame();
        }

        private void UpdateRightHand()
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
            else
            {
                if (_wasRightHandPinchedLastFrame)
                {
                    OnRightStoppedPinching();
                }
            }
        }
        
        private void UpdateLeftHand()
        {
            _jointIsValid = _aggregator.TryGetJoint(TrackedHandJoint.IndexTip, XRNode.LeftHand, out HandJointPose jointPose);

            if (_jointIsValid && IsLeftHandPinched())
            {
                if (!_wasLeftHandPinchedLastFrame)
                {
                    OnLeftStartedPinching();
                }
                
                OnLeftHandPinched(jointPose.Position);
            }
            else
            {
                if (_wasLeftHandPinchedLastFrame)
                {
                    OnLeftStoppedPinching();
                }
            }
        }

        private bool IsRightHandPinched()
        {
            return IsHandPinched(XRNode.RightHand);
        }

        private bool IsLeftHandPinched()
        {
            return IsHandPinched(XRNode.LeftHand);
        }
        
        private bool IsHandPinched(XRNode hand)
        {
            // Query pinch characteristics from the left hand.
            // pinchAmount is [0,1], normalized to the open/closed thresholds specified in the Aggregator configuration.
            // "isReadyToPinch" is adjusted with the HandRaiseCameraFOV and HandFacingAwayTolerance settings in the configuration.
            _aggregator.TryGetPinchProgress(hand, out bool isReadyToPinch,
                out bool isPinching, out float pinchAmount);
            
            //returns true if the right hand is fully pinched
            return (isPinching && pinchAmount >= 1f);
        }

        private void PrepareNextFrame()
        {
            _wasRightHandPinchedLastFrame = IsRightHandPinched();
            _wasLeftHandPinchedLastFrame = IsLeftHandPinched();
        }
    
    }   

}