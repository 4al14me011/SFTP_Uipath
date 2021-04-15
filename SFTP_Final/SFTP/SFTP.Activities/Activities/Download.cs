using System;
using System.IO;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using SFTP.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using UiPath.Shared.Activities.Utilities;
using Renci.SshNet;

namespace SFTP.Activities
{
    [LocalizedDisplayName(nameof(Resources.Download_DisplayName))]
    [LocalizedDescription(nameof(Resources.Download_Description))]
    public class Download : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.Download_RemoteFullPath_DisplayName))]
        [LocalizedDescription(nameof(Resources.Download_RemoteFullPath_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> RemoteFullPath { get; set; }

        [LocalizedDisplayName(nameof(Resources.Download_LocalDirectory_DisplayName))]
        [LocalizedDescription(nameof(Resources.Download_LocalDirectory_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> LocalDirectory { get; set; }

        #endregion


        #region Constructors

        public Download()
        {
            Constraints.Add(ActivityConstraints.HasParentType<Download, SFTPScope>(string.Format(Resources.ValidationScope_Error, Resources.SFTPScope_DisplayName)));
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (RemoteFullPath == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(RemoteFullPath)));
            if (LocalDirectory == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(LocalDirectory)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Object Container: Use objectContainer.Get<T>() to retrieve objects from the scope
            var objectContainer = context.GetFromContext<IObjectContainer>(SFTPScope.ParentContainerPropertyTag);

            // Inputs
            var remoteFullPath = RemoteFullPath.Get(context);
            var localDirectory = LocalDirectory.Get(context);

            ///////////////////////////
            // Add execution logic HERE
            ///////////////////////////
            SftpClient sftp_session = objectContainer.Get<SftpClient>();
            //sftp_session.Connect();
            using (var file = File.OpenWrite(Path.Combine(@localDirectory, Path.GetFileName(remoteFullPath))))
            {
                sftp_session.DownloadFile(@remoteFullPath, file);
            }
            //sftp_session.Disconnect();


            // Outputs
            return (ctx) => {
            };
        }

        #endregion
    }
}

