using UnityEngine;

using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.OMXCodeEditor;
using System.IO;

namespace UnityEditor.OMXCodeEditor
{
	public static class XCodePostProcess
	{


		[PostProcessBuild(999)]
		public static void OnPostProcessBuild( BuildTarget target, string pathToBuiltProject )
		{
			if (target != BuildTarget.iPhone) {
				Debug.LogWarning("Target is not iPhone. XCodePostProcess will not run");
				return;
			}

			// Create a new project object from build target
			XCProject project = new XCProject( pathToBuiltProject );

			// Find and run through all projmods files to patch the project.
			// Please pay attention that ALL projmods files in your project folder will be excuted!
			string projModPath = System.IO.Path.Combine(Application.dataPath, "Omniata/Editor/Omniata");
			string[] files = Directory.GetFiles( projModPath, "*.projmods", SearchOption.AllDirectories );
			foreach( string file in files ) {
				UnityEngine.Debug.Log("ProjMod File: "+file);
				project.ApplyMod( file );
			}

			// Finally save the xcode project
			project.Save();

		}


		// public static void Log(string message)
		// {
		// 	UnityEngine.Debug.Log("PostProcess: "+message);
		// }
	}
}