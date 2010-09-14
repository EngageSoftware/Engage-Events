// <copyright file="FeaturesController.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2010
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Events.Components
{
#if TRIAL
    using System;
#endif
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Definitions;
    using DotNetNuke.Security.Permissions;

    /// <summary>
    /// Controls which DNN features are available for this module.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Instantiated through reflection by DNN")]
    internal class FeaturesController : IUpgradeable
    {
        /// <summary>
        /// The permission code for all Engage: Events custom permissions
        /// </summary>
        public const string EngageEventsCustomPermissionsCode = "ENGAGE_EVENTS";

        /// <summary>
        /// The permission key for the custom permission to create categories
        /// </summary>
        public const string CreateCategoryCustomPermissionKey = "CREATE-CATEGORY";


        /// <summary>
        /// The permission key for the custom permission to manage categories
        /// </summary>
        public const string ManageCategoriesCustomPermissionKey = "MANAGE-CATEGORY";

#if TRIAL
        /// <summary>
        /// The license key for this module
        /// </summary>
        public static readonly Guid ModuleLicenseKey = new Guid("2A2C5DE3-8690-4D97-B027-4750409DAC9A");
#endif

        /// <summary>
        /// Performs an action when the module is installed/upgraded, based on the given <paramref name="version"/>.
        /// </summary>
        /// <param name="version">The version to which the module is being upgraded.</param>
        /// <returns>A status message</returns>
        public string UpgradeModule(string version)
        {
            var versionNumber = new Version(version);
            if (versionNumber.Equals(new Version(1, 4, 0)))
            {
                // add custom permissions, based on http://www.codeproject.com/KB/aspnet/dnn_custom_permissions.aspx
                var permissionController = new PermissionController();
                var existingPermissions = permissionController.GetPermissionByCodeAndKey(EngageEventsCustomPermissionsCode, CreateCategoryCustomPermissionKey);
                if (existingPermissions != null && existingPermissions.Cast<PermissionInfo>().Any())
                {
                    return "Engage: Events custom permissions were already created (presumably by DNN 5 manifest), no upgrade action taken";
                }

                var eventsDesktopModules = new DesktopModuleController().GetDesktopModuleByModuleName(Utility.DesktopModuleName);
                var moduleDefinition = new ModuleDefinitionController().GetModuleDefinitionByName(
                    eventsDesktopModules.DesktopModuleID, 
                    Utility.ModuleDefinitionFriendlyName);

                var createCategoryPermission = new PermissionInfo
                    {
                        PermissionCode = EngageEventsCustomPermissionsCode,
                        PermissionKey = CreateCategoryCustomPermissionKey,
                        PermissionName = "Create Category",
                        ModuleDefID = moduleDefinition.ModuleDefID
                    };

                var manageCategoriesPermission = new PermissionInfo
                    {
                        PermissionCode = EngageEventsCustomPermissionsCode,
                        PermissionKey = ManageCategoriesCustomPermissionKey,
                        PermissionName = "Manage Categories",
                        ModuleDefID = moduleDefinition.ModuleDefID
                    };

                permissionController.AddPermission(createCategoryPermission);
                permissionController.AddPermission(manageCategoriesPermission);

                return "Created custom permissions for category management in Engage: Events";
            }

            return "No upgrade action required for version " + version + " of Engage: Events";
        }
    }
}