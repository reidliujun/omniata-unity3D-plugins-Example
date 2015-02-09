// ------------------------------------------------------------------------------
//  <Omniata>
//		Unity3D project example
//		created by Jun, 23-10-2014.
//  </Omniata>
// ------------------------------------------------------------------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OmniataSDK;

namespace Omniatatest
{

	public class OmniataTest : MonoBehaviour
    {
		bool initialized;

		void Start()
		{
			initialized = false;

		}

		void OnApplicationPause(bool pause)
		{
			if (pause)
			{
				// we are in background
			}
			else
			{
				// we are in foreground again.
				if (initialized) 
				{
					// Automatically send om_load
					Omniata.Instance.TrackOmLoad();
				}
			}
		}

        void OnGUI()
        {
			//Initilize the Omniata instance
			int buttonCount = 5;
			int screenWidth = Screen.width;
			int screenHeight = Screen.height;
			
			int xSize = screenWidth / 3;
			int yMargin = Convert.ToInt32(screenHeight * 0.10);
			int ySize = (screenHeight - 2 * yMargin) / (buttonCount + (buttonCount - 1));
			
			int buttonXLeft = (screenWidth / 2) - (xSize / 2);
			
			// Make a background box
			int buttonIndex = 0;

			
			// Make the first button. If it is pressed, Omniata object will be initialized
			int buttonYTop = yMargin + (buttonIndex * ySize) + (buttonIndex * ySize);
			if (GUI.Button(new Rect(buttonXLeft, buttonYTop, xSize, ySize), "Initialize"))
			{	
				//set the loglevel only works for iOS and Android
				// Start the omniata SDK manually
				bool DEBUG = true;
				string API_KEY = "<API KEY>";
				string ORG = "<ORG>";
				Omniata.Instance.appDidLaunch(API_KEY, "<UID>", ORG, DEBUG ? Omniata.LogLevel.Debug : Omniata.LogLevel.Assert);
				Omniata.Instance.TrackOmLoad();
//				Omniata.Instance.appDidLaunch("a514370d", SystemInfo.deviceUniqueIdentifier, "demo", DEBUG ? Omniata.LogLevel.Debug : Omniata.LogLevel.Verbose);
//				Omniata.Instance.appDidLaunch("a514370d", SystemInfo.deviceUniqueIdentifier, "demo", Omniata.LogLevel.Debug);
				initialized=true;
			}
			
			// Make the second button, send track load events to Omniata
			buttonIndex++;
			buttonYTop = yMargin + (buttonIndex * ySize) + (buttonIndex * ySize);
			if (GUI.Button(new Rect(buttonXLeft, buttonYTop, xSize, ySize), "om_load"))
			{
				Omniata.Instance.TrackOmLoad(); //track load
//				Dictionary<string, string> parameters = new Dictionary<string, string>();
//				parameters.Add("para", "testpara");
//				Omniata.Instance.TrackOmLoad(parameters);//track load with parameters

			}
			
			// Make the third button, send track revenue events to Omniata
			buttonIndex++;
			buttonYTop = yMargin + (buttonIndex * ySize) + (buttonIndex * ySize);
			if (GUI.Button(new Rect(buttonXLeft, buttonYTop, xSize, ySize), "om_revenue "))
			{
				double total = 99.9;
				string currency_code = "EUR";
				Omniata.Instance.TrackOmRevenue(total,currency_code);//track revenue 
//				Dictionary<string, string> parameters = new Dictionary<string, string>();
//				parameters.Add("para", "testpara");
//				Omniata.Instance.TrackOmRevenue(total, currency_code, parameters);//track revenue with parameters
			}

			// Make the fourth button, customed the sending events to Omniata
			buttonIndex++;
			buttonYTop = yMargin + (buttonIndex * ySize) + (buttonIndex * ySize);
			if (GUI.Button(new Rect(buttonXLeft, buttonYTop, xSize, ySize), "om_event"))
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();
				parameters.Add("app", "testapp");
				parameters.Add("attack.attacker_won", "0");
				string eventType = "testing_event_type";
				Omniata.Instance.TrackOm(eventType,parameters);//track for local build



			}

			// Make the fifth button.
			/** Load the test message, only work for iOS build now
			 *  uncomment for Android build will return error
			 */
			buttonIndex++;
			buttonYTop = yMargin + (buttonIndex * ySize) + (buttonIndex * ySize);
			if (GUI.Button(new Rect(buttonXLeft, buttonYTop, xSize, ySize), "channel_info"))
			{
//				int ChannelId = 40;
//				Omniata.Instance.LoadOmChannelMessage(ChannelId); //load message for local build
//				string token = "testdevicetokenadfadfadfasdfasdf";
//				Omniata.Instance.EnableOmPushNotifications(token); // send device token to us for push notification
			}
       }
		
	}
}
