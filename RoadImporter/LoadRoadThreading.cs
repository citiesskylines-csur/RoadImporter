using ColossalFramework.UI;
using ICities;
using UnityEngine;
using System.Reflection;
using System.IO;

namespace RoadImporter
{
    public class LoadRoadThreading : ThreadingExtensionBase
    {
        private bool _actionProcessed = false;
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKey(KeyCode.L))
            {
                if (_actionProcessed) return;

                _actionProcessed = true;
                Environment.Runjobs();
                
            }
            else
            {
                _actionProcessed = false;
            }
        }
    }
}
