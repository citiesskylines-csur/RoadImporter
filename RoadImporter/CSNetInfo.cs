using System;


namespace RoadImporter
{
    public class CSNetInfo
    {
        [Flags]
        public enum LaneType : byte
        {
            None = 0x0,
            Vehicle = 0x1,
            Pedestrian = 0x2,
            Parking = 0x4,
            PublicTransport = 0x8,
            CargoVehicle = 0x10,
            TransportVehicle = 0x20,
            EvacuationTransport = 0x40,
            Tour = 0x80,
            All = byte.MaxValue
        }

        [Flags]
        public enum Direction : byte
        {
            None = 0x0,
            Forward = 0x1,
            Backward = 0x2,
            Both = 0x3,
            Avoid = 0xC,
            AvoidBackward = 0x7,
            AvoidForward = 0xB,
            AvoidBoth = 0xF
        }

        [Flags]
        public enum ConnectGroup
        {
            None = 0x0,
            NarrowTram = 0x1,
            WideTram = 0x2,
            SingleTram = 0x4,
            CenterTram = 0x8,
            DoubleTrain = 0x10,
            SingleTrain = 0x20,
            TrainStation = 0x40,
            DoubleMonorail = 0x100,
            SingleMonorail = 0x200,
            MonorailStation = 0x400,
            PathPowerLine = 0x10000,
            AllGroups = 0xF0FFF,
            OnewayStart = 0x1000,
            OnewayEnd = 0x2000,
            Oneway = 0x3000,
            Directional = 0x4000
        }

        [Flags]
        public enum VehicleType
        {
            None = 0x0,
            Car = 0x1,
            Metro = 0x2,
            Train = 0x4,
            Bicycle = 0x20,
            Tram = 0x40,
            Monorail = 0x800,
            CableCar = 0x1000,
            All = 0xFFFF
        }

        [Flags]
        public enum LaneFlags
        {
            None = 0x0,
            Created = 0x1,
            Deleted = 0x2,
            Inverted = 0x4,
            JoinedJunction = 0x8,
            JoinedJunctionInverted = 0xC,
            Forward = 0x10,
            Left = 0x20,
            Right = 0x40,
            Merge = 0x80,
            LeftForward = 0x30,
            LeftRight = 0x60,
            ForwardRight = 0x50,
            LeftForwardRight = 0x70,
            Stop = 0x100,
            Stop2 = 0x200,
            Stops = 0x300,
            YieldStart = 0x400,
            YieldEnd = 0x800,
            StartOneWayLeft = 0x1000,
            StartOneWayRight = 0x2000,
            EndOneWayLeft = 0x4000,
            EndOneWayRight = 0x8000,
            StartOneWayLeftInverted = 0x1004,
            StartOneWayRightInverted = 0x2004,
            EndOneWayLeftInverted = 0x4004,
            EndOneWayRightInverted = 0x8004
        }

        [Flags]
        public enum SegmentFlags
        {
            None = 0x0,
            Created = 0x1,
            Deleted = 0x2,
            Original = 0x4,
            Collapsed = 0x8,
            Invert = 0x10,
            Untouchable = 0x20,
            End = 0x40,
            Bend = 0x80,
            WaitingPath = 0x100,
            PathFailed = 0x200,
            PathLength = 0x400,
            AccessFailed = 0x800,
            TrafficStart = 0x1000,
            TrafficEnd = 0x2000,
            CrossingStart = 0x4000,
            CrossingEnd = 0x8000,
            StopRight = 0x10000,
            StopLeft = 0x20000,
            StopRight2 = 0x40000,
            StopLeft2 = 0x80000,
            HeavyBan = 0x100000,
            Blocked = 0x200000,
            Flooded = 0x400000,
            BikeBan = 0x800000,
            CarBan = 0x1000000,
            AsymForward = 0x2000000,
            AsymBackward = 0x4000000,
            CustomName = 0x8000000,
            NameVisible1 = 0x10000000,
            NameVisible2 = 0x20000000,
            YieldStart = 0x40000000,
            YieldEnd = int.MinValue,
            StopBoth = 0x30000,
            StopBoth2 = 0xC0000,
            StopAll = 0xF0000,
            CombustionBan = 0x100,
            All = -1
        }

