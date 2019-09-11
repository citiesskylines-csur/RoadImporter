using ColossalFramework.UI;
using ICities;
using UnityEngine;
using System.Reflection;
using System.IO;

namespace RoadImporter
{
    public class TestJobThreading : ThreadingExtensionBase
    {
        private bool _actionProcessed = false;
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKey(KeyCode.T))
            {
                if (_actionProcessed) return;

                _actionProcessed = true;

                // test a job queue
                ModelImport.Job job = new ModelImport.Job
                {
                    mode = "Elevated",
                    isNode = false,
                    meshid = 0,
                    name = "12DR_ELV"
                };
                ModelImport.worker.ProduceJob(job);
                job = new ModelImport.Job
                {
                    mode = "Slope",
                    isNode = false,
                    meshid = 0,
                    name = "Slope"
                };
                ModelImport.worker.ProduceJob(job);
                job = new ModelImport.Job
                {
                    mode = "Bridge",
                    isNode = false,
                    meshid = 0,
                    name = "12DR_ELV"
                };
                ModelImport.worker.ProduceJob(job);

                Debug.Log("job added to queue");

            }
            else
            {
                // not both keys pressed: Reset processed state
                _actionProcessed = false;
            }
        }
    }
}
