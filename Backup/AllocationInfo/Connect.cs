using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE80;
using EnvDTE;
using Extensibility;
using Microsoft.VisualStudio.CommandBars;
using System.Reflection;
using System.Resources;
using System.Globalization;

namespace SqlInternals.AllocationInfo.Addin
{
    /// <summary>The object for implementing an Add-in.</summary>
    /// <seealso class='IDTExtensibility2' />
    public class Connect : IDTExtensibility2, IDTCommandTarget
    {

        private DTE2 applicationObject;
        private AddIn addInInstance;

        /// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
        public Connect()
        {
        }

        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            applicationObject = (DTE2)application;
            addInInstance = (AddIn)addInInst;
            string viewMenuName;

            //For SSMS this need to be set to ext_ConnectMode.ext_cm_Startup rather than ext_ConnectMode.ext_cm_UISetup
            if (connectMode == ext_ConnectMode.ext_cm_Startup)
            {
                object[] contextGUIDS = new object[] { };
                Commands2 commands = (Commands2)applicationObject.Commands;

                try
                {
                    string resourceName;
                    ResourceManager resourceManager = new ResourceManager("SqlInternals.AllocationInfo.Addin.CommandBar", Assembly.GetExecutingAssembly());
                    CultureInfo cultureInfo = new CultureInfo(applicationObject.LocaleID);

                    if (cultureInfo.TwoLetterISOLanguageName == "zh")
                    {
                        System.Globalization.CultureInfo parentCultureInfo = cultureInfo.Parent;
                        resourceName = String.Concat(parentCultureInfo.Name, "View");
                    }
                    else
                    {
                        resourceName = String.Concat(cultureInfo.TwoLetterISOLanguageName, "View");
                    }

                    viewMenuName = resourceManager.GetString(resourceName);
                }
                catch
                {
                    viewMenuName = "View";
                }

                Microsoft.VisualStudio.CommandBars.CommandBar menuBarCommandBar = ((CommandBars)applicationObject.CommandBars)["MenuBar"];

                Microsoft.VisualStudio.CommandBars.CommandBarControl toolsControl = menuBarCommandBar.Controls[viewMenuName];
                CommandBarPopup toolsPopup = (CommandBarPopup)toolsControl;

                try
                {
                    Command command = commands.AddNamedCommand2(addInInstance,
                                                                "Display",
                                                                "Show Allocation Info",
                                                                "Displays the Allocation Info window",
                                                                true,
                                                                0,
                                                                ref contextGUIDS,
                                                                (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled,
                                                                (int)vsCommandStyle.vsCommandStylePictAndText,
                                                                vsCommandControlType.vsCommandControlTypeButton);

                    if ((command != null) && (toolsPopup != null))
                    {
                        command.AddControl(toolsPopup.CommandBar, 1);
                    }
                }
                catch (System.ArgumentException)
                {
                }
            }
        }

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />		
        public void OnAddInsUpdate(ref Array custom)
        {
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
        }

        /// <summary>Implements the QueryStatus method of the IDTCommandTarget interface. This is called when the command's availability is updated</summary>
        /// <param term='commandName'>The name of the command to determine state for.</param>
        /// <param term='neededText'>Text that is needed for the command.</param>
        /// <param term='status'>The state of the command in the user interface.</param>
        /// <param term='commandText'>Text requested by the neededText parameter.</param>
        /// <seealso class='Exec' />
        public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText)
        {
            if (neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
            {
                if (commandName == "SqlInternals.AllocationInfo.Addin.Connect.Display")
                {
                    status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
                    return;
                }
            }
        }

        /// <summary>Implements the Exec method of the IDTCommandTarget interface. This is called when the command is invoked.</summary>
        /// <param term='commandName'>The name of the command to execute.</param>
        /// <param term='executeOption'>Describes how the command should be run.</param>
        /// <param term='varIn'>Parameters passed from the caller to the command handler.</param>
        /// <param term='varOut'>Parameters passed from the command handler to the caller.</param>
        /// <param term='handled'>Informs the caller if the command was handled or not.</param>
        /// <seealso class='Exec' />
        public void Exec(string commandName, vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
        {
            handled = false;
            if (executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
            {
                if (commandName == "SqlInternals.AllocationInfo.Addin.Connect.Display")
                {
                    CreateAllocationWindow(applicationObject, addInInstance);

                    handled = true;
                    return;
                }
            }
        }

        /// <summary>
        /// Creates the allocation window.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="addinInstance">The addin instance.</param>
        public void CreateAllocationWindow(DTE2 application, AddIn addinInstance)
        {
            Guid id = new Guid("{65a48117-79b3-4863-b268-eb7eafc21feb}");

            Windows2 windows2 = applicationObject.Windows as Windows2;

            if (windows2 != null)
            {
                object controlObject = null;
                Assembly asm = Assembly.GetExecutingAssembly();

                Window toolWindow = windows2.CreateToolWindow2(addinInstance,
                                                               asm.Location,
                                                               "SqlInternals.AllocationInfo.Addin.AddinControl",
                                                               "Allocation Information", "{" + id.ToString() + "}",
                                                               ref controlObject);

                toolWindow.Visible = true;
            }
        }
    }
}

