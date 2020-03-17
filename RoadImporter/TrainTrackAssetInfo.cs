using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoadImporter
{
    public class TrainTrackAssetInfo : IAssetInfo
    {
        public class TrainTrackAIProperties
        {
            public int m_constructionCost = 1000;
            public int m_maintenanceCost = 2;
            public int m_noiseAccumulation = 10;
            public float m_noiseRadius = 40f;
            public string m_outsideConnection = null;
        }

        public class TrainBridgeAIProperties : TrainTrackAIProperties
        {
            public string m_bridgePillarInfo;
            public string m_middlePillarInfo;
            public int m_elevationCost = 2000;
            public float m_bridgePillarOffset;
            public float m_middlePillarOffset;
            public bool m_doubleLength;
            public bool m_canModify = true;
        }

        public class TrainTunnelAIProperties : TrainTrackAIProperties
        {
            public bool m_canModify = true;
        }


        public CSNetInfo basic = new CSNetInfo();
        public CSNetInfo elevated = new CSNetInfo();
        public CSNetInfo bridge = new CSNetInfo();
        public CSNetInfo slope = new CSNetInfo();
        public CSNetInfo tunnel = new CSNetInfo();

        public TrainTrackAIProperties basicAI = new TrainTrackAIProperties();
        public TrainBridgeAIProperties elevatedAI = new TrainBridgeAIProperties();
        public TrainBridgeAIProperties bridgeAI = new TrainBridgeAIProperties();
        public TrainTunnelAIProperties slopeAI = new TrainTunnelAIProperties();
        public TrainTunnelAIProperties tunnelAI = new TrainTunnelAIProperties();

        public NetModelInfo basicModel = new NetModelInfo();
        public NetModelInfo elevatedModel = new NetModelInfo();
        public NetModelInfo bridgeModel = new NetModelInfo();
        public NetModelInfo slopeModel = new NetModelInfo();
        public NetModelInfo tunnelModel = new NetModelInfo();

        public string name;



        public void ReadFromGame(NetInfo gameNetInfo)
        {
            this.name = gameNetInfo.name;
            Utils.CopyFromGame(gameNetInfo, this.basic);
            TrainTrackAI gameRoadAI = (TrainTrackAI)gameNetInfo.m_netAI;
            Utils.CopyFromGame(gameRoadAI.m_elevatedInfo, this.elevated);
            Utils.CopyFromGame(gameRoadAI.m_bridgeInfo, this.bridge);
            Utils.CopyFromGame(gameRoadAI.m_slopeInfo, this.slope);
            Utils.CopyFromGame(gameRoadAI.m_tunnelInfo, this.tunnel);

            Utils.CopyFromGame(gameRoadAI, this.basicAI);
            Utils.CopyFromGame(gameRoadAI.m_elevatedInfo?.GetAI(), this.elevatedAI);
            Utils.CopyFromGame(gameRoadAI.m_bridgeInfo?.GetAI(), this.bridgeAI);
            Utils.CopyFromGame(gameRoadAI.m_slopeInfo?.GetAI(), this.slopeAI);
            Utils.CopyFromGame(gameRoadAI.m_tunnelInfo?.GetAI(), this.tunnelAI);

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
            TrainTrackAI gameRoadAI = (TrainTrackAI)gameNetInfo.m_netAI;
            if (gameRoadAI.m_elevatedInfo != null)
            {
                Utils.CopyToGame(this.elevated, gameRoadAI.m_elevatedInfo);
                gameRoadAI.m_elevatedInfo.name = this.name + "_Elevated0";

            }
            if (gameRoadAI.m_bridgeInfo != null)
            {
                Utils.CopyToGame(this.bridge, gameRoadAI.m_bridgeInfo);
                gameRoadAI.m_bridgeInfo.name = this.name + "_Bridge0";
            }
            if (gameRoadAI.m_slopeInfo != null)
            {
                Utils.CopyToGame(this.slope, gameRoadAI.m_slopeInfo);
                gameRoadAI.m_slopeInfo.name = this.name + "_Slope0";
            }
            if (gameRoadAI.m_tunnelInfo != null)
            {
                Utils.CopyToGame(this.tunnel, gameRoadAI.m_tunnelInfo);
                gameRoadAI.m_tunnelInfo.name = this.name + "_Tunnel0";
            }
            Utils.CopyToGame(this.basicAI, gameRoadAI);
            Utils.CopyToGame(this.elevatedAI, gameRoadAI.m_elevatedInfo?.GetAI());
            Utils.CopyToGame(this.bridgeAI, gameRoadAI.m_bridgeInfo?.GetAI());
            Utils.CopyToGame(this.slopeAI, gameRoadAI.m_slopeInfo?.GetAI());
            Utils.CopyToGame(this.tunnelAI, gameRoadAI.m_tunnelInfo?.GetAI());

            Utils.RefreshRoadEditor();
            basicModel?.Apply(gameNetInfo, "Basic");
            elevatedModel?.Apply(gameRoadAI.m_elevatedInfo, "Elevated");
            bridgeModel?.Apply(gameRoadAI.m_bridgeInfo, "Bridge");
            slopeModel?.Apply(gameRoadAI.m_slopeInfo, "Slope");
            tunnelModel?.Apply(gameRoadAI.m_tunnelInfo, "Tunnel");
        }
    }
}
