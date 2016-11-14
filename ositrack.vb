
Option Strict On

''' <summary>
''' 
''' ------------------------------------------------------------------
''' ANY USAGE OF THIS CODE OUTSIDE OF SCHNEIDER ELECTRIC IS PROHIBITED
''' ------------------------------------------------------------------
''' 
''' Apporte les interfaçages nécessaires à la communication avec OSITRACK en MODBUS TCP/IP.
''' 
''' La version 2.0 apporte le format VAT1 (Variable Allocation Table) :
''' 
''' %MW0
''' |---|---|---|---|---|---|---|---||---|---|---|---|---|---|---|---|
''' | Magic = 123                   ||    nombre de variables        |
''' |---|---|---|---|---|---|---|---||---|---|---|---|---|---|---|---|
''' 
''' %MW1 et suivants
''' |---|---|---|---|---|---|---|---||---|---|---|---|---|---|---|---|
''' | Type V|    taille chaine      || Type V|    taille chaine      |
''' |---|---|---|---|---|---|---|---||---|---|---|---|---|---|---|---|
''' 
''' Type = 0
''' Type = 1 : MOT
''' Type = 2 : MOTLONG
''' Type = 3 : CHAINE
''' 
''' Taille chaine en octets sur 6 bits
''' 
''' %MW? et suivants : variables à touche-touche.
''' Mot longs et chaines poids faible en tête.
'''  
''' 
''' -------------------------------------------------------------------
''' 
''' ''' La version 2.1 apporte le format VAT2 (Variable Allocation Table) :
''' 
''' %MW0
''' |---|---|---|---|---|---|---|---||---|---|---|---|---|---|---|---|
''' |          Magic = 246          ||    Version ou n° de projet    |
''' |---|---|---|---|---|---|---|---||---|---|---|---|---|---|---|---|
''' %MW1
''' |---|---|---|---|---|---|---|---||---|---|---|---|---|---|---|---|
''' |                   Nombre de variables                          |
''' |---|---|---|---|---|---|---|---||---|---|---|---|---|---|---|---|
''' 
''' %MW2 et suivants
''' |---|---|---|---|---|---|---|---||---|---|---|---|---|---|---|---|
''' |         Nom de la variable en MAJ complété par des espaces     |
''' |---|---|---|---|---|---|---|---||---|---|---|---|---|---|---|---|
''' |---|---|---|---|---|---|---|---||---|---|---|---|---|---|---|---|
''' |         Nom de la variable en MAJ complété par des espaces     |
''' |---|---|---|---|---|---|---|---||---|---|---|---|---|---|---|---|
''' |---|---|---|---|---|---|---|---||---|---|---|---|---|---|---|---|
''' |Type V |           Taille de la chaine (octets)                 |
''' |---|---|---|---|---|---|---|---||---|---|---|---|---|---|---|---|
''' 
''' Type = 0
''' Type = 1 : MOT
''' Type = 2 : MOTLONG
''' Type = 3 : CHAINE
''' 
''' Taille chaine en octets sur 14 bits
''' 
''' %MW? et suivants : variables à touche-touche.
''' Mot longs et chaines poids faible en tête.
''' 
''' </summary>
Public Class Ositrack

    ' Version 1.0 JLB 21/10/08 : 
    ' Création

    ' Version 1.1 JLB 29/10/08 :
    ' Inversion MSB/LSB dans les chaines de caractères à cause de MODBUS

    ' Version 2.0 JLB 26/08/09
    ' Entrée de nouvelles fonctions de gestion évoluées des variables avec une VAT
    ' Variables Allocation Table V1

    ' Version 2.1 JLB 07/09/09
    ' Entrée de nouvelles fonctions de gestion évoluées des variables avec une VAT
    ' Variables Allocation Table V2
    ' Les stationsOsitrack sont maintenant décrites sous forme d'une CLASSE et non d'une STRUCTURE

    ' Version 2.2 JLB 23/10/09
    ' Correction de divers bugs de traduction chaine <-> mots


    Inherits System.Windows.Forms.Form

    Const FORMATVAT1 As Integer = 123
    Const FORMATVAT2 As Integer = 246
    Const NBMAXVARIABLES As Integer = 63000 '15000

    Structure tagVAT
        Dim magic As Byte
        Dim niveauVAT As Byte
        Dim nbVariables As UShort
        Dim numProjetVersion As Byte
        Structure variableTag
            Enum typesVariable As Short
                MOT = 1
                MOTDOUBLE = 2
                CHAINE = 3
            End Enum
            Dim typeVariable As typesVariable
            Dim nomVariable As String
            Dim longueurChaine As UShort
        End Structure
        Dim variables() As variableTag
    End Structure

    ' -------------------------------
    ' Structure
    ' -------------------------------
    Public Class stationOsitrack
        Public VAT As tagVAT
        Public adresseEsclave As Byte
        Public nomStation As String
        Public esclavePresent As Boolean
        Public etiquettePresente As Boolean
        Public compteurEtiquettes As Integer
        Public detectionAutomatiqueTailleEtiquette As Boolean
        Public tailleEtiquetteMots As UShort
        Public UID As String
        Public mwLectureOsitrack() As Short
        Public mwEcritureOsitrack() As Short
        Dim _mwTempoVAT() As Short
        Public echangeOk As Boolean

        Public Function ecrireMotsOsitrack(ByVal indexPremierMot As UShort, ByVal nombreMots As UShort) As Boolean

            If indexPremierMot + nombreMots > Me.mwEcritureOsitrack.Length Then
                MsgBox("Vous demandez à écrire trop de mots. Maximum " & Me.mwEcritureOsitrack.Length)
                Exit Function
            End If

            _indexPremierMot = indexPremierMot
            _nombreDeMots = nombreMots
            _demandePourEcrire = True

            While _demandePourEcrire = True
                Application.DoEvents()
            End While

            Return (echangeOk)

        End Function

        Public Function lireMotsOsitrack(ByVal indexPremierMot As UShort, ByVal nombreMots As UShort) As Boolean

            If indexPremierMot + nombreMots > Me.mwLectureOsitrack.Length Then
                MsgBox("Vous demandez à lire trop de mots. Maximum " & Me.mwLectureOsitrack.Length)
                Exit Function
            End If

            _indexPremierMot = indexPremierMot
            _nombreDeMots = nombreMots
            _demandePourLire = True

            While _demandePourLire = True
                Application.DoEvents()
            End While

            Return (echangeOk)

        End Function

        Protected Friend _demandePourEcrire As Boolean
        Protected Friend _demandePourLire As Boolean
        Protected Friend _indexPremierMot As UShort
        Protected Friend _nombreDeMots As UShort
        Protected Friend _modeVAT As Boolean
        Protected Friend _uneFoisSurDeux As Boolean

#Region "GèreVAT"

        ''' <summary>
        ''' Construit la table d'attribution des variables du TAG.
        ''' La construction a lieu dans le tableau MWEcritureOsitrack, au début.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        Private Sub construitVAT1()

            Dim nbMots As UShort = 1

            ' EnTete
            VAT.magic = FORMATVAT1
            mwEcritureOsitrack(0) = Ositrack.ByteToWord(VAT.magic, CByte(VAT.nbVariables))

            ' Les variables
            If VAT.nbVariables > 0 Then
                Dim entreesVat(100) As Byte
                Dim entreeVat As Byte = 0
                Dim indexEntree As Integer = 0
                For Each uneVariable As tagVAT.variableTag In VAT.variables

                    entreeVat = CByte(uneVariable.typeVariable) << 6

                    ' String only
                    If uneVariable.typeVariable = tagVAT.variableTag.typesVariable.CHAINE Then
                        entreeVat = entreeVat + CByte(uneVariable.longueurChaine)
                    End If

                    entreesVat(indexEntree) = entreeVat
                    indexEntree += 1
                Next

                ' On doit composer poids faible/poids fort
                For indexTableEntrees As Integer = 0 To indexEntree - 1 Step 2
                    mwEcritureOsitrack(CInt(indexTableEntrees \ 2) + 1) = Ositrack.ByteToWord(entreesVat(indexTableEntrees + 1), entreesVat(indexTableEntrees))
                    nbMots += CUShort(1)
                Next
            End If

            _modeVAT = True

        End Sub

        ''' <summary>
        ''' Construit la table d'attribution des variables du TAG.
        ''' La construction a lieu dans le tableau MWEcritureOsitrack, au début.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        Private Sub construitVAT2(Optional ByVal numProjet As Byte = 0)

            Dim nbMots As UShort = 1

            ' EnTete
            VAT.magic = FORMATVAT2
            mwEcritureOsitrack(0) = Ositrack.ByteToWord(VAT.magic, VAT.numProjetVersion)
            mwEcritureOsitrack(1) = CShort(VAT.nbVariables)

            ' Les variables
            If VAT.nbVariables > 0 Then
                Dim nomVariableTableau(2) As Short
                Dim indexEntree As Integer = 2
                For Each uneVariable As tagVAT.variableTag In VAT.variables

                    Ositrack.chaineVersMots((uneVariable.nomVariable & "    ").Substring(0, 4), 0, 2, nomVariableTableau)
                    mwEcritureOsitrack(indexEntree) = nomVariableTableau(0)
                    mwEcritureOsitrack(indexEntree + 1) = nomVariableTableau(1)
                    mwEcritureOsitrack(indexEntree + 2) = CShort(Ositrack.ByteToWord(CByte(uneVariable.typeVariable << 6), 0) + uneVariable.longueurChaine)
                    indexEntree += 3
                Next
            End If
            _modeVAT = True

        End Sub

        ''' <summary>
        ''' Construit la table d'attribution des variables du TAG en mémoire
        ''' La construction a lieu à partir du tableau MWLectureOsitrack qui est lu pour l'occasion.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        Public Sub litVAT(ByVal lireAussiToutesLesDonnees As Boolean)

            ' EnTete (plus long en VAT2)
            Call lireMotsOsitrack(0, 2)

            VAT.magic = Ositrack.HiByte(mwLectureOsitrack(0))

            If VAT.magic = FORMATVAT1 Then

                ' On a la clé on continue
                _modeVAT = True
                VAT.niveauVAT = 1
                VAT.nbVariables = Ositrack.LoByte(mwLectureOsitrack(0))

                ' Lit toute la VAT
                ' -1 pour le mot 0 (clé+taille)
                Call lireMotsOsitrack(1, CUShort(numeroMotVariableVAT(0) - 1))

                Dim entreesVat(200) As Byte
                Dim entreeVat As Byte = 0
                For indexVariable = 0 To VAT.nbVariables Step 2
                    entreesVat(indexVariable) = Ositrack.LoByte(mwLectureOsitrack(indexVariable \ 2))
                    entreesVat(indexVariable + 1) = Ositrack.HiByte(mwLectureOsitrack(indexVariable \ 2))
                Next

                ' On épluche
                For indexVariable As Integer = 0 To VAT.nbVariables - 1
                    ReDim Preserve VAT.variables(indexVariable)
                    VAT.variables(indexVariable).typeVariable = CType(entreesVat(indexVariable) >> 6, tagVAT.variableTag.typesVariable)

                    If VAT.variables(indexVariable).typeVariable = tagVAT.variableTag.typesVariable.CHAINE Then
                        VAT.variables(indexVariable).longueurChaine = CUShort(entreesVat(indexVariable) And &H3F)
                    End If
                Next

                ' On prépare déjà la réécriture des données
                ReDim _mwTempoVAT(NBMAXVARIABLES)

                If lireAussiToutesLesDonnees Then
                    Call lireMotsOsitrack(0, numeroMotVariableVAT(VAT.nbVariables))
                End If


            ElseIf VAT.magic = FORMATVAT2 Then

                ' On a la clé on continue
                _modeVAT = True
                VAT.niveauVAT = 2
                VAT.numProjetVersion = Ositrack.LoByte(mwLectureOsitrack(0))
                VAT.nbVariables = CUShort(mwLectureOsitrack(1))

                ' Lit toute la VAT
                ' -2 pour le mot 0 et le mot 1 (clé+taille)
                Call lireMotsOsitrack(2, CUShort(VAT.nbVariables * 3))

                ' On épluche
                Dim indexVAT As Integer = 0
                Dim motsChaine(1) As Short
                For indexVariable As Integer = 0 To VAT.nbVariables - 1
                    ReDim Preserve VAT.variables(indexVariable)
                    VAT.variables(indexVariable).typeVariable = CType(Ositrack.HiByte(mwLectureOsitrack(indexVAT + 2)) >> 6, tagVAT.variableTag.typesVariable)

                    motsChaine(0) = mwLectureOsitrack(indexVAT)
                    motsChaine(1) = mwLectureOsitrack(indexVAT + 1)
                    ' Complété par des espaces et ramené à 4 caractères
                    VAT.variables(indexVariable).nomVariable = (Ositrack.motsVersChaine(0, 2, motsChaine).ToUpper & "    ").Substring(0, 4)

                    If VAT.variables(indexVariable).typeVariable = tagVAT.variableTag.typesVariable.CHAINE Then
                        VAT.variables(indexVariable).longueurChaine = CUShort(mwLectureOsitrack(indexVAT + 2) And &HFFF)
                    Else
                        VAT.variables(indexVariable).longueurChaine = 0
                    End If

                    indexVAT += 3

                Next

                ' On prépare déjà la réécriture des données
                ReDim _mwTempoVAT(NBMAXVARIABLES)

                If lireAussiToutesLesDonnees Then
                    Call lireMotsOsitrack(0, numeroMotVariableVAT(VAT.nbVariables))
                End If

            Else
                VAT.nbVariables = 0
                _modeVAT = False
                VAT.niveauVAT = 0
            End If
        End Sub

        ''' <summary>
        ''' Donne l'index, dans MWLectureOsitrack, du premier mot d'une variable.
        ''' </summary>
        ''' <remarks>
        ''' Remarques TRES importantes :
        ''' 1) numeroMotVariableVAT(0) donne l'index de la fin de la VAT +1 puisque c'est l'adresse 
        ''' de la première variable
        ''' 
        ''' 2) numeroMotVariableVAT(VAT.nbVariables) pointe la FIN des variables (le DERNIER MOT LIBRE)
        ''' puisque c'est en fait le pointeur sur la PROCHAINE variable à allouer
        ''' 
        ''' </remarks>
        Public Function numeroMotVariableVAT(ByVal indexVariable As Integer) As UShort

            If Not _modeVAT Then
                MsgBox("Problème d'initialisation, vous n'êtes pas en mode VAT.")
                Exit Function
            End If

            Dim monPointeur As UShort

            If VAT.magic = FORMATVAT1 Then

                ' A la mode boeuf : 1 c'est pour le mot MAGIC
                monPointeur = 1
                If VAT.nbVariables Mod 2 = 0 Then
                    monPointeur += CUShort(VAT.nbVariables \ 2)
                Else
                    monPointeur += CUShort((VAT.nbVariables + 1) \ 2)
                End If

                ' La première commence FORCEMENT en 2
                If indexVariable > 0 Then
                    For indexVAT As Integer = 0 To indexVariable - 1
                        Select Case VAT.variables(indexVAT).typeVariable

                            Case tagVAT.variableTag.typesVariable.MOT
                                monPointeur += CUShort(1)

                            Case tagVAT.variableTag.typesVariable.MOTDOUBLE
                                monPointeur += CUShort(2)

                            Case tagVAT.variableTag.typesVariable.CHAINE
                                monPointeur += CUShort((VAT.variables(indexVAT).longueurChaine + 1) \ 2)

                        End Select
                    Next
                End If

            ElseIf VAT.magic = FORMATVAT2 Then

                ' A la mode boeuf : 1 c'est pour le mot MAGIC et 2 pour la taille
                monPointeur = 2
                monPointeur = CUShort(monPointeur + VAT.nbVariables * 3)

                ' La première commence FORCEMENT en 2
                If indexVariable > 0 Then
                    For indexVAT As Integer = 0 To indexVariable - 1
                        Select Case VAT.variables(indexVAT).typeVariable

                            Case tagVAT.variableTag.typesVariable.MOT
                                monPointeur += CUShort(1)

                            Case tagVAT.variableTag.typesVariable.MOTDOUBLE
                                monPointeur += CUShort(2)

                            Case tagVAT.variableTag.typesVariable.CHAINE
                                monPointeur += CUShort((VAT.variables(indexVAT).longueurChaine + 1) \ 2)

                        End Select
                    Next
                End If

            End If

            Return monPointeur

        End Function

        Public Sub effaceVAT1()

            VAT.magic = FORMATVAT1
            VAT.nbVariables = 0
            ReDim VAT.variables(0)
            _modeVAT = True
            Call construitVAT1()

        End Sub

        Public Sub effaceVAT2()

            VAT.magic = FORMATVAT2
            VAT.nbVariables = 0
            ReDim VAT.variables(0)
            _modeVAT = True
            Call construitVAT2()

        End Sub

        Private Sub ajouteMotVAT(Optional ByVal nomChaine As String = "NONA")

            If Not _modeVAT Then
                MsgBox("Problème d'initialisation, vous n'êtes pas en mode VAT.")
                Exit Sub
            End If

            VAT.nbVariables += CUShort(1)
            ReDim Preserve VAT.variables(VAT.nbVariables - 1)

            VAT.variables(VAT.nbVariables - 1).nomVariable = (nomChaine & "    ").ToUpper.Substring(0, 4)
            VAT.variables(VAT.nbVariables - 1).typeVariable = tagVAT.variableTag.typesVariable.MOT

        End Sub

        Private Sub ajouteMotDoubleVAT(Optional ByVal nomChaine As String = "NONA")

            If Not _modeVAT Then
                MsgBox("Problème d'initialisation, vous n'êtes pas en mode VAT.")
                Exit Sub
            End If

            VAT.nbVariables += CUShort(1)
            ReDim Preserve VAT.variables(VAT.nbVariables - 1)

            VAT.variables(VAT.nbVariables - 1).nomVariable = (nomChaine & "    ").ToUpper.Substring(0, 4)
            VAT.variables(VAT.nbVariables - 1).typeVariable = tagVAT.variableTag.typesVariable.MOTDOUBLE

        End Sub

        Private Sub ajouteChaineVAT(ByVal chaine As String, Optional ByVal nomChaine As String = "NONA")

            If Not _modeVAT Then
                MsgBox("Problème d'initialisation, vous n'êtes pas en mode VAT.")
                Exit Sub
            End If

            VAT.nbVariables += CUShort(1)
            ReDim Preserve VAT.variables(VAT.nbVariables - 1)

            VAT.variables(VAT.nbVariables - 1).nomVariable = (nomChaine & "    ").ToUpper.Substring(0, 4)
            VAT.variables(VAT.nbVariables - 1).typeVariable = tagVAT.variableTag.typesVariable.CHAINE
            VAT.variables(VAT.nbVariables - 1).longueurChaine = CUShort(chaine.Length)

        End Sub

