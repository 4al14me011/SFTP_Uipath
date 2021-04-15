using System;
using System.IO;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using SFTP.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using UiPath.Shared.Activities.Utilities;
using Renci.SshNet;

namespace SFTP.Activities
{
    [LocalizedDisplayName(nameof(Resources.GetFiles_DisplayName))]
    [LocalizedDescription(nameof(Resources.GetFiles_Description))]
    public class GetFiles : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.GetFiles_RemoteDirectoryPath_DisplayName))]
        [LocalizedDescription(nameof(Resources.GetFiles_RemoteDirectoryPath_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> RemoteDirectoryPath { get; set; }

        [LocalizedDisplayName(nameof(Resources.GetFiles_IList_DisplayName))]
        [LocalizedDescription(nameof(Resources.GetFiles_IList_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<List<string>> IList { get; set; }

        #endregion


        #region Constructors

        public GetFiles()
        {
            Constraints.Add(ActivityConstraints.HasParentType<GetFiles, SFTPScope>(string.Format(Resources.ValidationScope_Error, Resources.SFTPScope_DisplayName)));
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (RemoteDirectoryPath == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(RemoteDirectoryPath)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Object Container: Use objectContainer.Get<T>() to retrieve objects from the scope
            var objectContainer = context.GetFromContext<IObjectContainer>(SFTPScope.ParentContainerPropertyTag);

            // Inputs
            var remoteDirectoryPath = RemoteDirectoryPath.Get(context);

            ///////////////////////////
            // Add execution logic HERE
            ///////////////////////////
            ///
            SftpClient sftp_session = objectContainer.Get<SftpClient>();
            //sftp_session.Connect();

            //List<System.Collections.IList> ashley = new List<System.Collections.IList>();
            List<String> ashley = new List<String>();
            foreach (var entry in sftp_session.ListDirectory(@remoteDirectoryPath))
            {

                if (entry.IsDirectory)
                {
                    continue;
                }
                else
                {
                    ashley.Add(entry.Name);
                }
            }

            // Outputs
            return (ctx) => {
                IList.Set(ctx, ashley);
            };
        }

        #endregion
    }
}

