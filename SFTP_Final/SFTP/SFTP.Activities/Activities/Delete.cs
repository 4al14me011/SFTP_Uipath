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
    [LocalizedDisplayName(nameof(Resources.Delete_DisplayName))]
    [LocalizedDescription(nameof(Resources.Delete_Description))]
    public class Delete : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.Delete_RemoteFileFullPath_DisplayName))]
        [LocalizedDescription(nameof(Resources.Delete_RemoteFileFullPath_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> RemoteFileFullPath { get; set; }

        #endregion


        #region Constructors

        public Delete()
        {
            Constraints.Add(ActivityConstraints.HasParentType<Delete, SFTPScope>(string.Format(Resources.ValidationScope_Error, Resources.SFTPScope_DisplayName)));
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (RemoteFileFullPath == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(RemoteFileFullPath)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Object Container: Use objectContainer.Get<T>() to retrieve objects from the scope
            var objectContainer = context.GetFromContext<IObjectContainer>(SFTPScope.ParentContainerPropertyTag);

            // Inputs
            var remoteFileFullPath = RemoteFileFullPath.Get(context);

            ///////////////////////////
            // Add execution logic HERE
            ///////////////////////////
            ///
            SftpClient sftp_session = objectContainer.Get<SftpClient>();
            //sftp_session.Connect();
            sftp_session.DeleteFile(@remoteFileFullPath);
            //sftp_session.Disconnect();

            // Outputs
            return (ctx) => {
            };
        }

        #endregion
    }
}

