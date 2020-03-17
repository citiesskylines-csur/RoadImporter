using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoadImporter
{
    public interface IAssetInfo
    {
        void ReadFromGame(NetInfo gameNetInfo);
        void WriteToGame(NetInfo gameNetInfo);
    }
}
