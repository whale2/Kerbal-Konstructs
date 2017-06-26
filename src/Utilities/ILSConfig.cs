﻿using System;
using UnityEngine;
using KerbalKonstructs.Core;
using KerbalKonstructs.UI;
using System.IO;

namespace KerbalKonstructs
{
	
	public class ILSConfig
	{
		private static bool? detected = null;
		private static AssemblyLoader.LoadedAssembly navUtilAssm;

		public ILSConfig ()
		{
		}

		public static void generateFullILSConfig(StaticInstance inst) 
		{
			ScreenMessages.PostScreenMessage ("generateILSConfig()");

			// As long as we depend on working NavUtilities installation, we 
			// can call its methods instead of copypasting

			if (!(bool)detected)
				return;

			try {
				string siteName = inst.launchSite.LaunchSiteName;
				Log.Debug ("site name = " + siteName);
				string category = inst.launchSite.Category;
				bool isRunway = category != null && category.Equals ("Runway");

				Transform launchpad = inst.launchSite.lsGameObject.transform.Find(
					inst.launchSite.LaunchPadTransform);
				Log.Debug(String.Format("KK-ILS: launchpad transform: {0}, position: {1}",
					launchpad, launchpad.position));
				
				float heading = getHeading (launchpad);
				int hdg = (int)heading;
				if (hdg % 10 > 5)
					hdg += 10 - hdg % 10;

				int dg0 = hdg / 10 % 10;
				int dg1 = hdg / 100 % 10;
				String siteNameAndHdg = String.Format ("{0}-{1}{2}", siteName, dg1, dg0);
				ConfigNode cfg = generateILSConfig (siteNameAndHdg, launchpad.position, heading);
				string fileName = String.Format (
					"{0}/GameData/KerbalScienceFoundation/NavInstruments/Runways/{1}.rwy",
					KSPUtil.ApplicationRootPath,
					siteNameAndHdg);
				Log.Debug ("KK-ILS: writing NavUtils config to: " + fileName);


				if (isRunway) {
					// If it is runway, generate config for reversed direction
					Bounds bnd = ILSConfig.getBounds (inst.gameObject);
					Vector3 rwyLength = Vector3.Project(bnd.size, launchpad.forward);
					Log.Debug(String.Format("KK-ILS: runway length based on colliders: {0}", rwyLength));
					Vector3 farEnd = launchpad.position + rwyLength;
					Log.Debug(String.Format("KK-ILS: runway far end: {0}", farEnd));
					hdg = (hdg + 180) % 360;
					dg0 = hdg / 10 % 10;
					dg1 = hdg / 100 % 10;
					heading += 180;
					if (heading > 360)
						heading -= 360;
	
					String siteNameAndHdg0 = String.Format ("{0}-{1}{2}", siteName, dg1, dg0);
					cfg.GetNode("Runway").AddValue("IdentOfOpposite", siteNameAndHdg0);
					ConfigNode cfg0 = generateILSConfig (siteNameAndHdg0, farEnd, heading);
					cfg0.GetNode("Runway").AddValue("IdentOfOpposite", siteNameAndHdg);
					String fileName0 = String.Format (
						"{0}/GameData/KerbalScienceFoundation/NavInstruments/Runways/{1}.rwy",
						KSPUtil.ApplicationRootPath,
						siteNameAndHdg0);
					Log.Debug ("KK-ILS: writing NavUtils config to: " + fileName0);
					cfg0.Save(fileName0, "Generated by KerbalKonstructs");
				}
				cfg.Save(fileName, "Generated by KerbalKonstructs");
				reloadNavUtilsConfig();
			}
			catch (Exception e) {
				Debug.Log ("KK-ILS: Failed to get generate config: " + e);
				ScreenMessages.PostScreenMessage ("Failed to generate ILS config - check logs");
			}
		}

		public static ConfigNode generateILSConfig(string siteName, Vector3 endpoint, float heading) 
		{
			CelestialBody body = FlightGlobals.ActiveVessel.mainBody;
			Vector2d localizer = generateLocalizerCoords (V3to3d(endpoint), heading);

			ConfigNode ILScfg = new ConfigNode ();
			ILScfg.name = "Runway";
			ILScfg.AddValue ("ident", siteName);
			ILScfg.AddValue("shortID", siteName);
			ILScfg.AddValue ("hdg", heading);
			ILScfg.AddValue ("body", body.name);
			ILScfg.AddValue ("altMSL", body.GetAltitude (V3to3d (endpoint)));
			ILScfg.AddValue ("gsLatitude", body.GetLatitude (endpoint));
			ILScfg.AddValue ("gsLongitude", body.GetLongitude (endpoint));
			ILScfg.AddValue ("locLatitude", localizer.x);
			ILScfg.AddValue ("locLongitude", localizer.y);
			ILScfg.AddValue ("outerMarkerDist", 8000);
			ILScfg.AddValue ("middleMarkerDist", 2000);
			ILScfg.AddValue ("innerMarkerDist", 300);

			ConfigNode wrapper = new ConfigNode ();
			wrapper.AddNode (ILScfg);
			return wrapper;
		}