#End Region

#Region "VariablesVAT"

        ''' <summary>
        ''' On travaille dans un tableau TEMPORAIRE pour stocker les variables.
        ''' Pourquoi ? Parce que la VAT a une taille variable au fur et à mesure de l'entrée de variables nouvelles
        ''' et qu'on ne connait sa taille qu'à la fin.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        Private Function _numeroMotVariableTemporaire(ByVal indexVariable As Integer) As UShort

            If Not _modeVAT Then
                MsgBox("Problème d'initialisation, vous n'êtes pas en mode VAT.")
                Exit Function
            End If

            Dim monPointeur As UShort

            monPointeur = 0

            ' La première commence FORCEMENT en 0
            If indexVariable > 0 Then
                For indexVAT As Integer = 0 To indexVariable - 1
                    Select Case VAT.variables(indexVAT).typeVariable

                        Case tagVAT.variableTag.typesVariable.MOT
                            monPointeur += CUShort(1)

                        Case tagVAT.variableTag.typesVariable.MOTDOUBLE
                            monPointeur += CUShort(2)

                        Case tagVAT.variableTag.typesVariable.CHAINE
                            monPointeur += CUShort((VAT.variables(indexVAT).longueurChaine + 1) \ 2)

                    End Select
                Next
            End If

            Return monPointeur

        End Function

        Public Sub creeVariable(ByVal variable As Short, Optional ByVal nomVariable As String = "NONA")

            If Not _modeVAT Then
                MsgBox("Problème d'initialisation, vous n'êtes pas en mode VAT.")
                Exit Sub
            End If

            Call ajouteMotVAT(nomVariable)

            Dim indexVariable As Integer
            ' La dernière
            indexVariable = VAT.nbVariables - 1

            _mwTempoVAT(_numeroMotVariableTemporaire(indexVariable)) = variable

        End Sub

        Public Sub creeVariable(ByVal variable As Integer, Optional ByVal nomVariable As String = "NONA")

            If Not _modeVAT Then
                MsgBox("Problème d'initialisation, vous n'êtes pas en mode VAT.")
                Exit Sub
            End If

            Call ajouteMotDoubleVAT(nomVariable)

            Dim indexVariable As Integer
            ' La dernière
            indexVariable = VAT.nbVariables - 1

            Dim shortIntermediaire = variable And &HFFFF
            If shortIntermediaire > 32768 Then
                shortIntermediaire -= 32767
                shortIntermediaire *= -1
            End If
            _mwTempoVAT(_numeroMotVariableTemporaire(indexVariable)) = CShort(shortIntermediaire)
            _mwTempoVAT(_numeroMotVariableTemporaire(indexVariable) + 1) = CShort(variable \ 65536)


        End Sub

        Public Sub creeVariable(ByVal variable As String, Optional ByVal nomVariable As String = "NONA")

            If Not _modeVAT Then
                MsgBox("Problème d'initialisation, vous n'êtes pas en mode VAT.")
                Exit Sub
            End If

            Call ajouteChaineVAT(variable, nomVariable)

            Dim indexVariable As Integer
            ' La dernière
            indexVariable = VAT.nbVariables - 1

            Ositrack.chaineVersMots(variable, CShort(_numeroMotVariableTemporaire(indexVariable)), (variable.Length + 1) \ 2, _mwTempoVAT)

        End Sub
        ''' <summary>
        ''' Ramène une variable de l'espace MWLectureOsitrack et la traduit dans le bon type
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        Public Function indexVariableParNom(ByVal nomVariableVAT2 As String) As Integer

            If Not _modeVAT Then
                MsgBox("Problème d'initialisation, vous n'êtes pas en mode VAT.")
                Return Nothing
            End If

            If VAT.magic = FORMATVAT2 Then
                indexVariableParNom = -1
                For indexVariable As Integer = 0 To VAT.nbVariables
                    If VAT.variables(indexVariable).nomVariable.Trim = nomVariableVAT2.Trim Then
                        Return indexVariable
                    End If
                Next
            End If

        End Function
        ''' <summary>
        ''' Ramène une variable de l'espace MWLectureOsitrack et la traduit dans le bon type
        ''' </summary>
        ''' <remarks>
        ''' </remarks>

        Public Function litVariable(ByVal indexVariable As Integer, ByVal accesTagRFID As Boolean) As Object

            If Not _modeVAT Then
                MsgBox("Problème d'initialisation, vous n'êtes pas en mode VAT.")
                Return Nothing
            End If

            Select Case VAT.variables(indexVariable).typeVariable

                Case tagVAT.variableTag.typesVariable.MOT
                    If accesTagRFID Then
                        If lireMotsOsitrack(numeroMotVariableVAT(indexVariable), 1) = True Then
                            Return CShort(mwLectureOsitrack(0))
                        Else
                            Return Nothing
                        End If
                    Else
                        Return CShort(mwLectureOsitrack(numeroMotVariableVAT(indexVariable)))
                    End If

                Case tagVAT.variableTag.typesVariable.MOTDOUBLE
                    If accesTagRFID Then
                        If lireMotsOsitrack(numeroMotVariableVAT(indexVariable), 2) = True Then
                            Dim motTemporaire As Integer = mwLectureOsitrack(0)
                            If motTemporaire < 0 Then
                                motTemporaire *= -1
                                motTemporaire += 32767
                            End If
                            Return CInt(motTemporaire + mwLectureOsitrack(1) * 65536)
                        Else
                            Return Nothing
                        End If
                    Else
                        Dim motTemporaire As Integer = mwLectureOsitrack(numeroMotVariableVAT(indexVariable))
                        If motTemporaire < 0 Then
                            motTemporaire *= -1
                            motTemporaire += 32767
                        End If
                        Return CInt(motTemporaire + mwLectureOsitrack(numeroMotVariableVAT(indexVariable) + 1) * 65536)
                    End If

                Case tagVAT.variableTag.typesVariable.CHAINE
                    If accesTagRFID Then
                        If lireMotsOsitrack(numeroMotVariableVAT(indexVariable), CUShort((VAT.variables(indexVariable).longueurChaine + 1) \ 2)) Then
                            Return Ositrack.motsVersChaine(0, (VAT.variables(indexVariable).longueurChaine + 1) \ 2, mwLectureOsitrack).Substring(0, VAT.variables(indexVariable).longueurChaine)
                        Else
                            Return (Nothing)
                        End If
                    Else
                        Return Ositrack.motsVersChaine(numeroMotVariableVAT(indexVariable), (VAT.variables(indexVariable).longueurChaine + 1) \ 2, mwLectureOsitrack).Substring(0, VAT.variables(indexVariable).longueurChaine)
                    End If
            End Select

            Return Nothing

        End Function

        ''' <summary>
        ''' Ramène une variable de l'espace MWLectureOsitrack et la traduit dans le bon type
        ''' </summary>
        ''' <remarks>
        ''' </remarks>

        Public Function litVariable(ByVal nomVariable As String, ByVal accesTagRFID As Boolean) As Object

            If VAT.magic = FORMATVAT2 Then
                If Not _modeVAT Then
                    MsgBox("Problème d'initialisation, vous n'êtes pas en mode VAT.")
                    Return Nothing
                End If

                Dim indexVariable As Integer = indexVariableParNom(nomVariable)
                If indexVariable = -1 Then
                    MsgBox("Variable " & nomVariable & " non trouvée.")
                    Return Nothing
                End If

                ' Surcharge
                Return litVariable(indexVariableParNom(nomVariable), accesTagRFID)
            End If

            Return Nothing

        End Function

