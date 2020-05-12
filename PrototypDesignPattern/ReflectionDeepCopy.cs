using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace PrototypeDesignPattern
{
    public static class ReflectionDeepCopy
    {
        public static object CloneObject(object objSource)
        {
            //Step : 1 Get the type of source object and create a new instance of that type
            Type typeSource = objSource.GetType();
            object objTarget = Activator.CreateInstance(typeSource);

            //Step : 2 Get all the properties of source object type
            PropertyInfo[] propertyInfo = typeSource.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(p => p.GetIndexParameters().Length == 0).ToArray();

            //Step : 3 Assign all source property to taget object 's properties
            foreach (PropertyInfo property in propertyInfo)
            {
                //Check whether property can be written to
                if (property.CanWrite)
                {
                    //Step : 4 check whether property type is value type, enum or string type
                    if (property.PropertyType.IsValueType || property.PropertyType.IsEnum || property.PropertyType.Equals(typeof(System.String)))
                    {
                        property.SetValue(objTarget, property.GetValue(objSource, null), null);
                    }

                    else if (property.PropertyType.IsArray)
                    {
                        property.SetValue(objTarget, ((IEnumerable)property.GetValue(objSource, null)).Cast<object>().ToArray(), null);
                    }

                    //else property type is object/complex types, so need to recursively call this method until the end of the tree is reached
                    else
                    {
                        object objPropertyValue = property.GetValue(objSource, null);

                        if (objPropertyValue == null)
                        {
                            property.SetValue(objTarget, null, null);
                        }
                        else
                        {
                            property.SetValue(objTarget, CloneObject(objPropertyValue), null);
                        }
                    }
                }
            }

            //Step : 5 Get all the fields of source object type
            FieldInfo[] fieldInfo = typeSource.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            //Step : 6 Assign all source field to taget object 's fields
            foreach (FieldInfo field in fieldInfo)
            {
                //Step : 7 check whether field type is value type, enum or string type
                if (field.FieldType.IsValueType || field.FieldType.IsEnum || field.FieldType.Equals(typeof(System.String)))
                {
                    field.SetValue(objTarget, field.GetValue(objSource));
                }

                else if (field.FieldType.IsArray)
                {
                    field.SetValue(objTarget, ((IEnumerable)field.GetValue(objSource)));
                }

                //else field type is object/complex types, so need to recursively call this method until the end of the tree is reached
                //else if (field.FieldType is IEnumerable)
                //{
                //    object objFieldValue = field.GetValue(objSource);

                //    if (objFieldValue == null)
                //    {
                //        field.SetValue(objTarget, null);
                //    }
                //    else
                //    {
                //        field.SetValue(objTarget, CloneObject(objFieldValue));
                //    }
                //}
            }

            return objTarget;
        }
    }
}
