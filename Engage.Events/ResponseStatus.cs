// <copyright file="ResponseStatus.cs" company="Engage Software">
// Engage.Events - http://www.engagemodules.com
// Copyright (c) 2004-2010
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Events
{
    /// <summary>
    /// The status of an <see cref="Response"/>; that is, how the invitee responded to the invitation.
    /// </summary>
    public enum ResponseStatus
    {
        /// <summary>
        /// The invitee did not respond to an invitation
        /// </summary>
        NoResponse = 0,

        /// <summary>
        /// The invitee indicated that they will attend
        /// </summary>
        Attending = 1,

        /// <summary>
        /// The invitee indicated that they will not attend
        /// </summary>
        NotAttending = 2
    }
}