        [Flags]
        public enum NodeFlags
        {
            None = 0x0,
            Created = 0x1,
            Deleted = 0x2,
            Original = 0x4,
            Disabled = 0x8,
            End = 0x10,
            Middle = 0x20,
            Bend = 0x40,
            Junction = 0x80,
            Moveable = 0x100,
            Untouchable = 0x200,
            Outside = 0x400,
            Temporary = 0x800,
            Double = 0x1000,
            Fixed = 0x2000,
            OnGround = 0x4000,
            Ambiguous = 0x8000,
            Water = 0x10000,
            Sewage = 0x20000,
            ForbidLaneConnection = 0x40000,
            Underground = 0x80000,
            Transition = 0x100000,
            LevelCrossing = 0x200000,
            OneWayOut = 0x400000,
            TrafficLights = 0x800000,
            OneWayIn = 0x1000000,
            Heating = 0x2000000,
            Electricity = 0x4000000,
            Collapsed = 0x8000000,
            DisableOnlyMiddle = 0x10000000,
            AsymForward = 0x20000000,
            AsymBackward = 0x40000000,
            CustomTrafficLights = int.MinValue,
            OneWayOutTrafficLights = 0xC00000,
            UndergroundTransition = 0x180000,
            All = -1
        }

        public enum ColorMode
        {
            Default,
            StartState,
            EndState
        }

        public class Prop
        {

            public LaneFlags m_flagsRequired;
            public LaneFlags m_flagsForbidden;
            public NodeFlags m_startFlagsRequired;
            public NodeFlags m_startFlagsForbidden;
            public NodeFlags m_endFlagsRequired;
            public NodeFlags m_endFlagsForbidden;
            public ColorMode m_colorMode;
            public string m_prop;
            public string m_tree;
            public float[] m_position;
            public float m_angle;
            public float m_segmentOffset;
            public float m_repeatDistance;
            public float m_minLength;
            public float m_cornerAngle;
            public int m_probability = 100;
        }



        public class Lane
        {
            public float m_position;
            public float m_width = 3f;
            public float m_verticalOffset;
            public float m_stopOffset;
            public float m_speedLimit = 1f;
            public Direction m_direction = Direction.Forward;
            public LaneType m_laneType;
            public VehicleType m_vehicleType;
            public VehicleType m_stopType;
            public Prop[] m_laneProps;
            public bool m_allowConnect = true;
            public bool m_useTerrainHeight;
            public bool m_centerPlatform;
            public bool m_elevated;
            public Direction m_finalDirection;
        }


        public class Segment
        {
            public SegmentFlags m_forwardRequired;
            public SegmentFlags m_forwardForbidden;
            public SegmentFlags m_backwardRequired;
            public SegmentFlags m_backwardForbidden;
            public bool m_emptyTransparent;
            public bool m_disableBendNodes;

        }

        public class Node
        {
            public NodeFlags m_flagsRequired;
            public NodeFlags m_flagsForbidden;
            public ConnectGroup m_connectGroup;
            public bool m_directConnect;
            public bool m_emptyTransparent;

        }

        public float m_halfWidth = 8f;
        public float m_pavementWidth = 3f;
        public float m_segmentLength = 64f;
        public float m_minHeight;
        public float m_maxHeight = 5f;
        public float m_maxSlope = 0.25f;
        public float m_maxBuildAngle = 180f;
        public float m_maxTurnAngle = 180f;
        public float m_minCornerOffset;
        public float m_maxCornerOffset;
        public float m_buildHeight;
        public float m_surfaceLevel;
        public float m_terrainStartOffset;
        public float m_terrainEndOffset;
        public bool m_createPavement;
        public bool m_createGravel;
        public bool m_createRuining;
        public bool m_flattenTerrain;
        public bool m_lowerTerrain;
        public bool m_clipTerrain;
        public bool m_followTerrain;
        public bool m_flatJunctions;
        public bool m_clipSegmentEnds;
        public bool m_twistSegmentEnds;
        public bool m_straightSegmentEnds;
        public bool m_enableBendingSegments;
        public bool m_enableBendingNodes;
        public bool m_enableMiddleNodes;
        public bool m_requireContinuous;
        public bool m_canCrossLanes = true;
        public bool m_canCollide = true;
        public bool m_blockWater;
        public bool m_autoRemove;
        public bool m_overlayVisible = true;
        public ConnectGroup m_connectGroup;
        public Vehicle.Flags m_setVehicleFlags;
        public Lane[] m_lanes;
        public Segment[] m_segments;
        public Node[] m_nodes;
        public string m_UICategory;
    }
}
