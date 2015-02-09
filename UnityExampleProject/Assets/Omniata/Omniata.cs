using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OmniataSDK{

	/// <summary>
	/// This is an Unity-plugin for OM iOS and Android SDK.
	/// Android SDK version: 2.0.1, iOS SDK version: 2.0.1
	/// OM is the only integration point between a Unity application and the SDK.
	/// Details of the OM iOS and Android SDK, check the official documentation here:
	/// https://omniata.atlassian.net/wiki/display/DOC/SDKs
	/// </summary>
	public class Omniata: MonoBehaviour
	{
		public const string SDK_VERSION = "unitySDK-1.2.2";
		// Event parameter names consts
		private const string EVENT_PARAM_API_KEY = "api_key";
		private const string EVENT_PARAM_CURRENCY_CODE = "currency_code";
		private const string EVENT_PARAM_EVENT_TYPE = "om_event_type";
		private const string EVENT_PARAM_TOTAL = "total";
		private const string EVENT_PARAM_UID = "uid";
		private const string EVENT_PARAM_OM_DELTA = "om_delta";
		private const string EVENT_PARAM_OM_DEVICE = "om_device";
		private const string EVENT_PARAM_OM_PLATFORM = "om_platform";
		private const string EVENT_PARAM_OM_OS_VERSION = "om_os_version";
		private const string EVENT_PARAM_OM_SDK_VERSION = "om_sdk_version";
		private const string EVENT_PARAM_OM_RETRY = "om_retry";
		private const string EVENT_PARAM_OM_DISCARDED = "om_discarded";
		private const string EVENT_PARAM_OM_UNITY_SDK_VERSION = "om_unity_sdk_version";
		// Event type consts
		private const string EVENT_TYPE_OM_LOAD = "om_load";
		private const string EVENT_TYPE_OM_REVENUE = "om_revenue";
		
		// Channel type consts
		private const string CHANNEL_ID = "channel_id";
		
		public static Omniata Instance
		{
			get;
			private set;
		}
		private static string api_key;
		private static string uid;
		private static string org;
		public static string analyzerUrl;
		public static string engagerUrl;

		/**
		 * LogLevel value list
		 */ 
		public enum LogLevel {
			Verbose = 2,
			Debug,
			Info,
			Warn,
			Error,
			Assert
		}

		// Setting your personalized api_key, uid and org in Omniata.prefab.
		public string API_KEY = "<API KEY>";
		public string UID = "<User ID>";
		public string ORG = "<Orgnization Name>";
		public LogLevel LOGLEVEL = LogLevel.Verbose;
		public bool startManually = false;
		
		
		protected virtual void Awake() {
			Instance = this;
			
			if (!this.startManually) {;
				this.appDidLaunch (this.API_KEY, this.UID, this.ORG, this.LOGLEVEL);
			}
		}
		
		/// <summary>
		/// Apps the did launch.
		/// </summary>
		/// <param name="API_KEY">API_Key.</param>
		/// <param name="UID">UID.</param>
		/// <param name="ORG">ORG.</param>
		/// <param name="LOGLEVEL">LOGLEVE.</param>
		public void appDidLaunch(string API_KEY, string UID, string ORG, LogLevel LOGLEVEL){
			api_key = API_KEY;
			uid = UID;
			org = ORG;
			setURL (org);
			#if UNITY_IOS && !UNITY_EDITOR
			this.SetOmLoglevel (LOGLEVEL);
			Initialize (api_key, uid, org);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			this.SetOmLoglevel (LOGLEVEL);
			Initialize (api_key, uid, org);
			#elif UNITY_WEBPLAYER && !UNITY_EDITOR
			this.SetOmLoglevel (LOGLEVEL);
			#endif
			//send TrackLoad automatically when the app is launched for the first time
			this.TrackOmLoad ();
		}
		
		/// <summary>
		/// Sets the Omniata loglevel.
		/// </summary>
		/// <param name="priority">the priority of the loglevel</param>
		public void SetOmLoglevel(LogLevel priority){
			SetLogLevel ((int)priority);	
		}

		/// <summary>
		/// Omniata log
		/// </summary>
		/// <param name="message">Message.</param>
		public void LogOm(string message){
			Log (message);
		}

		/// <summary>
		/// Tracks Omniata load.
		/// </summary>
		public void TrackOmLoad() {
			#if UNITY_IOS && !UNITY_EDITOR
			TrackLoad();
			#elif UNITY_ANDROID && !UNITY_EDITOR
			TrackLoad();
			#elif UNITY_WEBPLAYER && !UNITY_EDITOR
			StartCoroutine(this.TrackLoadCoroutine ());
			#endif
		}
		
		/// <summary>
		/// Tracks Omniata load with parameters.
		/// </summary>
		/// <param name="parameters">Parameters.</param>
		public void TrackOmLoad(Dictionary<string, string> parameters){
			#if UNITY_IOS && !UNITY_EDITOR
			TrackLoad(parameters); 
			#elif UNITY_ANDROID && !UNITY_EDITOR
			TrackLoad(parameters);
			#elif UNITY_WEBPLAYER && !UNITY_EDITOR
			StartCoroutine(this.TrackLoadCoroutine (parameters));
			#endif
		}
		
		/// <summary>
		/// Tracks Omniata revenue.
		/// </summary>
		/// <param name="total">Total.</param>
		/// <param name="currency_code">Currency_code.</param>
		public void TrackOmRevenue(double total, string currency_code){
			#if UNITY_IOS && !UNITY_EDITOR
			TrackRevenue(total, currency_code);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			TrackRevenue(total, currency_code);
			#elif UNITY_WEBPLAYER && !UNITY_EDITOR
			StartCoroutine(this.TrackRevenueCoroutine (total, currency_code));
			#endif	
		}
		
		/// <summary>
		/// Tracks Omniata revenue with parameters.
		/// </summary>
		/// <param name="total">Total.</param>
		/// <param name="currency_code">Currency_code.</param>
		/// <param name="parameters">Parameters.</param>
		public void TrackOmRevenue(double total, string currency_code, Dictionary<string, string> parameters){
			#if UNITY_IOS && !UNITY_EDITOR
			TrackRevenue(total, currency_code, parameters);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			TrackRevenue(total, currency_code, parameters);
			#elif UNITY_WEBPLAYER && !UNITY_EDITOR
			StartCoroutine(this.TrackRevenueCoroutine (total, currency_code, parameters));
			#endif
		}
		
		/// <summary>
		/// Tracks Omniata event.
		/// </summary>
		/// <param name="eventType">Event type.</param>
		/// <param name="parameters">Parameters.</param>
		public void TrackOm(string eventType, Dictionary<string, string> parameters){
			#if UNITY_IOS && !UNITY_EDITOR
			Track(eventType, parameters);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			Track(eventType, parameters);
			#elif UNITY_WEBPLAYER && !UNITY_EDITOR
			StartCoroutine(this.TrackOmCoroutine (eventType, parameters));
			#endif	
			
		}
		
		/// <summary>
		/// Loads Omniata channel message.
		/// </summary>
		/// <param name="channelID">Channel ID.</param>
		public void LoadOmChannelMessage(int channelID){
			#if UNITY_IOS && !UNITY_EDITOR
			LoadChannelMessage(channelID);
			#elif UNITY_WEBPLAYER && !UNITY_EDITOR
			StartCoroutine(Omniata.LoadOmChannelMessageCoroutine (channelID));
			#endif	
		}
		/// <summary>
		/// Enables Omniata push notifications.
		/// </summary>
		/// <param name="device_token">Device_token.</param>
		public void EnableOmPushNotifications(String device_token){
			EnablePushNotifications (device_token);
		}

		/// <summary>
		/// Disables Omniata push notifications.
		/// </summary>
		public void DisableOmPushNotifications(){
			DisablePushNotifications ();
		}


		/**
		 * Extern loglevel of android and iOS SDK
		 * 
		 */


		#if UNITY_IOS && !UNITY_EDITOR
		
		[System.Runtime.InteropServices.DllImport("__Internal")]
		private static extern void SetLogLevel(int priority);
		
		#elif UNITY_ANDROID && !UNITY_EDITOR
		/**
		 * priority in Android is from 2-8, the lower the loglevel, the more verbose the log is
		 * check details in Android API documentation.
		 */
		private static void SetLogLevel(int priority){
			using (AndroidJavaClass javaClass = new AndroidJavaClass("com.omniata.android.sdk.Omniata"))
			{
				javaClass.CallStatic("setLogLevel",priority);
			}
		}
		#elif UNITY_WEBPLAYER && !UNITY_EDITOR
		private static void SetLogLevel(int priority){
		}
		#else
		private static void SetLogLevel(int priority){
		}
		#endif
		
		
		/**
         * Extern log of SDK
         */
		#if UNITY_IOS && !UNITY_EDITOR
		[System.Runtime.InteropServices.DllImport("__Internal")]
		private static extern void Log(string message);
		
		#elif UNITY_ANDROID && !UNITY_EDITOR
		private static void Log(string message){
			using (AndroidJavaClass javaClass = new AndroidJavaClass("com.omniata.android.sdk.Omniata"))
			{
				javaClass.CallStatic("unity_log",message);
			}
		}
		#elif UNITY_WEBPLAYER && !UNITY_EDITOR
		private void Log(string message){
			message = DateTime.Now + " Omniata" + ": " + message;
			Debug.Log (message);		
		}
		#else
		private void Log(string message){	
		}
		#endif
		
		/**
         * Get the current context of the activity.
         */	
		#if UNITY_ANDROID && !UNITY_EDITOR
		private static AndroidJavaObject playerActivityContext;
		private static void getContext()
		{
			using (var actClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
				playerActivityContext = actClass.GetStatic<AndroidJavaObject>("currentActivity");
			}
		}
		#endif
		
		/**
         * Extern initialize with api_key, user_id, org
         */	
		#if UNITY_IOS && !UNITY_EDITOR
		[System.Runtime.InteropServices.DllImport("__Internal")]
		private extern static void Initialize(string api_key, string uid, string org);
		#elif UNITY_ANDROID && !UNITY_EDITOR
		private static void Initialize(string apiKey, string userID, string org)
		{
			// Activity class name where you define the initialize method for omniata.
			using (AndroidJavaClass javaClass = new AndroidJavaClass("com.omniata.android.sdk.Omniata"))
			{
				getContext();
				javaClass.CallStatic("initialize", playerActivityContext, apiKey, userID, org);
			}
		}
		#endif
		
		/**
         * TrackLoad with and without additional parameters
         */
		#if UNITY_IOS && !UNITY_EDITOR
		[System.Runtime.InteropServices.DllImport("__Internal")]
		private extern static void TrackLoadWithParameters(string parameters);
		private static void TrackLoad(){
			Dictionary<string, string> dictPara = new Dictionary<string, string>();
			AddUnitySDKVersion(dictPara);
			String parameters;
			parameters = ToKeyValuePairString(dictPara);
			TrackLoadWithParameters(parameters);
		}
		private static void TrackLoad(Dictionary<string, string> dictPara){
			AddUnitySDKVersion(dictPara);
			String parameters;
			parameters = ToKeyValuePairString(dictPara);
			TrackLoadWithParameters(parameters);
		}
		
		#elif UNITY_ANDROID && !UNITY_EDITOR
		private static void TrackLoad(){
			using (AndroidJavaClass javaClass = new AndroidJavaClass("com.omniata.android.sdk.Omniata"))
			{
				Dictionary<string, string> dictPara = new Dictionary<string, string>();
				AddUnitySDKVersion(dictPara);
				String parameters;
				parameters = ToKeyValuePairString(dictPara);
				javaClass.CallStatic("unityTrackLoad",parameters);
			}		
		}
		private static void TrackLoad(Dictionary<string, string> dictPara){
			
			using (AndroidJavaClass javaClass = new AndroidJavaClass("com.omniata.android.sdk.Omniata"))
			{
				AddUnitySDKVersion(dictPara);
				String parameters;
				parameters = ToKeyValuePairString(dictPara);
				javaClass.CallStatic("unityTrackLoad",parameters);
			}		
		}
		#elif UNITY_WEBPLAYER && !UNITY_EDITOR
		
		private IEnumerator TrackLoadCoroutine() {
			Dictionary<string, string> parameters = new Dictionary<string, string>();	
			AddAutomaticParameters(parameters);
			parameters.Add(EVENT_PARAM_API_KEY, api_key);
			parameters.Add(EVENT_PARAM_UID, uid);
			string url = urlGenerator (analyzerUrl, parameters);
			WWW www = new WWW(url);
			yield return www;
		}
		private IEnumerator TrackLoadCoroutine(Dictionary<string, string> parameters) {	
			AddAutomaticParameters(parameters);
			parameters.Add(EVENT_PARAM_API_KEY, api_key);
			parameters.Add(EVENT_PARAM_UID, uid);
			string url = urlGenerator (analyzerUrl, parameters);
			WWW www = new WWW(url);
			yield return www;
		}
		#endif
		
		/**
         * Extern TrackRevenue with total, currency_code and optional additional parameters
         */
		#if UNITY_IOS && !UNITY_EDITOR
		[System.Runtime.InteropServices.DllImport("__Internal")]
		private extern static void TrackRevenue(double total, string currency_code);
		[System.Runtime.InteropServices.DllImport("__Internal")]
		private extern static void TrackRevenueWithParameters(double total, string currency_code, string parameters);
		private static void TrackRevenue(double total, string currency_code, Dictionary<string, string> dictPara){
			String parameters;
			parameters = ToKeyValuePairString(dictPara);
			TrackRevenueWithParameters(total, currency_code, parameters);
		}
		
		#elif UNITY_ANDROID && !UNITY_EDITOR
		private static void TrackRevenue(double total, string currencyCode)
		{
			using (AndroidJavaClass javaClass = new AndroidJavaClass("com.omniata.android.sdk.Omniata"))
			{
				javaClass.CallStatic("trackRevenue",total,currencyCode);
			}
		}
		private static void TrackRevenue(double total, string currencyCode, Dictionary<string, string> dictPara)
		{
			String parameters;
			parameters = ToKeyValuePairString(dictPara);
			using (AndroidJavaClass javaClass = new AndroidJavaClass("com.omniata.android.sdk.Omniata"))
			{
				javaClass.CallStatic("unityRevenueTrack",total,currencyCode,parameters);
			}
		}
		
		#elif UNITY_WEBPLAYER && !UNITY_EDITOR
		private IEnumerator TrackRevenueCoroutine(double total, string currency_code){
			Dictionary<string, string> parameters = new Dictionary<string, string>();	
			parameters.Add(EVENT_PARAM_API_KEY, api_key);
			parameters.Add(EVENT_PARAM_UID, uid);
			parameters.Add(EVENT_PARAM_TOTAL, total.ToString());
			parameters.Add(EVENT_PARAM_CURRENCY_CODE, currency_code);
			string url = urlGenerator (analyzerUrl, parameters);
			WWW www = new WWW(url);
			yield return www;
		}
		private IEnumerator TrackRevenueCoroutine(double total, string currency_code, Dictionary<string, string> parameters){
			parameters.Add(EVENT_PARAM_API_KEY, api_key);
			parameters.Add(EVENT_PARAM_UID, uid);
			parameters.Add(EVENT_PARAM_TOTAL, total.ToString());
			parameters.Add(EVENT_PARAM_CURRENCY_CODE, currency_code);
			string url = urlGenerator (analyzerUrl, parameters);
			WWW www = new WWW(url);
			yield return www;
		}
		#endif
		
		/**
         * Extern TrackEvent with type and parameters
         */
		#if UNITY_IOS && !UNITY_EDITOR
		[System.Runtime.InteropServices.DllImport("__Internal")]
		extern static void TrackEvent(string type, string parameters);
		private static void Track (string type, Dictionary<string, string> dictPara)
		{
			string parameters;
			parameters = ToKeyValuePairString(dictPara);
			TrackEvent(type, parameters);
		}
		#elif UNITY_ANDROID && !UNITY_EDITOR
		private static void Track(string eventType, Dictionary<string, string> dictPara)
		{
			using (AndroidJavaClass javaClass = new AndroidJavaClass("com.omniata.android.sdk.Omniata"))
			{
				String parameters;
				parameters = ToKeyValuePairString(dictPara);
				javaClass.CallStatic("unityTrack",eventType,parameters);
			}
		}
		#elif UNITY_WEBPLAYER && !UNITY_EDITOR
		private IEnumerator TrackOmCoroutine(string eventType, Dictionary<string, string> parameters){
			parameters.Add (EVENT_PARAM_EVENT_TYPE, eventType);
			parameters.Add(EVENT_PARAM_API_KEY, api_key);
			parameters.Add(EVENT_PARAM_UID, uid);
			string url = urlGenerator (analyzerUrl, parameters);
			WWW www = new WWW(url);
			yield return www;
		}
		#endif
		
		
		
		/**
         * Extern LoadChannelMessage with channelID
         * only support iOS for now.
         */
		#if UNITY_IOS && !UNITY_EDITOR
		[System.Runtime.InteropServices.DllImport("__Internal")]
		private extern static void LoadChannelMessage(int channelID);
		#elif UNITY_WEBPLAYER && !UNITY_EDITOR
		private static IEnumerator LoadOmChannelMessageCoroutine(int channelID){
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			parameters.Add(EVENT_PARAM_API_KEY, api_key);
			parameters.Add(EVENT_PARAM_UID, uid);
			parameters.Add (CHANNEL_ID, channelID.ToString());
			string url = urlGenerator (engagerUrl, parameters);
			WWW www = new WWW(url);
			yield return www;		
		}
		#endif


		#if UNITY_IOS && !UNITY_EDITOR
			[System.Runtime.InteropServices.DllImport("__Internal")]
			private extern static void EnablePushNotifications(string device_token);
		#elif UNITY_ANDROID && !UNITY_EDITOR
			private static void EnablePushNotifications(string device_token)
			{
				using (AndroidJavaClass javaClass = new AndroidJavaClass("com.omniata.android.sdk.Omniata"))
				{
					javaClass.CallStatic("enablePushNotifications",device_token);
				}
			}
		#else
			private static void EnablePushNotifications(string device_token){
//			Debug.Log("Enable push notifications");
			}
		#endif

		#if UNITY_IOS && !UNITY_EDITOR
			[System.Runtime.InteropServices.DllImport("__Internal")]
			private extern static void DisablePushNotifications();
		#elif UNITY_ANDROID && !UNITY_EDITOR
			private static void DisablePushNotifications()
			{
				using (AndroidJavaClass javaClass = new AndroidJavaClass("com.omniata.android.sdk.Omniata"))
				{
					javaClass.CallStatic("disablePushNotifications");
				}
			}
		#elif UNITY_WEBPLAYER && !UNITY_EDITOR
			private static void DisablePushNotifications(){
//				Debug.Log("Disable push notifications");
			}
		#else
			private static void DisablePushNotifications(){
			}
		#endif

		/// <summary>
		/// Sets the URL for Omniata analyzer and engager.
		/// </summary>
		/// <param name="morg">ORG.</param>
		private static void setURL(string morg){
			analyzerUrl = "https://"+morg+".analyzer.omniata.com/event?";
			engagerUrl = "https://"+morg+".engager.omniata.com/channel?";
		}

		/// <summary>
		/// Generated the automatic om parameters for platforms other than android and iOS
		/// </summary>
		/// <param name="parameters">Parameters.</param>
		private static void AddAutomaticParameters(Dictionary<string, string> parameters)
		{
			RuntimePlatform platform = Application.platform;
			parameters.Add(EVENT_PARAM_EVENT_TYPE,EVENT_TYPE_OM_LOAD);
			parameters.Add(EVENT_PARAM_OM_PLATFORM, platform.ToString());
			parameters.Add(EVENT_PARAM_OM_DEVICE, SystemInfo.deviceModel);
			parameters.Add(EVENT_PARAM_OM_OS_VERSION, SystemInfo.operatingSystem);
			parameters.Add(EVENT_PARAM_OM_SDK_VERSION, SDK_VERSION);
		}

		/// <summary>
		/// Adds the unity SDK version for a dictionary parameters.
		/// </summary>
		/// <param name="parameters">Parameters.</param>
		private static void AddUnitySDKVersion(Dictionary<string, string> parameters)
		{
			parameters.Add(EVENT_PARAM_OM_UNITY_SDK_VERSION,SDK_VERSION);
		}

		/// <summary>
		/// Convert dictionary to URL encoded key value pair strings.
		/// Calling TrackEvent with type and attributesString.
	    /// </summary>
		/// <returns>The key value pair string.</returns>
		/// <param name="parameters">Parameters.</param>
		private static string ToKeyValuePairString (Dictionary<string, string> parameters)
		{
			string attributesString = "";
			foreach(KeyValuePair<string, string> kvp in parameters)
			{
				attributesString += WWW.EscapeURL(kvp.Key) + "=" + WWW.EscapeURL(kvp.Value) + "\n";
			}
			return attributesString;
		}

		/// <summary>
		/// Genearted url for omniata event API with the parameters.
		/// </summary>
		/// <returns>The generator.</returns>
		/// <param name="baseUrl">Base URL.</param>
		/// <param name="parameters">Parameters.</param>
		private static string urlGenerator(string baseUrl, Dictionary<string, string> parameters){
			return baseUrl + String.Join ("&", ToKeyValuePairString (parameters).Split ('\n'));
		}
		
	}
}