		public static void dropILSConfig(String launchSiteName, bool doReload) {
			String[] files = Directory.GetFiles (
                 String.Format (
	                 "{0}/GameData/KerbalScienceFoundation/NavInstruments/Runways/",
	                 KSPUtil.ApplicationRootPath),
                 String.Format ("{0}-??.rwy", launchSiteName));
			foreach (String f in files)
				File.Delete (f);
			if (doReload)
				reloadNavUtilsConfig ();
		}

		public static void handleCategoryChange(string oldCategory, string newCategory, StaticInstance inst)
		{
			// Just drop old config without reloading
			dropILSConfig(inst.launchSite.LaunchSiteName, false);
		}

		public static void renameSite(String oldName, String newName) {
			ScreenMessages.PostScreenMessage ("renameSite()");
		}

		public static bool detectNavUtils() {
			if (detected != null)
				return (bool)detected;
			detected = false;
			foreach (AssemblyLoader.LoadedAssembly asm in AssemblyLoader.loadedAssemblies) {
				Debug.Log ("KK-ILS: assembly: " + asm.name);
				if (asm.name.Equals("NavUtilLib")) {
					navUtilAssm = asm;
					detected = true;
					break;
				}
			}
			return (bool)detected;
		}

		private static float getHeading(Transform transform) {
			CelestialBody body = FlightGlobals.ActiveVessel.mainBody;
			Vector3 upVector = body.GetSurfaceNVector(
				body.GetLatitude(V3to3d(transform.position)), 
				body.GetLongitude(V3to3d(transform.position))).normalized;
			Vector3 north = Vector3.ProjectOnPlane(body.transform.up, upVector).normalized;
			Vector3 east = Vector3.Cross(upVector, north).normalized;
			Vector3 forward = Vector3.ProjectOnPlane(transform.forward, upVector);
			return Vector3.Angle (forward, north);
		}


		// This wheel should be already invented more than 9000 times
		private static Vector3d V3to3d(Vector3 vec) {
			return new Vector3d ((double)vec.x, (double)vec.y, (double)vec.z);
		}

		private static Vector2d generateLocalizerCoords(Vector3d coords, float heading) {
			
			CelestialBody body = FlightGlobals.ActiveVessel.mainBody;
			Type t = navUtilAssm.assembly.GetType("NavUtilLib.Utils");
			var methodInfo = t.GetMethod ("CalcCoordinatesFromInitialPointBearingDistance",
				new Type[] { typeof(Vector2d), typeof(double), typeof(double), typeof(double) });
			if (methodInfo == null)
				throw new MissingMethodException();
			object[] parameters = new object[4];
			parameters [0] = new Vector2d (body.GetLatitude (coords), body.GetLongitude(coords));
			parameters [1] = heading;
			parameters [2] = 1000; // Localizer distance; Leaving hardcoded as of now
			parameters [3] = body.Radius;

			return (Vector2d)methodInfo.Invoke (null, parameters);
		}

		private static void reloadNavUtilsConfig() {
			Debug.Log ("ILS-KK: Reloading NavUtils runways");
			Type t = navUtilAssm.assembly.GetType ("NavUtilLib.GlobalVariables.Settings");
			var methodInfo = t.GetMethod ("loadNavAids");
			if (methodInfo == null)
				throw new MissingMethodException ();
			methodInfo.Invoke (null, null);
		}

		public static Bounds getBounds(GameObject obj) {
			
			Collider[] mfs = obj.GetComponentsInChildren<Collider> ();
			if (mfs.Length == 0)
				return new Bounds ();
			Bounds b = mfs [0].bounds;
			for (int i = 1; i < mfs.Length; i++)
				b.Encapsulate (mfs [i].bounds);
			
			Log.Debug (String.Format ("KK-ILS: bounds: {0}", b));
			return b;
		}
	}
}
