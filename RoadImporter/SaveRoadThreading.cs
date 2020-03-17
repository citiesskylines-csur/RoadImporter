using ColossalFramework.UI;
using ICities;
using UnityEngine;
using System.Reflection;
using System.IO;

namespace RoadImporter
{
    public class SaveRoadThreading : ThreadingExtensionBase
    {
        private bool _actionProcessed = false;
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKey(KeyCode.D))
            {
                if (_actionProcessed) return;

                _actionProcessed = true;
                var asset = ToolsModifierControl.toolController.m_editPrefabInfo as NetInfo;

                if (asset.m_netAI.GetType() == typeof(RoadAI))
                {
                    RoadAssetInfo myRoad = new RoadAssetInfo();
                    myRoad.ReadFromGame(asset);
                    Utils.SaveAsset(myRoad, Path.Combine(Environment.modPath, $"{myRoad.name}.xml"), typeof(RoadAI));
                } else if (asset.m_netAI.GetType() == typeof(TrainTrackAI))
                {
                    TrainTrackAssetInfo myRoad = new TrainTrackAssetInfo();
                    myRoad.ReadFromGame(asset);
                    Utils.SaveAsset(myRoad, Path.Combine(Environment.modPath, $"{myRoad.name}.xml"), typeof(TrainTrackAI));
                }
                ExceptionPanel panel = UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel");
                panel.SetMessage("Road Importer", $"Saved NetInfo {asset.name}", false);

            }
            else
            {
                // not both keys pressed: Reset processed state
                _actionProcessed = false;
            }
        }
    }
}
