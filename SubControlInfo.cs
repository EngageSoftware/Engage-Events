// <copyright file="SubControlInfo.cs" company="Engage Software">
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
    using System.Diagnostics;

    /// <summary>
    /// Information about the sub-controls loaded by Engage: Events based on a key on the <c>QueryString</c>.
    /// </summary>
    public struct SubControlInfo : IEquatable<SubControlInfo>
    {
        /// <summary>
        /// Backing field for <see cref="ControlPath"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string controlPath;

        /// <summary>
        /// Backing field for <see cref="RequiresEditPermission"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool requiresEditPermission;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubControlInfo"/> struct.
        /// </summary>
        /// <param name="controlPath">The path to the control, relative to the module's <see cref="Dnn.Framework.ModuleBase.DesktopModuleFolderName"/>.</param>
        /// <param name="requiresEditPermission">if set to <c>true</c> this sub-control requires the user to have edit permission in order to view it.</param>
        /// <exception cref="ArgumentNullException"><paramref name="controlPath"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="controlPath"/> does not have a value</exception>
        public SubControlInfo(string controlPath, bool requiresEditPermission)
        {
            if (!Engage.Utility.HasValue(controlPath))
            {
                if (controlPath == null)
                {
                    throw new ArgumentNullException("controlPath");
                }
                else
                {
                    throw new ArgumentException("controlPath", "controlPath must have a value");
                }
            }

            this.controlPath = controlPath;
            this.requiresEditPermission = requiresEditPermission;
        }

        /// <summary>
        /// Gets the path to the control, relative to the module's <see cref="Dnn.Framework.ModuleBase.DesktopModuleFolderName"/>.
        /// </summary>
        /// <value>The path to the control</value>
        public string ControlPath
        {
            [DebuggerStepThrough]
            get { return this.controlPath; }
        }

        /// <summary>
        /// Gets a value indicating whether this sub-control requires the user to have edit permission in order to view it.
        /// </summary>
        /// <value>
        /// <c>true</c> if this sub-control requires the user to have edit permission in order to view it; otherwise, <c>false</c>.
        /// </value>
        public bool RequiresEditPermission
        {
            [DebuggerStepThrough]
            get { return this.requiresEditPermission; }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">
        /// An object to compare with this object.
        /// </param>
        public bool Equals(SubControlInfo other)
        {
            return other.controlPath.Equals(this.controlPath, StringComparison.Ordinal) && other.requiresEditPermission.Equals(this.requiresEditPermission);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj.GetType() == typeof(SubControlInfo) && this.Equals((SubControlInfo)obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (this.controlPath.GetHashCode() * 397) ^ this.requiresEditPermission.GetHashCode();
            }
        }
    }
}