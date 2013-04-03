// <copyright file="Assemblyinfo.cs" company="Engage Software">
// Engage: Events
// Copyright (c) 2004-2013
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Web.UI;

[assembly: AssemblyTitle("Engage: Events")]
[assembly: AssemblyDescription("The DotNetNuke Events module by Engage Software (www.engagesoftware.com)")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Engage Software, Inc.")]
[assembly: AssemblyProduct("")]
[assembly: AssemblyCopyright("2013 Engage Software Inc.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]

[assembly: AssemblyFileVersion("1.7.2.0")]
[assembly: AssemblyVersion("1.7.2.*")]

[assembly: InternalsVisibleTo("EngageEvents.Tests")]

[assembly: WebResource("Engage.Dnn.Events.JavaScript.EngageEvents.Actions.RegisterAction.combined.js", "text/javascript")] // TODO: Move into template
[assembly: WebResource("Engage.Dnn.Events.JavaScript.EngageEvents.TemplatedDisplayOptions.combined.js", "text/javascript")]
[assembly: WebResource("Engage.Dnn.Events.JavaScript.EngageEvents.EventEdit.combined.js", "text/javascript")]
[assembly: WebResource("Engage.Dnn.Events.JavaScript.EngageEvents.Actions.data-confirm.combined.js", "text/javascript")]