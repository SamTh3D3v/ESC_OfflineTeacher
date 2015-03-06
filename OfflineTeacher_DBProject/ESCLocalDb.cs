namespace OfflineTeacher_DBProject {
    
    
    public partial class ESCLocalDbSyncAgent {
        
        partial void OnInitialized()
        {
            this.ANNEES.SyncDirection = Microsoft.Synchronization.Data.SyncDirection.DownloadOnly;
            this.CATEGORIES.SyncDirection = Microsoft.Synchronization.Data.SyncDirection.DownloadOnly; 
            this.ENSEIGNANTS.SyncDirection = Microsoft.Synchronization.Data.SyncDirection.DownloadOnly;
            this.ETUDIANTS.SyncDirection = Microsoft.Synchronization.Data.SyncDirection.DownloadOnly; 
            this.EXAMENS.SyncDirection = Microsoft.Synchronization.Data.SyncDirection.DownloadOnly;
            this.EXAMENS_ANNEES_MODES_ETUDES.SyncDirection = Microsoft.Synchronization.Data.SyncDirection.DownloadOnly; 
            this.GROUPES.SyncDirection = Microsoft.Synchronization.Data.SyncDirection.DownloadOnly;
            this.MATIERES.SyncDirection = Microsoft.Synchronization.Data.SyncDirection.DownloadOnly;
            this.NOTE.SyncDirection = Microsoft.Synchronization.Data.SyncDirection.DownloadOnly;
            this.SECTIONS.SyncDirection = Microsoft.Synchronization.Data.SyncDirection.DownloadOnly;
            this.SPECIALITES.SyncDirection = Microsoft.Synchronization.Data.SyncDirection.DownloadOnly;
            this.SPECIALITES_ANNEES_MODES_ETUDES.SyncDirection = Microsoft.Synchronization.Data.SyncDirection.DownloadOnly;
            this.SPECIALITES_MATIERES.SyncDirection = Microsoft.Synchronization.Data.SyncDirection.DownloadOnly;
            this.NOTES_EXAMEN.SyncDirection = Microsoft.Synchronization.Data.SyncDirection.Bidirectional;
            this.NOTE_DETTE.SyncDirection = Microsoft.Synchronization.Data.SyncDirection.Bidirectional;
            this.LOG.SyncDirection = Microsoft.Synchronization.Data.SyncDirection.UploadOnly;
        }
    }
}
