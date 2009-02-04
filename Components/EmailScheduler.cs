// <copyright file="EmailScheduler.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2009
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
    using DotNetNuke.Services.Scheduling;
    using Routing;

    /// <summary>
    /// A scheduler client for running scheduled email tasks.
    /// </summary>
    public class EmailScheduler : SchedulerClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailScheduler"/> class.
        /// </summary>
        /// <param name="objScheduleHistoryItem">The obj schedule history item.</param>
        public EmailScheduler(ScheduleHistoryItem objScheduleHistoryItem)
        {
            ScheduleHistoryItem = objScheduleHistoryItem;
        }

        /// <summary>
        /// Does the work.
        /// </summary>
        public override void DoWork()
        {
            RoutingManager rm = RoutingManager.Instance;
            try
            {
                rm.RunServiceEvents(0);
                ScheduleHistoryItem.Succeeded = true;
                ScheduleHistoryItem.AddLogNote("Email Scheduler completed successfully.<br />");
            }
            catch (Exception e)
            {
                ScheduleHistoryItem.Succeeded = false;
                ScheduleHistoryItem.AddLogNote(e.Message + "<br />");
            }
        }
    }
}