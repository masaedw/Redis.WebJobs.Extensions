using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs.Host.Triggers;
using Redis.WebJobs.Extensions.Bindings;
using Redis.WebJobs.Extensions.Triggers;

namespace Redis.WebJobs.Extensions.Config
{
    internal class RedisExtensionConfig : IExtensionConfigProvider
    {
        private readonly RedisConfiguration _redisConfig;
        private readonly INameResolver _nameResolver;

        public RedisExtensionConfig(JobHostConfiguration config, RedisConfiguration redisConfig)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            if (redisConfig == null)
            {
                throw new ArgumentNullException("redisConfig");
            }

            _nameResolver = config.NameResolver;
            _redisConfig = redisConfig;
        }

        public RedisConfiguration Config { get { return _redisConfig; } }

        /// <inheritdoc />
        public void Initialize(ExtensionConfigContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            context.Config.RegisterBindingExtensions(
                new RedisTriggerAttributeBindingProvider(_redisConfig, _nameResolver, context.Trace),
                new RedisAttributeBindingProvider(_redisConfig, context.Trace));
        }
    }
}
