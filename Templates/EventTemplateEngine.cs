// <copyright file="TemplateEngine.cs" company="Engage Software">
// Engage: Events - http://www.engagemodules.com
// Copyright (c) 2004-2008
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Events.Templating
{
    using System.Web.UI;
    using Engage.Templating;

    public delegate void EventTagProcessDelegate(Control container, Tag tag, Event ev);

    /// <summary>
    /// Summary description for TemplateEngine.
    /// </summary>
    public class EventTemplateEngine : TemplateEngine
    {
        public EventTemplateEngine(int tabModuleId)
            : base(tabModuleId)
        {
        }

        public static void ProcessEventTags(Control container, TagList tags, Event ev, EventTagProcessDelegate processTagDelegate)
        {
            foreach (Tag childTag in tags)
            {
                switch (childTag.TagType)
                {
                    case TagType.Comment:
                        container.Controls.Add(new LiteralControl(childTag.ToString()));
                        break;
                    case TagType.Open:
                        if (childTag is EngageTag)
                        {
                            //handled elsewhere in calling class
                            processTagDelegate(container, childTag, ev);
                        }
                        else
                        {
                            container.Controls.Add(new LiteralControl(childTag.ToString()));
                        }
                        break;
                    case TagType.Close:
                        if (childTag is EngageTag)
                        {
                            //since we are injecting our own control, no need to close the tag here.
                        }
                        else
                        {
                            container.Controls.Add(new LiteralControl(childTag.ToString()));
                        }
                        break;
                    case TagType.Text:
                        //if (childTag.ToString().Trim() != string.Empty)
                        //{
                            container.Controls.Add(new LiteralControl(childTag.ToString()));
                        //}
                        break;
                    default:
                        break;
                }
                
                ProcessEventTags(container, childTag.ChildTags, ev, processTagDelegate);
            }
        }
    }
}