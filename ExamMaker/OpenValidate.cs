using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Schema;
using System.IO;

namespace ExamMaker
{
    class OpenValidate
    {
        public bool failed = false;
        private string status = null;

        private void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                status += "\nWarning: Matching schema not found.  No validation occurred." + args.Message;
            else
                status = "\tValidation error: " + args.Message;
            failed = true;
        }
        public void ValidateXml(string xmlFilename, string schemaFilename)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType |= ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
            //XmlSchemaSet schemas = new XmlSchemaSet();
            //settings.Schemas = schemas;
            settings.Schemas.Add("urn:Question-Schema", schemaFilename);

            XmlReader validator = XmlReader.Create(xmlFilename, settings);
            while (validator.Read()) ;
        }
    }

}
