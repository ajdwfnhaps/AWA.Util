using CSRedis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWA.Util.CSRedis
{
    public class CSRedis : ICSRedisManager
    {
        private readonly CSRedisClient _instance;

        #region Utilities

        protected virtual byte[] Serialize(object item)
        {
            var jsonString = JsonConvert.SerializeObject(item, this.jsonSerializerSettings());
            return Encoding.UTF8.GetBytes(jsonString);
        }
        protected virtual T Deserialize<T>(byte[] serializedObject)
        {
            if (serializedObject == null)
                return default(T);

            var jsonString = Encoding.UTF8.GetString(serializedObject);
            return JsonConvert.DeserializeObject<T>(jsonString, this.jsonSerializerSettings());
        }

        private Func<JsonSerializerSettings> jsonSerializerSettings = () =>
        {
            var st = new JsonSerializerSettings();
            st.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            st.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            st.DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
            return st;
        };


        #endregion

        public CSRedis(CSRedisClient cSRedisClient)
        {
            _instance = cSRedisClient;
        }

        [Obsolete("NotImplemented")]
        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            //Nothing to do
            //throw new NotImplementedException();
        }

        public T Get<T>(string key) => _instance.Get<T>(key);


        public bool KeyExists(string key) => _instance.Exists(key);

        public bool KeyExpire(string key, TimeSpan expiry) => _instance.Expire(key, expiry);

        public bool KeyRename(string key, string newKey) => _instance.Rename(key, newKey);

        public void Remove(string key) => _instance.Del(key);

        public bool Set<T>(string key, T obj, TimeSpan? expiry = null)
        {
            var entryBytes = Serialize(obj);

            var expireSeconds = -1;
            if (expiry.HasValue)
            {
                expireSeconds = (int)expiry.Value.TotalSeconds;
            }
            return _instance.Set(key, entryBytes, expireSeconds);
        }

        private const int CacheShellTimeoutSeconds = 600;

        public T TryGet<T>(string key, Func<T> createFn = null, TimeSpan? expiry = null) => _instance.CacheShell(key, (expiry.HasValue ? (int)expiry.Value.TotalSeconds : CacheShellTimeoutSeconds), createFn);

        public Task<T> TryGetAsync<T>(string key, Func<Task<T>> createFn = null, TimeSpan? expiry = null) => _instance.CacheShellAsync(key, (expiry.HasValue ? (int)expiry.Value.TotalSeconds : CacheShellTimeoutSeconds), createFn);



        /// <summary>
        /// 获取多个指定 key 的值(数组)
        /// </summary>
        /// <param name="keys">不含prefix前辍</param>
        /// <returns></returns>
        public string[] MGet(params string[] keys) => _instance.MGet(keys);
        /// <summary>
        /// 获取多个指定 key 的值(数组)
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="keys">不含prefix前辍</param>
        /// <returns></returns>
        public T[] MGet<T>(params string[] keys) => _instance.MGet<T>(keys);
        /// <summary>
        /// 同时设置一个或多个 key-value 对
        /// </summary>
        /// <param name="keyValues">key1 value1 [key2 value2]</param>
        /// <returns></returns>
        public bool MSet(params object[] keyValues) => _instance.MSet(keyValues);
    }
}
