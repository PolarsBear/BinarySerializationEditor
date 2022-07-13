﻿using System;
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
            ListItem,
            TopLevel
        }

        public enum Classification // If represented thing is an object, dictionary, list or primitive type (string, int, float, etc...)
        {
            Object, 
            Dictionary,
            List,
            Primitive
        }


        public Dictionary<string, SerializationElement> children = new Dictionary<string, SerializationElement>(); // All children, depending on classification, will be created in different ways

        public SerializationElement parent;
        public bool topLevel;

        public dynamic value; // Represented object in raw form

        public string name; // Key in parent's children dictionary

        public OriginType origin; // Origin of element
        public Classification classification; // Classification of element

        public bool changedValid = false; // If the element's value has been modified and is valid


        public SerializationElement(string name, dynamic obj, OriginType originType, SerializationElement parent) // Recursive constructor
        {
            Console.WriteLine($"---{name}---");

            // Set argument stuff
            origin = originType;
            value = obj;
            this.name = name;
            this.parent = parent;

            // [TODO] Add proper functionality for Enums
            if (value == null || Utils.IsPrimitive(value.GetType()) || value.GetType().IsEnum) // Is primitive
            {
                classification = Classification.Primitive;
                return; // Primitive types have no children
            }

            AddAllChildren(); // Add Children
        }


        // Child Adders

        public void AddAllChildren()
        {
            children.Clear(); // Remove all children first

            if (value is IEnumerable) // Is enumerable
            {
                if (value is IDictionary) // Has two values for each item
                {
                    classification = Classification.Dictionary;
                    foreach (dynamic keyValue in value) // Iterate as Dictionary
                    {
                        AddChildFromKeyValuePair(keyValue.Key, keyValue.Value);
                    }
                }
                else //if (!(value is string)) // Has only one value for each item
                {
                    classification = Classification.List;
                    int iter = 0;
                    foreach (dynamic i in value) // Iterate as list
                    {
                        //Console.WriteLine(i);
                        AddChildFromListItem(iter, i);
                        iter++;
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
            var child = new SerializationElement(field.Name, field.GetValue(value), OriginType.Field, this); // Create field child
            children.Add(field.Name, child);
            return child;
        }

        public SerializationElement AddChildFromKeyValuePair(dynamic key, dynamic value)
        {
            var child = new SerializationElement(Convert.ToString(key), value, OriginType.DictEntry, this); // Create dictionary entry child
            children.Add(Convert.ToString(key), child);
            return child;
        }
        public SerializationElement AddChildFromListItem(int index, dynamic value)
        {
            var child = new SerializationElement(Convert.ToString(index), value, OriginType.ListItem, this); // Create list item child
            children.Add(Convert.ToString(index), child);
            return child;
        }
    }
}
