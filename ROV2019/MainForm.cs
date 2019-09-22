using ROV2019.Views;
using ROV2019.Models;
using ROV2019.Presenters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ROV2019.ControllerConfigurations;
using SlimDX.DirectInput;
using ROV2019.Controllers;
using System.Threading;

namespace ROV2019
{
    public partial class Main : Form
    {
        ConnectionManager connectionManager;
        ArduinoConnection selectedConnection = null;
        ConnectionContext openConnection = null;

        ConnectionControllerMesher mesher;

        ControllerManager ControllerManager;
        ControllerInfo SelectedController = null;
        bool UsingSelectedController = false;

        bool IsOpen;

        public Main()
        {
            InitializeComponent();

            connectionManager = new ConnectionManager();
            ControllerManager = new ControllerManager();

            PopulateConnectionsList();
            PopulateControllersList();
            IsOpen = true;

            Thread controllerCheckerThread = new Thread(UpdateControllerConnectedStatus);
            controllerCheckerThread.IsBackground = true;
            controllerCheckerThread.Start();
        }

        private void UpdateControllerConnectedStatus()
        {
            bool wasPreviouslyDisconnected = false;
            while (IsOpen)
            {
                if (IsHandleCreated && !(Disposing || IsDisposed))
                {
                    try
                    {
                        Invoke(new MethodInvoker(delegate
                        {
                                foreach (Control c in ControllersListTable.Controls)
                                {
                                    ControllerInfo info = (ControllerInfo)c.Tag;
                                    if (!ControllerManager.IsControllerConnected(info))
                                    {
                                        c.BackColor = Color.Orange;
                                    if (SelectedController == info)
                                    {
                                        StopMesh();
                                        wasPreviouslyDisconnected = true;
                                    }
                                    }
                                    else
                                    {
                                        if (SelectedController == info)
                                        {
                                            c.BackColor = (UsingSelectedController ? Color.Green : Color.Yellow);
                                           if (wasPreviouslyDisconnected)
                                                InitiateMesh();
                                           wasPreviouslyDisconnected = false;
                                        }
                                        else
                                        {
                                            c.BackColor = Color.Transparent;
                                        }
                                    }
                                }
                        }));
                    }
                    catch (Exception) { IsOpen = false; };
                }
                Thread.Sleep(100);
            }
        }

        private void PopulateConnectionsList()
        {
            ConnectionsList.Controls.Clear();
            //populate connections list table
            foreach (ArduinoConnection con in connectionManager.SavedConnections)
            {
                ConnectionListItem connectionItem = new ConnectionListItem(con, RemoveButton_Click, null, ConnectionSelected);
                if (selectedConnection == con)
                    connectionItem.ToggleSelected();
                ConnectionsList.Controls.Add(connectionItem, 0, ConnectionsList.RowCount - 1);
            }
        }

        private void PopulateControllersList()
        {
            ControllersListTable.Controls.Clear();

            foreach (ControllerInfo info in ControllerManager.SavedControllers)
            {
                ControllerListItem listItem = new ControllerListItem(info, RemoveController, null, SelectController);
                if (info == SelectedController)
                    listItem.ToggleSelected();
                if (!ControllerManager.IsControllerConnected(info))
                    listItem.BackColor = Color.Orange;
                ControllersListTable.Controls.Add(listItem, 0, ControllersListTable.RowCount - 1);
            }
        }


        private void SetConnectionSpecificButtonsEnabled(bool enabled)
        {
            ConnectButton.Enabled = enabled;
            ConfigureTrimButton.Enabled = enabled;
            SensorButton.Enabled = enabled;
        }

        private void setConnectionListItemDisplayConnected(bool connected)
        {
            ConnectionListItem listItem = (ConnectionListItem)ConnectionsList.Controls.Find(selectedConnection.FriendlyName + selectedConnection.IpAddress, true)[0];
            if (connected)
            {
                listItem.BackColor = Color.Green;
                listItem.Click -= ConnectionSelected;
                listItem.Cursor = Cursors.Arrow;
            }
            else
            {
                listItem.BackColor = Color.Yellow;
                listItem.Click += ConnectionSelected;
                listItem.Cursor = Cursors.Hand;
            }
        }


        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsOpen = false;
            StopMesh();
            if(openConnection != null)
            {
                openConnection.Close();
            }
        }


        #region Click Handlers

        private void PIDAssistField_CheckedChanged(object sender, EventArgs e)
        {
            if (mesher != null)
            {
                mesher.IsUsingPID = ((CheckBox)sender).Checked;
            }
        }

        private void RemoveController(object sender, EventArgs e)
        {
            if (mesher == null || !mesher.IsMeshing)
            {
                ControllerInfo info = (ControllerInfo)((Button)sender).Tag;
                ControllerManager.Remove(info);
                PopulateControllersList();
            }
        }

        private void SelectController(object sender, EventArgs e)
        {
            ControllerListItem listItem = (ControllerListItem)sender;
            if (listItem.Selected && mesher == null)
            {
                //un-select
                UseControllerButton.Enabled = listItem.ToggleSelected();
                SelectedController = null;
            }
            else if (SelectedController == null)
            {
                //select
                SelectedController = (ControllerInfo)listItem.Tag;
                UseControllerButton.Enabled = listItem.ToggleSelected();
            }
        }

