using EHealth.ManageItemLists.Domain.Shared.Redis;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Infrastructure.Redis
{
    public class CacheService : ICacheService
    {
        private StackExchange.Redis.IDatabase _db;
        private readonly IConfiguration _config;
        private static Lazy<ConnectionMultiplexer> lazyConnection;
        public CacheService(IConfiguration config)
        {
            _config = config;

            var options = ConfigurationOptions.Parse(_config.GetSection("Redis:RedisURL").Value); // host1:port1, host2:port2, ...
            options.Password = _config.GetSection("Redis:RedisPassword").Value;
            options.AbortOnConnectFail = false;

            if (lazyConnection == null)
            {
                lazyConnection = new Lazy<ConnectionMultiplexer>(() => {
                    return ConnectionMultiplexer.Connect(options);
                });
            }

            _db = lazyConnection.Value.GetDatabase();
        }

        public T GetData<T>(string key)
        {
            var value = _db.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return default;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = _db.StringSet(key, JsonConvert.SerializeObject(value), expiryTime);

            return isSet;
        }
        public object RemoveData(string key)
        {
            bool _isKeyExist = _db.KeyExists(key);
            if (_isKeyExist == true)
            {
                return _db.KeyDelete(key);
            }
            return false;
        }
    }
}
