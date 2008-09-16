// <copyright file="ListingMode.cs" company="Engage Software">
// Engage.Events - http://www.engagemodules.com
// Copyright (c) 2004-2008
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
    /// Defines the date range for which to retrieve a list of events.
    /// </summary>
    public enum ListingMode
    {
        /// <summary>
        /// Display all events, from the beginning of time to the end of time.
        /// </summary>
        All,

        /// <summary>
        /// From today until the end of the month
        /// </summary>
        CurrentMonth,

        /// <summary>
        /// From the beginning of next month until the end of time
        /// </summary>
        Future,

        /// <summary>
        /// From the beginning of time until today
        /// </summary>
        Past
    }
}