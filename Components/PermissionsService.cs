// <copyright file="PermissionsService.cs" company="Engage Software">
// Engage: Events
// Copyright (c) 2004-2011
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Events.Components
{
    using System;
    using System.Linq;

    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Definitions;
    using DotNetNuke.Security.Permissions;

    /// <summary>
    /// Manages custom permissions for the module
    /// </summary>
    public class PermissionsService
    {
        /// <summary>
        /// The permission code for all Engage: Events custom permissions
        /// </summary>
        public const string EngageEventsCustomPermissionsCode = "ENGAGE_EVENTS";

        /// <summary>
        /// The permission key for the custom permission to manage events
        /// </summary>
        public const string ManageEventsCustomPermissionKey = "MANAGE-EVENTS";

        /// <summary>
        /// The permission key for the custom permission to manage categories
        /// </summary>
        public const string ManageCategoriesCustomPermissionKey = "MANAGE-CATEGORIES";

        /// <summary>
        /// The permission key for the custom permission to manage the display (list or calendar mode, templates in list mode)
        /// </summary>
        public const string ManageDisplayCustomPermissionKey = "MANAGE-DISPLAY";

        /// <summary>
        /// The permission key for the custom permission to view event responses
        /// </summary>
        public const string ViewResponsesCustomPermissionKey = "VIEW-RESPONSES";

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionsService"/> class.
        /// </summary>
        /// <param name="moduleConfiguration">The module configuration.</param>
        public PermissionsService(ModuleInfo moduleConfiguration)
        {
            if (moduleConfiguration == null)
            {
                throw new ArgumentNullException("moduleConfiguration");
            }

// Only ModulePermissions getter should be marked Obsolete
#pragma warning disable 612
            this.ModulePermissions = moduleConfiguration.ModulePermissions;
#pragma warning restore 612

            this.TabPermissions = TabPermissionController.GetTabPermissions(moduleConfiguration.TabID, moduleConfiguration.PortalID);
        }

        /// <summary>
        /// Gets a value indicating whether the current user can edit the current module.
        /// </summary>
        /// <remarks>
        /// A user who can edit the module has access to the entire module 
        /// (with the exception of module settings, which are restricted to page editors).</remarks>
        /// <value>
        /// <c>true</c> if the current user is a module editor; otherwise, <c>false</c>.
        /// </value>
        public bool CanEditModule
        {
            get { return ModulePermissionController.HasModulePermission(this.ModulePermissions, "EDIT"); }
        }

        /// <summary>
        /// Gets a value indicating whether the current user can manage events in the current module.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current user can manage events; otherwise, <c>false</c>.
        /// </value>
        public bool CanManageEvents
        {
            get { return this.CanEditModule || ModulePermissionController.HasModulePermission(this.ModulePermissions, ManageEventsCustomPermissionKey); }
        }

        /// <summary>
        /// Gets a value indicating whether the current user can manage categories (rename and delete) in the current module.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current user can manage categories; otherwise, <c>false</c>.
        /// </value>
        public bool CanManageCategories
        {
            get { return this.CanEditModule || ModulePermissionController.HasModulePermission(this.ModulePermissions, ManageCategoriesCustomPermissionKey); }
        }

        /// <summary>
        /// Gets a value indicating whether the current user can manage the display of the current module.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current user can manage the module display; otherwise, <c>false</c>.
        /// </value>
        public bool CanManageDisplay
        {
            get { return this.CanEditModule || ModulePermissionController.HasModulePermission(this.ModulePermissions, ManageDisplayCustomPermissionKey); }
        }

        /// <summary>
        /// Gets a value indicating whether the current user can view responses in the current module.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current user can view responses; otherwise, <c>false</c>.
        /// </value>
        public bool CanViewResponses
        {
            get { return this.CanEditModule || ModulePermissionController.HasModulePermission(this.ModulePermissions, ViewResponsesCustomPermissionKey); }
        }

        /// <summary>
        /// Gets a value indicating whether the current user can access the built-in module settings for the given module.
        /// </summary>
        /// <remarks>Module settings access is needed to configure some module settings, as well as configuring module permissions</remarks>
        /// <value>
        /// <c>true</c> if the current user can access module settings; otherwise, <c>false</c>.
        /// </value>
        public bool CanAccessModuleSettings
        {
            get { return TabPermissionController.HasTabPermission(this.TabPermissions, "EDIT"); }
        }

        /// <summary>
        /// Gets a value indicating whether the current user can view responses in the current module.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current user can view responses; otherwise, <c>false</c>.
        /// </value>
        public bool HasAnyPermission
        {
            get
            {
                return this.CanEditModule 
                    || this.CanManageEvents 
                    || this.CanEditModule 
                    || this.CanManageCategories 
                    || this.CanManageDisplay 
                    || this.CanViewResponses;
            }
        }

        /// <summary>
        /// Gets or sets the set of module permissions to perform checks against.
        /// </summary>
        /// <value>The module permissions.</value>
        private ModulePermissionCollection ModulePermissions { get; set; }

        /// <summary>
        /// Gets or sets the set of tab permissions to perform checks against.
        /// </summary>
        /// <value>The tab permissions.</value>
        private TabPermissionCollection TabPermissions { get; set; }

        /// <summary>
        /// Creates the custom permissions for the module.  Expected to be called once from <see cref="FeaturesController.UpgradeModule"/>
        /// </summary>
        /// <remarks>
        /// based on http://www.codeproject.com/KB/aspnet/dnn_custom_permissions.aspx
        /// </remarks>
        /// <returns>A status message for <see cref="IUpgradeable.UpgradeModule"/></returns>
        public static string CreateCustomPermissions()
        {
            var permissionController = new PermissionController();
            var existingPermissions = permissionController.GetPermissionByCodeAndKey(EngageEventsCustomPermissionsCode, ManageEventsCustomPermissionKey);
            if (existingPermissions != null && existingPermissions.Cast<PermissionInfo>().Any())
            {
                return "Engage: Events custom permissions were already created (presumably by DNN 5 manifest), no upgrade action taken";
            }

            var eventsDesktopModules = new DesktopModuleController().GetDesktopModuleByModuleName(Utility.DesktopModuleName);
            var moduleDefinition = new ModuleDefinitionController().GetModuleDefinitionByName(
                eventsDesktopModules.DesktopModuleID, 
                Utility.ModuleDefinitionFriendlyName);

            permissionController.AddPermission(new PermissionInfo
                {
                    PermissionCode = EngageEventsCustomPermissionsCode,
                    PermissionKey = ManageEventsCustomPermissionKey,
                    PermissionName = "Manage Events",
                    ModuleDefID = moduleDefinition.ModuleDefID
                });
            permissionController.AddPermission(new PermissionInfo
                {
                    PermissionCode = EngageEventsCustomPermissionsCode,
                    PermissionKey = ManageCategoriesCustomPermissionKey,
                    PermissionName = "Manage Categories",
                    ModuleDefID = moduleDefinition.ModuleDefID
                });
            permissionController.AddPermission(new PermissionInfo
                {
                    PermissionCode = EngageEventsCustomPermissionsCode,
                    PermissionKey = ManageDisplayCustomPermissionKey,
                    PermissionName = "Manage Display",
                    ModuleDefID = moduleDefinition.ModuleDefID
                });
            permissionController.AddPermission(new PermissionInfo
                {
                    PermissionCode = EngageEventsCustomPermissionsCode,
                    PermissionKey = ViewResponsesCustomPermissionKey,
                    PermissionName = "View Responses",
                    ModuleDefID = moduleDefinition.ModuleDefID
                });

            return "Created custom permissions for category management in Engage: Events";
        }
    }
}