#End Region

        Public Sub formatteTagVAT1()

            Call effaceVAT1()
            ' On prépare déjà la réécriture des données
            ReDim _mwTempoVAT(1)
            Call fermeTagVAT()

        End Sub

        Public Sub formatteTagVAT2()

            Call effaceVAT2()
            ReDim _mwTempoVAT(1)
            Call fermeTagVAT()

        End Sub


        ''' <summary>
        ''' Vide l'espace temporaire vers l'espace réel d'écriture du tag et écrit le tag.
        ''' </summary>
        ''' <remarks>
        ''' numeroMotVariableVAT(VAT.nbVariables) est la taille du tableau à écrire car en fait c'est le pointeur
        ''' sur la prochaine variable qui sera créée.
        ''' </remarks>
        Public Sub fermeTagVAT()

            If VAT.magic = FORMATVAT1 Then
                Call construitVAT1()
                ' La VAT est dans les MWEcriture et s'arrête à l'index de la première variable.
                Dim indexMotTempo As Integer = 0
                For indexMot = numeroMotVariableVAT(0) To numeroMotVariableVAT(VAT.nbVariables)
                    mwEcritureOsitrack(indexMot) = _mwTempoVAT(indexMotTempo)
                    indexMotTempo += 1
                Next
                Call Me.ecrireMotsOsitrack(0, numeroMotVariableVAT(VAT.nbVariables))
            ElseIf VAT.magic = FORMATVAT2 Then
                Call construitVAT2()
                ' La VAT est dans les MWEcriture et s'arrête à l'index de la première variable.
                Dim indexMotTempo As Integer = 0
                For indexMot = numeroMotVariableVAT(0) To numeroMotVariableVAT(VAT.nbVariables)
                    mwEcritureOsitrack(indexMot) = _mwTempoVAT(indexMotTempo)
                    indexMotTempo += 1
                Next
                Call Me.ecrireMotsOsitrack(0, numeroMotVariableVAT(VAT.nbVariables))
            End If

        End Sub

    End Class
    '-----------------------------------------------------------------------------------------------

    Public periodeScrutationMs As UShort = 100 ' Millisecondes
    Public stationsDecouvertes As Short

    ' Variables internes
    Dim _adresseIP As String

    ' --------------------------------------------------------
    ' Champs de la classe
    ' --------------------------------------------------------
    Public stationsOsitrack(14) As stationOsitrack

    Const MAXI_MOTS As UInteger = 110

    Private WithEvents BackgroundWorkerOsitrack As System.ComponentModel.BackgroundWorker
    Private WithEvents ComModbus As ControlesCom.ComModbus

    '---------------------------------------------------------
    '----- PROPRIETES ----------------------------------------
    '---------------------------------------------------------

    Property adresseIp() As String
        Get
            Return _adresseIP
        End Get
        Set(ByVal value As String)
            If ComModbus.AdresseIP <> "" Then
                MsgBox("Le changement d'adresse IP est interdit. merci de détruire et réinstancier l'objet.")
            Else
                If value.Length > 7 Then
                    _adresseIP = value
                    ComModbus.AdresseIP = value
                Else
                    MsgBox("Adresse IP un peu courte !", MsgBoxStyle.Critical, "adresseIp")
                End If
            End If
        End Set
    End Property

    Public Sub New()
        Call InitializeComponent()

        For indexStation As Integer = 0 To 14
            stationsOsitrack(indexStation) = New stationOsitrack
        Next

        BackgroundWorkerOsitrack.CancelAsync()
    End Sub

    Public Sub decouvreStations(Optional ByVal Progressbar As ProgressBar = Nothing)

        Try
            For indexStation As Byte = 1 To 15

                ' Affichage
                If Not IsNothing(Progressbar) Then
                    Progressbar.Maximum = 15
                    Progressbar.Value = indexStation
                    Progressbar.Visible = True
                    Progressbar.Refresh()
                End If

                ' On va tenter de lire en &H8000
                Dim unMot(0) As Short
                If ComModbus.LireMots(32768, unMot, indexStation) Then
                    SetBit(stationsDecouvertes, indexStation)
                End If
            Next
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Public Sub lanceScrutation()
        Try
            If _adresseIP = "" Then
                MsgBox("Aucune adresse IP n'a été définie.")
                Exit Sub
            End If
            BackgroundWorkerOsitrack.RunWorkerAsync()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub stoppeScrutation()
        Try
            BackgroundWorkerOsitrack.CancelAsync()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Function pingOsitrack() As Boolean

        Return My.Computer.Network.Ping(ComModbus.AdresseIP)

    End Function

    Private Sub BackgroundWorkerAutomate_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorkerOsitrack.DoWork
        Static indexScrutation As UShort
        Dim resultatOperation As Boolean = False
        Static tamponLecture() As Short
        Static tamponEcriture() As Short

        Do
            ' Demande d'arrêt ?
            If BackgroundWorkerOsitrack.CancellationPending Then
                e.Cancel = True
                GoTo fin
            End If

            ' Go
            For indexStationScrutation As UShort = 0 To CUShort(stationsOsitrack.GetUpperBound(0))

                With stationsOsitrack(indexStationScrutation)
                    If .adresseEsclave = 0 Then
                        GoTo esclaveNonConfigure
                    End If

                    ' Les flags
                    Dim statusStation(5) As Short
                    If stationsOsitrack(indexStationScrutation)._uneFoisSurDeux Then
                        If ComModbus.LireMots(32768, statusStation, .adresseEsclave) = True Then
                            ' C'est bien lu :
                            .esclavePresent = True
                            If Bit(statusStation(0), 0) = False Then
                                .etiquettePresente = False
                            Else
                                ' Front montant de presenceEtiquette : c'est le moment de détecter sa taille.
                                If .etiquettePresente = False And .detectionAutomatiqueTailleEtiquette = True Then
                                    ' Par défaut
                                    .tailleEtiquetteMots = 56

                                    Dim unmot(0) As Short
                                    If ComModbus.LireMots(127, unmot, .adresseEsclave) Then
                                        .tailleEtiquetteMots = 128
                                        If ComModbus.LireMots(1700, unmot, .adresseEsclave) Then
                                            .tailleEtiquetteMots = CUShort(3408 / 2)
                                            If ComModbus.LireMots(6500, unmot, .adresseEsclave) Then
                                                .tailleEtiquetteMots = CUShort(13632 / 2)
                                            End If
                                        End If
                                    End If
                                    ReDim .mwEcritureOsitrack(.tailleEtiquetteMots - 1)
                                    ReDim .mwLectureOsitrack(.tailleEtiquetteMots - 1)
                                End If

                                .etiquettePresente = True

                                .UID = Hex2(LoByte(statusStation(5))) & Hex2(HiByte(statusStation(5))) & _
                                                 Hex2(LoByte(statusStation(4))) & Hex2(HiByte(statusStation(4))) & _
                                                 Hex2(LoByte(statusStation(3))) & Hex2(HiByte(statusStation(3))) & _
                                                 Hex2(LoByte(statusStation(2))) & Hex2(HiByte(statusStation(2)))

                                .compteurEtiquettes = statusStation(1)

                            End If
                        Else
                            .esclavePresent = False
                            .etiquettePresente = False
                            .tailleEtiquetteMots = 0
                            .UID = ""
                        End If
                    End If

                    stationsOsitrack(indexStationScrutation)._uneFoisSurDeux = CBool(IIf(stationsOsitrack(indexStationScrutation)._uneFoisSurDeux, False, True))

                    ' ------------------------------------
                    ' Ecriture
                    ' ------------------------------------
                    If ._demandePourEcrire = True Then

                        ' Init
                        indexScrutation = 0

                        While indexScrutation < ._nombreDeMots

                            ' Deux cas
                            If (._nombreDeMots - indexScrutation) >= MAXI_MOTS Then
                                ReDim tamponEcriture(MAXI_MOTS - 1)
                            Else
                                ReDim tamponEcriture(._nombreDeMots - indexScrutation - 1)
                            End If

                            ' Transfert du tableau vers tableau temporaire
                            For indexCopie As Integer = 0 To tamponEcriture.GetUpperBound(0)
                                tamponEcriture(indexCopie) = .mwEcritureOsitrack(indexScrutation + indexCopie)
                            Next

                            ' Cool
                            .echangeOk = ComModbus.EcrireMots(._indexPremierMot + indexScrutation, tamponEcriture, .adresseEsclave)
                            If .echangeOk Then
                                Debug.Print("Ecriture " & resultatOperation.ToString)
                            End If
                            indexScrutation += CUShort(MAXI_MOTS)
                        End While

                        ._demandePourEcrire = False
                    End If

                    ' ------------------------------------
                    ' LECTURE
                    ' ------------------------------------
                    If ._demandePourLire Then

                        ' Init
                        indexScrutation = 0

                        ' Go
                        While indexScrutation < ._nombreDeMots
                            ' Deux cas
                            If (._nombreDeMots - indexScrutation) >= MAXI_MOTS Then
                                ReDim tamponLecture(MAXI_MOTS - 1)
                            Else
                                ReDim tamponLecture(._nombreDeMots - indexScrutation - 1)
                            End If

                            ' Cool
                            .echangeOk = ComModbus.LireMots(._indexPremierMot + indexScrutation, tamponLecture, .adresseEsclave)
                            If .echangeOk Then
                                Debug.Print("Lecture " & resultatOperation.ToString)

                                ' Transfert du tableau temporaire vers tableau
                                For indexCopie As Integer = 0 To tamponLecture.GetUpperBound(0)
                                    .mwLectureOsitrack(indexScrutation + indexCopie) = tamponLecture(indexCopie)
                                Next
                            End If
                            indexScrutation += CUShort(MAXI_MOTS)
                        End While

                        ._demandePourLire = False
                    End If

esclaveNonConfigure:
                End With
            Next

            System.Threading.Thread.Sleep(periodeScrutationMs)
        Loop

fin:

    End Sub

    ''' <summary>
    ''' Transforme une lecture en chaîne de caractères
    ''' </summary>
    ''' <returns>Une chaîne.</returns>
    Public Function motsVersChaine(ByVal adresseDebut As Integer, ByVal nombreMots As Integer, ByVal indexStation As UShort) As String
        ' %MW => Chaine ASCII
        Dim chaineTemp As String = "", char1$, char2$

        ' on recree la chaine 
        For Index As Integer = adresseDebut To adresseDebut + nombreMots - 1
            Dim mot32bitsTempo As Long
            If stationsOsitrack(indexStation).mwLectureOsitrack(Index) >= 0 Then
                mot32bitsTempo = CLng(stationsOsitrack(indexStation).mwLectureOsitrack(Index))
            Else
                mot32bitsTempo = CLng(stationsOsitrack(indexStation).mwLectureOsitrack(Index)) + 65536L
            End If

            char1$ = Chr(CInt(mot32bitsTempo And &HFF))
            char2$ = Chr(CInt((mot32bitsTempo >> 8) And &HFF))
            chaineTemp = chaineTemp & char1$
            chaineTemp = chaineTemp & char2$
        Next

        ' On enleve les espaces
        Return chaineTemp.Replace(Chr(0), " ").Trim
        Return chaineTemp

    End Function

    ''' <summary>
    ''' Transforme une partie lecture en chaîne de caractères
    ''' </summary>
    ''' <returns>Une chaîne.</returns>
    Shared Function motsVersChaine(ByVal adresseDebut As Integer, ByVal nombreMots As Integer, ByVal mwLectureOsitrack() As Short) As String
        ' %MW => Chaine ASCII
        Dim chaineTemp As String = "", char1$, char2$

        ' on recree la chaine en ôtant les NULLs
        For Index As Integer = adresseDebut To adresseDebut + nombreMots - 1
            Dim mot32bitsTempo As Long
            If mwLectureOsitrack(Index) >= 0 Then
                mot32bitsTempo = CLng(mwLectureOsitrack(Index))
            Else
                mot32bitsTempo = CLng(mwLectureOsitrack(Index)) + 65536L
            End If

            ' INVERSION POIDS FORT/POIDS FAIBLE
            char1$ = Chr(CInt(mot32bitsTempo And &HFF))
            char2$ = Chr(CInt((mot32bitsTempo >> 8) And &HFF))
            chaineTemp = chaineTemp & char2$
            chaineTemp = chaineTemp & char1$
        Next

        ' On enleve les espaces
        'Return chaineTemp.Replace(Chr(0), "").Trim
        Return chaineTemp

    End Function


    ''' <summary>
    ''' Transforme une chaîne en mots dans la zone d'écriture.
    ''' </summary>
    Public Sub chaineVersMots(ByVal chaine As String, ByVal indexPremierMot As Short, ByVal indexStation As Integer)
        ' Surcharge
        If chaine.Length Mod 2 = 0 Then
            Call chaineVersMots(chaine, indexPremierMot, CInt(Int(CSng(chaine.Length) / 2)), stationsOsitrack(indexStation).mwEcritureOsitrack)
        Else
            Call chaineVersMots(chaine, indexPremierMot, CInt(Int(CSng(chaine.Length) / 2) + 1), stationsOsitrack(indexStation).mwEcritureOsitrack)
        End If
    End Sub

    Shared Sub chaineVersMots(ByVal chaine As String, ByVal indexPremierMot As Short, ByVal nombreMots As Integer, ByRef mots() As Short)
        ' Chaine ASCII => tableauMots()
        ' MODIFIED FOR XS156 !!!
        Dim index2 As Integer = 1, chaineTemp As String

        Try
            ' On met des espaces a la fin de la chaine
            chaineTemp = chaine & Space(nombreMots * 2)
            ' Couic !
            chaineTemp = chaineTemp.Substring(0, nombreMots * 2)

            ' On la convertit en tableauMots()
            For Index As Integer = indexPremierMot To indexPremierMot + nombreMots - 1
                ' INVERSION POIDS FORT/POIDS FAIBLE
                mots(Index) = CShort(256.0! * Asc(Mid(chaineTemp, index2 + 1, 1)) + Asc(Mid(chaineTemp, index2, 1)))
                index2 += 2
            Next
        Catch ex As Exception
            MsgBox("La chaîne " & chaine & " n'a pas pu être convertie !", MsgBoxStyle.Critical Or MsgBoxStyle.Information)
        End Try
    End Sub

    ''' <summary>
    ''' Test l'etat d'un bit de mot (entier signé 16 bits)
    ''' </summary>
    ''' <param name="w">Mot (entier signé 16 bits)</param>
    ''' <param name="b">Index du bit de 0 a 15</param>
    ''' <returns>Etat du bit</returns>
    ''' <remarks></remarks>
    Public Function Bit(ByVal w As Int16, ByVal b As Integer) As Boolean
        Dim ui As Int32

        ui = w
        If ui < 0 Then ui += &H10000

        Return CBool(ui And CInt(2 ^ b))
    End Function

    ''' <summary>
    ''' Set ou Reset d'un bit de mot (entier signé de 16 bits)
    ''' </summary>
    ''' <param name="w">Mot (entier signé de 16 bits)</param>
    ''' <param name="b">Index du bit de 0 a 15</param>
    ''' <param name="etat">Etat du bit</param>
    ''' <remarks></remarks>
    Public Sub SetBit(ByRef w As Int16, ByVal b As Integer, Optional ByVal etat As Boolean = True)
        Dim ui As Int32

        ui = w
        If ui < 0 Then ui += &H10000

        If etat Then
            ui = ui Or CInt(2 ^ b)
        Else
            ui = ui And CInt(&HFFFF - (2 ^ b))
        End If

        If ui > &H7FFF Then
            w = CShort(-(&H10000 - ui))
        Else
            w = CShort(ui)
        End If

    End Sub

    ''' <summary>
    ''' Retourne l'octet de poids faible d'un mot (entier signé de 16 bits)
    ''' </summary>
    ''' <param name="word">Mot (entier signé de 16 bits)</param>
    ''' <returns>Octet de poids faible</returns>
    ''' <remarks></remarks>
    Shared Function LoByte(ByVal word As Int16) As Byte
        Dim hexa As String

        hexa = Hex(word)
        While Len(hexa) < 4
            hexa = "0" + hexa
        End While

        Return CType("&h" + Mid(hexa, 3, 2), Byte)
    End Function

    ''' <summary>
    ''' Retourne l'octet de poids FORT d'un mot (entier signé de 16 bits)
    ''' </summary>
    ''' <param name="word">Mot (entier signé de 16 bits)</param>
    ''' <returns>Octet de poids FORT</returns>
    ''' <remarks></remarks>
    Shared Function HiByte(ByVal word As Int16) As Byte
        Dim hexa As String

        hexa = Hex(word)
        While Len(hexa) < 4
            hexa = "0" + hexa
        End While

        Return CType("&h" + Mid(hexa, 1, 2), Byte)
    End Function

    ''' <summary>
    ''' Constituation d'un mot a partir de deux octets
    ''' </summary>
    ''' <param name="hi">Octet de poids faible</param>
    ''' <param name="lo">Octet de poids FAIBLE</param>
    ''' <returns>Mot signé de 16 bits</returns>
    ''' <remarks></remarks>
    Shared Function ByteToWord(ByVal hi As Byte, ByVal lo As Byte) As Int16
        Dim ui As UInt32

        ui = CUInt((hi * 256) + lo)
        If ui > &H7FFF Then
            Return CShort(-(&H10000 - ui))
        Else
            Return CShort(ui)
        End If
    End Function

    ''' <summary>
    ''' Création d'une chaine de caractére a partir
    ''' d'un extrait de tableau d'octets.
    ''' </summary>
    ''' <param name="tableau">Tableau d'octets</param>
    ''' <param name="debut">Premier octet de la chaine a extraire</param>
    ''' <param name="longueur">Nombre d'octets a lire</param>
    ''' <returns>Chaine exraite</returns>
    ''' <remarks></remarks>
    Public Function ByteToString(ByVal tableau() As Byte, Optional ByVal debut As Integer = 0, Optional ByVal longueur As Integer = 0) As String
        'adapter les parametres
        If debut < 0 Then debut = 0
        If debut > tableau.Length Then debut = tableau.Length
        If longueur = 0 Then longueur = tableau.Length
        If debut + longueur > tableau.Length Then longueur = tableau.Length - debut

        'on transforme en chaine
        Dim i As Integer
        Dim chaine As String = ""
        For i = debut To debut + longueur
            If tableau(i) <> 0 Then chaine += Chr(tableau(i))
        Next

        'on retourne le tout
        Return chaine
    End Function

    ''' <summary>
    ''' Création d'une chaine de caractére a partir
    ''' d'un extrait de tableau de mots (int16).
    ''' </summary>
    ''' <param name="tableau">Tableau de mots (int16)</param>
    ''' <param name="debut">Premier mot de la chaine a extraire</param>
    ''' <param name="longueur">Nombre de mots a lire</param>
    ''' <returns>Chaine exraite</returns>
    ''' <remarks></remarks>
    Shared Function WordToString(ByVal tableau() As Int16, Optional ByVal debut As Integer = 0, Optional ByVal longueur As Integer = 0) As String
        'adapter les parametres
        If debut < 0 Then debut = 0
        If debut > (tableau.Length - 1) Then debut = (tableau.Length - 1)
        If longueur = 0 Then longueur = tableau.Length
        'If debut + longueur > (tableau.Length - 1) Then longueur = tableau.Length - debut - 1

        'on transforme le tableau en chaine
        Dim i As Integer
        Dim chaine As String = ""
        For i = debut To (debut + longueur - 1)
            If LoByte(tableau(i)) <> 0 Then chaine += Chr(LoByte(tableau(i)))
            If HiByte(tableau(i)) <> 0 Then chaine += Chr(HiByte(tableau(i)))
        Next

        'on retourne le tout
        Return chaine
    End Function

    ''' <summary>
    ''' Ecriture d'une chaine de caractere dans un tableau de mots
    ''' </summary>
    ''' <param name="chaine">Chaine de caractére</param>
    ''' <param name="tableau">Tableau de mots</param>
    ''' <param name="debut">Premier mot a utiliser</param>
    ''' <param name="longueur">Nombre de mots a utiliser</param>
    ''' <remarks></remarks>
    Shared Sub StringToWord(ByVal chaine As String, ByRef tableau() As Int16, Optional ByVal debut As Integer = 0, Optional ByVal longueur As Integer = 0)
        'adapter les parametres
        If Len(chaine) Mod 2 = 1 Then chaine += " "
        If debut < 0 Then debut = 0
        If debut > (tableau.Length - 1) Then debut = (tableau.Length - 1)
        If longueur = 0 Or longueur > (Len(chaine) / 2) Then longueur = CInt((Len(chaine) / 2))
        'If debut + longueur > (tableau.Length - 1) Then longueur = tableau.Length - debut - 1
        If Len(chaine) > (longueur * 2) Then chaine = chaine.Substring(1, longueur * 2)

        'on transforme la chaine en tableau
        Dim i As Integer
        For i = 0 To (longueur - 1)
            tableau(i + debut) = ByteToWord(CByte(Asc(Mid(chaine, (i * 2) + 2, 1))), CByte(Asc(Mid(chaine, (i * 2) + 1, 1))))
            'tableau(i + debut) = ByteToWord(CByte(Asc(Mid(chaine, (i * 2) + 1, 1))), CByte(Asc(Mid(chaine, (i * 2) + 2, 1))))
        Next
    End Sub

    ''' <summary>
    ''' Equivalent a la fonction hex, mais retourne une chaine de 
    ''' 2 caractéres avec des zéros a droite
    ''' </summary>
    ''' <param name="h">Valeure a traduire en hexa</param>
    ''' <returns>Chaine de 4 caractéres</returns>
    ''' <remarks></remarks>
    Function Hex2(ByVal h As Byte) As String
        Dim r As String

        r = Hex(h)
        r = StrDup(2 - Len(r), "0") + r

        Return r
    End Function

    ''' <summary>
    ''' Equivalent a la fonction hex, mais retourne une chaine de 
    ''' 4 caractéres avec des zéros a droite
    ''' </summary>
    ''' <param name="h">Valeure a traduire en hexa</param>
    ''' <returns>Chaine de 4 caractéres</returns>
    ''' <remarks></remarks>
    Function Hex4(ByVal h As Int16) As String
        Dim r As String

        r = Hex(h)
        r = StrDup(4 - Len(r), "0") + r

        Return r
    End Function

    ''' <summary>
    ''' Equivalent a la fonction hex, mais retourne une chaine de 
    ''' 8 caractéres avec des zéros a droite
    ''' </summary>
    ''' <param name="h">Valeure a traduire en hexa</param>
    ''' <returns>Chaine de 4 caractéres</returns>
    ''' <remarks></remarks>
    Function Hex8(ByVal h As Int32) As String
        Dim r As String

        r = Hex(h)
        r = StrDup(8 - Len(r), "0") + r

        Return r
    End Function

    Private Sub InitializeComponent()
        Me.ComModbus = New ControlesCom.ComModbus
        Me.BackgroundWorkerOsitrack = New System.ComponentModel.BackgroundWorker
        Me.SuspendLayout()
        '
        'ComModbus
        '
        Me.ComModbus.AdresseIP = Nothing
        Me.ComModbus.Location = New System.Drawing.Point(6, 7)
        Me.ComModbus.MaximumSize = New System.Drawing.Size(1000, 18)
        Me.ComModbus.MinimumSize = New System.Drawing.Size(70, 18)
        Me.ComModbus.Name = "ComModbus"
        Me.ComModbus.NomFenetreDebug = Nothing
        Me.ComModbus.Size = New System.Drawing.Size(239, 18)
        Me.ComModbus.TabIndex = 0
        '
        'BackgroundWorkerOsitrack
        '
        Me.BackgroundWorkerOsitrack.WorkerSupportsCancellation = True
        '
        'Ositrack
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(239, 31)
        Me.ControlBox = False
        Me.Controls.Add(Me.ComModbus)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Name = "Ositrack"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "comOsitrack - Double clic pour debug"
        Me.ResumeLayout(False)

    End Sub
End Class
