using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Challenges._1._Basic_Progress_Bar.Scripts

{
    /// <summary>
    /// Edit this script for the ProgressBar challenge.
    /// </summary>
    public class ProgressBar : MonoBehaviour, IProgressBar
    {   
        /// <summary>
        /// You can add more options
        /// </summary>
        private enum ProgressSnapOptions
        {
            SnapToLowerValue,
            SnapToHigherValue,
            DontSnap
        }
        
        /// <summary>
        /// You can add more options
        /// </summary>
        private enum TextPosition
        {
            BarCenter,
            Progress,
            NoText
        }
        
        /// <summary>
        /// These settings below must function
        /// </summary>
        [Header("Options")]
        [SerializeField]
        private float baseSpeed;
        [SerializeField]
        private ProgressSnapOptions snapOptions;
        [SerializeField]
        private TextPosition textPosition;
        private GameObject fillbar;
        [SerializeField]
        private GameObject textField;
        private TextMeshProUGUI text;

        private void Awake()
        {
            fillbar = GameObject.FindGameObjectWithTag("fillBar");
            textField = GameObject.FindGameObjectWithTag("percentText");
            text = textField.GetComponent<TextMeshProUGUI>();
        }

        /// <summary>
        /// Sets the progress bar to the given normalized value instantly.
        /// </summary>
        /// <param name="value">Must be in range [0,1]</param>
        public void ForceValue(float value)
        {
            float fillbar_x = fillbar.transform.localScale.x;
            Vector3 newScaleVector = new Vector3(0f, 0f, 0f);
            if (value > 1)
            {
                newScaleVector = fillbar.transform.localScale;
                newScaleVector.x = 1;
                fillbar.transform.localScale = newScaleVector;
                text.text = newScaleVector.x.ToString();
                return;
            }
            else if(value < 0)
            {
                newScaleVector = fillbar.transform.localScale;
                newScaleVector.x = 0;
                fillbar.transform.localScale = newScaleVector;
                return;
            }
            newScaleVector = fillbar.transform.localScale;
            newScaleVector.x = value;
            fillbar.transform.localScale = newScaleVector;
            //Modifying the number from 0.465737 to 46
            int newInt = ((int)(newScaleVector.x * 100));
            //Then adding percent sign to display full text
            text.text = newInt.ToString() + "%";
        }

        /// <summary>
        /// The progress bar will move to the given value
        /// </summary>
        /// <param name="value">Must be in range [0,1]</param>
        /// <param name="speedOverride">Will override the base speed if one is given</param>
        public void SetTargetValue(float value, float? speedOverride = null)
        {
            Vector3 newScaleVector = fillbar.transform.localScale;
            float fillbar_x = fillbar.transform.localScale.x;
            if (value > 1)
            {
                newScaleVector = fillbar.transform.localScale;
                newScaleVector.x = 1;
            }
            else if (value <= 0)
            {
                newScaleVector = fillbar.transform.localScale;
                newScaleVector.x = 0;
            }
            else
            {
                newScaleVector = fillbar.transform.localScale;
                newScaleVector.x = value;
            }
            int newInt = ((int)(newScaleVector.x * 100));
            //Then adding percent sign to display full text
            text.text = newInt.ToString() + "%";
            StartCoroutine(scale(newScaleVector));
        }


        IEnumerator scale(Vector3 newScaleVector)
        {
            if (fillbar.transform.localScale.x < newScaleVector.x)
            {
                while (fillbar.transform.localScale.x < newScaleVector.x)
                {
                    fillbar.transform.localScale += (new Vector3(baseSpeed/10, 0, 0) * Time.deltaTime);
                    yield return (1f);
                }
                
            }
            else if (fillbar.transform.localScale.x > newScaleVector.x)
            {
                while (fillbar.transform.localScale.x > newScaleVector.x)
                {
                    fillbar.transform.localScale -= (new Vector3(baseSpeed / 10, 0, 0) * Time.deltaTime);
                    yield return (1f);
                }
            }
        }

        /*Enums can be done in other ways too such as calling the setTargetValue inside forceValue and vice versa also
          both functions will take the the ProgressSnapOptions as well as TextPosition parameter. */

        private void FinalCall(float value, ProgressSnapOptions snapOptions, TextPosition textPosition) 
        {
            switch (snapOptions)
            {
                case ProgressSnapOptions.DontSnap:
                    SetTargetValue(value);
                    TextEdit(textPosition);
                    return;
                case ProgressSnapOptions.SnapToHigherValue:
                    if(value > fillbar.transform.localScale.x)
                    {
                        ForceValue(value);
                        TextEdit(textPosition);
                        return;
                    }
                    SetTargetValue(value);
                    TextEdit(textPosition);
                    return;
                case ProgressSnapOptions.SnapToLowerValue:
                    if (value <= fillbar.transform.localScale.x)
                    {
                        ForceValue(value);
                        TextEdit(textPosition);
                        return;
                    }
                    SetTargetValue(value);
                    TextEdit(textPosition);
                    return;
            }

        }

        //We can make both functions take the textPosition as variable and keep moving from there but helper functions are easy to
        //extend and bring a lot more accesibility. 
        private void TextEdit(TextPosition textPosition)
        {
            switch (textPosition)
            {
                case TextPosition.BarCenter:
                    text.alignment = TextAlignmentOptions.Center;
                    int newInt = ((int)(fillbar.transform.localScale.x * 100));
                    //Then adding percent sign to display full text
                    text.text = newInt.ToString() + "%";
                    return;
                case TextPosition.NoText:
                    text.SetText("");
                    return;
                case TextPosition.Progress:
                    /*
                    text.transform.Translate(new Vector3(fillbar.transform.localScale.x,0,0));
                    */
                    return;
            }
        }
    }




}
