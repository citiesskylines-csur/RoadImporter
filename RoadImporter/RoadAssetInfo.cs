using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoadImporter
{
    public class RoadAssetInfo
    {
        public class RoadAIProperties
        {
            public bool m_trafficLights;
            public bool m_highwayRules;
            public bool m_accumulateSnow = true;
            public int m_noiseAccumulation = 10;
            public float m_noiseRadius = 40f;
            public float m_centerAreaWidth;
            public int m_constructionCost = 1000;
            public int m_maintenanceCost = 2;
        }

        public class BridgeAIProperties : RoadAIProperties
        {
            public string m_bridgePillarInfo;
            public string m_middlePillarInfo;
            public int m_elevationCost = 2000;
            public float m_bridgePillarOffset;
            public float m_middlePillarOffset;
            public bool m_doubleLength;
            public bool m_canModify = true;
        }

        public class TunnelAIProperties : RoadAIProperties
        {
            public bool m_canModify = true;
        }


        public CSNetInfo basic = new CSNetInfo();
        public CSNetInfo elevated = new CSNetInfo();
        public CSNetInfo bridge = new CSNetInfo();
        public CSNetInfo slope = new CSNetInfo();
        public CSNetInfo tunnel = new CSNetInfo();

        public RoadAIProperties basicAI = new RoadAIProperties();
        public BridgeAIProperties elevatedAI = new BridgeAIProperties();
        public BridgeAIProperties bridgeAI = new BridgeAIProperties();
        public TunnelAIProperties slopeAI = new TunnelAIProperties();
        public TunnelAIProperties tunnelAI = new TunnelAIProperties();

        public NetModelInfo basicModel;
        public NetModelInfo elevatedModel;
        public NetModelInfo bridgeModel;
        public NetModelInfo slopeModel;
        public NetModelInfo tunnelModel;
         
        public string name;

        public void ReadFromGame(NetInfo gameNetInfo)
        {
            this.name = gameNetInfo.name;
            Utils.CopyFromGame(gameNetInfo, this.basic);
            RoadAI gameRoadAI = (RoadAI)gameNetInfo.m_netAI;

            Utils.CopyFromGame(gameRoadAI.m_elevatedInfo, this.elevated);
            Utils.CopyFromGame(gameRoadAI.m_bridgeInfo, this.bridge);
            Utils.CopyFromGame(gameRoadAI.m_slopeInfo, this.slope);
            Utils.CopyFromGame(gameRoadAI.m_tunnelInfo, this.tunnel);

            Utils.CopyFromGame(gameRoadAI, this.basicAI);
            Utils.CopyFromGame(gameRoadAI.m_elevatedInfo.m_netAI, this.elevatedAI);
            Utils.CopyFromGame(gameRoadAI.m_bridgeInfo.m_netAI, this.bridgeAI);
            Utils.CopyFromGame(gameRoadAI.m_slopeInfo.m_netAI, this.slopeAI);
            Utils.CopyFromGame(gameRoadAI.m_tunnelInfo.m_netAI, this.tunnelAI);

            basicModel.Read(gameNetInfo, "Basic");
            elevatedModel.Read(gameRoadAI.m_elevatedInfo, "Elevated");
            bridgeModel.Read(gameRoadAI.m_bridgeInfo, "Bridge");
            slopeModel.Read(gameRoadAI.m_slopeInfo, "Slope");
            tunnelModel.Read(gameRoadAI.m_tunnelInfo, "Tunnel");

        }


        public void WriteToGame(NetInfo gameNetInfo)
        {
            gameNetInfo.name = this.name;
            Utils.CopyToGame(this.basic, gameNetInfo);
            RoadAI gameRoadAI = (RoadAI)gameNetInfo.m_netAI;


            Utils.CopyToGame(this.elevated, gameRoadAI.m_elevatedInfo);
            Utils.CopyToGame(this.bridge, gameRoadAI.m_bridgeInfo);
            Utils.CopyToGame(this.slope, gameRoadAI.m_slopeInfo);
            Utils.CopyToGame(this.tunnel, gameRoadAI.m_tunnelInfo);

            Utils.CopyToGame(this.basicAI, gameRoadAI);
            Utils.CopyToGame(this.elevatedAI, gameRoadAI.m_elevatedInfo.m_netAI);
            Utils.CopyToGame(this.bridgeAI, gameRoadAI.m_bridgeInfo.m_netAI);
            Utils.CopyToGame(this.slopeAI, gameRoadAI.m_slopeInfo.m_netAI);
            Utils.CopyToGame(this.tunnelAI, gameRoadAI.m_tunnelInfo.m_netAI);

            Utils.RefreshRoadEditor();
            basicModel.Apply(gameNetInfo, "Basic");
            elevatedModel.Apply(gameRoadAI.m_elevatedInfo, "Elevated");
            bridgeModel.Apply(gameRoadAI.m_bridgeInfo, "Bridge");
            slopeModel.Apply(gameRoadAI.m_slopeInfo, "Slope");
            tunnelModel.Apply(gameRoadAI.m_tunnelInfo, "Tunnel");

        }
    }
}
