using BasePoint.Core.Configurations.JsonSerializer;
using BasePoint.Core.Domain.Entities.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BasePoint.Core.Extensions
{
    public static class IBaseEntityEntension
    {
        public static IBaseEntity DeepClone(this IBaseEntity self, params string[] ignoreProperties)
        {
            var serialized = JsonConvert.SerializeObject(
                self,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            var json = JToken.Parse(serialized);

            foreach (var ignoreProperty in ignoreProperties)
            {
                json[ignoreProperty]?.Parent.Remove();
            }

            return (IBaseEntity)JsonConvert.DeserializeObject(
                json.ToString(),
                self.GetType(),
                new JsonSerializerSettings
                {
                    ContractResolver = new PrivateSetterContractResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.Objects
                });
        }
    }
}