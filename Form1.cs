using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace ZUIKI_to_JRE {
    public class Form1 : Form {
        public Joystick curJoystick;
        public List<JKinf> JKinfs = new List<JKinf>();
        public List<int> keyAct = new List<int>();
        public bool isgameON = false;
        public IntPtr gamePID = new IntPtr(0);
        public int lastPov = -1;
        public string lastState = "0";
        public int lastValue = -1;
        public bool[] lastButtons = new bool[14];
        public int nowPov;
        public string nowState;
        public int nowValue;
        public bool[] nowButtons = new bool[14];
        private IContainer components = null;
        private Label labelB;
        private Label labelA;
        private Label labelY;
        private Label labelX;
        private Label labelDown;
        private Label labelRight;
        private Label labelLeft;
        private Label labelUp;
        private Label labelHome;
        private Label labelCapture;
        private Label labelR;
        private Label labelPlus;
        private Label labelMinus;
        private Label labelL;
        private Label labelZR;
        private Label labelCredits;
        private Label labelHandle;
        private Button buttonConnect;
        private Button buttonDetect;
        private ComboBox comboBoxDevice;
        private Label labelSelectHandle;
        private Timer timerHandle;
        private Timer timerGame;
        private Timer timerHorn;
        private RadioButton radioStdMode;
        private RadioButton radioKihaMode;
        private Label labelKihaArrow;
        private Label labelKihaHandle;
        private ComboBox comboBoxL;
        private Label labelZL;
        private ComboBox comboBoxZL;
        private ComboBox comboBoxR;
        private ComboBox comboBoxZR;
        private ComboBox comboBoxMinus;
        private ComboBox comboBoxPlus;
        private ComboBox comboBoxCapture;
        private ComboBox comboBoxHome;
        private ComboBox comboBoxRight;
        private ComboBox comboBoxLeft;
        private ComboBox comboBoxDown;
        private ComboBox comboBoxUp;
        private ComboBox comboBoxY;
        private ComboBox comboBoxX;
        private ComboBox comboBoxB;
        private ComboBox comboBoxA;
        private IniFile iniFile;
        private Dictionary<int, string> commands = new Dictionary<int, string> {
            { 0, "" },
            { 67, "[C] Show/hide cabin" },
            { 86, "[V] Show/hide HUD" },
            { 27, "[Esc] Pause game" },
            { 38, "[Up] Reverser forward" },
            { 40, "[Down] Reverser reverse" },
            { 69, "[E] Deadman switch" },
            { 13, "[Enter -> Bksp] Horn" },
            { 8,  "[Bksp] Horn" },
            { 32, "[Space] ATS Confirmation" },
            { 88, "[X] Alarm stop" },
            { 89, "[Y] ATS reset (service)" },
            { 85, "[U] ATS reset (emergency)" },
            { 66, "[B] Buzzer" },
            { 73, "[I] TASC Inching" },
            { 84, "[T] TASC Switch" },
            { 87, "[W] Cruise ctrl/Speed suppr." },
            { 68, "[D] Speed suppr." },
            { 75, "[K] Hill start" },
            { 82, "[R] Gear shift to 'Direct'" },
            { 70, "[F] Gear shift to 'Change'" }
        };
        private bool skipZL = false;

        [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        public static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        public Form1() => this.InitializeComponent();

        private void button1_Click(object sender, EventArgs e) {
            this.JKinfs.Clear();
            this.comboBoxDevice.Items.Clear();
            foreach (DeviceInstance device in (IEnumerable<DeviceInstance>)new DirectInput().GetDevices()) {
                this.JKinfs.Add(new JKinf(device.InstanceGuid));
                this.comboBoxDevice.Items.Add((object)device.ProductName);
            }
            if (this.JKinfs.Count == 0)
                return;
            this.comboBoxDevice.SelectedIndex = this.JKinfs.Count - 1;
            this.buttonConnect.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e) {
            try {
                this.keyAct.Clear();
                this.curJoystick = new Joystick(new DirectInput(), this.JKinfs[this.comboBoxDevice.SelectedIndex].Devguid);
                this.curJoystick.Acquire();
                this.comboBoxDevice.Enabled = false;
                this.buttonDetect.Enabled = false;
                this.buttonConnect.Enabled = false;
                this.timerHandle.Enabled = true;
                this.timerGame.Enabled = true;
            } catch {
                int num = (int)MessageBox.Show("Handle not selected");
                this.comboBoxDevice.Enabled = true;
                this.buttonDetect.Enabled = true;
                this.buttonConnect.Enabled = true;
                this.timerHandle.Enabled = false;
                this.timerGame.Enabled = false;
            }
        }

        private void timerHandle_Tick(object sender, EventArgs e) {
            try {
                JoystickState currentState = curJoystick.GetCurrentState();
                this.nowPov = currentState.PointOfViewControllers[0];
                Array.ConstrainedCopy(currentState.Buttons, 0, nowButtons, 0, 14);
                if (currentState.Y < 640) {
                    this.nowState = "EB";
                    this.nowValue = 9;
                } else if (currentState.Y < 3072) {
                    this.nowState = "B";
                    this.nowValue = 8;
                } else if (currentState.Y < 6528) {
                    this.nowState = "B";
                    this.nowValue = 7;
                } else if (currentState.Y < 9984) {
                    this.nowState = "B";
                    this.nowValue = 6;
                } else if (currentState.Y < 13568) {
                    this.nowState = "B";
                    this.nowValue = 5;
                } else if (currentState.Y < 17023) {
                    this.nowState = "B";
                    this.nowValue = 4;
                } else if (currentState.Y < 20479) {
                    this.nowState = "B";
                    this.nowValue = 3;
                } else if (currentState.Y < 24063) {
                    this.nowState = "B";
                    this.nowValue = 2;
                } else if (currentState.Y < 29311) {
                    this.nowState = "B";
                    this.nowValue = 1;
                } else if (currentState.Y < 36668) {
                    this.nowState = "N";
                    this.nowValue = 0;
                } else if (currentState.Y < 43690) {
                    this.nowState = "P";
                    this.nowValue = 1;
                } else if (currentState.Y < 49801) {
                    this.nowState = "P";
                    this.nowValue = 2;
                } else if (currentState.Y < 55913) {
                    this.nowState = "P";
                    this.nowValue = 3;
                } else if (currentState.Y < 62284) {
                    this.nowState = "P";
                    this.nowValue = 4;
                } else {
                    this.nowState = "P";
                    this.nowValue = 5;
                }
                if (this.nowState != this.lastState || this.nowValue != this.lastValue) {
                    this.skipZL = this.nowState == "EB";
                    this.labelZL.Visible = !skipZL;
                    this.comboBoxZL.Visible = !skipZL;
                    switch (this.nowState) {
                        case "EB":
                            if (this.radioStdMode.Checked)
                                this.keyAct.Add(0);
                            else if (this.radioKihaMode.Checked)
                                this.keyAct.Add(8);
                            this.labelHandle.Text = "EB";
                            this.labelHandle.BackColor = Color.Red;
                            this.labelKihaHandle.Text = "Emergency";
                            this.labelKihaHandle.BackColor = Color.Red;                            
                            break;
                        case "B":
                            if (this.radioStdMode.Checked) {
                                switch (this.lastState) {
                                    case "EB":
                                        for (int index = 0; index < 9 - this.nowValue; ++index)
                                            this.keyAct.Add(2);
                                        break;
                                    case "B":
                                        if (this.nowValue > this.lastValue) {
                                            for (int index = 0; index < this.nowValue - this.lastValue; ++index)
                                                this.keyAct.Add(1);
                                            break;
                                        }
                                        for (int index = 0; index < this.lastValue - this.nowValue; ++index)
                                            this.keyAct.Add(2);
                                        break;
                                    default:
                                        this.keyAct.Add(3);
                                        for (int index = 0; index < this.nowValue; ++index)
                                            this.keyAct.Add(1);
                                        break;
                                }
                            } else if (this.radioKihaMode.Checked) {
                                if (this.nowValue >= 5 && this.nowValue <= 8) {
                                    this.keyAct.Add(8);
                                    this.labelKihaHandle.Text = "Emergency";
                                    this.labelKihaHandle.BackColor = Color.Red;
                                } else if (this.nowValue >= 2) {
                                    this.keyAct.Add(7);
                                    this.labelKihaHandle.Text = "Service";
                                    this.labelKihaHandle.BackColor = Color.Orange;
                                } else if (this.nowValue == 1) {
                                    this.keyAct.Add(6);
                                    this.labelKihaHandle.Text = "Lapped";
                                    this.labelKihaHandle.BackColor = Color.Yellow;
                                }
                            }
                            this.labelHandle.Text = "B" + this.nowValue.ToString();
                            this.labelHandle.BackColor = Color.Orange;
                            break;
                        case "N":
                            this.keyAct.Add(3);
                            this.labelHandle.Text = "N";
                            this.labelHandle.BackColor = Color.LimeGreen;
                            this.labelKihaHandle.Text = "Running";
                            this.labelKihaHandle.BackColor = Color.LimeGreen;
                            break;
                        case "P":
                            if (this.lastState == "P") {
                                if (this.nowValue > this.lastValue) {
                                    for (int index = 0; index < this.nowValue - this.lastValue; ++index)
                                        this.keyAct.Add(5);
                                } else {
                                    for (int index = 0; index < this.lastValue - this.nowValue; ++index)
                                        this.keyAct.Add(4);
                                }
                            } else {
                                this.keyAct.Add(3);
                                for (int index = 0; index < this.nowValue; ++index)
                                    this.keyAct.Add(5);
                            }
                            this.labelHandle.Text = "P" + this.nowValue.ToString();
                            this.labelHandle.BackColor = Color.DeepSkyBlue;
                            this.labelKihaHandle.Text = "P" + this.nowValue.ToString();
                            this.labelKihaHandle.BackColor = Color.DeepSkyBlue;
                            break;
                    }
                    this.lastState = this.nowState;
                    this.lastValue = this.nowValue;
                }
                if (this.keyAct.Count != 0) {
                    if (this.isgameON) {
                        switch (this.keyAct[0]) {
                            case 0:
                                // Send / (Emergency)
                                PostMessage(this.gamePID, 256U, 191, 0);
                                PostMessage(this.gamePID, 257U, 191, 0);
                                break;
                            case 1:
                                // Send . (Braking)
                                PostMessage(this.gamePID, 256U, 190, 0);
                                PostMessage(this.gamePID, 257U, 190, 0);
                                break;
                            case 2:
                                // Send , (Release brakes)
                                PostMessage(this.gamePID, 256U, 188, 0);
                                PostMessage(this.gamePID, 257U, 188, 0);
                                break;
                            case 3:
                                // Send S + M (no throttle + no brakes)
                                PostMessage(this.gamePID, 256U, 83, 0);
                                PostMessage(this.gamePID, 257U, 83, 0);
                                PostMessage(this.gamePID, 256U, 77, 0);
                                PostMessage(this.gamePID, 257U, 77, 0);
                                break;
                            case 4:
                                // Send A (Throttle down)
                                PostMessage(this.gamePID, 256U, 65, 0);
                                PostMessage(this.gamePID, 257U, 65, 0);
                                break;
                            case 5:
                                // Send Z (Throttle up)
                                PostMessage(this.gamePID, 256U, 90, 0);
                                PostMessage(this.gamePID, 257U, 90, 0);
                                break;
                            case 6:
                                // Send S + , (no throttle + Release brakes)
                                PostMessage(this.gamePID, 256U, 83, 0);
                                PostMessage(this.gamePID, 257U, 83, 0);
                                PostMessage(this.gamePID, 256U, 188, 0);
                                PostMessage(this.gamePID, 257U, 188, 0);
                                break;
                            case 7:
                                // Send S + . (no throttle + Braking)
                                PostMessage(this.gamePID, 256U, 83, 0);
                                PostMessage(this.gamePID, 257U, 83, 0);
                                PostMessage(this.gamePID, 256U, 190, 0);
                                PostMessage(this.gamePID, 257U, 190, 0);
                                break;
                            case 8:
                                // Send S + / (no throttle + Emergency)
                                PostMessage(this.gamePID, 256U, 83, 0);
                                PostMessage(this.gamePID, 257U, 83, 0);
                                PostMessage(this.gamePID, 256U, 191, 0);
                                PostMessage(this.gamePID, 257U, 191, 0);
                                break;
                        }
                    }
                    this.keyAct.RemoveAt(0);
                }
                if (this.nowPov != this.lastPov) {
                    switch (this.nowPov) {
                        case 0:
                            SendKey(true, labelUp, comboBoxUp);
                            SendKey(false, labelRight, comboBoxRight);
                            SendKey(false, labelDown, comboBoxDown);
                            SendKey(false, labelLeft, comboBoxLeft);
                            break;
                        case 4500:
                            SendKey(true, labelUp, comboBoxUp);
                            SendKey(true, labelRight, comboBoxRight);
                            SendKey(false, labelDown, comboBoxDown);
                            SendKey(false, labelLeft, comboBoxLeft);
                            break;
                        case 9000:
                            SendKey(false, labelUp, comboBoxUp);
                            SendKey(true, labelRight, comboBoxRight);
                            SendKey(false, labelDown, comboBoxDown);
                            SendKey(false, labelLeft, comboBoxLeft);
                            break;
                        case 13500:
                            SendKey(false, labelUp, comboBoxUp);
                            SendKey(true, labelRight, comboBoxRight);
                            SendKey(true, labelDown, comboBoxDown);
                            SendKey(false, labelLeft, comboBoxLeft);
                            break;
                        case 18000:
                            SendKey(false, labelUp, comboBoxUp);
                            SendKey(false, labelRight, comboBoxRight);
                            SendKey(true, labelDown, comboBoxDown);
                            SendKey(false, labelLeft, comboBoxLeft);
                            break;
                        case 22500:
                            SendKey(false, labelUp, comboBoxUp);
                            SendKey(false, labelRight, comboBoxRight);
                            SendKey(true, labelDown, comboBoxDown);
                            SendKey(true, labelLeft, comboBoxLeft);
                            break;
                        case 27000:
                            SendKey(false, labelUp, comboBoxUp);
                            SendKey(false, labelRight, comboBoxRight);
                            SendKey(false, labelDown, comboBoxDown);
                            SendKey(true, labelLeft, comboBoxLeft);
                            break;
                        case 31500:
                            SendKey(true, labelUp, comboBoxUp);
                            SendKey(false, labelRight, comboBoxRight);
                            SendKey(false, labelDown, comboBoxDown);
                            SendKey(true, labelLeft, comboBoxLeft);
                            break;
                        default:
                            SendKey(false, labelUp, comboBoxUp);
                            SendKey(false, labelRight, comboBoxRight);
                            SendKey(false, labelDown, comboBoxDown);
                            SendKey(false, labelLeft, comboBoxLeft);
                            break;
                    }
                    this.lastPov = this.nowPov;
                }
                if (this.nowButtons[0] != this.lastButtons[0]) {
                    SendKey(this.nowButtons[0], labelY, comboBoxY);
                    this.lastButtons[0] = this.nowButtons[0];
                }
                if (this.nowButtons[1] != this.lastButtons[1]) {
                    SendKey(this.nowButtons[1], labelB, comboBoxB);
                    this.lastButtons[1] = this.nowButtons[1];
                }
                if (this.nowButtons[2] != this.lastButtons[2]) {
                    SendKey(this.nowButtons[2], labelA, comboBoxA);
                    this.lastButtons[2] = this.nowButtons[2];
                }
                if (this.nowButtons[3] != this.lastButtons[3]) {
                    SendKey(this.nowButtons[3], labelX, comboBoxX);
                    this.lastButtons[3] = this.nowButtons[3];
                }
                if (this.nowButtons[4] != this.lastButtons[4]) {
                    SendKey(this.nowButtons[4], labelL, comboBoxL);
                    this.lastButtons[4] = this.nowButtons[4];
                }
                if (this.nowButtons[5] != this.lastButtons[5]) {
                    SendKey(this.nowButtons[5], labelR, comboBoxR);
                    this.lastButtons[5] = this.nowButtons[5];
                }
                if (this.nowButtons[6] != this.lastButtons[6] && !this.skipZL) {
                    SendKey(this.nowButtons[6], labelZL, comboBoxZL);
                    this.lastButtons[6] = this.nowButtons[6];
                }
                if (this.nowButtons[7] != this.lastButtons[7]) {
                    SendKey(this.nowButtons[7], labelZR, comboBoxZR);
                    this.lastButtons[7] = this.nowButtons[7];
                }
                if (this.nowButtons[8] != this.lastButtons[8]) {
                    SendKey(this.nowButtons[8], labelMinus, comboBoxMinus);
                    this.lastButtons[8] = this.nowButtons[8];
                }
                if (this.nowButtons[9] != this.lastButtons[9]) {
                    SendKey(this.nowButtons[9], labelPlus, comboBoxPlus);
                    this.lastButtons[9] = this.nowButtons[9];
                }
                if (this.nowButtons[12] != this.lastButtons[12]) {
                    SendKey(this.nowButtons[12], labelHome, comboBoxHome);
                    this.lastButtons[12] = this.nowButtons[12];
                }
                if (this.nowButtons[13] != this.lastButtons[13]) {
                    SendKey(this.nowButtons[13], labelCapture, comboBoxCapture);
                    this.lastButtons[13] = this.nowButtons[13];
                }
                GC.Collect();
            } catch {
                this.timerHandle.Enabled = false;
                this.timerGame.Enabled = false;
                this.timerHorn.Enabled = false;
                MessageBox.Show("Communication error. Check the connection and restart.");
                Environment.Exit(0);
            }
        }

        private void timerGame_Tick(object sender, EventArgs e) {
            try {
                Process[] processesByName = Process.GetProcessesByName("JREAST_TrainSimulator");
                if (processesByName.Length != 0) {
                    foreach (Process process in processesByName) {
                        if (this.gamePID != process.MainWindowHandle)
                            this.gamePID = process.MainWindowHandle;
                        this.isgameON = true;
                    }
                } else
                    this.isgameON = false;
            } catch {
                this.timerHandle.Enabled = false;
                this.timerGame.Enabled = false;
                this.timerHorn.Enabled = false;
                MessageBox.Show("Error when detecting the game. Please restart and try again.");
                Environment.Exit(0);
            }
        }

        private void timerHorn_Tick(object sender, EventArgs e) {
            PostMessage(this.gamePID, 256U, 8, 0);
            this.timerHorn.Enabled = false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) => Environment.Exit(0);

        private void radioStdMode_CheckedChanged(object sender, EventArgs e) {
            if (this.radioStdMode.Checked) {
                this.labelKihaArrow.Visible = false;
                this.labelKihaHandle.Visible = false;
            } else {
                this.labelKihaArrow.Visible = true;
                this.labelKihaHandle.Visible = true;
            }

            ReadIniFile();
        }

        private void Form1_Load(object sender, EventArgs e) {
            this.labelKihaArrow.Visible = false;
            this.labelKihaHandle.Visible = false;
            this.iniFile = new IniFile("config.ini");
            ReadIniFile();
        }

        protected override void Dispose(bool disposing) {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            this.components = new Container();
            this.labelB = new Label();
            this.labelA = new Label();
            this.labelY = new Label();
            this.labelX = new Label();
            this.labelDown = new Label();
            this.labelRight = new Label();
            this.labelLeft = new Label();
            this.labelUp = new Label();
            this.labelHome = new Label();
            this.labelCapture = new Label();
            this.labelR = new Label();
            this.labelPlus = new Label();
            this.labelMinus = new Label();
            this.labelL = new Label();
            this.labelZR = new Label();
            this.labelCredits = new Label();
            this.labelHandle = new Label();
            this.buttonConnect = new Button();
            this.buttonDetect = new Button();
            this.comboBoxDevice = new ComboBox();
            this.labelSelectHandle = new Label();
            this.timerHandle = new Timer(this.components);
            this.timerGame = new Timer(this.components);
            this.timerHorn = new Timer(this.components);
            this.radioStdMode = new RadioButton();
            this.radioKihaMode = new RadioButton();
            this.labelKihaArrow = new Label();
            this.labelKihaHandle = new Label();
            this.comboBoxL = new ComboBox();
            this.labelZL = new Label();
            this.comboBoxZL = new ComboBox();
            this.comboBoxR = new ComboBox();
            this.comboBoxZR = new ComboBox();
            this.comboBoxMinus = new ComboBox();
            this.comboBoxPlus = new ComboBox();
            this.comboBoxCapture = new ComboBox();
            this.comboBoxHome = new ComboBox();
            this.comboBoxRight = new ComboBox();
            this.comboBoxLeft = new ComboBox();
            this.comboBoxDown = new ComboBox();
            this.comboBoxUp = new ComboBox();
            this.comboBoxY = new ComboBox();
            this.comboBoxX = new ComboBox();
            this.comboBoxB = new ComboBox();
            this.comboBoxA = new ComboBox();
            this.SuspendLayout();
            // 
            // labelB
            // 
            this.labelB.AutoSize = true;
            this.labelB.Location = new Point(228, 292);
            this.labelB.Name = "labelB";
            this.labelB.Size = new Size(14, 13);
            this.labelB.TabIndex = 63;
            this.labelB.Text = "B";
            // 
            // labelA
            // 
            this.labelA.AutoSize = true;
            this.labelA.Location = new Point(228, 265);
            this.labelA.Name = "labelA";
            this.labelA.Size = new Size(14, 13);
            this.labelA.TabIndex = 62;
            this.labelA.Text = "A";
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.Location = new Point(228, 346);
            this.labelY.Name = "labelY";
            this.labelY.Size = new Size(14, 13);
            this.labelY.TabIndex = 61;
            this.labelY.Text = "Y";
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Location = new Point(228, 319);
            this.labelX.Name = "labelX";
            this.labelX.Size = new Size(14, 13);
            this.labelX.TabIndex = 60;
            this.labelX.Text = "X";
            // 
            // labelDown
            // 
            this.labelDown.AutoSize = true;
            this.labelDown.Location = new Point(23, 319);
            this.labelDown.Name = "labelDown";
            this.labelDown.Size = new Size(13, 13);
            this.labelDown.TabIndex = 59;
            this.labelDown.Text = "↓";
            // 
            // labelRight
            // 
            this.labelRight.AutoSize = true;
            this.labelRight.Location = new Point(21, 292);
            this.labelRight.Name = "labelRight";
            this.labelRight.Size = new Size(18, 13);
            this.labelRight.TabIndex = 58;
            this.labelRight.Text = "→";
            // 
            // labelLeft
            // 
            this.labelLeft.AutoSize = true;
            this.labelLeft.Location = new Point(18, 346);
            this.labelLeft.Name = "labelLeft";
            this.labelLeft.Size = new Size(18, 13);
            this.labelLeft.TabIndex = 57;
            this.labelLeft.Text = "←";
            // 
            // labelUp
            // 
            this.labelUp.AutoSize = true;
            this.labelUp.BackColor = SystemColors.Control;
            this.labelUp.Location = new Point(23, 265);
            this.labelUp.Name = "labelUp";
            this.labelUp.Size = new Size(13, 13);
            this.labelUp.TabIndex = 56;
            this.labelUp.Text = "↑";
            // 
            // labelHome
            // 
            this.labelHome.AutoSize = true;
            this.labelHome.Location = new Point(226, 238);
            this.labelHome.Name = "labelHome";
            this.labelHome.Size = new Size(16, 13);
            this.labelHome.TabIndex = 55;
            this.labelHome.Text = "▲";
            // 
            // labelCapture
            // 
            this.labelCapture.AutoSize = true;
            this.labelCapture.Location = new Point(21, 238);
            this.labelCapture.Name = "labelCapture";
            this.labelCapture.Size = new Size(16, 13);
            this.labelCapture.TabIndex = 54;
            this.labelCapture.Text = "■";
            // 
            // labelR
            // 
            this.labelR.AutoSize = true;
            this.labelR.Location = new Point(227, 157);
            this.labelR.Name = "labelR";
            this.labelR.Size = new Size(15, 13);
            this.labelR.TabIndex = 53;
            this.labelR.Text = "R";
            // 
            // labelPlus
            // 
            this.labelPlus.AutoSize = true;
            this.labelPlus.Location = new Point(228, 211);
            this.labelPlus.Name = "labelPlus";
            this.labelPlus.Size = new Size(13, 13);
            this.labelPlus.TabIndex = 52;
            this.labelPlus.Text = "+";
            // 
            // labelMinus
            // 
            this.labelMinus.AutoSize = true;
            this.labelMinus.Location = new Point(25, 211);
            this.labelMinus.Name = "labelMinus";
            this.labelMinus.Size = new Size(10, 13);
            this.labelMinus.TabIndex = 51;
            this.labelMinus.Text = "-";
            // 
            // labelL
            // 
            this.labelL.AutoSize = true;
            this.labelL.Location = new Point(23, 157);
            this.labelL.Name = "labelL";
            this.labelL.Size = new Size(13, 13);
            this.labelL.TabIndex = 50;
            this.labelL.Text = "L";
            // 
            // labelZR
            // 
            this.labelZR.AutoSize = true;
            this.labelZR.Location = new Point(220, 184);
            this.labelZR.Name = "labelZR";
            this.labelZR.Size = new Size(22, 13);
            this.labelZR.TabIndex = 49;
            this.labelZR.Text = "ZR";
            // 
            // labelCredits
            // 
            this.labelCredits.AutoSize = true;
            this.labelCredits.Location = new Point(15, 10);
            this.labelCredits.Name = "labelCredits";
            this.labelCredits.Size = new Size(141, 13);
            this.labelCredits.TabIndex = 48;
            this.labelCredits.Text = "by:客电直幺拐二/cracrayol";
            // 
            // labelHandle
            // 
            this.labelHandle.AutoSize = true;
            this.labelHandle.Font = new Font("Microsoft YaHei", 21.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.labelHandle.Location = new Point(10, 107);
            this.labelHandle.Name = "labelHandle";
            this.labelHandle.Size = new Size(64, 39);
            this.labelHandle.TabIndex = 47;
            this.labelHandle.Text = "NA";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Enabled = false;
            this.buttonConnect.Location = new Point(340, 28);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new Size(67, 25);
            this.buttonConnect.TabIndex = 45;
            this.buttonConnect.Text = "2. Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new EventHandler(this.button2_Click);
            // 
            // buttonDetect
            // 
            this.buttonDetect.Location = new Point(269, 28);
            this.buttonDetect.Name = "buttonDetect";
            this.buttonDetect.Size = new Size(65, 25);
            this.buttonDetect.TabIndex = 44;
            this.buttonDetect.Text = "1. Detect";
            this.buttonDetect.UseVisualStyleBackColor = true;
            this.buttonDetect.Click += new EventHandler(this.button1_Click);
            // 
            // comboBoxDevice
            // 
            this.comboBoxDevice.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxDevice.FormattingEnabled = true;
            this.comboBoxDevice.Location = new Point(15, 59);
            this.comboBoxDevice.Name = "comboBoxDevice";
            this.comboBoxDevice.Size = new Size(392, 21);
            this.comboBoxDevice.TabIndex = 43;
            // 
            // labelSelectHandle
            // 
            this.labelSelectHandle.AutoSize = true;
            this.labelSelectHandle.Location = new Point(15, 34);
            this.labelSelectHandle.Name = "labelSelectHandle";
            this.labelSelectHandle.Size = new Size(78, 13);
            this.labelSelectHandle.TabIndex = 42;
            this.labelSelectHandle.Text = "Select device：";
            // 
            // timerHandle
            // 
            this.timerHandle.Interval = 1;
            this.timerHandle.Tick += new EventHandler(this.timerHandle_Tick);
            // 
            // timerGame
            // 
            this.timerGame.Interval = 3000;
            this.timerGame.Tick += new EventHandler(this.timerGame_Tick);
            // 
            // timerHorn
            // 
            this.timerHorn.Interval = 1000;
            this.timerHorn.Tick += new EventHandler(this.timerHorn_Tick);
            // 
            // radioStdMode
            // 
            this.radioStdMode.AutoSize = true;
            this.radioStdMode.Checked = true;
            this.radioStdMode.Location = new Point(17, 87);
            this.radioStdMode.Name = "radioStdMode";
            this.radioStdMode.Size = new Size(97, 17);
            this.radioStdMode.TabIndex = 80;
            this.radioStdMode.TabStop = true;
            this.radioStdMode.Text = "Standard mode";
            this.radioStdMode.UseVisualStyleBackColor = true;
            this.radioStdMode.CheckedChanged += new EventHandler(this.radioStdMode_CheckedChanged);
            // 
            // radioKihaMode
            // 
            this.radioKihaMode.AutoSize = true;
            this.radioKihaMode.Location = new Point(118, 87);
            this.radioKihaMode.Name = "radioKihaMode";
            this.radioKihaMode.Size = new Size(90, 17);
            this.radioKihaMode.TabIndex = 81;
            this.radioKihaMode.Text = "Kiha 54 mode";
            this.radioKihaMode.UseVisualStyleBackColor = true;
            // 
            // labelKihaArrow
            // 
            this.labelKihaArrow.AutoSize = true;
            this.labelKihaArrow.Font = new Font("Microsoft YaHei", 21.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.labelKihaArrow.Location = new Point(66, 107);
            this.labelKihaArrow.Name = "labelKihaArrow";
            this.labelKihaArrow.Size = new Size(46, 39);
            this.labelKihaArrow.TabIndex = 82;
            this.labelKihaArrow.Text = "→";
            // 
            // labelKihaHandle
            // 
            this.labelKihaHandle.AutoSize = true;
            this.labelKihaHandle.Font = new Font("Microsoft YaHei", 21.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.labelKihaHandle.Location = new Point(108, 107);
            this.labelKihaHandle.Name = "labelKihaHandle";
            this.labelKihaHandle.Size = new Size(64, 39);
            this.labelKihaHandle.TabIndex = 83;
            this.labelKihaHandle.Text = "NA";
            // 
            // comboBoxL
            // 
            this.comboBoxL.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxL.FormattingEnabled = true;
            this.comboBoxL.Location = new Point(42, 154);
            this.comboBoxL.Name = "comboBoxL";
            this.comboBoxL.Size = new Size(159, 21);
            this.comboBoxL.TabIndex = 84;
            this.comboBoxL.SelectedIndexChanged += new EventHandler(this.SelectedIndexChanged);
            this.comboBoxL.DataSource = new BindingSource(this.commands, null);
            this.comboBoxL.ValueMember = "Key";
            this.comboBoxL.DisplayMember = "Value";
            // 
            // labelZL
            // 
            this.labelZL.AutoSize = true;
            this.labelZL.Location = new Point(16, 184);
            this.labelZL.Name = "labelZL";
            this.labelZL.Size = new Size(20, 13);
            this.labelZL.TabIndex = 85;
            this.labelZL.Text = "ZL";
            // 
            // comboBoxZL
            // 
            this.comboBoxZL.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxZL.FormattingEnabled = true;
            this.comboBoxZL.Location = new Point(42, 181);
            this.comboBoxZL.Name = "comboBoxZL";
            this.comboBoxZL.Size = new Size(159, 21);
            this.comboBoxZL.TabIndex = 86;
            this.comboBoxZL.SelectedIndexChanged += new EventHandler(this.SelectedIndexChanged);
            this.comboBoxZL.DataSource = new BindingSource(this.commands, null);
            this.comboBoxZL.ValueMember = "Key";
            this.comboBoxZL.DisplayMember = "Value";
            // 
            // comboBoxR
            // 
            this.comboBoxR.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxR.FormattingEnabled = true;
            this.comboBoxR.Location = new Point(248, 154);
            this.comboBoxR.Name = "comboBoxR";
            this.comboBoxR.Size = new Size(159, 21);
            this.comboBoxR.TabIndex = 87;
            this.comboBoxR.SelectedIndexChanged += new EventHandler(this.SelectedIndexChanged);
            this.comboBoxR.DataSource = new BindingSource(this.commands, null);
            this.comboBoxR.ValueMember = "Key";
            this.comboBoxR.DisplayMember = "Value";
            // 
            // comboBoxZR
            // 
            this.comboBoxZR.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxZR.FormattingEnabled = true;
            this.comboBoxZR.Location = new Point(248, 181);
            this.comboBoxZR.Name = "comboBoxZR";
            this.comboBoxZR.Size = new Size(159, 21);
            this.comboBoxZR.TabIndex = 88;
            this.comboBoxZR.SelectedIndexChanged += new EventHandler(this.SelectedIndexChanged);
            this.comboBoxZR.DataSource = new BindingSource(this.commands, null);
            this.comboBoxZR.ValueMember = "Key";
            this.comboBoxZR.DisplayMember = "Value";
            // 
            // comboBoxMinus
            // 
            this.comboBoxMinus.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxMinus.FormattingEnabled = true;
            this.comboBoxMinus.Location = new Point(42, 208);
            this.comboBoxMinus.Name = "comboBoxMinus";
            this.comboBoxMinus.Size = new Size(159, 21);
            this.comboBoxMinus.TabIndex = 89;
            this.comboBoxMinus.SelectedIndexChanged += new EventHandler(this.SelectedIndexChanged);
            this.comboBoxMinus.DataSource = new BindingSource(this.commands, null);
            this.comboBoxMinus.ValueMember = "Key";
            this.comboBoxMinus.DisplayMember = "Value";
            // 
            // comboBoxPlus
            // 
            this.comboBoxPlus.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxPlus.FormattingEnabled = true;
            this.comboBoxPlus.Location = new Point(248, 208);
            this.comboBoxPlus.Name = "comboBoxPlus";
            this.comboBoxPlus.Size = new Size(159, 21);
            this.comboBoxPlus.TabIndex = 90;
            this.comboBoxPlus.SelectedIndexChanged += new EventHandler(this.SelectedIndexChanged);
            this.comboBoxPlus.DataSource = new BindingSource(this.commands, null);
            this.comboBoxPlus.ValueMember = "Key";
            this.comboBoxPlus.DisplayMember = "Value";
            // 
            // comboBoxCapture
            // 
            this.comboBoxCapture.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxCapture.FormattingEnabled = true;
            this.comboBoxCapture.Location = new Point(42, 235);
            this.comboBoxCapture.Name = "comboBoxCapture";
            this.comboBoxCapture.Size = new Size(159, 21);
            this.comboBoxCapture.TabIndex = 91;
            this.comboBoxCapture.SelectedIndexChanged += new EventHandler(this.SelectedIndexChanged);
            this.comboBoxCapture.DataSource = new BindingSource(this.commands, null);
            this.comboBoxCapture.ValueMember = "Key";
            this.comboBoxCapture.DisplayMember = "Value";
            // 
            // comboBoxHome
            // 
            this.comboBoxHome.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxHome.FormattingEnabled = true;
            this.comboBoxHome.Location = new Point(248, 235);
            this.comboBoxHome.Name = "comboBoxHome";
            this.comboBoxHome.Size = new Size(159, 21);
            this.comboBoxHome.TabIndex = 92;
            this.comboBoxHome.SelectedIndexChanged += new EventHandler(this.SelectedIndexChanged);
            this.comboBoxHome.DataSource = new BindingSource(this.commands, null);
            this.comboBoxHome.ValueMember = "Key";
            this.comboBoxHome.DisplayMember = "Value";
            // 
            // comboBoxRight
            // 
            this.comboBoxRight.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxRight.FormattingEnabled = true;
            this.comboBoxRight.Location = new Point(42, 289);
            this.comboBoxRight.Name = "comboBoxRight";
            this.comboBoxRight.Size = new Size(159, 21);
            this.comboBoxRight.TabIndex = 93;
            this.comboBoxRight.SelectedIndexChanged += new EventHandler(this.SelectedIndexChanged);
            this.comboBoxRight.DataSource = new BindingSource(this.commands, null);
            this.comboBoxRight.ValueMember = "Key";
            this.comboBoxRight.DisplayMember = "Value";
            // 
            // comboBoxLeft
            // 
            this.comboBoxLeft.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxLeft.FormattingEnabled = true;
            this.comboBoxLeft.Location = new Point(42, 343);
            this.comboBoxLeft.Name = "comboBoxLeft";
            this.comboBoxLeft.Size = new Size(159, 21);
            this.comboBoxLeft.TabIndex = 94;
            this.comboBoxLeft.SelectedIndexChanged += new EventHandler(this.SelectedIndexChanged);
            this.comboBoxLeft.DataSource = new BindingSource(this.commands, null);
            this.comboBoxLeft.ValueMember = "Key";
            this.comboBoxLeft.DisplayMember = "Value";
            // 
            // comboBoxDown
            // 
            this.comboBoxDown.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxDown.FormattingEnabled = true;
            this.comboBoxDown.Location = new Point(42, 316);
            this.comboBoxDown.Name = "comboBoxDown";
            this.comboBoxDown.Size = new Size(159, 21);
            this.comboBoxDown.TabIndex = 95;
            this.comboBoxDown.SelectedIndexChanged += new EventHandler(this.SelectedIndexChanged);
            this.comboBoxDown.DataSource = new BindingSource(this.commands, null);
            this.comboBoxDown.ValueMember = "Key";
            this.comboBoxDown.DisplayMember = "Value";
            // 
            // comboBoxUp
            // 
            this.comboBoxUp.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxUp.FormattingEnabled = true;
            this.comboBoxUp.Location = new Point(42, 262);
            this.comboBoxUp.Name = "comboBoxUp";
            this.comboBoxUp.Size = new Size(159, 21);
            this.comboBoxUp.TabIndex = 96;
            this.comboBoxUp.SelectedIndexChanged += new EventHandler(this.SelectedIndexChanged);
            this.comboBoxUp.DataSource = new BindingSource(this.commands, null);
            this.comboBoxUp.ValueMember = "Key";
            this.comboBoxUp.DisplayMember = "Value";
            // 
            // comboBoxY
            // 
            this.comboBoxY.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxY.FormattingEnabled = true;
            this.comboBoxY.Location = new Point(248, 343);
            this.comboBoxY.Name = "comboBoxY";
            this.comboBoxY.Size = new Size(159, 21);
            this.comboBoxY.TabIndex = 97;
            this.comboBoxY.SelectedIndexChanged += new EventHandler(this.SelectedIndexChanged);
            this.comboBoxY.DataSource = new BindingSource(this.commands, null);
            this.comboBoxY.ValueMember = "Key";
            this.comboBoxY.DisplayMember = "Value";
            // 
            // comboBoxX
            // 
            this.comboBoxX.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxX.FormattingEnabled = true;
            this.comboBoxX.Location = new Point(248, 316);
            this.comboBoxX.Name = "comboBoxX";
            this.comboBoxX.Size = new Size(159, 21);
            this.comboBoxX.TabIndex = 98;
            this.comboBoxX.SelectedIndexChanged += new EventHandler(this.SelectedIndexChanged);
            this.comboBoxX.DataSource = new BindingSource(this.commands, null);
            this.comboBoxX.ValueMember = "Key";
            this.comboBoxX.DisplayMember = "Value";
            // 
            // comboBoxB
            // 
            this.comboBoxB.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxB.FormattingEnabled = true;
            this.comboBoxB.Location = new Point(248, 289);
            this.comboBoxB.Name = "comboBoxB";
            this.comboBoxB.Size = new Size(159, 21);
            this.comboBoxB.TabIndex = 99;
            this.comboBoxB.SelectedIndexChanged += new EventHandler(this.SelectedIndexChanged);
            this.comboBoxB.DataSource = new BindingSource(this.commands, null);
            this.comboBoxB.ValueMember = "Key";
            this.comboBoxB.DisplayMember = "Value";
            // 
            // comboBoxA
            // 
            this.comboBoxA.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxA.FormattingEnabled = true;
            this.comboBoxA.Location = new Point(248, 262);
            this.comboBoxA.Name = "comboBoxA";
            this.comboBoxA.Size = new Size(159, 21);
            this.comboBoxA.TabIndex = 100;
            this.comboBoxA.SelectedIndexChanged += new EventHandler(this.SelectedIndexChanged);
            this.comboBoxA.DataSource = new BindingSource(this.commands, null);
            this.comboBoxA.ValueMember = "Key";
            this.comboBoxA.DisplayMember = "Value";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(420, 375);
            this.Controls.Add(this.comboBoxA);
            this.Controls.Add(this.comboBoxB);
            this.Controls.Add(this.comboBoxX);
            this.Controls.Add(this.comboBoxY);
            this.Controls.Add(this.comboBoxUp);
            this.Controls.Add(this.comboBoxDown);
            this.Controls.Add(this.comboBoxLeft);
            this.Controls.Add(this.comboBoxRight);
            this.Controls.Add(this.comboBoxHome);
            this.Controls.Add(this.comboBoxCapture);
            this.Controls.Add(this.comboBoxPlus);
            this.Controls.Add(this.comboBoxMinus);
            this.Controls.Add(this.comboBoxZR);
            this.Controls.Add(this.comboBoxR);
            this.Controls.Add(this.comboBoxZL);
            this.Controls.Add(this.labelZL);
            this.Controls.Add(this.comboBoxL);
            this.Controls.Add(this.labelKihaHandle);
            this.Controls.Add(this.labelKihaArrow);
            this.Controls.Add(this.radioKihaMode);
            this.Controls.Add(this.radioStdMode);
            this.Controls.Add(this.labelB);
            this.Controls.Add(this.labelA);
            this.Controls.Add(this.labelY);
            this.Controls.Add(this.labelX);
            this.Controls.Add(this.labelDown);
            this.Controls.Add(this.labelRight);
            this.Controls.Add(this.labelLeft);
            this.Controls.Add(this.labelUp);
            this.Controls.Add(this.labelHome);
            this.Controls.Add(this.labelCapture);
            this.Controls.Add(this.labelR);
            this.Controls.Add(this.labelPlus);
            this.Controls.Add(this.labelMinus);
            this.Controls.Add(this.labelL);
            this.Controls.Add(this.labelZR);
            this.Controls.Add(this.labelCredits);
            this.Controls.Add(this.labelHandle);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.buttonDetect);
            this.Controls.Add(this.comboBoxDevice);
            this.Controls.Add(this.labelSelectHandle);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "ZUIKI controller to JRE";
            this.FormClosed += new FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        public class JKinf {
            private Guid _devguid;

            public JKinf(Guid Devguid) => _devguid = Devguid;

            public Guid Devguid => _devguid;
        }

        /// <summary>
        /// Write the selected value of the combobox in the ini file.
        /// </summary>
        private void SelectedIndexChanged(object sender, EventArgs e) {
            ComboBox combo = (ComboBox)sender;
            string mode = this.radioKihaMode.Checked ? "KIHA54" : "STANDARD";
            this.iniFile.Write(combo.Name.Remove(0, 8),( (int)combo.SelectedValue).ToString(), mode);
        }

        /// <summary>
        /// Read the given key from the ini file and return a Key/Value pair.
        /// </summary>
        private KeyValuePair<int, string> GetConfValue(string keyName) {
            string mode = this.radioKihaMode.Checked ? "KIHA54" : "STANDARD";
            string value = this.iniFile.Read(keyName, mode);
            int key = value != "" ? Int32.Parse(value) : 0;
            return new KeyValuePair<int, string>(key, this.commands[key]);
        }

        /// <summary>
        /// Send the selected key to the game (down or up) and change label backgroung color
        /// </summary>
        private void SendKey(bool isDown, Label label, ComboBox combo) {
            if (isDown) {
                label.BackColor = Color.LimeGreen;
                if (this.isgameON) {
                    PostMessage(this.gamePID, 256U, (int)combo.SelectedValue, 0);

                    // If horn lvl 1 (enter) is used, enable the timer for horn lvl 2.
                    if ((int)combo.SelectedValue == 13)
                        this.timerHorn.Enabled = true;
                }
            } else {
                label.BackColor = SystemColors.Control;
                if (this.isgameON) {
                    PostMessage(this.gamePID, 257U, (int)combo.SelectedValue, 0);

                    // If horn lvl 1 (enter) is used, disable the timer for horn lvl 2.
                    if ((int)combo.SelectedValue == 13) {
                        PostMessage(this.gamePID, 257U, 8, 0);
                        this.timerHorn.Enabled = false;
                    }
                }
            }
        }

        private void ReadIniFile() {
            this.comboBoxL.SelectedItem = GetConfValue("L");
            this.comboBoxZL.SelectedItem = GetConfValue("ZL");
            this.comboBoxR.SelectedItem = GetConfValue("R");
            this.comboBoxZR.SelectedItem = GetConfValue("ZR");
            this.comboBoxMinus.SelectedItem = GetConfValue("Minus");
            this.comboBoxPlus.SelectedItem = GetConfValue("Plus");
            this.comboBoxCapture.SelectedItem = GetConfValue("Capture");
            this.comboBoxHome.SelectedItem = GetConfValue("Home");
            this.comboBoxUp.SelectedItem = GetConfValue("Up");
            this.comboBoxDown.SelectedItem = GetConfValue("Down");
            this.comboBoxLeft.SelectedItem = GetConfValue("Left");
            this.comboBoxRight.SelectedItem = GetConfValue("Right");
            this.comboBoxA.SelectedItem = GetConfValue("A");
            this.comboBoxB.SelectedItem = GetConfValue("B");
            this.comboBoxX.SelectedItem = GetConfValue("X");
            this.comboBoxY.SelectedItem = GetConfValue("Y");
        }
    }
}
