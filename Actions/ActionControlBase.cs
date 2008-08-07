// <copyright file="ActionControlBase.cs" company="Engage Software">
// Engage: Events - http://www.engagemodules.com
// Copyright (c) 2004-2008
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Events
{
    using System;
    using System.Globalization;
    using Engage.Events;

    /// <summary>
    /// The base class for all controls in the Engage: Events module. Since this module is licensed it 
    /// inherits from LicenseModuleBase and requires a unique GUID be defined. DO NOT CHANGE THIS!
    /// </summary>
    public abstract class ActionControlBase : ModuleBase
    {
        /// <summary>
        /// Backing field for <see cref="CurrentEvent"/>
        /// </summary>
        private Event currentEvent;

        /// <summary>
        /// Gets or sets the current event that this control is displaying actions for.
        /// </summary>
        /// <value>The current event that this control is displaying actions for.</value>
        internal Event CurrentEvent
        {
            get
            {
                return this.currentEvent ?? Event.Load(this.CurrentEventId);
            }

            set
            {
                this.currentEvent = value;
                this.CurrentEventId = this.currentEvent.Id;
                this.BindData();
            }
        }

        /// <summary>
        /// Gets or sets the current event id.
        /// </summary>
        /// <value>The current event id.</value>
        internal int CurrentEventId
        {
            get { return Convert.ToInt32(this.ViewState["id"], CultureInfo.InvariantCulture); }

            set { this.ViewState["id"] = value.ToString(CultureInfo.InvariantCulture); }
        }

        protected abstract void BindData();
    }
}

