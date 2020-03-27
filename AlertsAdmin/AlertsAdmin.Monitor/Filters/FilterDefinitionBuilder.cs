using AlertsAdmin.Monitor.Filters;
using AlertsAdmin.Monitor.Filters.CustomFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AlertsAdmin.Monitor.Filters
{
    public class FilterDefinitionBuilder
    {
        public List<FilterDefinition> Build(CustomFiltersOptions jsonObject)
        {
            var filterDefinitions = new List<FilterDefinition>();

            if (jsonObject == null)
            {
                return filterDefinitions;
            }

            foreach (var df in jsonObject.filters)
            {
                var filter = new FilterDefinition()
                {
                    Name = df.Name,
                    ApplyTo = new List<string>(),
                    Model = FilterUtils.GetModelType(df.Model),
                    Conditions = new List<FilterDefinition.ConditionDefinition>()
                };

                if (df.ApplyTo != null)
                {
                    foreach (var applyName in df.ApplyTo)
                    {
                        filter.ApplyTo.Add(applyName);
                    }
                }

                if (df.Match != null)
                {
                    foreach (var f in df.Match)
                    {
                        switch (f.QueryType.ToLower())
                        {
                            case "text":
                                // equality
                                filter.Conditions.Add(new FilterDefinition.ConditionDefinition()
                                {
                                    Function = (object r) =>
                                    {
                                        var model = Convert.ChangeType(r, filter.Model);
                                        string value = r.GetType().GetProperty(f.Field)?.GetValue(r)?.ToString();

                                        return value != null && value.Equals(f.QueryValue);
                                    }
                                });

                                break;

                            case "regex":
                                // regex
                                filter.Conditions.Add(new FilterDefinition.ConditionDefinition()
                                {
                                    Function = (r) =>
                                    {
                                            // todo implement regex
                                            Regex rx = new Regex(f.QueryValue);

                                        var model = Convert.ChangeType(r, filter.Model);
                                        string value = r.GetType().GetProperty(f.Field)?.GetValue(r)?.ToString();

                                        return value != null && rx.Match(value).Success;
                                    }
                                });

                                break;

                            case "function":
                                // use custom function
                                filter.Conditions.Add(new FilterDefinition.ConditionDefinition()
                                {
                                    Function = (object r) =>
                                    {
                                        Assembly library = null;
                                        object libraryClass = null;
                                        MethodInfo method = null;
                                        Func<Object, bool> fn;

                                        var fnDef = f.QueryValue?.Split('|');
                                        if (fnDef == null)
                                        {
                                            fn = (x) => false;

                                            return fn(r);
                                        }

                                        if (fnDef.Length == 1)
                                        {
                                            libraryClass = new DefaultFilterFunctions();
                                            method = libraryClass.GetType().GetMethod(fnDef[0]);
                                        }
                                        else if (fnDef.Length == 2)
                                        {
                                            library = Assembly.GetExecutingAssembly();
                                            var classType = library.GetTypes().FirstOrDefault(x => x.Name.Equals(fnDef[0]));
                                            libraryClass = Activator.CreateInstance(classType);
                                            method = libraryClass.GetType().GetMethod(fnDef[1]);
                                        }
                                        else if (fnDef.Length == 3)
                                        {
                                            library = Assembly.LoadFrom(fnDef[0]);
                                            var classType = library.GetTypes().FirstOrDefault(x => x.Name.Equals(fnDef[1]));
                                            libraryClass = Activator.CreateInstance(classType);
                                            method = libraryClass.GetType().GetMethod(fnDef[2]);
                                        }

                                        fn = (x) => (bool)method.Invoke(libraryClass, new object[] { x });

                                        var model = Convert.ChangeType(r, filter.Model);

                                        return fn(model);
                                    }
                                });

                                break;

                            default:
                                break;
                        }
                    }
                }

                filterDefinitions.Add(filter);
            }

            return filterDefinitions;
        }
    }
}