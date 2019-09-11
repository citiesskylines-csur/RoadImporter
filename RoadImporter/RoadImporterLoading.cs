using ColossalFramework.UI;
using System.IO;
using ICities;

namespace RoadImporter
{
    public class RoadImporterLoading : ILoadingExtension
    {
        // called when level loading begins
        public void OnCreated(ILoading loading)
        {
            Directory.CreateDirectory(Environment.modPath);
        }

        // called when level is loaded
        public void OnLevelLoaded(LoadMode mode)
        {
            Environment.CreateJobs(Path.Combine(Environment.modPath, "imports.txt"));
            string message = $"Found {Environment.jobs.Length} items to import, press CTRL+L to start\n";
            message += File.ReadAllText(Path.Combine(Environment.modPath, "imports.txt"));


            ExceptionPanel panel = UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel");
            panel.SetMessage("Road Importer", message, false);
        }

        // called when unloading begins
        public void OnLevelUnloading()
        {
        }

        // called when unloading finished
        public void OnReleased()
        {
        }
    }
}


