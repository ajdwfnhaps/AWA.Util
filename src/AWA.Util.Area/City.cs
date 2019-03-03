using System;

namespace AWA.Util.Area
{
    public class City
    {
        public int id { get; set; }
        public string cityID { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string father { get; set; }
    }


    public class Area
    {
        public int id { get; set; }
        public string areaID { get; set; } = string.Empty;
        public string area { get; set; } = string.Empty;
        public string father { get; set; }
    }
}
