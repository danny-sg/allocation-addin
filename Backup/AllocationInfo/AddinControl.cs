using System;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo.RegSvrEnum;
using Microsoft.SqlServer.Management.UI.VSIntegration;
using Microsoft.SqlServer.Management.UI.VSIntegration.ObjectExplorer;

namespace SqlInternals.AllocationInfo.Addin
{
    public partial class AddinControl : UserControl
    {
        public AddinControl()
        {
            GetConnection();
            InitializeComponent();
        }

        private bool GetConnection()
        {
            SqlConnectionInfo info = null;

            try
            {
                UIConnectionInfo connectionInfo = null;

                if (ServiceCache.ScriptFactory.CurrentlyActiveWndConnectionInfo != null)
                {
                    connectionInfo = ServiceCache.ScriptFactory.CurrentlyActiveWndConnectionInfo.UIConnectionInfo;
                }

                if (connectionInfo != null)
                {
                    Internals.ServerConnection.CurrentConnection().SetCurrentServer(connectionInfo.ServerName,
                                                                                    string.IsNullOrEmpty(connectionInfo.Password),
                                                                                    connectionInfo.UserName,
                                                                                    connectionInfo.Password);

                    return true;
                }
                else
                {
                    IObjectExplorerService objExplorer = ServiceCache.GetObjectExplorer();

                    int arraySize;
                    INodeInformation[] nodes;

                    objExplorer.GetSelectedNodes(out arraySize, out nodes);

                    if (nodes.Length > 0)
                    {
                        info = nodes[0].Connection as SqlConnectionInfo;

                        Internals.ServerConnection.CurrentConnection().SetCurrentServer(info.ServerName,
                                                                                        info.UseIntegratedSecurity,
                                                                                        info.UserName,
                                                                                        info.Password);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }
    }
}
