using IoTClient.Enums;
using System;

namespace IoTClient.Tool.Helper
{
    public static class StringExtension
    {
        /// <summary>
        /// 转出对应数据类型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ToDataFormType(this string str, Type type)
        {
            str = str?.Trim();
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return bool.Parse(str);
                case TypeCode.Byte:
                    return byte.Parse(str);
                case TypeCode.Char:
                    return char.Parse(str);
                case TypeCode.DateTime:
                    return DateTime.Parse(str);
                case TypeCode.Decimal:
                    return decimal.Parse(str);
                case TypeCode.Double:
                    return double.Parse(str);
                case TypeCode.Int16:
                    return short.Parse(str);
                case TypeCode.Int32:
                    return int.Parse(str);
                case TypeCode.Int64:
                    return long.Parse(str);
                case TypeCode.SByte:
                    return sbyte.Parse(str);
                case TypeCode.Single:
                    return float.Parse(str);
                case TypeCode.UInt16:
                    return ushort.Parse(str);
                case TypeCode.UInt32:
                    return uint.Parse(str);
                case TypeCode.UInt64:
                    return ulong.Parse(str);
                default: throw new ArgumentException("暂未定义类型");
            }
        }

        /// <summary>
        /// 转出对应数据类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static object ToDataFormType(this string value, DataTypeEnum type)
        {
            object newVlue;
            value = value?.Trim();
            switch (type)
            {
                case DataTypeEnum.Bool:
                    newVlue = value == "1" || value?.ToLower() == "true";
                    break;
                case DataTypeEnum.Byte:
                    newVlue = byte.Parse(value);
                    break;
                case DataTypeEnum.Int16:
                    newVlue = short.Parse(value);
                    break;
                case DataTypeEnum.UInt16:
                    newVlue = ushort.Parse(value);
                    break;
                case DataTypeEnum.Int32:
                    newVlue = int.Parse(value);
                    break;
                case DataTypeEnum.UInt32:
                    newVlue = uint.Parse(value);
                    break;
                case DataTypeEnum.Float:
                    newVlue = float.Parse(value);
                    break;
                case DataTypeEnum.String:
                    newVlue = value;
                    break;
                default: throw new Exception($"暂未定义类型:{type}");
            }
            return newVlue;
        }
    }
}
