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
    [LocalizedDisplayName(nameof(Resources.Upload_DisplayName))]
    [LocalizedDescription(nameof(Resources.Upload_Description))]
    public class Upload : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.Upload_UploadPath_DisplayName))]
        [LocalizedDescription(nameof(Resources.Upload_UploadPath_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> UploadPath { get; set; }

        [LocalizedDisplayName(nameof(Resources.Upload_RemoteDirectoryPath_DisplayName))]
        [LocalizedDescription(nameof(Resources.Upload_RemoteDirectoryPath_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> RemoteDirectoryPath { get; set; }

        #endregion


        #region Constructors

        public Upload()
        {
            Constraints.Add(ActivityConstraints.HasParentType<Upload, SFTPScope>(string.Format(Resources.ValidationScope_Error, Resources.SFTPScope_DisplayName)));
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (UploadPath == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(UploadPath)));
            if (RemoteDirectoryPath == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(RemoteDirectoryPath)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Object Container: Use objectContainer.Get<T>() to retrieve objects from the scope
            var objectContainer = context.GetFromContext<IObjectContainer>(SFTPScope.ParentContainerPropertyTag);

            // Inputs
            var uploadPath = UploadPath.Get(context);
            var remoteDirectoryPath = RemoteDirectoryPath.Get(context);

            ///////////////////////////
            // Add execution logic HERE
            ///////////////////////////
            ///
            SftpClient sftp_session = objectContainer.Get<SftpClient>();
            //sftp_session.Connect();
            using (var file = File.OpenRead(@uploadPath))
            {
                sftp_session.UploadFile(file, Path.Combine(@remoteDirectoryPath, Path.GetFileName(uploadPath)));
            }

            //sftp_session.Disconnect();

            // Outputs
            return (ctx) => {
            };
        }

        #endregion
    }
}

