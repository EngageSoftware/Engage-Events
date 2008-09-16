// <copyright file="ReminderEmailTransaction.cs" company="Engage Software">
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
    using Events;
    using Routing;

    public class ReminderEmailTransaction : EmailTransaction
    {
        public ReminderEmailTransaction(int routingEventId, string firstName, string lastName, string email, Template mergeData, int createdBy)
            : base(-1, routingEventId, firstName, lastName, email, mergeData, createdBy)
        {
        }

        public ReminderEmailTransaction(int routingTransactionId, int routingEventId, string firstName, string lastName, string email, Template mergeData, int createdBy)
            : base(routingTransactionId, routingEventId, firstName, lastName, email, mergeData, createdBy)
        {
        }

        /// <summary>
        /// This method overrides the default behavior of looking at HtmlBodyLocation1 to inspect the recipient's 
        /// <see cref="ResponseStatus"/> and decide which message to send.
        /// </summary>
        protected override void GetMessageBody()
        {
            switch (this.ResponseStatus)
            {
                case ResponseStatus.Attending:
                    this.MessageBody = Utility.GetHtmlFromUrl(this.EmailEvent.HtmlBodyLocation1);
                    break;
                case ResponseStatus.NotAttending:
                    this.MessageBody = Utility.GetHtmlFromUrl(this.EmailEvent.HtmlBodyLocation2);
                    break;
                case ResponseStatus.NoResponse:
                    this.MessageBody = Utility.GetHtmlFromUrl(this.EmailEvent.HtmlBodyLocation3);
                    break;
            }

            this.MessageBody += GetEmailFooter();
        }

        /// <summary>
        /// Gets the response status.
        /// </summary>
        /// <value>The response status.</value>
        private ResponseStatus ResponseStatus
        {
            get { return Response.GetResponseStatus(this.EventId, this.EventDate, this.Email); }
        }
    }
}