using ColossalFramework.UI;
using ICities;
using UnityEngine;
using System;

namespace RoadImporter
{
    
    public class ModelImport
    {
        public class Job
        {
            public string mode = "Basic";
            public bool isNode;
            public int meshid;
            public string name;
            public NetInfo target;
            public Shader shader;
            public Color color;
            public string texture;
            public Material material = null;
            public Material lodMaterial = null;
        }

        public static RoadImporterThreading worker;
        
        public static UIComponent TogglePanel(Job job)
        {
            Debug.Log("call TogglePanel");
            UIView.Find("RoadEditorMainPanel").Find(job.mode).SimulateClick();
            var panel = UIView.Find($"{job.mode}Panel").components[0];
            Debug.Log(panel);
            int partid = job.isNode ? 3 : 2;
            var togglePartButton = panel.components[partid].components[0];
            togglePartButton.SimulateClick();
            return panel.components[partid];
        }

        public static void RunImport(Job job)
        {
            job.material = Environment.FindMaterial(job.texture, job.name);
            if (job.material != null) job.lodMaterial = Environment.GetMaterial(job.texture + "_lod");
            Debug.Log("call RunImport");
            var partListPanel = TogglePanel(job).components[1];
            Debug.Log(partListPanel);
            var modifyPartButton = partListPanel.components[job.meshid].components[0];
            Debug.Log(modifyPartButton);
            modifyPartButton.SimulateClick();
            UIView.Find("RoadEditorSidePanel(Clone)").Find("ImportButton").SimulateClick();
        }

        public static void FinalizeImport(Job job)
        {
            UIView.Find("RoadEditorSidePanel(Clone)").Find("CloseButton").SimulateClick();
            TogglePanel(job);
            
            if (job.isNode)
            {
                if (job.material != null)
                {
                    job.target.m_nodes[job.meshid].m_material = job.material;
                    job.target.m_nodes[job.meshid].m_lodMaterial = job.lodMaterial;
                } else
                {
                    job.target.m_nodes[job.meshid].m_material.shader = job.shader;
                    job.target.m_nodes[job.meshid].m_material.color = job.color;
                    job.target.m_nodes[job.meshid].m_lodMaterial.shader = job.shader;
                    job.target.m_nodes[job.meshid].m_lodMaterial.color = job.color;
                    Environment.CacheMaterial(job.texture, job.target.m_nodes[job.meshid].m_material);
                    Environment.CacheMaterial(job.texture + "_lod", job.target.m_nodes[job.meshid].m_lodMaterial);
                }
                
            } else
            {
                if (job.material != null)
                {
                    job.target.m_segments[job.meshid].m_material = job.material;
                    job.target.m_segments[job.meshid].m_lodMaterial = job.lodMaterial;
                }
                else
                {
                    job.target.m_segments[job.meshid].m_material.shader = job.shader;
                    job.target.m_segments[job.meshid].m_material.color = job.color;
                    job.target.m_segments[job.meshid].m_lodMaterial.shader = job.shader;
                    job.target.m_segments[job.meshid].m_lodMaterial.color = job.color;
                    Environment.CacheMaterial(job.texture, job.target.m_segments[job.meshid].m_material);
                    Environment.CacheMaterial(job.texture + "_lod", job.target.m_segments[job.meshid].m_lodMaterial);
                }
            }
            
        }
    }
}
