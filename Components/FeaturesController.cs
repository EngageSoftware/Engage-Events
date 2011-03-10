// <copyright file="FeaturesController.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using DotNetNuke.Entities.Modules;

    /// <summary>
    /// Controls which DNN features are available for this module.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Instantiated through reflection by DNN")]
    internal class FeaturesController : IUpgradeable
    {
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
                // this should only occur for DNN 4, the DNN 5 manifest doesn't setup the Event Queue to call UpgradeModule
                // and if it does need to call UpgradeModule in the future, it shouldn't include 1.4.0 in the version list
                // because we can add permissions declaratively through the DNN 5 manifest
                return PermissionsService.CreateCustomPermissions();
            }

            return "No upgrade action required for version " + version + " of Engage: Events";
        }
    }
}