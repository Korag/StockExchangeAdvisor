using StateDesignPattern;
using System;
using System.Collections;
using System.Collections.Generic;
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

                    else if (property.PropertyType.Equals(typeof(List<double>)))
                    {
                        property.SetValue(objTarget, ((IEnumerable)property.GetValue(objSource, null)).Cast<double>().ToList(), null);
                    }

                    //else property type is object/complex types, so need to recursively call this method until the end of the tree is reached
                    else
                    {
                        object objPropertyValue = property.GetValue(objSource, null);
                        
                        if (property.PropertyType.FullName == "StateDesignPattern.ASignalState")
                        {
                            //Type stateType = objPropertyValue.GetType();
                            //object stateTarget = Activator.CreateInstance(stateType);

                            //PropertyInfo[] statePropertyInfo = stateType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(p => p.GetIndexParameters().Length == 0).ToArray();

                            //foreach (var stateProperty in statePropertyInfo)
                            //{
                            //    stateProperty.SetValue(stateTarget, stateProperty.GetValue(objPropertyValue, null), null);
                            //}

                            //property.SetValue(objTarget, stateTarget, null);

                            ASignalState stateSource = (ASignalState)objPropertyValue;
                            
                            Type stateType = objPropertyValue.GetType();
                            ASignalState stateTarget = (ASignalState)Activator.CreateInstance(stateType);

                            stateTarget.Context = stateSource.Context;
                            stateTarget.SignalValue = stateSource.SignalValue;

                            property.SetValue(objTarget, stateTarget, null);
                        }

                        else if (objPropertyValue == null)
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

            return objTarget;
        }
    }
}
