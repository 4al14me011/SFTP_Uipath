using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using System.Activities.Statements;
using System.ComponentModel;
using SFTP.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using Renci.SshNet;

namespace SFTP.Activities
{
    [LocalizedDisplayName(nameof(Resources.SFTPScope_DisplayName))]
    [LocalizedDescription(nameof(Resources.SFTPScope_Description))]
    public class SFTPScope : ContinuableAsyncNativeActivity
    {
        #region Properties
        private SftpClient sftp_Session_Var;

        [Browsable(false)]
        public ActivityAction<IObjectContainer​> Body { get; set; }

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.SFTPScope_HostName_DisplayName))]
        [LocalizedDescription(nameof(Resources.SFTPScope_HostName_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> HostName { get; set; }

        [LocalizedDisplayName(nameof(Resources.SFTPScope_PortNumber_DisplayName))]
        [LocalizedDescription(nameof(Resources.SFTPScope_PortNumber_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<int> PortNumber { get; set; }

        [LocalizedDisplayName(nameof(Resources.SFTPScope_UserName_DisplayName))]
        [LocalizedDescription(nameof(Resources.SFTPScope_UserName_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> UserName { get; set; }

        [LocalizedDisplayName(nameof(Resources.SFTPScope_Password_DisplayName))]
        [LocalizedDescription(nameof(Resources.SFTPScope_Password_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Password { get; set; }

        // A tag used to identify the scope in the activity context
        internal static string ParentContainerPropertyTag => "ScopeActivity";

        // Object Container: Add strongly-typed objects here and they will be available in the scope's child activities.
        private readonly IObjectContainer _objectContainer;

        #endregion


        #region Constructors

        public SFTPScope(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;

            Body = new ActivityAction<IObjectContainer>
            {
                Argument = new DelegateInArgument<IObjectContainer> (ParentContainerPropertyTag),
                Handler = new Sequence { DisplayName = Resources.Do }
            };
        }

        public SFTPScope() : this(new ObjectContainer())
        {

        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            if (HostName == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(HostName)));
            if (PortNumber == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(PortNumber)));
            if (UserName == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(UserName)));
            if (Password == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Password)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<NativeActivityContext>> ExecuteAsync(NativeActivityContext  context, CancellationToken cancellationToken)
        {
            // Inputs
            var hostName = HostName.Get(context);
            var portNumber = PortNumber.Get(context);
            var userName = UserName.Get(context);
            var password = Password.Get(context);
            
            
            var sftp_session = new SftpClient(hostName, portNumber, userName, password);
            sftp_session.Connect();
            sftp_Session_Var = sftp_session;
            _objectContainer.Add<SftpClient>(sftp_session);

            return (ctx) => {
                // Schedule child activities
                if (Body != null)
				    ctx.ScheduleAction<IObjectContainer>(Body, _objectContainer, OnCompleted, OnFaulted);

                // Outputs
            };
        }

        #endregion


        #region Events

        private void OnFaulted(NativeActivityFaultContext faultContext, Exception propagatedException, ActivityInstance propagatedFrom)
        {
            sftp_Session_Var.Disconnect();
            faultContext.CancelChildren();
            Cleanup();
           
        }

        private void OnCompleted(NativeActivityContext context, ActivityInstance completedInstance)
        {
            sftp_Session_Var.Disconnect();
            Cleanup();
        }

        #endregion


        #region Helpers
        
        private void Cleanup()
        {
            var disposableObjects = _objectContainer.Where(o => o is IDisposable);
            foreach (var obj in disposableObjects)
            {
                if (obj is IDisposable dispObject)
                    dispObject.Dispose();
            }
            _objectContainer.Clear();
        }

        #endregion
    }
}

