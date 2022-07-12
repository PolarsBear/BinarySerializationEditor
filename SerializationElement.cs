using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;


namespace BinarySerializationEditor
{
    public class SerializationElement
    {
        public enum OriginType
        {
            Field,
            DictEntry,
            ListItem
        }

        public enum Classification
        {
            Object,
            Dictionary,
            List
        }


        public Dictionary<string, SerializationElement> children;
        public dynamic rawObj;
        public string name;
        public OriginType origin;
        public Classification classification;


        public SerializationElement(string name, dynamic obj, OriginType originType)
        {
            origin = originType;
            rawObj = obj;
            this.name = name;
            if (obj is IEnumerable) // Is enumerable
            {
                if (obj is IDictionary) // Has two values
                {
                    foreach (KeyValuePair<dynamic, dynamic> keyValue in obj) // Iterate as Dictionary
                    {
                        AddChildFromKeyValuePair(keyValue);
                    }
                }
                else // Has only one value
                {
                    int i = 0;
                    foreach (dynamic value in obj) // Iterate as list
                    {
                        CreateListEntryUI(value, i);
                        i++;
                    }
                }
            }
            else // Is object
            {
                foreach (FieldInfo field in obj.GetType().GetFields())
                {
                    AddChildFromField(field);
                }
            }
        }

        public SerializationElement AddChildFromField(FieldInfo field)
        {
            var child = new SerializationElement(field.Name, field.GetValue(rawObj), OriginType.Field);
            children.Add(field.Name, child);
            return child;
        }

        public SerializationElement AddChildFromKeyValuePair(KeyValuePair<object, object> kvp)
        {
            var child = new SerializationElement(Convert.ToString(kvp.Key), kvp.Value, OriginType.DictEntry);
            children.Add(Convert.ToString(kvp.Key), child);
            return child;
        }
        public SerializationElement AddChildFromKeyValuePair(int index, dynamic value)
        {
            var child = new SerializationElement(Convert.ToString(index), value, OriginType.ListItem);
            children.Add(Convert.ToString(index), child);
            return child;
        }
    }
}
