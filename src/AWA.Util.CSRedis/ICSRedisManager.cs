using AWA.Util.Cache;

namespace AWA.Util.CSRedis
{
    public interface ICSRedisManager : ICacheManager
    {
        /// <summary>
        /// 获取多个指定 key 的值(数组)
        /// </summary>
        /// <param name="keys">不含prefix前辍</param>
        /// <returns></returns>
        string[] MGet(params string[] keys);
        /// <summary>
        /// 获取多个指定 key 的值(数组)
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="keys">不含prefix前辍</param>
        /// <returns></returns>
        T[] MGet<T>(params string[] keys);
        /// <summary>
        /// 同时设置一个或多个 key-value 对
        /// </summary>
        /// <param name="keyValues">key1 value1 [key2 value2]</param>
        /// <returns></returns>
        bool MSet(params object[] keyValues);
    }
}
