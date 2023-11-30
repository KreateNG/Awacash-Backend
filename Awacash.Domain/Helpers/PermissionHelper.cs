using System;
using Awacash.Domain.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Awacash.Domain.Enums;
using System.Globalization;

namespace Awacash.Domain.Helpers
{
    public class PermissionHelper
    {
        public static List<PermissionDto> GetPermissionsToDisplay(Type enumType)
        {
            var result = new List<PermissionDto>();
            foreach (var permissionName in Enum.GetNames(enumType))
            {
                var member = enumType.GetMember(permissionName);
                //This allows you to obsolete a permission and it won't be shown as a possible option, but is still there so you won't reuse the number
                var obsoleteAttribute = member[0].GetCustomAttribute<ObsoleteAttribute>();
                if (obsoleteAttribute != null)
                    continue;
                //If there is no DisplayAttribute then the Enum is not used
                var displayAttribute = member[0].GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute == null)
                    continue;


                var permission = (Pemission)Enum.Parse(enumType, permissionName, false);

                result.Add(new PermissionDto(displayAttribute.GroupName, displayAttribute.Name,
                        displayAttribute.Description, permission));
            }

            return result;
        }
    }

    public static class PermissionPackers
    {
        public const char PackType = 'H';
        public const int PackedSize = 4;

        public static string FormDefaultPackPrefix()
        {
            return $"{PackType}{PackedSize:D1}-";
        }

        public static string PackPermissionsIntoString(this IEnumerable<Pemission> permissions)
        {
            return permissions.Aggregate(FormDefaultPackPrefix(), (s, permission) => s + ((int)permission).ToString("X4"));
        }

        public static IEnumerable<int> UnpackPermissionValuesFromString(this string packedPermissions)
        {
            var packPrefix = FormDefaultPackPrefix();
            if (packedPermissions == null)
                throw new ArgumentNullException(nameof(packedPermissions));
            if (!packedPermissions.StartsWith(packPrefix))
            {
                throw new InvalidOperationException($"The format of the packed permissions is wrong - should start with {packPrefix}");
            }

            int index = packPrefix.Length;
            while (index < packedPermissions.Length)
            {
                yield return int.Parse(packedPermissions.Substring(index, PackedSize), NumberStyles.HexNumber);
                index += PackedSize;
            }
        }

        public static IEnumerable<Pemission> UnpackPermissionsFromString(this string packedPermissions)
        {
            return packedPermissions.UnpackPermissionValuesFromString().Select(x => ((Pemission)x));
        }

        public static Pemission? FindPermissionViaName(this string permissionName)
        {
            return Enum.TryParse(permissionName, out Pemission permission)
                ? (Pemission?)permission
                : null;
        }

    }
}

