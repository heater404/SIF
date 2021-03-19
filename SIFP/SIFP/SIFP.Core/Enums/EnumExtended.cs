using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SIFP.Core.Enums
{
    /// <summary>
    /// SubWorkModeE 枚举对象的拓展方法
    /// </summary>
    public static class EnumExtended
    {
        /// <summary>
        /// 用于获取枚举类型的特性的泛型方法
        /// </summary>
        /// <typeparam name="T">特性类型对象</typeparam>
        /// <param name="e">枚举类型对象</param>
        /// <returns></returns>
        public static T GetTAttribute<T>(this Enum e) where T : Attribute
        {
            Type type = e.GetType();
            FieldInfo field = type.GetField(e.ToString());
            if (field.IsDefined(typeof(T), true))
            {
                T attr = (T)field.GetCustomAttribute(typeof(T));
                return attr;
            }
            return null;
        }
    }
}
