using System;
using System.Reflection;
using System.Linq;
using System.ComponentModel;
using AlertsAdmin.Domain.Attributes;

namespace AlertsAdmin.Domain.Extensions
{
    public static class EnumExtensions
    {
        public static bool TryGetQueueName(this Enum GenericEnum, out string QueueName)
        {
            QueueName = GetQueueName(GenericEnum);
            if (string.IsNullOrEmpty(QueueName))
                return false;
            return true;
        }

        public static string GetQueueName(this Enum GenericEnum)
        {
            CheckIfQueueTable(GenericEnum);
            if (TryGetAttribute<QueueName>(GenericEnum, out var attribute))
                return attribute.Name;
            return "";
        }

        public static bool TryGetQuery(this Enum GenericEnum, out string Query)
        {
            Query = GetQuery(GenericEnum);
            if (string.IsNullOrEmpty(Query))
                return false;
            return true;
        }

        public static string GetQuery(this Enum GenericEnum)
        {
            CheckIfQueueTable(GenericEnum);
            if (TryGetAttribute<QueryAttribute>(GenericEnum, out var attribute))
                return attribute.Query;
            return "";
        }

        public static bool IsQueueTable<T>()
        {
            Type genericType = typeof(T);
            if (genericType.IsDefined(typeof(QueueTableAttribute)))
                return true;
            return false;
        }

        public static bool TryGetResource(this Enum GenericEnum, out string Resource)
        {
            Resource = GetResource(GenericEnum);
            if (string.IsNullOrEmpty(Resource))
                return false;
            return true;
        }

        public static string GetResource(this Enum GenericEnum)
        {
            CheckIfQueueTable(GenericEnum);
            if (TryGetAttribute<QueueResourceAttribute>(GenericEnum, out var attribute))
                return attribute.Resource;
            return "";
        }


        #region PrivateMethods

        private static void CheckIfQueueTable(Enum GenericEnum)
        {
            if (!IsQueueTable(GenericEnum))
                throw new ArgumentException($"{GenericEnum.GetType().ToString()} does not have the QueueTableAttribute");
        }

        private static bool IsQueueTable(Enum GenericEnum)
        {
            Type genericEnumType = GenericEnum.GetType();
            if (genericEnumType.IsDefined(typeof(QueueTableAttribute)))
                return true;
            return false;
        }

        private static bool TryGetAttribute<T>(Enum GenericEnum,out T Attribute)
        {
            Type genericEnumType = GenericEnum.GetType();

            MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(T), false);
                if ((_Attribs != null && _Attribs.Count() > 0))
                {
                    Attribute = ((T)_Attribs.ElementAt(0));
                    return true;
                }
            }
            Attribute = default(T);
            return false;
        }

        #endregion
    }
}