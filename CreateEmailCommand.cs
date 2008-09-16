// <copyright file="CreateEmailCommand.cs" company="Engage Software">
// Engage.Communication - http://www.engagemodules.com
// Copyright (c) 2004-2008
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Communication.Email
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Routing;

    [Serializable]
    public class CreateEmailCommand : Services.Command
    {
        private readonly EmailEvent emailEvent;

        public CreateEmailCommand(EmailEvent emailEvent)
        {
            this.emailEvent = emailEvent;
        }

        public void WriteLocalData(int eventId, DateTime eventStart, int createdBy)
        {
            this.WriteMessageId(eventId, createdBy);

            this.WriteResponses(eventId, eventStart, createdBy);
        }

        protected override void DataServicesExecute()
        {
            // save the email
            this.emailEvent.Save();            

            // write the recipients
            this.WriteRecipients();
        }

        protected override void OnDataServicesException(Services.DataServicesEventArgs e)
        {
            // do something client side with server side exception
        }

        private void WriteRecipients()
        {
            List<Template> templates = this.emailEvent.GetRecipients();

            foreach (Template template in templates)
            {
                ReminderEmailTransaction transaction = new ReminderEmailTransaction(this.emailEvent.RoutingEventId, template.FirstName, template.LastName, template.Email, template, this.emailEvent.CreatedBy);
                transaction.Save();
            }
        }

        /// <exception cref="DBException">If there is an error writing the message ID to the database</exception>
        private void WriteMessageId(int eventId, int createdBy)
        {
            IDataProvider dp = DataProvider.Instance;

            try
            {
                dp.ExecuteNonQuery(
                    CommandType.StoredProcedure,
                    dp.NamePrefix + "spWriteMessageId",
                    Utility.CreateIntegerParam("@EventId", eventId),
                    Utility.CreateIntegerParam("@MessageId", this.emailEvent.RoutingEventId),
                    Utility.CreateIntegerParam("@CreatedBy", createdBy));

            }
            catch (SystemException de)
            {
                throw new DBException("spWriteMessageId", de);
            }
        }

        private void WriteResponses(int eventId, DateTime eventStart, int createdBy)
        {
            // So that it will be much much easier for report we will write a record in Engage_Response with
            // a default status of NoResponse until they actually RVSP or not.
            foreach (Template template in this.emailEvent.GetRecipients())
            {
                if (this.emailEvent.WriteResponseEntries)
                {
                    Events.Response response = Events.Response.Create(eventId, eventStart, template.FirstName, template.LastName, template.Email);
                    response.Save(createdBy);
                }
            }
        }
    }
}
