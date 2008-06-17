// <copyright file="EventCalendar.ascx.cs" company="Engage Software">
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
    using DotNetNuke.Services.Exceptions;
    using Engage.Events;

    public partial class EventCalendar : ModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                this.BindData();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }    
        }

        protected void RadScheduler1_AppointmentInsert(object sender, Telerik.Web.UI.SchedulerCancelEventArgs e)
        {
            Event ev = Event.Create(PortalId, ModuleId, UserInfo.Email, e.Appointment.Subject, "", e.Appointment.Start);
            ev.EventEnd = e.Appointment.Start.Add(e.Appointment.Duration);
            ev.Save(UserId);
        }

        protected void RadScheduler1_AppointmentDelete(object sender, Telerik.Web.UI.SchedulerCancelEventArgs e)
        {
            Event.Delete(Convert.ToInt32(e.Appointment.ID));
        }

        protected void RadScheduler1_AppointmentUpdate(object sender, Telerik.Web.UI.AppointmentUpdateEventArgs e)
        {
            Event ev = Event.Load(Convert.ToInt32(e.Appointment.ID));
            ev.EventStart = e.Appointment.Start;
            ev.EventEnd = e.Appointment.Start.Add(e.Appointment.Duration);
            ev.Title = e.Appointment.Subject;
            ev.Save(UserId);
        }

        protected void RadScheduler1_AppointmentCreated(object sender, Telerik.Web.UI.AppointmentCreatedEventArgs e)
        {
            Event ev = Event.Load(Convert.ToInt32(e.Appointment.ID, CultureInfo.InvariantCulture));
            e.Appointment.ToolTip = "Overview: " + ev.Overview;
        }

        private void BindData()
        {
            EventCollection events = EventCollection.Load(PortalId, "EventStart", 0, 0, true);
            RadScheduler1.DataSource = events;
            RadScheduler1.DataBind();

            if (Settings[Setting.SkinSelection.PropertyName] != null)
            {
                RadScheduler1.Skin = Settings[Setting.SkinSelection.PropertyName].ToString();
            }
        }
    }
}