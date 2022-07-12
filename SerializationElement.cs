using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;


namespace BinarySerializationEditor
{
    public class SerializationElement // Organized way to keep track of elements
    {
        public enum OriginType // If represented thing is a field, dictionary entry or list item
        {
            Field,
            DictEntry,
            ListItem
        }

        public enum Classification // If represented thing is an object, dictionary, list or primitive type (string, int, float, etc...)
        {
            Object, 
            Dictionary,
            List,
            Primitive
        }


        public Dictionary<string, SerializationElement> children; // All children, depending on classification, will be created in different ways

        public dynamic value; // Represented object in raw form

        public string name; // Key in parent's children dictionary

        public OriginType origin; // Origin of element
        public Classification classification; // Classification of element


        public SerializationElement(string name, dynamic obj, OriginType originType) // Recursive constructor
        {
            // Set argument stuff
            origin = originType;
            value = obj;
            this.name = name;

            AddAllChildren(); // Add Children
        }


        // Child Adders

        public void AddAllChildren()
        {
            children.Clear(); // Remove all children first

            if (value is IEnumerable) // Is enumerable
            {
                if (value is IDictionary) // Has two values
                {
                    classification = Classification.Dictionary;
                    foreach (KeyValuePair<dynamic, dynamic> keyValue in value) // Iterate as Dictionary
                    {
                        AddChildFromKeyValuePair(keyValue);
                    }
                }
                else // Has only one value
                {
                    classification = Classification.List;
                    int i = 0;
                    foreach (object value in value) // Iterate as list
                    {
                        AddChildFromListItem(i, value);
                        i++;
                    }
                }
            }
            else // Is object
            {
                classification = Classification.Object;
                foreach (FieldInfo field in value.GetType().GetFields()) // Iterate through all fields
                {
                    AddChildFromField(field);
                }
            }
        }

        // Individual child adders
        public SerializationElement AddChildFromField(FieldInfo field)
        {
            var child = new SerializationElement(field.Name, field.GetValue(value), OriginType.Field); // Create field child
            children.Add(field.Name, child);
            return child;
        }

        public SerializationElement AddChildFromKeyValuePair(KeyValuePair<object, object> kvp)
        {
            var child = new SerializationElement(Convert.ToString(kvp.Key), kvp.Value, OriginType.DictEntry); // Create dictionary entry child
            children.Add(Convert.ToString(kvp.Key), child);
            return child;
        }
        public SerializationElement AddChildFromListItem(int index, object value)
        {
            var child = new SerializationElement(Convert.ToString(index), value, OriginType.ListItem); // Create list item child
            children.Add(Convert.ToString(index), child);
            return child;
        }
    }
}
