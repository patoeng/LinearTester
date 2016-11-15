<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormMain
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormMain))
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.MenuStrip = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuConfig = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.QuitToolStripMenuQuit = New System.Windows.Forms.ToolStripMenuItem()
        Me.Timer = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.LabelOrderIn = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.PictureBoxStop = New System.Windows.Forms.PictureBox()
        Me.PictureBoxPlay = New System.Windows.Forms.PictureBox()
        Me.LabelQtySent = New System.Windows.Forms.Label()
        Me.LabelQtyLaunched = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.LabelReferenceIn = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.PictureBoxAckInputHazard = New System.Windows.Forms.PictureBox()
        Me.LabelInputHazardEvent = New System.Windows.Forms.Label()
        Me.LabelInputStatus = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.PictureBoxAckHiPotHazard = New System.Windows.Forms.PictureBox()
        Me.LabelHiPotHazardEvent = New System.Windows.Forms.Label()
        Me.LabelHiPotStatus = New System.Windows.Forms.Label()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.LabelPaletteOut = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.LabelOrderOut = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.LabelReferenceOut = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.PictureBoxAckOutputHazard = New System.Windows.Forms.PictureBox()
        Me.LabelOutputHazardEvent = New System.Windows.Forms.Label()
        Me.LabelOutputStatus = New System.Windows.Forms.Label()
        Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer()
        Me.Product4 = New Microsoft.VisualBasic.PowerPacks.OvalShape()
        Me.Product3 = New Microsoft.VisualBasic.PowerPacks.OvalShape()
        Me.Product2 = New Microsoft.VisualBasic.PowerPacks.OvalShape()
        Me.Product1 = New Microsoft.VisualBasic.PowerPacks.OvalShape()
        Me.MyDataacquisitionDataSet = New XS156LinearTesterPole.dataacquisitionDataSet()
        Me.EquipmentsTableAdapter = New XS156LinearTesterPole.dataacquisitionDataSetTableAdapters.equipmentsTableAdapter()
        Me.ManufacturingordersTableAdapter = New XS156LinearTesterPole.dataacquisitionDataSetTableAdapters.manufacturingordersTableAdapter()
        Me.WorkordersTableAdapter = New XS156LinearTesterPole.dataacquisitionDataSetTableAdapters.workordersTableAdapter()
        Me.MyBdd_badgesDataSet = New XS156LinearTesterPole.bdd_badgesDataSet()
        Me.Ctrl_dielectriqueTableAdapter = New XS156LinearTesterPole.bdd_badgesDataSetTableAdapters.ctrl_dielectriqueTableAdapter()
        Me.ReferencesTableAdapter = New XS156LinearTesterPole.bdd_badgesDataSetTableAdapters.referencesTableAdapter()
        Me.Test_finalTableAdapter = New XS156LinearTesterPole.bdd_badgesDataSetTableAdapters.test_finalTableAdapter()
        Me.HazardNamesTableAdapter = New XS156LinearTesterPole.HazardEventsNamesDataSetTableAdapters.HazardNamesTableAdapter()
        Me.MyHazardEventsNamesDataSet = New XS156LinearTesterPole.HazardEventsNamesDataSet()
        Me.MeasurementresultsTableAdapter = New XS156LinearTesterPole.dataacquisitionDataSetTableAdapters.measurementresultsTableAdapter()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.PictureBoxStop, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBoxPlay, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBoxAckInputHazard, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        CType(Me.PictureBoxAckHiPotHazard, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox4.SuspendLayout()
        CType(Me.PictureBoxAckOutputHazard, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MyDataacquisitionDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MyBdd_badgesDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MyHazardEventsNamesDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.XS156LinearTesterPole.My.Resources.Resources.Telemecanique_Sensors
        Me.PictureBox1.Location = New System.Drawing.Point(756, 27)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(258, 77)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.White
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Font = New System.Drawing.Font("Arial Black", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(87, 40)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(611, 54)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "XS156 Hi-Pot + Linear Tester"
        '
        'MenuStrip
        '
        Me.MenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem})
        Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip.Name = "MenuStrip"
        Me.MenuStrip.Size = New System.Drawing.Size(1014, 24)
        Me.MenuStrip.TabIndex = 2
        Me.MenuStrip.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuConfig, Me.ToolStripSeparator1, Me.QuitToolStripMenuQuit})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'ToolStripMenuConfig
        '
        Me.ToolStripMenuConfig.Image = Global.XS156LinearTesterPole.My.Resources.Resources.applications_321
        Me.ToolStripMenuConfig.Name = "ToolStripMenuConfig"
        Me.ToolStripMenuConfig.Size = New System.Drawing.Size(152, 22)
        Me.ToolStripMenuConfig.Text = "&Config"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(149, 6)
        '
        'QuitToolStripMenuQuit
        '
        Me.QuitToolStripMenuQuit.Image = Global.XS156LinearTesterPole.My.Resources.Resources.delete_321
        Me.QuitToolStripMenuQuit.Name = "QuitToolStripMenuQuit"
        Me.QuitToolStripMenuQuit.Size = New System.Drawing.Size(152, 22)
        Me.QuitToolStripMenuQuit.Text = "&Quit"
        '
        'Timer
        '
        Me.Timer.Interval = 500
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.LabelOrderIn)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.PictureBoxStop)
        Me.GroupBox2.Controls.Add(Me.PictureBoxPlay)
        Me.GroupBox2.Controls.Add(Me.LabelQtySent)
        Me.GroupBox2.Controls.Add(Me.LabelQtyLaunched)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.LabelReferenceIn)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.PictureBoxAckInputHazard)
        Me.GroupBox2.Controls.Add(Me.LabelInputHazardEvent)
        Me.GroupBox2.Controls.Add(Me.LabelInputStatus)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 110)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(1002, 247)
        Me.GroupBox2.TabIndex = 4
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Loading..."
        '
        'LabelOrderIn
        '
        Me.LabelOrderIn.BackColor = System.Drawing.Color.WhiteSmoke
        Me.LabelOrderIn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LabelOrderIn.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelOrderIn.Location = New System.Drawing.Point(68, 62)
        Me.LabelOrderIn.Name = "LabelOrderIn"
        Me.LabelOrderIn.Size = New System.Drawing.Size(159, 37)
        Me.LabelOrderIn.TabIndex = 19
        Me.LabelOrderIn.Text = "00000000"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(29, 78)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(33, 13)
        Me.Label5.TabIndex = 18
        Me.Label5.Text = "Order"
        '
        'PictureBoxStop
        '
        Me.PictureBoxStop.Image = Global.XS156LinearTesterPole.My.Resources.Resources.Stop1PressedBlue
        Me.PictureBoxStop.Location = New System.Drawing.Point(920, 25)
        Me.PictureBoxStop.Name = "PictureBoxStop"
        Me.PictureBoxStop.Size = New System.Drawing.Size(64, 64)
        Me.PictureBoxStop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBoxStop.TabIndex = 17
        Me.PictureBoxStop.TabStop = False
        Me.PictureBoxStop.Visible = False
        '
        'PictureBoxPlay
        '
        Me.PictureBoxPlay.Image = Global.XS156LinearTesterPole.My.Resources.Resources.Play1Pressed
        Me.PictureBoxPlay.Location = New System.Drawing.Point(839, 25)
        Me.PictureBoxPlay.Name = "PictureBoxPlay"
        Me.PictureBoxPlay.Size = New System.Drawing.Size(64, 64)
        Me.PictureBoxPlay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBoxPlay.TabIndex = 16
        Me.PictureBoxPlay.TabStop = False
        '
        'LabelQtySent
        '
        Me.LabelQtySent.AutoSize = True
        Me.LabelQtySent.BackColor = System.Drawing.Color.WhiteSmoke
        Me.LabelQtySent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LabelQtySent.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelQtySent.Location = New System.Drawing.Point(447, 67)
        Me.LabelQtySent.Name = "LabelQtySent"
        Me.LabelQtySent.Size = New System.Drawing.Size(27, 27)
        Me.LabelQtySent.TabIndex = 15
        Me.LabelQtySent.Text = "0"
        '
        'LabelQtyLaunched
        '
        Me.LabelQtyLaunched.AutoSize = True
        Me.LabelQtyLaunched.BackColor = System.Drawing.Color.WhiteSmoke
        Me.LabelQtyLaunched.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LabelQtyLaunched.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelQtyLaunched.Location = New System.Drawing.Point(447, 26)
        Me.LabelQtyLaunched.Name = "LabelQtyLaunched"
        Me.LabelQtyLaunched.Size = New System.Drawing.Size(27, 27)
        Me.LabelQtyLaunched.TabIndex = 14
        Me.LabelQtyLaunched.Text = "0"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(343, 76)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(98, 13)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "Qty sent on the line"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(371, 35)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(70, 13)
        Me.Label3.TabIndex = 12
        Me.Label3.Text = "Qty launched"
        '
        'LabelReferenceIn
        '
        Me.LabelReferenceIn.BackColor = System.Drawing.Color.WhiteSmoke
        Me.LabelReferenceIn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LabelReferenceIn.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelReferenceIn.Location = New System.Drawing.Point(68, 16)
        Me.LabelReferenceIn.Name = "LabelReferenceIn"
        Me.LabelReferenceIn.Size = New System.Drawing.Size(268, 37)
        Me.LabelReferenceIn.TabIndex = 11
        Me.LabelReferenceIn.Text = "XS..."
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(5, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(57, 13)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Reference"
        '
        'PictureBoxAckInputHazard
        '
        Me.PictureBoxAckInputHazard.Image = Global.XS156LinearTesterPole.My.Resources.Resources.EjectNormalRed
        Me.PictureBoxAckInputHazard.Location = New System.Drawing.Point(930, 165)
        Me.PictureBoxAckInputHazard.Name = "PictureBoxAckInputHazard"
        Me.PictureBoxAckInputHazard.Size = New System.Drawing.Size(64, 64)
        Me.PictureBoxAckInputHazard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBoxAckInputHazard.TabIndex = 2
        Me.PictureBoxAckInputHazard.TabStop = False
        '
        'LabelInputHazardEvent
        '
        Me.LabelInputHazardEvent.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelInputHazardEvent.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LabelInputHazardEvent.Location = New System.Drawing.Point(11, 175)
        Me.LabelInputHazardEvent.Name = "LabelInputHazardEvent"
        Me.LabelInputHazardEvent.Size = New System.Drawing.Size(913, 54)
        Me.LabelInputHazardEvent.TabIndex = 1
        Me.LabelInputHazardEvent.Text = "   ..."
        Me.LabelInputHazardEvent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LabelInputStatus
        '
        Me.LabelInputStatus.AutoSize = True
        Me.LabelInputStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelInputStatus.ForeColor = System.Drawing.Color.RoyalBlue
        Me.LabelInputStatus.Location = New System.Drawing.Point(9, 141)
        Me.LabelInputStatus.Name = "LabelInputStatus"
        Me.LabelInputStatus.Size = New System.Drawing.Size(68, 20)
        Me.LabelInputStatus.TabIndex = 0
        Me.LabelInputStatus.Text = "Status..."
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.PictureBoxAckHiPotHazard)
        Me.GroupBox3.Controls.Add(Me.LabelHiPotHazardEvent)
        Me.GroupBox3.Controls.Add(Me.LabelHiPotStatus)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 363)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(1002, 119)
        Me.GroupBox3.TabIndex = 5
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Hi-Pot..."
        '
        'PictureBoxAckHiPotHazard
        '
        Me.PictureBoxAckHiPotHazard.Image = Global.XS156LinearTesterPole.My.Resources.Resources.EjectNormalRed
        Me.PictureBoxAckHiPotHazard.Location = New System.Drawing.Point(929, 33)
        Me.PictureBoxAckHiPotHazard.Name = "PictureBoxAckHiPotHazard"
        Me.PictureBoxAckHiPotHazard.Size = New System.Drawing.Size(64, 64)
        Me.PictureBoxAckHiPotHazard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBoxAckHiPotHazard.TabIndex = 5
        Me.PictureBoxAckHiPotHazard.TabStop = False
        '
        'LabelHiPotHazardEvent
        '
        Me.LabelHiPotHazardEvent.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelHiPotHazardEvent.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LabelHiPotHazardEvent.Location = New System.Drawing.Point(11, 43)
        Me.LabelHiPotHazardEvent.Name = "LabelHiPotHazardEvent"
        Me.LabelHiPotHazardEvent.Size = New System.Drawing.Size(913, 54)
        Me.LabelHiPotHazardEvent.TabIndex = 4
        Me.LabelHiPotHazardEvent.Text = "   ..."
        Me.LabelHiPotHazardEvent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LabelHiPotStatus
        '
        Me.LabelHiPotStatus.AutoSize = True
        Me.LabelHiPotStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelHiPotStatus.ForeColor = System.Drawing.Color.RoyalBlue
        Me.LabelHiPotStatus.Location = New System.Drawing.Point(9, 21)
        Me.LabelHiPotStatus.Name = "LabelHiPotStatus"
        Me.LabelHiPotStatus.Size = New System.Drawing.Size(68, 20)
        Me.LabelHiPotStatus.TabIndex = 3
        Me.LabelHiPotStatus.Text = "Status..."
        Me.LabelHiPotStatus.Visible = False
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.LabelPaletteOut)
        Me.GroupBox4.Controls.Add(Me.Label15)
        Me.GroupBox4.Controls.Add(Me.LabelOrderOut)
        Me.GroupBox4.Controls.Add(Me.Label11)
        Me.GroupBox4.Controls.Add(Me.LabelReferenceOut)
        Me.GroupBox4.Controls.Add(Me.Label13)
        Me.GroupBox4.Controls.Add(Me.Label9)
        Me.GroupBox4.Controls.Add(Me.Label8)
        Me.GroupBox4.Controls.Add(Me.Label7)
        Me.GroupBox4.Controls.Add(Me.Label6)
        Me.GroupBox4.Controls.Add(Me.PictureBoxAckOutputHazard)
        Me.GroupBox4.Controls.Add(Me.LabelOutputHazardEvent)
        Me.GroupBox4.Controls.Add(Me.LabelOutputStatus)
        Me.GroupBox4.Controls.Add(Me.ShapeContainer1)
        Me.GroupBox4.Location = New System.Drawing.Point(12, 488)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(1002, 232)
        Me.GroupBox4.TabIndex = 6
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Unloading..."
        '
        'LabelPaletteOut
        '
        Me.LabelPaletteOut.AutoSize = True
        Me.LabelPaletteOut.BackColor = System.Drawing.Color.WhiteSmoke
        Me.LabelPaletteOut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LabelPaletteOut.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelPaletteOut.Location = New System.Drawing.Point(447, 140)
        Me.LabelPaletteOut.Name = "LabelPaletteOut"
        Me.LabelPaletteOut.Size = New System.Drawing.Size(27, 27)
        Me.LabelPaletteOut.TabIndex = 25
        Me.LabelPaletteOut.Text = "0"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(401, 149)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(40, 13)
        Me.Label15.TabIndex = 24
        Me.Label15.Text = "Palette"
        '
        'LabelOrderOut
        '
        Me.LabelOrderOut.BackColor = System.Drawing.Color.WhiteSmoke
        Me.LabelOrderOut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LabelOrderOut.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelOrderOut.Location = New System.Drawing.Point(68, 179)
        Me.LabelOrderOut.Name = "LabelOrderOut"
        Me.LabelOrderOut.Size = New System.Drawing.Size(159, 37)
        Me.LabelOrderOut.TabIndex = 23
        Me.LabelOrderOut.Text = "00000000"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(29, 195)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(33, 13)
        Me.Label11.TabIndex = 22
        Me.Label11.Text = "Order"
        '
        'LabelReferenceOut
        '
        Me.LabelReferenceOut.BackColor = System.Drawing.Color.WhiteSmoke
        Me.LabelReferenceOut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LabelReferenceOut.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelReferenceOut.Location = New System.Drawing.Point(68, 133)
        Me.LabelReferenceOut.Name = "LabelReferenceOut"
        Me.LabelReferenceOut.Size = New System.Drawing.Size(268, 37)
        Me.LabelReferenceOut.TabIndex = 21
        Me.LabelReferenceOut.Text = "XS..."
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(5, 149)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(57, 13)
        Me.Label13.TabIndex = 20
        Me.Label13.Text = "Reference"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(836, 156)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(16, 16)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "4"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(758, 157)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(16, 16)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "3"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(682, 156)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(16, 16)
        Me.Label7.TabIndex = 8
        Me.Label7.Text = "2"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(604, 157)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(16, 16)
        Me.Label6.TabIndex = 7
        Me.Label6.Text = "1"
        '
        'PictureBoxAckOutputHazard
        '
        Me.PictureBoxAckOutputHazard.Image = Global.XS156LinearTesterPole.My.Resources.Resources.EjectNormalRed
        Me.PictureBoxAckOutputHazard.Location = New System.Drawing.Point(929, 33)
        Me.PictureBoxAckOutputHazard.Name = "PictureBoxAckOutputHazard"
        Me.PictureBoxAckOutputHazard.Size = New System.Drawing.Size(64, 64)
        Me.PictureBoxAckOutputHazard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBoxAckOutputHazard.TabIndex = 5
        Me.PictureBoxAckOutputHazard.TabStop = False
        '
        'LabelOutputHazardEvent
        '
        Me.LabelOutputHazardEvent.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelOutputHazardEvent.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LabelOutputHazardEvent.Location = New System.Drawing.Point(11, 43)
        Me.LabelOutputHazardEvent.Name = "LabelOutputHazardEvent"
        Me.LabelOutputHazardEvent.Size = New System.Drawing.Size(913, 54)
        Me.LabelOutputHazardEvent.TabIndex = 4
        Me.LabelOutputHazardEvent.Text = "   ..."
        Me.LabelOutputHazardEvent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LabelOutputStatus
        '
        Me.LabelOutputStatus.AutoSize = True
        Me.LabelOutputStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelOutputStatus.ForeColor = System.Drawing.Color.RoyalBlue
        Me.LabelOutputStatus.Location = New System.Drawing.Point(9, 21)
        Me.LabelOutputStatus.Name = "LabelOutputStatus"
        Me.LabelOutputStatus.Size = New System.Drawing.Size(68, 20)
        Me.LabelOutputStatus.TabIndex = 3
        Me.LabelOutputStatus.Text = "Status..."
        '
        'ShapeContainer1
        '
        Me.ShapeContainer1.Location = New System.Drawing.Point(3, 16)
        Me.ShapeContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.ShapeContainer1.Name = "ShapeContainer1"
        Me.ShapeContainer1.Shapes.AddRange(New Microsoft.VisualBasic.PowerPacks.Shape() {Me.Product4, Me.Product3, Me.Product2, Me.Product1})
        Me.ShapeContainer1.Size = New System.Drawing.Size(996, 213)
        Me.ShapeContainer1.TabIndex = 6
        Me.ShapeContainer1.TabStop = False
        '
        'Product4
        '
        Me.Product4.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Opaque
        Me.Product4.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid
        Me.Product4.Location = New System.Drawing.Point(811, 117)
        Me.Product4.Name = "Product4"
        Me.Product4.Size = New System.Drawing.Size(60, 60)
        '
        'Product3
        '
        Me.Product3.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Opaque
        Me.Product3.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid
        Me.Product3.Location = New System.Drawing.Point(733, 118)
        Me.Product3.Name = "Product3"
        Me.Product3.Size = New System.Drawing.Size(60, 60)
        '
        'Product2
        '
        Me.Product2.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Opaque
        Me.Product2.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid
        Me.Product2.Location = New System.Drawing.Point(657, 118)
        Me.Product2.Name = "Product2"
        Me.Product2.Size = New System.Drawing.Size(60, 60)
        '
        'Product1
        '
        Me.Product1.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Opaque
        Me.Product1.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid
        Me.Product1.Location = New System.Drawing.Point(580, 118)
        Me.Product1.Name = "Product1"
        Me.Product1.Size = New System.Drawing.Size(60, 60)
        '
        'MyDataacquisitionDataSet
        '
        Me.MyDataacquisitionDataSet.DataSetName = "dataacquisitionDataSet"
        Me.MyDataacquisitionDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'EquipmentsTableAdapter
        '
        Me.EquipmentsTableAdapter.ClearBeforeFill = True
        '
        'ManufacturingordersTableAdapter
        '
        Me.ManufacturingordersTableAdapter.ClearBeforeFill = True
        '
        'WorkordersTableAdapter
        '
        Me.WorkordersTableAdapter.ClearBeforeFill = True
        '
        'MyBdd_badgesDataSet
        '
        Me.MyBdd_badgesDataSet.DataSetName = "bdd_badgesDataSet"
        Me.MyBdd_badgesDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'Ctrl_dielectriqueTableAdapter
        '
        Me.Ctrl_dielectriqueTableAdapter.ClearBeforeFill = True
        '
        'ReferencesTableAdapter
        '
        Me.ReferencesTableAdapter.ClearBeforeFill = True
        '
        'Test_finalTableAdapter
        '
        Me.Test_finalTableAdapter.ClearBeforeFill = True
        '
        'HazardNamesTableAdapter
        '
        Me.HazardNamesTableAdapter.ClearBeforeFill = True
        '
        'MyHazardEventsNamesDataSet
        '
        Me.MyHazardEventsNamesDataSet.DataSetName = "HazardEventsNamesDataSet"
        Me.MyHazardEventsNamesDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'MeasurementresultsTableAdapter
        '
        Me.MeasurementresultsTableAdapter.ClearBeforeFill = True
        '
        'FormMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1014, 732)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.MenuStrip)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip
        Me.Name = "FormMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "XS156 Linear Tester pole"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip.ResumeLayout(False)
        Me.MenuStrip.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.PictureBoxStop, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBoxPlay, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBoxAckInputHazard, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.PictureBoxAckHiPotHazard, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        CType(Me.PictureBoxAckOutputHazard, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MyDataacquisitionDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MyBdd_badgesDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MyHazardEventsNamesDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents MenuStrip As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuConfig As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents QuitToolStripMenuQuit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MyBdd_badgesDataSet As XS156LinearTesterPole.bdd_badgesDataSet
    Friend WithEvents Ctrl_dielectriqueTableAdapter As XS156LinearTesterPole.bdd_badgesDataSetTableAdapters.ctrl_dielectriqueTableAdapter
    Friend WithEvents ReferencesTableAdapter As XS156LinearTesterPole.bdd_badgesDataSetTableAdapters.referencesTableAdapter
    Friend WithEvents Test_finalTableAdapter As XS156LinearTesterPole.bdd_badgesDataSetTableAdapters.test_finalTableAdapter
    Friend WithEvents MyDataacquisitionDataSet As XS156LinearTesterPole.dataacquisitionDataSet
    Friend WithEvents EquipmentsTableAdapter As XS156LinearTesterPole.dataacquisitionDataSetTableAdapters.equipmentsTableAdapter
    Friend WithEvents ManufacturingordersTableAdapter As XS156LinearTesterPole.dataacquisitionDataSetTableAdapters.manufacturingordersTableAdapter
    Friend WithEvents WorkordersTableAdapter As XS156LinearTesterPole.dataacquisitionDataSetTableAdapters.workordersTableAdapter
    Friend WithEvents Timer As System.Windows.Forms.Timer
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents LabelInputStatus As System.Windows.Forms.Label
    Friend WithEvents PictureBoxAckInputHazard As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBoxAckHiPotHazard As System.Windows.Forms.PictureBox
    Friend WithEvents LabelHiPotStatus As System.Windows.Forms.Label
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents PictureBoxAckOutputHazard As System.Windows.Forms.PictureBox
    Friend WithEvents LabelOutputStatus As System.Windows.Forms.Label
    Friend WithEvents LabelInputHazardEvent As System.Windows.Forms.Label
    Friend WithEvents LabelHiPotHazardEvent As System.Windows.Forms.Label
    Friend WithEvents LabelOutputHazardEvent As System.Windows.Forms.Label
    Friend WithEvents HazardNamesTableAdapter As XS156LinearTesterPole.HazardEventsNamesDataSetTableAdapters.HazardNamesTableAdapter
    Friend WithEvents MyHazardEventsNamesDataSet As XS156LinearTesterPole.HazardEventsNamesDataSet
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
    Friend WithEvents Product4 As Microsoft.VisualBasic.PowerPacks.OvalShape
    Friend WithEvents Product3 As Microsoft.VisualBasic.PowerPacks.OvalShape
    Friend WithEvents Product2 As Microsoft.VisualBasic.PowerPacks.OvalShape
    Friend WithEvents Product1 As Microsoft.VisualBasic.PowerPacks.OvalShape
    Friend WithEvents MeasurementresultsTableAdapter As XS156LinearTesterPole.dataacquisitionDataSetTableAdapters.measurementresultsTableAdapter
    Friend WithEvents LabelOrderIn As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents PictureBoxStop As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBoxPlay As System.Windows.Forms.PictureBox
    Friend WithEvents LabelQtySent As System.Windows.Forms.Label
    Friend WithEvents LabelQtyLaunched As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents LabelReferenceIn As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents LabelPaletteOut As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents LabelOrderOut As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents LabelReferenceOut As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label

End Class