        private void ConnectionSelected(object sender, EventArgs e)
        {
            ConnectionListItem selectedConn = (ConnectionListItem)sender;
            if (selectedConn.Selected)
            {
                //unselecting
                this.selectedConnection = null;
                SetConnectionSpecificButtonsEnabled(selectedConn.ToggleSelected());
            }
            else
            {
                //selecting
                //make sure one is not already selected
                if (this.selectedConnection == null)
                {
                    SetConnectionSpecificButtonsEnabled(true);
                    this.selectedConnection = (ArduinoConnection)selectedConn.Tag;
                    selectedConn.ToggleSelected();
                }
            }

        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            ArduinoConnection conn = (ArduinoConnection)((Button)sender).Tag;
            if (openConnection == null || openConnection.connection != conn)
            {
                connectionManager.Remove(conn);
                if (conn == selectedConnection)
                    selectedConnection = null;
                PopulateConnectionsList();
            }
        }

        private void manualAddButton_Click(object sender, EventArgs e)
        {
            new ManualConnectionAddDialog(connectionManager);
            PopulateConnectionsList();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.Text.Equals("Connect"))
            {
                this.openConnection = connectionManager.GetConnectionContext(selectedConnection);
                if (openConnection.OpenConnection())
                {
                    //check authorization
                    string userPassword = null;
                    //TODO: Figure out if API will close socket or not.
                    while (!openConnection.Authorize())
                    {
                        //prompt user for correct password
                        userPassword = Dialog.ShowPrompt("Password Please:", selectedConnection.Password);
                        selectedConnection.Password = userPassword;
                        openConnection.connection = selectedConnection;
                        connectionManager.Save(selectedConnection);
                        if (userPassword.Equals(""))
                            return;
                    }
                    //TODO: Refactor maybe
                    button.Text = "Disconnect";
                    //Accelerations.StartPolling(openConnection);
                    setConnectionListItemDisplayConnected(true);
                    //start mesh if controller available
                    if(SelectedController != null && ControllerManager.IsControllerConnected(SelectedController))
                    {
                        InitiateMesh();
                    }
                }
                else
                {
                    Dialog.ShowMessageDialog("Error Opening Connection to ROV. Most likely Connection Refused.");
                }
            }
            else
            {
                StopMesh();
                Accelerations.StopPolling();
                openConnection.Close();
                button.Text = "Connect";
                //TODO: Refactor
                setConnectionListItemDisplayConnected(false);
            }
        }

        private void ScanButton_Click(object sender, EventArgs e)
        {
            Progress<ArduinoConnection> discoverConnectionListener = new Progress<ArduinoConnection>(connection =>
            {
                PopulateConnectionsList();
            });
            ConnectionScanProgressDialog scanProgressDialog = new ConnectionScanProgressDialog(connectionManager, discoverConnectionListener);
            scanProgressDialog.ShowDialog();
        }

        private void ConfigureTrimButton_Click(object sender, EventArgs e)
        {
            TrimConfigurationDialog configurationDialog = new TrimConfigurationDialog(connectionManager, selectedConnection);
            configurationDialog.ShowDialog();
            //need to repopulate to get the new trim properties so if this dialog is opened again, it reflects those values.
            PopulateConnectionsList();
        }

        private void SensorButton_Click(object sender, EventArgs e)
        {
            if(openConnection != null)
            {
                SensorsDialog dialog = new SensorsDialog(openConnection);
                dialog.Show();
            }
        }

        private void AddControllerButton_Click(object sender, EventArgs e)
        {
            //TODO: Show add controller dialog.
            AddControllerForm dialog = new AddControllerForm(ControllerManager);
            dialog.ShowDialog();
            PopulateControllersList();
        }

        private void UseControllerButton_Click(object sender, EventArgs e)
        {
            if (UseControllerButton.Text.Contains("Stop"))
            {
                UsingSelectedController = false;
                StopMesh();
                UseControllerButton.Text = "Use";
            }
            else
            {
                UsingSelectedController = true;
                InitiateMesh();
                UseControllerButton.Text = "Stop Using";
            }
        }

    #endregion

    private void StopMesh()
    {
            if (mesher != null)
            {
                mesher.StopMesh();
                mesher = null;
            }

            if (openConnection != null)
                openConnection.Stop();

        }

    private bool InitiateMesh()
    {
        if (openConnection != null && openConnection.isConnected() && ControllerManager.IsControllerConnected(SelectedController)
                && UsingSelectedController && mesher == null)
        {
            //start the mesh

            //read what type of configuration to use from saved properties.
            ControllerConfiguration config = ControllerManager.GetConfiguration(SelectedController);
            mesher = new ConnectionControllerMesher(openConnection, config);
            mesher.StartMesh();
            return true;

        }
            return false;
    }

        #region Timer

        int seconds = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            seconds++;
            int leftOverseconds = seconds % 60;
            TimerLabel.Text = seconds/60+":"+(leftOverseconds/10)+leftOverseconds%10;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if(button.Text.Equals("Start"))
            {
                button.Text = "Stop";
                timer1.Interval = 990;
                timer1.Start();
            }
            else
            {
                button.Text = "Start";
                timer1.Stop();
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            seconds = 0;
            TimerLabel.Text = "0:00";
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            //new ShapeDetector().Show();
            new FormShapeDetection().Show();
        }
    }
}
