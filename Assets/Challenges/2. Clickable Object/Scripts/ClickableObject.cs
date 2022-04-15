using System;
using UnityEngine;

namespace Challenges._2._Clickable_Object.Scripts
{
    public class InvalidInteractionMethodException : Exception
    {
        private const string MessageWithMethodArgument =
            "Attempted to register to an invalid method of clickable interaction. The ClickableObject '{0}' does not allow interaction of type {1}";
        public InvalidInteractionMethodException(string gameObjectName, ClickableObject.InteractionMethod interactionMethod) : base(string.Format(MessageWithMethodArgument,gameObjectName,interactionMethod))
        {
        }
    }
    [RequireComponent(typeof(Collider))]
    public class ClickableObject : MonoBehaviour, IClickableObject
    {

        
        // Do not remove the provided 3 options, you can add more if you like
        [Flags]
        public enum InteractionMethod
        {
            Tap=2,
            DoubleTap=4,
            TapAndHold=8
        }
        
        
        /// <summary>
        /// Dont edit
        /// </summary>
        [SerializeField]
        private InteractionMethod allowedInteractionMethods;

        [SerializeField]
        private float lastClickTime;
        //Time Between Clics to identify as double click
        [SerializeField]
        private float _DoubleConst = 0.21f;
        //Last InteractionMethod
        private InteractionMethod _interactionMethod = 0;
        int clickTimes = 0;
        //Delegates
        OnClickableClicked Click;
        OnClickableClickedUnspecified Unsclick;

        [SerializeField]
        float _timeSinceLastClick = 100f;
        private void Start()
        {   
        }
        private void Update()
        {
            //Tap
            if (Input.GetMouseButtonDown(0))
            {
                clickTimes = 0;
                _timeSinceLastClick = Time.time - lastClickTime;
                lastClickTime = Time.time;
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 120f))
                {
                    if (hit.transform != null)
                    {
                        if (_timeSinceLastClick <= _DoubleConst && IsInteractionMethodValid(InteractionMethod.DoubleTap))
                        {
                            clickTimes = 2;
                            Debug.Log("DoubleTap");
                            RegisterToClickableDoubleTap(Doubletap);
                            return;
                        }
                        else if(_timeSinceLastClick > _DoubleConst && IsInteractionMethodValid(InteractionMethod.Tap))
                        {
                            clickTimes = 1;
                            Debug.Log("Tap");
                            RegisterToClickableTap(Tap);
                        }

                        
                    }
                }
            }   
        }

        /// <summary>
        /// Checks if the given interaction method is valid for this clickable object.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public bool IsInteractionMethodValid(InteractionMethod method)
        {
            return allowedInteractionMethods.HasFlag(method);
        }


        /// <summary>
        /// Updates the interaction method of the clickable object. Can contain more than one value due to bitflags
        /// </summary>
        public void SetInteractionMethod(InteractionMethod method)
        {
            if (IsInteractionMethodValid(method))
            {
                _interactionMethod = _interactionMethod & method;
                
                //allowedInteractionMethods = allowedInteractionMethods & method;
            }
        }
        
        
        /// <summary>
        /// Will invoke the given callback when the clickable object is interacted with alongside the method of interaction
        /// </summary>
        /// <param name="callback">Function to invoke</param>
        public void RegisterToClickable(OnClickableClicked callback)
        {
            callback(this, _interactionMethod);
            //callback.BeginInvoke(this,_interactionMethod,null,this);
        }

        /// <summary>
        /// Will unregister a previously provided callback
        /// </summary>
        /// <param name="callback">Function previously given</param>
        public void UnregisterFromClickable(OnClickableClicked callback)
        {
            int len = callback.GetInvocationList().Length;
            callback.GetInvocationList().SetValue(null, len-1);
        }

        /// <summary>
        /// Will invoke the given callback when the clickable object is tapped. 
        /// </summary>
        /// <param name="onTapCallback"></param>
        /// <exception cref="InvalidInteractionMethodException">If tapping is not allowed for this clickable</exception>
        public void RegisterToClickableTap(OnClickableClickedUnspecified onTapCallback) 
        {
            /*
            if (IsInteractionMethodValid(InteractionMethod.Tap))
            {
                onTapCallback();
                //onTapCallback.BeginInvoke(onTapCallback.EndInvoke, _interactionMethod);
            }
            else
            {
                throw new InvalidInteractionMethodException(this.transform.name, InteractionMethod.Tap);
            }
            */
            if(clickTimes == 1)
            {
                onTapCallback += Tap;
            }
            
        }
        
        /// <summary>
        /// Will invoke the given callback when the clickable object is tapped. 
        /// </summary>
        /// <param name="onTapCallback"></param>
        /// <exception cref="InvalidInteractionMethodException">If double tapping is not allowed for this clickable</exception>
        public void RegisterToClickableDoubleTap(OnClickableClickedUnspecified onTapCallback) 
        {
            if(clickTimes == 2)
            {
               onTapCallback += Doubletap;
            }
            
        }


        public void Tap()
        {
            Debug.Log("Tap" + this.name);
        }
        public void Doubletap()
        {
            Debug.Log("DoubleTap" + this.name);
        }


    }
}
