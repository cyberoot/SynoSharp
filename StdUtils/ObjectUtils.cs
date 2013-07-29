using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StdUtils
{
    public class ObjectUtils
    {
        public static string HumanReadable(object _object, string formatString = "{2}[{0}]: {1}", List<string> objPrefix = null, string objPrefixFormat = "  ")
        {
            var outStrList = new List<string>();
            var keyPrefix = objPrefix ?? new List<string>();
            
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(_object))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(_object);
                if (value != null)
                {
                    var namePrefix = String.Join("", (from obj in keyPrefix select String.Format(objPrefixFormat, obj)).ToArray());
                    string strValue = ValueToString(name, value, formatString, keyPrefix, objPrefixFormat);
                    if (strValue != String.Empty)
                    {
                        outStrList.Add(String.Format(formatString, name, strValue, namePrefix));
                    }
                }
            }
            if (objPrefix != null)
            {
                objPrefix.RemoveAt(objPrefix.Count - 1);
            }
            return String.Join("\n", outStrList);
        }

        public static string ValueToString(string name, object value, string formatString = "{2}[{0}]: {1}", List<string> objPrefix = null, string objPrefixFormat = "\t")
        {
            if (value == null)
            {
                return "null"; 
            }
            if (value is string)
            {
                return string.Format("{0}", value);
            }
            if (value is ValueType)
            {
                return value.ToString();
            }
            if (value is IEnumerable)
            {
                var result = new List<string>();
                foreach (var obj in (IEnumerable)value)
                {
                    result.Add(ValueToString(name, obj, formatString, objPrefix, objPrefixFormat));
                }
                return String.Join("\n", result);
            }
            var newPrefix = objPrefix ?? new List<string>();
            newPrefix.Add(name);
            return String.Format("\n{0}", HumanReadable(value, formatString, newPrefix, objPrefixFormat));
        }
    }
}
