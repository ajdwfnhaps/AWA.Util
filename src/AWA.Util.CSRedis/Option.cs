namespace AWA.Util.CSRedis
{
    public class Option
    {
        /// <summary>
        /// 链接串
        /// </summary>
        public string ConnectionString { get; set; }
    }

    public class SentinelOption : Option
    {
        /// <summary>
        /// 哨兵节点，如：ip1:26379、ip2:26379
        /// </summary>
        public string[] Sentinels { get; set; }
    }
}
