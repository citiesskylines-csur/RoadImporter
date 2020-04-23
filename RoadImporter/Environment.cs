using System.IO;
using System.Collections.Generic;
using UnityEngine;
using ColossalFramework.IO;

namespace RoadImporter
{
    public class Environment
    {    
        public static string modPath = Path.Combine(DataLocation.localApplicationData, "RoadImporter");
        public static string importPath = Path.Combine(Path.Combine(DataLocation.localApplicationData, "Addons"), "Import");
        public static string texturePath = Path.Combine(modPath, "textures");
        
        public static string[] jobs;

        // Optimization level of asset,
        // O0: same as nomal CRP asset
        // O1: main texture is not saved and will be loaded using a separate mod. Only saves LODTexture.
        // O2: both main and LOD textures are not saved. Consumes more RAM than O1 due to game mechanics
        public static byte optLevel = 1;

        public static NetInfo gameAsset;
        public static IAssetInfo loadedAsset;

        public static int jobId = -1;

        private static Dictionary<string, Material> _materialCache = new Dictionary<string, Material>();

        public static string CurrentJob => jobId < jobs.Length ? jobs[jobId] : null;

        public static Material FindMaterial(string name, string meshName)
        {
            if (_materialCache.ContainsKey(name))
            {
                return _materialCache[name];
            } else
            {
                string [] textures = Directory.GetFiles(texturePath, name + "_?.png", SearchOption.AllDirectories);
                foreach (string t in textures)
                {
                    string suffix = t.Substring(t.Length - 6);
                    string placeholderName = t[0] == 'd' ? "placeholder.png" : "placeholder2.png";
                    string sourceFile = optLevel == 0 ? Path.Combine(texturePath, name + suffix) : Path.Combine(texturePath, placeholderName);
                    File.Copy(sourceFile, Path.Combine(importPath, meshName + suffix), true);
                    if (File.Exists(Path.Combine(texturePath, name + "_lod" + suffix)))
                    {
                        File.Copy(Path.Combine(texturePath, name + "_lod" + suffix), Path.Combine(importPath, meshName + "_lod" + suffix), true);
                    }
                }
                return null;
                
            }  
        }

        public static Material GetMaterial(string name)
        {
            return _materialCache[name];
        }

        public static void CacheMaterial(string name, Material material)
        {
            _materialCache.Add(name, material);
        }

        public static void CreateJobs(string path)
        {
            jobs = File.ReadAllLines(path);
        }

        private static void PrepareJob()
        {
            // move files to addon/import directory
            string[] files = Directory.GetFiles(Path.Combine(modPath, "import"), CurrentJob + "_*", SearchOption.AllDirectories);
            Debug.Log($"found {files.Length} files to import");
            foreach (string file in files)
            {
                string dst = Path.Combine(importPath, Path.GetFileName(file));
                try
                {
                    File.Move(file, dst);
                }
                catch (IOException e)
                {
                    Debug.Log(e);
                    Debug.Log($"File {file} already exists!");
                }

            }
            // load asset data from XML
            loadedAsset = Utils.LoadAsset(Path.Combine(importPath, $"{CurrentJob}_data.xml"), gameAsset.m_netAI.GetType());
        }

        private static void CleanUp()
        {
            // remove texture files copied if optimization is enabled
            string[] files = Directory.GetFiles(importPath, "*_?.png", SearchOption.AllDirectories);
            if (optLevel > 0)
            {
                foreach (string file in files) File.Delete(file);
            }
            
            // move files back to mod directory
            files = Directory.GetFiles(importPath, CurrentJob + "*.*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                string dst = Path.Combine(Path.Combine(modPath, "import"), Path.GetFileName(file));
                try
                {
                    File.Move(file, dst);
                } catch (IOException e)
                {
                    Debug.Log(e);
                    Debug.Log("Game still using the file, copy back to RoadImporter folder instead");
                    try
                    {
                        File.Copy(file, dst);
                    } catch (IOException ee)
                    {
                        Debug.Log(ee);
                        Debug.Log("File is already present in RoadImporter folder");
                    }
                }
            }
        } 

        public static void NextJob()
        {
            Debug.Log($"Call NextJob: {jobId}");
            // cleanup last job if exists
            if (jobId >= 0) CleanUp();
            jobId++;
            // prepare and run next job if exists
            if (jobId < jobs.Length)
            {
                Debug.Log($"Running job {CurrentJob}");
                PrepareJob();
                Debug.Log(loadedAsset);
                Debug.Log(gameAsset);
                loadedAsset.WriteToGame(gameAsset);
            } else
            {
                Debug.Log("All completed!");
            }
        }
           
        public static void Runjobs()
        {
            gameAsset = ToolsModifierControl.toolController.m_editPrefabInfo as NetInfo;
            Debug.Log("Run!");
            NextJob();
        }

    }
}
