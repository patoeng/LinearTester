Public Class FormMain

    Public referenceLaunched As String
    Public qtyLaunched As Integer
    Public qtySent As Integer
    Public currentRefRecord As bdd_badgesDataSet.referencesRow
    Public currentManufacturingOrder As dataacquisitionDataSet.manufacturingordersRow
    Dim currentInputWorkOrder As dataacquisitionDataSet.workordersRow
    Dim currentOutputWorkOrder As dataacquisitionDataSet.workordersRow

    Dim PLC As New classAutomateTCPIP.Automate
    Dim MyOsitrack As New Ositrack

    Dim paletteNumber As Integer

    Dim stopWorkOrderInput As Boolean = False
    Dim AckInputHazard As Boolean = False
    Dim AckHiPotHazard As Boolean = False
    Dim AckFinalHazard As Boolean = False
    Public WithEvents Xs156Client As XS156Client35.Xs156Client
    Dim myPrinter As New Compatibility.VB6.Printer

    Private Sub FormMain_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = False
        Try

            Xs156Client = New XS156Client35.Xs156Client
            'Xs156Client.StartUpdater()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        Try
            ' Set correct Ip address
            EquipmentsTableAdapter.Connection.ConnectionString = My.Settings.XS156dataacquisitionConnectionString.Replace("wxsg103030118d.apa", My.Settings.IPAddressOfDataAcquisitionServer)
            ManufacturingordersTableAdapter.Connection.ConnectionString = My.Settings.XS156dataacquisitionConnectionString.Replace("wxsg103030118d.apa", My.Settings.IPAddressOfDataAcquisitionServer)
            WorkordersTableAdapter.Connection.ConnectionString = My.Settings.XS156dataacquisitionConnectionString.Replace("wxsg103030118d.apa", My.Settings.IPAddressOfDataAcquisitionServer)
            MeasurementresultsTableAdapter.Connection.ConnectionString = My.Settings.XS156dataacquisitionConnectionString.Replace("wxsg103030118d.apa", My.Settings.IPAddressOfDataAcquisitionServer)

            ' Fill
            Ctrl_dielectriqueTableAdapter.Fill(MyBdd_badgesDataSet.ctrl_dielectrique)
            Test_finalTableAdapter.Fill(MyBdd_badgesDataSet.test_final)

            HazardNamesTableAdapter.Fill(MyHazardEventsNamesDataSet.HazardNames)

            EquipmentsTableAdapter.Fill(MyDataacquisitionDataSet.equipments)

            ' PLC
            PLC.adresseRTDBDansAutomate = 800
            PLC.adresseIp = My.Settings.PLCIpAddress
            PLC.nbMotsRTDB = 100
            PLC.periodeScrutationMs = 1000
            PLC.lanceScrutation()

            ' Ositrack
            MyOsitrack.adresseIp = My.Settings.RFIDIpAddress
            MyOsitrack.periodeScrutationMs = 500
            MyOsitrack.stationsOsitrack(0).adresseEsclave = CInt(My.Settings.ModbusAddressLoading)
            MyOsitrack.stationsOsitrack(0).nomStation = "Loading station"
            MyOsitrack.stationsOsitrack(1).adresseEsclave = CInt(My.Settings.ModbusAddressUnloading)
            MyOsitrack.stationsOsitrack(1).nomStation = "Unloading station"

            MyOsitrack.stationsOsitrack(0).detectionAutomatiqueTailleEtiquette = True
            MyOsitrack.stationsOsitrack(1).detectionAutomatiqueTailleEtiquette = True
            MyOsitrack.lanceScrutation()

            Timer.Enabled = True

        Catch ex As Exception
            SplashScreen.Visible = False
            Application.DoEvents()
            MessageBox.Show("Initialisation failed : " & ex.Message, "XS156 linear tester", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub ToolStripMenuConfig_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuConfig.Click

        If DialogConfig.ShowDialog() = Windows.Forms.DialogResult.OK Then

        End If

    End Sub

    Private Sub QuitToolStripMenuQuit_Click(sender As System.Object, e As System.EventArgs) Handles QuitToolStripMenuQuit.Click

        End

    End Sub

    Private Sub PictureBoxPlay_Click(sender As System.Object, e As System.EventArgs) Handles PictureBoxPlay.Click

        If DialogNewOrder.ShowDialog() = Windows.Forms.DialogResult.OK Then
            ''' Disini kita buat new reference untuk traceability
            qtyLaunched = currentManufacturingOrder.manufacturingOrderQuantity
            qtySent = 0
            referenceLaunched = currentManufacturingOrder.manufacturingOrderCommercialSymbol
            paletteNumber = 1
           
            ' Find me
            Try
                For Each oneEquipment As dataacquisitionDataSet.equipmentsRow In MyDataacquisitionDataSet.equipments
                    If oneEquipment.equipmentLabel = My.Settings.EquipmentNameHiPot Then
                        currentInputWorkOrder = MyDataacquisitionDataSet.workorders.NewworkordersRow
                        currentInputWorkOrder.workOrderId = Now.Ticks.ToString & "-" & oneEquipment.equipmentId.ToString
                        currentInputWorkOrder.workOrderEquipment = oneEquipment.equipmentId
                        currentInputWorkOrder.workOrderManufacturingOrder = currentManufacturingOrder.manufacturingOrderId
                        currentInputWorkOrder.workOrderBeginDate = Now
                        currentInputWorkOrder.workOrderEndDate = Now
                        currentInputWorkOrder.workOrderProcessedQuantity = currentManufacturingOrder.manufacturingOrderQuantity
                        currentInputWorkOrder.workOrderQuantityNok = 0
                        currentInputWorkOrder.workOrderQuantityOk = 0
                        currentInputWorkOrder.workOrderDuration = 0
                        currentInputWorkOrder.workOrderPhase = 135
                        MyDataacquisitionDataSet.workorders.AddworkordersRow(currentInputWorkOrder)
                        WorkordersTableAdapter.Update(MyDataacquisitionDataSet.workorders)
                        Exit For
                    End If
                    '''Disini kita buat new reference untuk traceability
                Next
            Catch ex As Exception
                MessageBox.Show("Error creating the workorder record : " & ex.Message)
            End Try
        End If

    End Sub


    Private Sub Timer_Tick(sender As System.Object, e As System.EventArgs) Handles Timer.Tick

        Static antiFeedBack As Boolean

        If antiFeedBack Then
            Exit Sub
        End If

        antiFeedBack = True

        ' -------------------------------------------------------
        ' STOP WO
        ' -------------------------------------------------------
        If stopWorkOrderInput Then
            Try
                If currentInputWorkOrder IsNot Nothing Then
                    currentInputWorkOrder.workOrderEndDate = Now
                    currentInputWorkOrder.workOrderDuration = CULng((Now - currentInputWorkOrder.workOrderBeginDate).TotalSeconds)
                    WorkordersTableAdapter.Update(MyDataacquisitionDataSet.workorders)
                    currentInputWorkOrder = Nothing
                End If
            Catch ex As Exception
                MessageBox.Show("Error closing the workorder record : " & ex.Message)
            End Try

            ' Manage this out of the RFID programmation process
            currentManufacturingOrder = Nothing
            currentRefRecord = Nothing
            qtyLaunched = 0
            qtySent = 0
            stopWorkOrderInput = False
        End If




        ' -------------------------------------------------------
        ' DISPLAY MANAGEMENT
        ' -------------------------------------------------------
        If currentManufacturingOrder IsNot Nothing And currentRefRecord IsNot Nothing Then
            LabelQtyLaunched.Text = qtyLaunched.ToString
            LabelQtySent.Text = qtySent.ToString
            LabelReferenceIn.Text = referenceLaunched
            LabelOrderIn.Text = currentManufacturingOrder.manufacturingOrderId
            PictureBoxStop.Visible = True
            PictureBoxPlay.Visible = False
          
        Else
            LabelReferenceIn.Text = "Waiting..."
            LabelQtySent.Text = "0"
            LabelQtyLaunched.Text = "0"
            PictureBoxStop.Visible = False
            PictureBoxPlay.Visible = True
        End If

        Dim hazardLine As HazardEventsNamesDataSet.HazardNamesRow
        If PLC.mwLectureAutomate(1) <> 0 Then
            hazardLine = MyHazardEventsNamesDataSet.HazardNames.FindByCode(PLC.mwLectureAutomate(1))
            If hazardLine IsNot Nothing Then
                LabelInputHazardEvent.Text = hazardLine.Text
            Else
                LabelInputHazardEvent.Text = PLC.mwLectureAutomate(1).ToString
            End If
        End If

        If AckInputHazard Then
            PLC.mwEcritureAutomate(2) = 1
            PLC.ecritMotsAutomate(2, 1)
            AckInputHazard = False
        End If

        If PLC.mwLectureAutomate(21) <> 0 Then
            hazardLine = MyHazardEventsNamesDataSet.HazardNames.FindByCode(PLC.mwLectureAutomate(21))
            If hazardLine IsNot Nothing Then
                LabelHiPotHazardEvent.Text = hazardLine.Text
            Else
                LabelHiPotHazardEvent.Text = PLC.mwLectureAutomate(21).ToString
            End If
        End If

        If AckHiPotHazard Then
            PLC.mwEcritureAutomate(22) = 1
            PLC.ecritMotsAutomate(22, 1)
            AckHiPotHazard = False
        End If

        If PLC.mwLectureAutomate(41) <> 0 Then
            hazardLine = MyHazardEventsNamesDataSet.HazardNames.FindByCode(PLC.mwLectureAutomate(41))
            If hazardLine IsNot Nothing Then
                LabelOutputHazardEvent.Text = hazardLine.Text
            Else
                LabelOutputHazardEvent.Text = PLC.mwLectureAutomate(41).ToString
            End If
        End If

        If AckFinalHazard Then
            PLC.mwEcritureAutomate(43) = 1
            PLC.ecritMotsAutomate(43, 1)
            AckFinalHazard = False
        End If

        ' Tags management
        ' -------------------------------------------------------
        ' NEW TAG ON INPUT
        ' -------------------------------------------------------

        ' This variable will create a rising edge.
        Static LoadingStatus As UInteger
        If PLC.mwLectureAutomate(0) = 1 Then

            ' Conditions
            If currentManufacturingOrder Is Nothing Or currentRefRecord Is Nothing Then
                LabelInputStatus.Text = "No order running."
                GoTo continue1
            End If

            If Not MyOsitrack.stationsOsitrack(0).etiquettePresente Then
                LabelInputStatus.Text = "No tag detected in front of the INPUT station."
                GoTo continue1
            End If

            If LoadingStatus = 0 Then
                ' Go 
                Call fillTag()
                '''Di sini kita modify untuk traceablity quantity update

                ' Confirm to PLC
                LoadingStatus = 1
                PLC.mwEcritureAutomate(0) = 0
                PLC.ecritMotsAutomate(0, 1)

                ' Keep a trace
                If currentInputWorkOrder IsNot Nothing Then
                    Try
                        currentInputWorkOrder.workOrderEndDate = Now
                        currentInputWorkOrder.workOrderQuantityOk = qtySent
                        currentInputWorkOrder.workOrderDuration = CULng((Now - currentInputWorkOrder.workOrderBeginDate).TotalSeconds)
                        WorkordersTableAdapter.Update(MyDataacquisitionDataSet.workorders)
                    Catch ex As Exception
                        LabelInputStatus.Text = "Error updating the workorder record : " & ex.Message
                    End Try
                End If

                ' Close order ?
                If qtySent >= qtyLaunched Then
                    stopWorkOrderInput = True
                End If
            End If
        Else
            LabelInputStatus.Text = "Waiting..."
            LoadingStatus = 0
        End If
        ' -------------------------------------------------------
        '
continue1:


        ' Tags management
        ' -------------------------------------------------------
        ' NEW TAG ON OUTPUT
        ' -------------------------------------------------------
        ' This variable will create a rising edge.
        Static UnLoadingStatus As UInteger
        If PLC.mwLectureAutomate(40) = 1 Then
            If UnLoadingStatus = 0 Then

                If Not MyOsitrack.stationsOsitrack(1).etiquettePresente Then
                    LabelOutputStatus.Text = "No tag detected in front of the OUTPUT station."
                    GoTo continue2
                End If
                LabelOutputStatus.Text = "Print default and record data..."
                Application.DoEvents()

                ' Read tag
                'MyOsitrack.stationsOsitrack(1).lireMotsOsitrack(0, 8000)
                MyOsitrack.stationsOsitrack(1).lireMotsOsitrack(0, 1000)

                Dim orderOutput As String
                ' From W91 for 4 words station 1
                orderOutput = MyOsitrack.motsVersChaine(91, 4, 1)
                Dim referenceOut As String
                referenceOut = MyOsitrack.motsVersChaine(0, 8, 1)
                If currentOutputWorkOrder Is Nothing OrElse currentOutputWorkOrder.workOrderManufacturingOrder <> orderOutput Then
                    ' Not existing or not the good one : create it
                    Try
                        For Each oneEquipment As dataacquisitionDataSet.equipmentsRow In MyDataacquisitionDataSet.equipments
                            If oneEquipment.equipmentLabel = My.Settings.EquipmentNameFinal Then
                                currentOutputWorkOrder = MyDataacquisitionDataSet.workorders.NewworkordersRow
                                currentOutputWorkOrder.workOrderId = Now.Ticks.ToString & "-" & oneEquipment.equipmentId.ToString
                                currentOutputWorkOrder.workOrderEquipment = oneEquipment.equipmentId
                                currentOutputWorkOrder.workOrderManufacturingOrder = orderOutput
                                currentOutputWorkOrder.workOrderBeginDate = Now
                                currentOutputWorkOrder.workOrderEndDate = Now
                                currentOutputWorkOrder.workOrderProcessedQuantity = MyOsitrack.stationsOsitrack(1).mwLectureOsitrack(84)
                                currentOutputWorkOrder.workOrderQuantityNok = MyOsitrack.stationsOsitrack(1).mwLectureOsitrack(88)
                                currentOutputWorkOrder.workOrderQuantityOk = MyOsitrack.stationsOsitrack(1).mwLectureOsitrack(87)
                                currentOutputWorkOrder.workOrderDuration = 0
                                currentOutputWorkOrder.workOrderPhase = 145
                                MyDataacquisitionDataSet.workorders.AddworkordersRow(currentOutputWorkOrder)
                                WorkordersTableAdapter.Update(MyDataacquisitionDataSet.workorders)
                                Exit For
                            End If
                        Next
                    Catch ex As Exception
                        LabelOutputStatus.Text = "Error creating the workorder record on output : " & ex.Message
                    End Try
                Else
                    Try
                        ' WorkOrder still opened
                        currentOutputWorkOrder.workOrderProcessedQuantity = MyOsitrack.stationsOsitrack(1).mwLectureOsitrack(84)
                        currentOutputWorkOrder.workOrderQuantityOk += MyOsitrack.stationsOsitrack(1).mwLectureOsitrack(87)
                        currentOutputWorkOrder.workOrderQuantityNok += MyOsitrack.stationsOsitrack(1).mwLectureOsitrack(88)
                        currentOutputWorkOrder.workOrderEndDate = Now
                        currentOutputWorkOrder.workOrderDuration = (Now - currentOutputWorkOrder.workOrderBeginDate).TotalSeconds
                        WorkordersTableAdapter.Update(MyDataacquisitionDataSet.workorders)
                    Catch ex As Exception
                        LabelOutputStatus.Text = "Error updating the workorder record on output : " & ex.Message
                    End Try
                End If
                ''' Disini Traceability Insert
                ''' 
                Try
                    If Not Xs156Client Is Nothing Then
                        If Xs156Client.GetCurrentOrderNumber() <> orderOutput Then
                            If Not Xs156Client.LoadByOrderNumber(orderOutput) Then
                                Xs156Client.StartNewTrackingProcess(referenceOut, 48, orderOutput)
                            End If
                        End If
                        If Xs156Client.GetCurrentOrderNumber() = orderOutput Then
                            Xs156Client.UpdateOutputQuantity(MyOsitrack.stationsOsitrack(1).mwLectureOsitrack(87))
                            Xs156Client.UpdateRejectedQuantity(MyOsitrack.stationsOsitrack(1).mwLectureOsitrack(88))
                            '' Xs156Client.SetOutputQuantity(currentOutputWorkOrder.workOrderQuantityOk)
                            '' Xs156Client.SetRejectQuantity(currentOutputWorkOrder.workOrderQuantityNok)
                        End If
                    End If
                Catch
                    MessageBox.Show("Traceability Error", Err.ToString())
                End Try

                ' Display product status and print defaults and record measurements
                Call present(1)
                Call present(2)
                Call present(3)
                Call present(4)

                ' Displays
                LabelReferenceOut.Text = MyOsitrack.motsVersChaine(0, 8, 1)
                LabelOrderOut.Text = MyOsitrack.motsVersChaine(91, 4, 1)
                LabelPaletteOut.Text = MyOsitrack.stationsOsitrack(1).mwLectureOsitrack(85).ToString

continue2:
                ' Confirm to PLC
                UnLoadingStatus = 1
                PLC.mwEcritureAutomate(40) = 0
                PLC.ecritMotsAutomate(40, 1)
            End If
        Else
            LabelOutputStatus.Text = "Waiting..."
            UnLoadingStatus = 0
        End If
        ' -------------------------------------------------------

        antiFeedBack = False

    End Sub

    Private Sub PictureBoxStop_Click(sender As System.Object, e As System.EventArgs) Handles PictureBoxStop.Click
        ''' Disini kita check bila Final belum close
        stopWorkOrderInput = True

    End Sub

    Private Sub fillTag()

        Try
            Dim hipotDocumentReference As String
            Dim finalControlDocumentReference As String
            hipotDocumentReference = currentRefRecord.ref_ctrl_diel
            finalControlDocumentReference = currentRefRecord.ref_test

            Dim hipotDocument As bdd_badgesDataSet.ctrl_dielectriqueRow
            Dim finalDocument As bdd_badgesDataSet.test_finalRow
            hipotDocument = MyBdd_badgesDataSet.ctrl_dielectrique.FindByref_ctrl_dielectrique(hipotDocumentReference)
            finalDocument = MyBdd_badgesDataSet.test_final.FindByref_test(finalControlDocumentReference)

            LabelInputStatus.Text = "Compose tag"
            Application.DoEvents()

            ' Ok, go.
            With MyOsitrack.stationsOsitrack(0)

                ' Clear ALL
                For indexArea1to3 As Integer = 0 To 1000
                    .mwEcritureOsitrack(indexArea1to3) = 0
                Next

                ' 0 ref com
                MyOsitrack.chaineVersMots(currentRefRecord.ref_comm.PadRight(16, " "), 0, 0)
                'classOsitrack.Ositrack.chaineVersMots(currentRefRecord.ref_comm.PadRight(16, " "), 0, 8, .mwEcritureOsitrack)
                ' 8 ref indus
                MyOsitrack.chaineVersMots(currentRefRecord.ref_indus.PadRight(16, " "), 8, 0)
                ' 19 list of operations (LSB first)
                ' 1 then 135 then 140 then 145
                .mwEcritureOsitrack(19) = (135L * 256L + 1L) - 65536
                .mwEcritureOsitrack(20) = (145L * 256L + 140L) - 65536
                .mwEcritureOsitrack(21) = 0
                .mwEcritureOsitrack(22) = 0
                .mwEcritureOsitrack(23) = 0
                .mwEcritureOsitrack(24) = 0
                .mwEcritureOsitrack(25) = 0
                .mwEcritureOsitrack(26) = 0
                .mwEcritureOsitrack(27) = 0
                .mwEcritureOsitrack(28) = 0
                .mwEcritureOsitrack(29) = 0
                .mwEcritureOsitrack(30) = 0
                .mwEcritureOsitrack(31) = 0
                .mwEcritureOsitrack(32) = 0
                .mwEcritureOsitrack(33) = 0
                .mwEcritureOsitrack(34) = 0
                ' 35 last phase
                .mwEcritureOsitrack(35) = 1
                ' 36 Diameter
                .mwEcritureOsitrack(36) = currentRefRecord.diametre

                ' Palette informations
                .mwEcritureOsitrack(84) = currentManufacturingOrder.manufacturingOrderQuantity
                .mwEcritureOsitrack(85) = paletteNumber

                .mwEcritureOsitrack(86) = currentManufacturingOrder.manufacturingOrderQuantity / 4

                .mwEcritureOsitrack(87) = 0
                .mwEcritureOsitrack(88) = 0
                .mwEcritureOsitrack(89) = 0
                .mwEcritureOsitrack(90) = 0
                MyOsitrack.chaineVersMots(currentManufacturingOrder.manufacturingOrderId, 91, 0)

                ' Other informations
                .mwEcritureOsitrack(95) = 0
                MyOsitrack.chaineVersMots("00000", 96, 0)
                .mwEcritureOsitrack(99) = 8
                .mwEcritureOsitrack(100) = 0
                .mwEcritureOsitrack(103) = 0
                .mwEcritureOsitrack(104) = Now.Year
                .mwEcritureOsitrack(105) = Now.Month
                .mwEcritureOsitrack(106) = Now.Day
                .mwEcritureOsitrack(107) = Now.Hour
                .mwEcritureOsitrack(108) = Now.Minute
                .mwEcritureOsitrack(109) = 0
                .mwEcritureOsitrack(110) = 0
                .mwEcritureOsitrack(111) = 0
                .mwEcritureOsitrack(112) = 0
                .mwEcritureOsitrack(113) = currentManufacturingOrder.manufacturingOrderQuantity
                .mwEcritureOsitrack(114) = currentManufacturingOrder.manufacturingOrderQuantity
                .mwEcritureOsitrack(115) = 1

                ' Hi Pot
                .mwEcritureOsitrack(276) = hipotDocument.tension_finale
                .mwEcritureOsitrack(277) = hipotDocument.duree_application
                .mwEcritureOsitrack(278) = hipotDocument.pente_vs
                .mwEcritureOsitrack(279) = toWord(hipotDocument.configuration.Substring(8, 2) & hipotDocument.configuration.Substring(10, 2))
                .mwEcritureOsitrack(280) = toWord(hipotDocument.configuration.Substring(4, 2) & hipotDocument.configuration.Substring(6, 2))
                .mwEcritureOsitrack(281) = toWord(hipotDocument.configuration.Substring(0, 2) & hipotDocument.configuration.Substring(2, 2))
                .mwEcritureOsitrack(282) = hipotDocument.courant_declenchement

                ' Final control
                .mwEcritureOsitrack(316) = finalDocument.programme
                .mwEcritureOsitrack(317) = finalDocument.portee_max_23d
                .mwEcritureOsitrack(318) = finalDocument.portee_min_23d
                .mwEcritureOsitrack(319) = finalDocument.hysteresis_max_23d
                .mwEcritureOsitrack(320) = finalDocument.hysteresis_min_23d
                .mwEcritureOsitrack(321) = finalDocument.V1
                .mwEcritureOsitrack(322) = finalDocument.V2
                .mwEcritureOsitrack(323) = finalDocument.courant_charge
                .mwEcritureOsitrack(324) = finalDocument.al1_conso_actif_min
                .mwEcritureOsitrack(325) = finalDocument.al1_conso_actif_max
                .mwEcritureOsitrack(326) = finalDocument.al2_conso_actif_min
                .mwEcritureOsitrack(327) = finalDocument.al2_conso_actif_max
                .mwEcritureOsitrack(328) = finalDocument.al1_conso_repos_min
                .mwEcritureOsitrack(329) = finalDocument.al1_conso_repos_max
                .mwEcritureOsitrack(330) = finalDocument.al2_conso_repos_min
                .mwEcritureOsitrack(331) = finalDocument.al2_conso_repos_max
                .mwEcritureOsitrack(332) = finalDocument.al1_courant_fuite_min
                .mwEcritureOsitrack(333) = finalDocument.al1_courant_fuite_max
                .mwEcritureOsitrack(334) = finalDocument.al2_courant_fuite_min
                .mwEcritureOsitrack(335) = finalDocument.al2_courant_fuite_max
                .mwEcritureOsitrack(336) = finalDocument.al1_tension_dechet_min
                .mwEcritureOsitrack(337) = finalDocument.al1_tension_dechet_max
                .mwEcritureOsitrack(338) = finalDocument.al2_tension_dechet_min
                .mwEcritureOsitrack(339) = finalDocument.al2_tension_dechet_max
                .mwEcritureOsitrack(340) = toWord(finalDocument.configuration.Substring(8, 2) & finalDocument.configuration.Substring(10, 2))
                .mwEcritureOsitrack(341) = toWord(finalDocument.configuration.Substring(4, 2) & finalDocument.configuration.Substring(6, 2))
                .mwEcritureOsitrack(342) = toWord(finalDocument.configuration.Substring(0, 2) & finalDocument.configuration.Substring(2, 2))
                .mwEcritureOsitrack(343) = finalDocument.tps_appli_cc
                .mwEcritureOsitrack(344) = finalDocument.portee_max_23d_C4
                .mwEcritureOsitrack(345) = finalDocument.portee_min_23d_C4

                ' The end
                MyOsitrack.chaineVersMots(currentManufacturingOrder.manufacturingOrderId, 377, 0)
                .mwEcritureOsitrack(382) = paletteNumber

                LabelInputStatus.Text = "Program tag"
                Application.DoEvents()

                ' Write
                .ecrireMotsOsitrack(0, 1000)

                ' Update counters
                paletteNumber += 1
                qtySent += 4

            End With

        Catch ex As Exception
            Application.DoEvents()
            MessageBox.Show("Programming failed : " & ex.Message, "XS156 linear tester", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Function toWord(hexaString As String) As Short
        Dim longWord As Long
        longWord = CLng("&H" & hexaString)

        If longWord > &H7FFF Then
            Return CShort(-(&H10000 - longWord))
        Else
            Return CShort(longWord)
        End If

    End Function


    Private Sub PictureBoxAckInputHazard_Click(sender As System.Object, e As System.EventArgs) Handles PictureBoxAckInputHazard.Click

        ' Ack
        ' Confirm to PLC
        AckInputHazard = True

    End Sub

    Private Sub PictureBoxAckOutputHazard_Click(sender As System.Object, e As System.EventArgs) Handles PictureBoxAckHiPotHazard.Click

        ' Ack
        ' Confirm to PLC
        AckHiPotHazard = True

    End Sub

    Private Sub PictureBoxAckOutputHazard_Click_1(sender As System.Object, e As System.EventArgs) Handles PictureBoxAckOutputHazard.Click

        ' Ack
        ' Confirm to PLC
        AckFinalHazard = True

    End Sub

    Function present(ByVal productNumber As Integer) As Boolean

        myPrinter.FontName = "Courier New"
        myPrinter.FontSize = 9

        ' Taken "as is" from Limoges

        Dim phase_defaut, index, mauv_absent As Integer
        index = productNumber * 8 - 8
        phase_defaut = MyOsitrack.stationsOsitrack(1).mwLectureOsitrack((1068 + index) / 2)
        mauv_absent = MyOsitrack.stationsOsitrack(1).mwLectureOsitrack(178 / 2)
        'le produit est-il présent?
        If (mauv_absent And CUInt(2 ^ (productNumber - 1))) = 0 Then
            'présent et bon
            Select Case productNumber
                Case 1
                    printProductData(productNumber, phase_defaut, False)
                    Product1.FillColor = Color.Green
                Case 2
                    printProductData(productNumber, phase_defaut, False)
                    Product2.FillColor = Color.Green
                Case 3
                    printProductData(productNumber, phase_defaut, False)
                    Product3.FillColor = Color.Green
                Case 4
                    printProductData(productNumber, phase_defaut, False)
                    Product4.FillColor = Color.Green
            End Select
            Return True
        Else
            'mauvais ou absent ?
            '   s'il a un code défaut : présent et mauvais, sinon : absent
            Select Case productNumber
                Case 1
                    If (phase_defaut <> 0) Then
                        printProductData(productNumber, phase_defaut, True)
                        Product1.FillColor = Color.Red
                    Else
                        Product1.FillColor = Me.BackColor
                    End If
                Case 2
                    If (phase_defaut <> 0) Then
                        printProductData(productNumber, phase_defaut, True)
                        Product2.FillColor = Color.Red
                    Else
                        Product2.FillColor = Me.BackColor
                    End If
                Case 3
                    If (phase_defaut <> 0) Then
                        printProductData(productNumber, phase_defaut, True)
                        Product3.FillColor = Color.Red
                    Else
                        Product3.FillColor = Me.BackColor
                    End If
                Case 4
                    If (phase_defaut <> 0) Then
                        printProductData(productNumber, phase_defaut, True)
                        Product4.FillColor = Color.Red
                    Else
                        Product4.FillColor = Me.BackColor
                    End If
            End Select

            Return (phase_defaut <> 0)
        End If
    End Function

    Sub printProductData(productNumber As Integer, phase_defaut As Integer, bad As Boolean)

        Dim SrU1 As Integer
        SrU1 = MyOsitrack.stationsOsitrack(1).mwLectureOsitrack((1556 + 52 * (productNumber - 1)) / 2)
        Dim SrU2 As Integer
        SrU2 = MyOsitrack.stationsOsitrack(1).mwLectureOsitrack((1558 + 52 * (productNumber - 1)) / 2)
        Dim HysU1 As Integer
        HysU1 = MyOsitrack.stationsOsitrack(1).mwLectureOsitrack((1560 + 52 * (productNumber - 1)) / 2)
        Dim HysU2 As Integer
        HysU2 = MyOsitrack.stationsOsitrack(1).mwLectureOsitrack((1562 + 52 * (productNumber - 1)) / 2)
        Dim Vd1 As Integer
        ' NPN or PNP so we can add the 2 values : one of them is always 0
        Vd1 = MyOsitrack.stationsOsitrack(1).mwLectureOsitrack((1532 + 52 * (productNumber - 1)) / 2) + MyOsitrack.stationsOsitrack(1).mwLectureOsitrack((1540 + 52 * (productNumber - 1)) / 2)
        Dim Vd2 As Integer
        Vd2 = MyOsitrack.stationsOsitrack(1).mwLectureOsitrack((1534 + 52 * (productNumber - 1)) / 2) + MyOsitrack.stationsOsitrack(1).mwLectureOsitrack((1542 + 52 * (productNumber - 1)) / 2)
        Dim Currentf1 As Integer
        '        Currentf1 = MyOsitrack.stationsOsitrack(1).mwLectureOsitrack(1520 + 52 * (productNumber - 1) / 2) + MyOsitrack.stationsOsitrack(1).mwLectureOsitrack(1528 + 52 * (productNumber - 1) / 2)
        Currentf1 = MyOsitrack.stationsOsitrack(1).mwLectureOsitrack((1548 + 52 * (productNumber - 1)) / 2) + MyOsitrack.stationsOsitrack(1).mwLectureOsitrack(1528 + 52 * (productNumber - 1) / 2)
        Dim Currentf2 As Integer
        'Currentf2 = MyOsitrack.stationsOsitrack(1).mwLectureOsitrack(1522 + 52 * (productNumber - 1) / 2) + MyOsitrack.stationsOsitrack(1).mwLectureOsitrack(1530 + 52 * (productNumber - 1) / 2)
        Currentf2 = MyOsitrack.stationsOsitrack(1).mwLectureOsitrack((1550 + 52 * (productNumber - 1)) / 2) + MyOsitrack.stationsOsitrack(1).mwLectureOsitrack(1530 + 52 * (productNumber - 1) / 2)
        If bad Then
            myPrinter.PrintAction = Printing.PrintAction.PrintToPrinter
            myPrinter.Print("Order " & MyOsitrack.motsVersChaine(91, 4, 1) & " " & MyOsitrack.motsVersChaine(0, 8, 1))
            myPrinter.Print("Product # " & productNumber.ToString & " Phase # " & phase_defaut.ToString)
            myPrinter.Print("Sr U1 (1/100mm) = " & SrU1.ToString)
            myPrinter.Print("Sr U2 (1/100mm) = " & SrU2.ToString)
            myPrinter.Print("Hyst U1 (1/100mm) = " & HysU1.ToString)
            myPrinter.Print("Hyst U2 (1/100mm) = " & HysU2.ToString)
            myPrinter.Print("Vd U1(V) = " & Vd1.ToString)
            myPrinter.Print("Vd U2(V) = " & Vd1.ToString)
            myPrinter.EndDoc()
        End If

        ' All products are tracked
        If currentOutputWorkOrder IsNot Nothing Then
            Try
                For Each oneEquipment As dataacquisitionDataSet.equipmentsRow In MyDataacquisitionDataSet.equipments
                    If oneEquipment.equipmentLabel = My.Settings.EquipmentNameFinal Then
                        MeasurementresultsTableAdapter.Insert(Now.Ticks.ToString & "-" & oneEquipment.equipmentId.ToString & productNumber.ToString, (MyOsitrack.stationsOsitrack(1).mwLectureOsitrack(85) - 1) * 4 + productNumber, _
                        currentOutputWorkOrder.workOrderId, Now, CUInt(phase_defaut), "Sr U1 (mm)", SrU1 / 100, "Sr U2 (mm)", SrU2 / 100, "Hyst U1  (mm)", HysU1 / 100, "Hyst U2  (mm)", HysU2 / 100,
                        "Vd1 (V)", Vd1 / 100, "Vd2 (V)", Vd2 / 100, "I U1(mA)", Currentf1, "I U2 (mA)", Currentf2, Nothing, 0, Nothing, 0, Nothing, 0, Nothing, 0, Nothing, 0, Nothing, 0, Nothing, 0, Nothing, 0, Nothing, 0, Nothing, 0, Nothing, 0, Nothing, 0)
                        Exit For
                    End If
                Next
            Catch ex As Exception
                LabelOutputStatus.Text = "Error creating the measurements record on output : " & ex.Message
            End Try
        End If


    End Sub


    Private Sub LabelQtyLaunched_Click(sender As Object, e As EventArgs) Handles LabelQtyLaunched.Click

    End Sub

    Private Sub LabelReferenceIn_Click(sender As Object, e As EventArgs) Handles LabelReferenceIn.Click

    End Sub

    Private Sub LabelQtySent_Click(sender As Object, e As EventArgs) Handles LabelQtySent.Click

    End Sub

    Private Sub Xs156Client_ExceptionEvent(info As String) Handles Xs156Client.ExceptionEvent
        '' MessageBox.Show(info)
    End Sub

    Private Sub Xs156Client_TrackingDataBagUpdatedEvent(data As XS156Client35.Models.TrackingDataBag) Handles Xs156Client.TrackingDataBagUpdatedEvent

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        qtySent += 4
    End Sub

    Private Sub MenuStrip_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles MenuStrip.ItemClicked

    End Sub
End Class
