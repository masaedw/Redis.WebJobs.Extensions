﻿using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Config;
using Redis.WebJobs.Extensions.Config;

namespace Redis.WebJobs.Extensions
{
    public static class JobHostConfigurationExtensions
    {
        public static void UseRedis(this JobHostConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            config.UseRedis(new RedisConfiguration());
        }

        public static void UseRedis(this JobHostConfiguration config, TimeSpan checkCacheFrequency)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            var redisConfig = new RedisConfiguration();
            redisConfig.CheckCacheFrequency = checkCacheFrequency;

            config.UseRedis(redisConfig);
        }

        public static void UseRedis(this JobHostConfiguration config, RedisConfiguration redisConfig)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            if (redisConfig == null)
            {
                throw new ArgumentNullException("redisConfig");
            }

            config.RegisterExtensionConfigProvider(new RedisExtensionConfig(config, redisConfig));
        }
    }
}
