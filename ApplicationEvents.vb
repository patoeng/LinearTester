Namespace My

    ' Les événements suivants sont disponibles pour MyApplication :
    ' 
    ' Startup : déclenché au démarrage de l'application avant la création du formulaire de démarrage.
    ' Shutdown : déclenché après la fermeture de tous les formulaires de l'application. Cet événement n'est pas déclenché si l'application se termine de façon anormale.
    ' UnhandledException : déclenché si l'application rencontre une exception non gérée.
    ' StartupNextInstance : déclenché lors du lancement d'une application à instance unique et si cette application est déjà active. 
    ' NetworkAvailabilityChanged : déclenché lorsque la connexion réseau est connectée ou déconnectée.
    Partial Friend Class MyApplication

        Private Sub MyApplication_Startup(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            If System.IO.File.Exists(Settings.Bdd_badgesDirectory & "\BDD_BADGES.MDB") Then
                Try
                    System.IO.File.Copy(Settings.Bdd_badgesDirectory & "\BDD_BADGES.MDB", Application.Info.DirectoryPath & "\BDD_BADGES.MDB", True)
                Catch ex As Exception
                End Try
            Else
                System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = False
                SplashScreen.Hide()
                Application.DoEvents()
                MessageBox.Show("BDD_BADGES.MDB not found !")
            End If
        End Sub

    End Class


End Namespace

