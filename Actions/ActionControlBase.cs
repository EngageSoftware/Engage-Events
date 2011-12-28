// <copyright file="ActionControlBase.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2011
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
    /// The base class for all action controls in the Engage: Events module.
    /// </summary>
    public abstract class ActionControlBase : ModuleBase
    {
        /// <summary>
        /// Backing field for <see cref="CurrentEvent"/>
        /// </summary>
        private Event currentEvent;

        /// <summary>
        /// Gets or sets the CSS class.
        /// </summary>
        /// <value>The CSS class.</value>
        public string CssClass { get; set; }

        /// <summary>
        /// Gets or sets the current event that this control is displaying actions for.
        /// </summary>
        /// <value>The current event that this control is displaying actions for.</value>
        internal Event CurrentEvent
        {
            get
            {
                if (this.currentEvent != null)
                {
                    return this.currentEvent;
                }

                var e = Event.Load(this.CurrentEventId);
                if (this.CanShowEvent(e))
                {
                    return e;
                }

                throw new InvalidOperationException("Event requested which this module cannot access");
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

        /// <summary>
        /// Performs all necessary operations to display the control's data correctly.
        /// </summary>
        protected virtual void BindData()
        {
        }
    }
}