using Abp.Dependency;

namespace MHPQ
{
    public class AppFolders : IAppFolders, ISingletonDependency
    {
        public string TempFileDownloadFolder { get; set; }

        public string SampleProfileImagesFolder { get; set; }

        public string WebLogsFolder { get; set; }
        public string TempUploadFolder { get; set; }
    }
}