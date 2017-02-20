/*
 * EDITOR SCRIPT
 * [Unity Editor -> Window -> StreetView 2 Skybox]
 * 
 * Uses the Google Street View Image API
 * https://developers.google.com/maps/documentation/streetview/
 * The resolution of the images is limited to a max. size of 640x640.
 * 
 * The inspiration for this plugin came from this thread:
 * http://forum.unity3d.com/threads/google-streetview-in-oculus-rift-hmd.182867/
 * 
 * For more information about this script, visit:
 * http://support.syntetics.ch
 * 
 * Enjoy!
 * 
 * Version: 1.0.0
 */

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace MK
{
    /// <summary>
    /// Editor-Plugin that downloads the images via Google Street View Image API and then combines them into a 6-Sided SkyBox
    /// It will create a folder, you can specify and put the skyboxes there. Inside that folder a 2nd one is created called
    /// textures were all the images are stored.
    /// </summary>
    public class StreetView2Skybox : EditorWindow
    {
        

        /// <summary>
        /// The menu entry
        /// Feel free to change it
        /// </summary>
        #pragma warning disable 0219
        [MenuItem("Window/Vendor/MK/StreetView To Skybox")]
        public static void  Init()
        {
	        StreetView2Skybox leWindow = EditorWindow.GetWindowWithRect<StreetView2Skybox>(new Rect(0, 0, 450, 400), true, "Street View", true);
        }



        private float fov;
        private List<Direction> directions;
        private string folder;
        private SkyBoxPreference prefs;
    
        void OnEnable()
        {
            //get the folder where the project resides
            this.folder = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this));
            this.folder = this.folder.Remove(folder.Length - 20);

            //setup basic components
            this.fov = 90f;
            directions = new List<Direction>()
            {
                new Direction {Name = "_Front", Heading =   0, Pitch =   0},
                new Direction {Name = "_Back" , Heading = 180, Pitch =   0},
                new Direction {Name = "_Left" , Heading =  90, Pitch =   0},
                new Direction {Name = "_Right", Heading = 270, Pitch =   0},
                new Direction {Name = "_Up"   , Heading =   0, Pitch =  90},
                new Direction {Name = "_Down" , Heading =   0, Pitch = -90}
            };

            //try to load preferences
            LoadPreferenceData();
        }

        void OnLostFocus()
        {
	        this.SavePreferenceData();
	    }

	    void OnDestroy()
        {
		    this.SavePreferenceData();
	    }

        /// <summary>
        /// Draws the little window with all the stuff in it to make it work
        /// </summary>
        void OnGUI()
        {
		
            GUI.DrawTexture(new Rect(0,0,450,50), AssetDatabase.LoadAssetAtPath(this.folder + "logo.png", typeof(Texture2D)) as Texture2D);

            GUILayout.Space(60);

            prefs.skyBoxName = EditorGUILayout.TextField("Skybox Name", prefs.skyBoxName);
        
            EditorGUILayout.Space();

            prefs.directory = EditorGUILayout.TextField("Path To Save", prefs.directory);
        
            EditorGUILayout.Space();

            prefs.apiKey = EditorGUILayout.TextField("API Key", prefs.apiKey);
        
            EditorGUILayout.Space();

            EditorGUILayout.HelpBox("The StreetView-URL contains an @. The next two comma-separated numbers are the coordinates.", MessageType.None, true);
            prefs.latitude    = EditorGUILayout.DoubleField("Latitude", prefs.latitude);
            prefs.longitude   = EditorGUILayout.DoubleField("Longitude", prefs.longitude);
        
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Image Size to download(width and height are equal)");
            prefs.imgSize  = EditorGUILayout.IntField("ImageSize", prefs.imgSize);

            GUILayout.Space(20);

            if(GUILayout.Button("Create Skybox"))
            {
                //create the dir if it does not exist
                if (!Directory.Exists(prefs.directory))
                    Directory.CreateDirectory(prefs.directory);
                //set up a textures directory too
                if (!Directory.Exists(prefs.directory + "/textures"))
                    Directory.CreateDirectory(prefs.directory + "/textures");
                //prepare the SkyBox
                Material skyboxMat = new Material(Shader.Find("Skybox/6 Sided"));
                //now download and assign the textures
                foreach (Direction dir in this.directions)
                {
                    GetStreetviewTexture( dir.Heading, dir.Pitch, prefs.skyBoxName + dir.Name);
                    //refresh or errors will happen!
                    AssetDatabase.Refresh();
                    //Load and clamp the texture to get rid of the seams
                    Texture tx = AssetDatabase.LoadAssetAtPath<Texture>(Path.Combine(prefs.directory + "/textures/", prefs.skyBoxName + dir.Name + ".png"));
                    tx.wrapMode = TextureWrapMode.Clamp;
                    //link image to the correct skybox slot
                    skyboxMat.SetTexture( dir.Name +"Tex", tx);
                }
                //add the skybox to the asset DB
                AssetDatabase.CreateAsset(skyboxMat, Path.Combine(prefs.directory, prefs.skyBoxName + ".mat"));
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                //and done...
            }
						
						GUILayout.Space(50);
						
						EditorGUILayout.HelpBox("Normal/Free Accounts can downloads max. 640x640. Google offers premium accounts that can download up to 2048x2048. Check the link below.", MessageType.None, true);
						if(GUILayout.Button("Pricing And Plans of the Google Map API"))
						{
								Application.OpenURL("https://developers.google.com/maps/pricing-and-plans/#details");
						}
        }

        /// <summary>
        /// Builds the URL, calls Google, downloads and stores the pictures
        /// </summary>
        /// <param name="heading">Heading direction for the streetview-camera</param>
        /// <param name="pitch">Pitch direction for the streetview-camera</param>
        /// <param name="imgName">Name of the image on the disk</param>
        private void GetStreetviewTexture(double heading, double pitch, string imgName)
        {
            string url = "http://maps.googleapis.com/maps/api/streetview?"
                + "size=" + prefs.imgSize + "x" + prefs.imgSize
                + "&location=" + prefs.latitude + "," + prefs.longitude
                + "&heading=" + (heading) % 360.0 + "&pitch=" + (pitch) % 360.0
                + "&fov=" + this.fov;

            if (prefs.apiKey != "")
                url += "&key=" + prefs.apiKey;

            WWW www = new WWW(url);

            //wait (blocking) until done
            while (!www.isDone);

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogWarning("Unable to DL texture: " + www.error);
            }
            else
            {
                //recived some data, store it
                byte[] bytes = www.texture.EncodeToPNG();
                System.IO.File.WriteAllBytes(System.IO.Path.Combine(prefs.directory + "/textures/", imgName + ".png"), bytes);

                Debug.Log(imgName + " downloaded.");
            }
        }

        /// <summary>
        /// Saves the preferences the user has entered as XML
        /// </summary>
        private void SavePreferenceData()
        {
            try
            {
                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(prefs.GetType());
                StreamWriter file = new System.IO.StreamWriter(Path.Combine(this.folder, "preference.xml"));
                writer.Serialize(file, prefs);
                file.Close();
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
            
        }

        /// <summary>
        /// Loads the preferences the user has entered as XML
        /// </summary>
        private void LoadPreferenceData()
        {
            prefs = new SkyBoxPreference();
            if( File.Exists(Path.Combine(this.folder, "preference.xml")) )
            {
                try
                {
                    System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(prefs.GetType());
                    System.IO.StreamReader file = new System.IO.StreamReader(Path.Combine(this.folder, "preference.xml"));
                    prefs = (SkyBoxPreference) reader.Deserialize(file);
                    file.Close();
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }

    /// <summary>
    /// Preference stuff
    /// </summary>
    public class SkyBoxPreference
    {
        //basic values if the preferences.xml was not found
        public string apiKey     = "";
        public string skyBoxName = "Skybox_Matterhorn";
        public string directory  = "Assets/StreetViewSkyboxes";
        public double latitude   = 45.9764232;
        public double longitude  =  7.6584592;
        public int    imgSize    = 512;
    }

    /// <summary>
    /// Small helper class so i have all the Coordinate-Naming stuff in one place
    /// </summary>
    class Direction
    {
        public string Name    {get; set;}
        public double Heading {get; set;}
        public double Pitch   {get; set;}
    }
}