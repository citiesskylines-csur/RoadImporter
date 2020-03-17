using ColossalFramework.UI;
using ICities;
using UnityEngine;
using System;

namespace RoadImporter
{
    public class NetModelInfo
    {
        public class CSMesh
        {
            public int index = -1;
            public float[] color = { 1, 1, 1 };
            public string shader = "";
            public string texture = "";
            public string name = "";
        }

        public string mode = "Basic";
        public CSMesh[] segmentMeshes;
        public CSMesh[] nodeMeshes;

        public void Apply(NetInfo gameNet, string _mode)
        {
            this.mode = _mode;
            if (gameNet == null) return;
            if (nodeMeshes != null)
            {
                for (int i = 0; i < nodeMeshes.Length; i++)
                {
                    ModelImport.Job job = new ModelImport.Job
                    {
                        mode = this.mode,
                        isNode = true,
                        meshid = nodeMeshes[i].index > -1 ? nodeMeshes[i].index : i,
                        target = gameNet,
                        name = nodeMeshes[i].name,
                        shader = Shader.Find(nodeMeshes[i].shader),
                        color = new Color(nodeMeshes[i].color[0], nodeMeshes[i].color[1], nodeMeshes[i].color[2], 1),
                        texture = nodeMeshes[i].texture
                    };
                    ModelImport.worker.ProduceJob(job);
                }
            }
            if (segmentMeshes != null)
            {
                for (int i = 0; i < segmentMeshes.Length; i++)
                {
                    ModelImport.Job job = new ModelImport.Job
                    {
                        mode = this.mode,
                        isNode = false,
                        meshid = segmentMeshes[i].index > -1 ? segmentMeshes[i].index : i,
                        target = gameNet,
                        name = segmentMeshes[i].name,
                        shader = Shader.Find(segmentMeshes[i].shader),
                        color = new Color(segmentMeshes[i].color[0], segmentMeshes[i].color[1], segmentMeshes[i].color[2], 1),
                        texture = segmentMeshes[i].texture
                    };
                    ModelImport.worker.ProduceJob(job);
                }
            }

        }

        public void Read(NetInfo gameNet, string _mode)
        {
            this.mode = _mode;
            if (gameNet == null) return;
            nodeMeshes = new CSMesh[gameNet.m_nodes.Length];
            segmentMeshes = new CSMesh[gameNet.m_segments.Length];
            for (int i = 0; i < nodeMeshes.Length; i++)
            {
                nodeMeshes[i] = new CSMesh
                {
                    index = i,
                    name = gameNet.m_nodes[i].m_mesh.name,
                    shader = gameNet.m_nodes[i].m_material.shader.name,
                };
                for (int d = 0; d < 3; d++)
                {
                    nodeMeshes[i].color[d] = gameNet.m_nodes[i].m_material.color[d];
                }   
            }
            for (int i = 0; i < segmentMeshes.Length; i++)
            {
                segmentMeshes[i] = new CSMesh
                {
                    index = i,
                    name = gameNet.m_segments[i].m_mesh.name,
                    shader = gameNet.m_segments[i].m_material.shader.name,
                };
                for (int d = 0; d < 3; d++)
                {
                    segmentMeshes[i].color[d] = gameNet.m_segments[i].m_material.color[d];
                }
            }
        }
    }
}
