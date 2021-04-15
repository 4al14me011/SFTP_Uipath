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

            builder.AddCustomAttributes(typeof(SFTPFileDownload), categoryAttribute);
            builder.AddCustomAttributes(typeof(SFTPFileDownload), new DesignerAttribute(typeof(SFTPFileDownloadDesigner)));
            builder.AddCustomAttributes(typeof(SFTPFileDownload), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(SFTPFileUpLoad), categoryAttribute);
            builder.AddCustomAttributes(typeof(SFTPFileUpLoad), new DesignerAttribute(typeof(SFTPFileUpLoadDesigner)));
            builder.AddCustomAttributes(typeof(SFTPFileUpLoad), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(SFTPDelete), categoryAttribute);
            builder.AddCustomAttributes(typeof(SFTPDelete), new DesignerAttribute(typeof(SFTPDeleteDesigner)));
            builder.AddCustomAttributes(typeof(SFTPDelete), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
