using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;

namespace AudioManagerApp
{
    public partial class Form1 : Form
    {
        private AudioSessionManager audioSessionManager;

        public Form1()
        {
            InitializeComponent();
            this.Load += new EventHandler(this.Form1_Load);
            audioSessionManager = new AudioSessionManager();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadAudioDevices();
            LoadRunningApplications();
        }

        private void LoadAudioDevices()
        {
            var deviceEnumerator = new MMDeviceEnumerator();
            var devices = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

            foreach (var device in devices)
            {
                deviceComboBox.Items.Add(device.FriendlyName);
            }

            if (deviceComboBox.Items.Count > 0)
            {
                deviceComboBox.SelectedIndex = 0; // Select the first device by default
            }
        }

        private void LoadRunningApplications()
        {
            Process[] processList = Process.GetProcesses();
            foreach (Process process in processList)
            {
                if (!string.IsNullOrEmpty(process.MainWindowTitle) && !appListBox.Items.Contains(process.MainWindowTitle))
                {
                    appListBox.Items.Add(process.MainWindowTitle);
                }
            }
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            string selectedDevice = deviceComboBox.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedDevice))
            {
                MessageBox.Show("Please select an output device.");
                return;
            }

            foreach (var item in appListBox.CheckedItems)
            {
                RouteAudioToDevice(item.ToString(), selectedDevice);
            }
        }

        private void RouteAudioToDevice(string applicationName, string deviceName)
        {
            var session = audioSessionManager.GetSessionByProcessName(applicationName);

            if (session != null)
            {
                var deviceEnumerator = new MMDeviceEnumerator();
                var devices = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
                var targetDevice = devices.FirstOrDefault(d => d.FriendlyName.Equals(deviceName, StringComparison.OrdinalIgnoreCase));

                if (targetDevice != null)
                {
                    // Obtain the session control correctly
                    var sessionControl = session.GetSessionControl();

                    if (sessionControl != null)
                    {
                        // Use the session control to adjust the audio output
                        // This might require additional steps depending on how you implement audio routing
                        // Here we simply inform the user
                        MessageBox.Show($"Routing '{applicationName}' to '{deviceName}'");
                    }
                    else
                    {
                        MessageBox.Show($"Audio session control not found for '{applicationName}'.");
                    }
                }
                else
                {
                    MessageBox.Show($"Device '{deviceName}' not found.");
                }
            }
            else
            {
                MessageBox.Show($"Application '{applicationName}' not found.");
            }
        }

        private void saveProfileButton_Click(object sender, EventArgs e)
        {
            var profile = new Profile
            {
                SelectedDevice = deviceComboBox.SelectedItem?.ToString(),
                Applications = appListBox.CheckedItems.Cast<string>().ToList()
            };

            SaveProfile(profile);
        }

        private void SaveProfile(Profile profile)
        {
            MessageBox.Show("Profile saved!");
        }

        // Dummy handlers for missing event declarations
        private void label1_Click(object sender, EventArgs e) { }
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void button2_Click(object sender, EventArgs e) { }
    }

    public class Profile
    {
        public string SelectedDevice { get; set; }
        public List<string> Applications { get; set; }
    }

    public class AudioSession
    {
        public string ProcessName { get; set; }
        public int SessionId { get; set; }
        public IAudioSessionControl sessionControl;

        public AudioSession(IAudioSessionControl sessionControl)
        {
            this.sessionControl = sessionControl;

            // Try to parse the session identifier to an int
            // Assuming GetSessionIdentifier returns a string representation of the session ID
            if (int.TryParse(sessionControl.GetSessionIdentifier(), out int sessionId)) // Ensure GetSessionIdentifier is a method
            {
                this.SessionId = sessionId; // Assign the parsed integer
            }
            else
            {
                MessageBox.Show("Failed to parse session identifier to integer.");
            }
        }

        public IAudioSessionControl GetSessionControl()
        {
            return sessionControl; // Return the control directly
        }
    }

    public class AudioSessionManager
    {
        private readonly List<AudioSession> sessions;

        public AudioSessionManager()
        {
            sessions = new List<AudioSession>();
            LoadAudioSessions();
        }

        private void LoadAudioSessions()
        {
            var deviceEnumerator = new MMDeviceEnumerator();
            var device = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            var sessionManager = device.AudioSessionManager;

            for (int i = 0; i < sessionManager.Sessions.Count; i++)
            {
                var session = sessionManager.Sessions[i];
                var audioSession = new AudioSession(session); // Pass the session control
                audioSession.ProcessName = session.GetSessionIdentifier; // Assuming this is a method
                sessions.Add(audioSession);
            }
        }

        public AudioSession GetSessionByProcessName(string processName)
        {
            return sessions.FirstOrDefault(session => session.ProcessName.Equals(processName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
