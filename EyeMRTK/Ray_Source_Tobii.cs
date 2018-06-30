using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Research.Unity;


public class Ray_Source_Tobii: MonoBehaviour
{



	// The Unity EyeTracker helper object.
	private VREyeTracker _eyeTracker;
	private VRCalibration _calibrationObject;

	private IVRGazeData last_ivrGazeData;


	public enum OptionsTobiiData
	{
		LatestGazeData,
		NextData,
		LatestProcessedGazeData
	}

	public OptionsTobiiData datatype;



	public enum OptionsAsRay
	{
		Left_Gaze,
		Right_Gaze,
		Combined_Gaze
	}

	public OptionsAsRay eye;

	private OutputRay output_ray;

	protected virtual bool HasEyeTracker
	{
		get
		{
			return _eyeTracker != null;
		}
	}
	protected virtual bool CalibrationInProgress
	{
		get
		{
			return _calibrationObject != null ? _calibrationObject.CalibrationInProgress : false;
		}
	}
	// Use this for initialization
	void Start ()
	{

		output_ray = this.GetComponentInParent<OutputRay> ();


		_eyeTracker = VREyeTracker.Instance;
		_calibrationObject = VRCalibration.Instance;


		// Get EyeTracker unity object
		_eyeTracker = VREyeTracker.Instance;
		if (HasEyeTracker) {
			Debug.Log ("Failed to find tobii eye tracker, has it been added to scene?");
		}

	}

	// Update is called once per frame
	void Update ()
	{


		if (CalibrationInProgress || _eyeTracker.LatestGazeData.CombinedGazeRayWorldValid!=true )
		{
			// Don't do anything if we are calibrating.
			if (output_ray != null) 
				output_ray._ray =default(Ray);
			return;
		}



		if (HasEyeTracker) {
			switch (datatype) {
			case OptionsTobiiData.NextData:
				last_ivrGazeData = _eyeTracker.NextData;
				break;
			case  OptionsTobiiData.LatestGazeData:
				last_ivrGazeData = _eyeTracker.LatestGazeData;
				break;
			case  OptionsTobiiData.LatestProcessedGazeData:
				last_ivrGazeData = _eyeTracker.LatestProcessedGazeData;
				break;
			default:
				last_ivrGazeData = _eyeTracker.LatestGazeData;
				break;
			}


			if (output_ray != null) {

				switch (eye) {
				case (OptionsAsRay.Combined_Gaze):

					output_ray._ray = last_ivrGazeData.CombinedGazeRayWorld;

					break;
				case (OptionsAsRay.Left_Gaze):

					output_ray._ray = last_ivrGazeData.Left.GazeRayWorld;

					break;
				case (OptionsAsRay.Right_Gaze):

					output_ray._ray = last_ivrGazeData.Right.GazeRayWorld;

					break;

				}


			}



		}

	}
}
