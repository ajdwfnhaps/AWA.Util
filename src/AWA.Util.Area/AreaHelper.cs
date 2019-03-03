using System.Collections.Generic;
using System.Linq;

namespace AWA.Util.Area
{
    /// <summary>
    /// 区域帮助类
    /// </summary>
    public static class AreaHelper
    {
        public static SingletonArea AreaInstance
        {
            get { return SingletonArea.Instance; }
        }

        public static Province GetProvince(string provinceCode)
        {
            var pro = AreaInstance.Provinces.Find(p => p.provinceID == provinceCode);
            if (pro == null) pro = new Province();
            return pro;
        }

        public static Province GetProvinceByName(string provinceName)
        {
            var pro = AreaInstance.Provinces.Find(p => p.province == provinceName);
            if (pro == null) pro = new Province();
            return pro;
        }


        public static City GetCity(string cityCode)
        {
            //try
            //{
            var city = AreaInstance.Citys.Find(c => c.cityID == cityCode);
            if (city == null) city = new City();
            return city;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(SingletonArea.Instance.Citys.Count.ToString() + "------" + ex.Message);
            //}
        }

        public static City GetCityByName(string cityName)
        {
            var city = AreaInstance.Citys.Find(c => c.city == cityName);
            if (city == null) city = new City();
            return city;
        }

        public static List<City> GetCityList(string provinceCode)
        {
            return AreaInstance.Citys.Where(c => c.father == provinceCode).ToList();
        }

        public static Area GetArea(string areaCode)
        {
            var area = AreaInstance.Areas.Find(p => p.areaID == areaCode);
            if (area == null) area = new Area();
            return area;
        }


        public static List<Area> GetAreaList(string cityCode)
        {
            return AreaInstance.Areas.Where(a => a.father == cityCode).ToList();
        }

        /// <summary>
        /// 根据省、市、区代码获取对应中文
        /// </summary>
        /// <param name="provinceCode">省份</param>
        /// <param name="cityCode">城市</param>
        /// <param name="areaCode">县区</param>
        /// <param name="separator">分隔符</param>
        /// <remarks>2018-8-16 11:30:29 add by alenliao</remarks>
        /// <returns></returns>
        public static string GetFullArea(string provinceCode, string cityCode,string areaCode,string separator = " ")
        {
            var areaArr = new List<string>()
            {
                GetProvince(provinceCode).province,
                GetCity(cityCode).city,
                GetArea(areaCode).area
            };
            return string.Join(separator, areaArr);
        }


        ///// <summary>
        ///// 转换成IEnumerable的ProvinceDto列表数据
        ///// 提供外部接口使用
        ///// </summary>
        ///// <returns></returns>
        //public static IEnumerable<ProvinceDto> ToAreaData()
        //{
        //    var res = new List<ProvinceDto>();

        //    AreaInstance.Provinces.ForEach(p =>
        //    {
        //        var province = new ProvinceDto() { ProvinceCode = p.provinceID, ProvinceName = p.province };

        //        var citys = GetCityList(p.provinceID);

        //        citys.ForEach(c =>
        //        {
        //            var _areas = new List<AreaDto>();

        //            GetAreaList(c.cityID).ForEach(a => { _areas.Add(new AreaDto { AreaCode = a.areaID, AreaName = a.area }); });

        //            province.Citys.Add(new CityDto() { CityCode = c.cityID, CityName = c.city, Areas = _areas });
        //        });

        //        res.Add(province);
        //    });

        //    return res;
        //}
    }
}
