using ColossalFramework.UI;
using ICities;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Reflection;

namespace RoadImporter
{
    public class RoadImporterThreading : ThreadingExtensionBase
    {
        private bool _actionProcessed = false;

        private ModelImport.Job _workingJob = null;
        private UIButton _continueButton = null;
        private Queue<ModelImport.Job> jobQueue = new Queue<ModelImport.Job>();
        private bool _hasJobs = false;
        // 0 - saving has not begun; 1 - on save panel; 2 - saving in background
        private int _isSaving = 0;
        private readonly object jobQueueLock = new object();


        public void ProduceJob(ModelImport.Job job)
        {
            Debug.Log("call ProduceJob");
            lock (jobQueueLock)
            {
                jobQueue.Enqueue(job);
            }
        }
         

        public void ConsumeJob()
        {
            Debug.Log("call ConsumeJob");
            lock (jobQueueLock)
            {
                _workingJob = jobQueue.Dequeue();
            }
        }


        public override void OnCreated(IThreading threading)
        {

            base.OnCreated(threading);
            ModelImport.worker = this;
            
        }

        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            if (_actionProcessed) return;

            _actionProcessed = true;
            //Debug.Log($"running job queue: {jobQueue.Count} jobs in queue");
            bool importFinished = WatchImportStatus();
            // import has finished
            if (importFinished) InvokeSavePanel(Environment.CurrentJob, Environment.CurrentJob);
            bool saveFinished = WatchSaveStatus();
            if (saveFinished) Environment.NextJob();
            _actionProcessed = false;
        }

        public bool WatchImportStatus()
        {
            if (_workingJob == null && jobQueue.Count != 0)
            {
                _hasJobs = true;
                ConsumeJob();
                ModelImport.RunImport(_workingJob);
                var fileList = UIView.Find("ModelImportPanel").Find("FileList") as UIListBox;
                int index = Array.IndexOf(fileList.items, $"{_workingJob.name}.FBX");
                if (index < 0)
                {
                    index = Array.IndexOf(fileList.items, $"{_workingJob.name}.fbx");
                }
                fileList.selectedIndex = index;
                _continueButton = UIView.Find("ModelImportPanel").Find("SelectAsset").components[0] as UIButton;
            }
            if (_workingJob != null && _continueButton != null)
            {
                if (_continueButton.isEnabled)
                {
                    Debug.Log("Click continue");
                    _continueButton.SimulateClick();
                    _continueButton = null;
                    ModelImport.FinalizeImport(_workingJob);
                    _workingJob = null;
                }
            }
            if (_workingJob == null && jobQueue.Count == 0 && _hasJobs)
            {
                _hasJobs = false;
                Utils.RefreshRoadEditor();
                return true;
            } else
            {
                return false;
            }
        }

        public void InvokeSavePanel(string name, string description)
        {
            Debug.Log("Invoke save panel");
            SaveAssetPanel.lastLoadedName = name;
            SaveAssetPanel.lastLoadedAsset = name;
            SaveAssetPanel.lastAssetDescription = description ?? "";
            UIView.Find("Esc").SimulateClick();
            UIView.Find("SaveAsset").SimulateClick();
            _isSaving = 1;
        }

        public bool WatchSaveStatus()
        {
            if (_isSaving == 0) return false;
            if (_isSaving == 2 && LoadSaveStatus.activeTask == null)
            {
                _isSaving = 0;
                return true;
            }

            // watches saving status
            if (UIView.Find("SaveAssetPanel(Clone)").isVisible)
            {
                SaveAssetPanel save = UIView.Find("SaveAssetPanel(Clone)").GetComponent<SaveAssetPanel>();
                string stagingPath = typeof(SaveAssetPanel).GetField("m_StagingPath", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(save) as string;
                if (Directory.Exists(stagingPath))
                {
                    ApplyThumbnail(Environment.CurrentJob);
                    save.OnSave();
                }
            } else {
                if (LoadSaveStatus.activeTask != null) _isSaving = 2;
            }
            return false;
        }

        private void ApplyThumbnail(string name)
        {
            SaveAssetPanel save = UIView.Find("SaveAssetPanel(Clone)").GetComponent<SaveAssetPanel>();
            string thumbPath = typeof(SaveAssetPanel).GetField("m_ThumbPath", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(save) as string;
            if (File.Exists(Path.Combine(Environment.importPath, name + "_thumb.png")))
            {
                File.Copy(Path.Combine(Environment.importPath, name + "_thumb.png"), thumbPath + ".png", true);
                if (File.Exists(Path.Combine(Environment.importPath, name + "_hovered.png")))
                {
                    File.Copy(Path.Combine(Environment.importPath, name + "_hovered.png"), thumbPath + "_hovered.png", true);
                    File.Copy(Path.Combine(Environment.importPath, name + "_pressed.png"), thumbPath + "_pressed.png", true);
                    File.Copy(Path.Combine(Environment.importPath, name + "_focused.png"), thumbPath + "_focused.png", true);
                    File.Copy(Path.Combine(Environment.importPath, name + "_disabled.png"), thumbPath + "_disabled.png", true);
                }
                typeof(SaveAssetPanel).GetMethod("ReloadThumbnail", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(save, new object[] { false });
            }
        }
    }
}
