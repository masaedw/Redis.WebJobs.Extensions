﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Redis.WebJobs.Extensions.Framework;

namespace Redis.WebJobs.Extensions.Bindings
{
    internal class RedisValueBinder<TInput> : JsonStringValueBinder<TInput>
    {
        private readonly RedisEntity _entity;

        public RedisValueBinder(RedisEntity entity) 
            : base(BindStepOrder.Enqueue)
        {
            _entity = entity;
        }

        public override async Task<object> GetValueAsync()
        {
            string value = await _entity.GetAsync();

            if (value == null)
            {
                return default(TInput);
            }

            TInput contents;
            if (TryJsonConvert(value, out contents))
            {
                return contents;
            }
            else
            {
                return value;
            }
        }

        public override string ToInvokeString()
        {
            return _entity.ChannelOrKey;
        }

        public override Task SetValueAsync(object value, CancellationToken cancellationToken)
        {
            string message;
            if (typeof (TInput) == typeof (string))
            {
                message = value.ToString();
            }
            else
            {
                // Pass data to RedisEntity to bind the channel or key path
                _entity.SetBindingData(value.GetType(), value);

                message = ConvertToJson((TInput) value);
            }

            if (_entity.Mode == Mode.PubSub)
            {
                return _entity.SendAsync(message);
            }
            else
            {
                return _entity.SetAsync(message);
            }
        }
        
    }
}
