using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using SFTP.Activities.Design.Designers;
using SFTP.Activities.Design.Properties;

namespace SFTP.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(SFTPScope), categoryAttribute);
            builder.AddCustomAttributes(typeof(SFTPScope), new DesignerAttribute(typeof(SFTPScopeDesigner)));
            builder.AddCustomAttributes(typeof(SFTPScope), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(Download), categoryAttribute);
            builder.AddCustomAttributes(typeof(Download), new DesignerAttribute(typeof(DownloadDesigner)));
            builder.AddCustomAttributes(typeof(Download), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(Upload), categoryAttribute);
            builder.AddCustomAttributes(typeof(Upload), new DesignerAttribute(typeof(UploadDesigner)));
            builder.AddCustomAttributes(typeof(Upload), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(Delete), categoryAttribute);
            builder.AddCustomAttributes(typeof(Delete), new DesignerAttribute(typeof(DeleteDesigner)));
            builder.AddCustomAttributes(typeof(Delete), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(GetFiles), categoryAttribute);
            builder.AddCustomAttributes(typeof(GetFiles), new DesignerAttribute(typeof(GetFilesDesigner)));
            builder.AddCustomAttributes(typeof(GetFiles), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
