using System.Runtime.Serialization;

namespace AppStorageService.Core.Test
{
    [DataContract]
    public class TestModel
    {
        [DataMember]
        public int IntProperty { get; set; }
        [DataMember]
        public string StringProperty { get; set; }

        public override bool Equals(object obj)
        {
            var objAsCurrent = obj as TestModel;
            if (objAsCurrent == null) return false;

            if (IntProperty != objAsCurrent.IntProperty) return false;
            if (StringProperty != objAsCurrent.StringProperty) return false;

            return true;
        }

        public override int GetHashCode()
        {
            var output = 13;
            output = 7 * output + IntProperty.GetHashCode();
            output = 7 * output + StringProperty.GetHashCode();
            return output;
        }

        public static TestModel Generate()
        {
            return new TestModel
            {
                IntProperty = 4,
                StringProperty = "TestProp"
            };
        }
    }
}
