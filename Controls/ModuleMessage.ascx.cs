// <copyright file="ModuleMessage.ascx.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2009
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Events.Controls
{
    using System;
    using System.Diagnostics;
    using System.Web.UI;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.UI.Skins;

    /// <summary>
    /// The type of message that is being displayed by the <see cref="ModuleMessage"/> control.
    /// </summary>
    public enum ModuleMessageType
    {
        /// <summary>
        /// No module type, will not display the module message.
        /// </summary>
        None = 0,

        /// <summary>
        /// Used to indicate that an error has occurred during the processing of an operation.
        /// </summary>
        Error,

        /// <summary>
        /// Used to present informational messages or warnings about potential problems.
        /// </summary>
        Warning,

        /// <summary>
        /// Used to indicate success of an operation.
        /// </summary>
        Success
    }

    /// <summary>
    /// A control to display a message within a module.
    /// </summary>
    public partial class ModuleMessage : SkinObjectBase
    {
        /// <summary>
        /// The backing field for <see cref="TextResourceKey"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string textResourceKey = string.Empty;

        /// <summary>
        /// The backing field for <see cref="CssClass"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string cssClass = string.Empty;

        /// <summary>
        /// The backing field for <see cref="MessageType"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ModuleMessageType messageType = ModuleMessageType.Success;

        /// <summary>
        /// Gets or sets the resource key to be used to get the message text.
        /// </summary>
        /// <value>The text resource key.</value>
        public string TextResourceKey
        {
            [DebuggerStepThrough]
            get { return this.textResourceKey; }
            [DebuggerStepThrough]
            set { this.textResourceKey = value; }
        }

        /// <summary>
        /// Gets or sets the text to display as the main content of this message.
        /// </summary>
        /// <value>The text to display as the main content of this message.</value>
        public string Text
        {
            get { return this.messageLabel.Text; }
            set { this.messageLabel.Text = value; }
        }

        /// <summary>
        /// Gets or sets the Cascading Style Sheet (CSS) class rendered by the Web server control on the client.
        /// </summary>
        /// <value>The CSS class rendered by the Web server control on the client. The default is <see cref="string.Empty"/>.</value>
        public string CssClass
        {
            [DebuggerStepThrough]
            get { return this.cssClass; }
            [DebuggerStepThrough]
            set { this.cssClass = value; }
        }

        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        /// <value>The type of the message.</value>
        public ModuleMessageType MessageType
        {
            [DebuggerStepThrough]
            get 
            { 
                return this.messageType; 
            }

            set
            {
                this.messageType = value;
                if (this.messageType == ModuleMessageType.None)
                {
                    this.Visible = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a server control is rendered as UI on the page.
        /// </summary>
        /// <value>Whether the control is visible on the page</value>
        public override bool Visible
        {
            [DebuggerStepThrough]
            get
            {
                return base.Visible;
            }

            set
            {
                if (this.messageType != ModuleMessageType.None)
                {
                    base.Visible = value;
                }
                else
                {
                    Debug.Assert(base.Visible == false, "Visibility should be set permanently to false when MessageType is None");
                }
            }
        }

        /// <summary>
        /// Gets the message style, to be used in determining the correct CSS class to use.
        /// </summary>
        /// <value>The message style.</value>
        protected string MessageStyle
        {
            get { return this.messageType.ToString(); }
        }

        /// <summary>
        /// GetLocalizedText gets the localized text for the provided key
        /// </summary>
        /// <param name="key">The resource key</param>
        /// <param name="control">The current control</param>
        /// <returns>Localized text for the given <paramref name="key"/></returns>
        /// <history>
        /// [cnurse] 9/8/2004  Created
        /// [bdukes] 6/13/2008 Adapted for new <see cref="ModuleMessage"/> control
        /// </history>
        protected static string GetLocalizedText(string key, Control control)
        {
            // We need to find the parent module
            PortalModuleBase parentControl = control.Parent as PortalModuleBase;
            if (parentControl != null)
            {
                // We are at the Module Level so return key
                // Get Resource File Root from Parents LocalResourceFile Property
                return Localization.GetString(key, parentControl.LocalResourceFile);
            }
            
            System.Reflection.PropertyInfo pi = control.Parent.GetType().GetProperty("LocalResourceFile");
            if (pi != null && pi.PropertyType.Equals(typeof(string)))
            {
                // If control has a LocalResourceFile property use this
                return Localization.GetString(key, (string)pi.GetValue(control.Parent, null));
            }
            
            // Drill up to the next level 
            return GetLocalizedText(key, control.Parent);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += this.Page_Load;
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            // We need to make sure not to overwrite the value if they've only specified the Text property. BD
            if (!string.IsNullOrEmpty(this.TextResourceKey))
            {
                this.messageLabel.Text = GetLocalizedText(this.TextResourceKey, this);
            }
        }
    }
}