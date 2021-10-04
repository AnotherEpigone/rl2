using Newtonsoft.Json;
using Roguelike2.Serialization;

namespace Roguelike2Tests.Serialization
{
    public static class SerializationTestHelper
    {
        public static T SerializeDeserialize<T>(T value)
        {
            var settings = new SaveManager().JsonSettings;
            var payload = JsonConvert.SerializeObject(value, settings);
            return JsonConvert.DeserializeObject<T>(payload, settings);
        }
    }
}
