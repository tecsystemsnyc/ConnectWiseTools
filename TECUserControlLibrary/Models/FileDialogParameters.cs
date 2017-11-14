namespace TECUserControlLibrary.Models
{
    public struct FileDialogParameters
    {
        public string Filter;
        public string DefaultExtension;

        #region File Parameters
        public static FileDialogParameters EstimateFileParameters
        {
            get
            {
                FileDialogParameters fileParams;
                fileParams.Filter = "Estimate Database Files (*.edb)|*.edb" + "|All Files (*.*)|*.*";
                fileParams.DefaultExtension = "edb";
                return fileParams;
            }
        }
        public static FileDialogParameters TemplatesFileParameters
        {
            get
            {
                FileDialogParameters fileParams;
                fileParams.Filter = "Templates Database Files (*.tdb)|*.tdb" + "|All Files (*.*)|*.*";
                fileParams.DefaultExtension = "tdb";
                return fileParams;
            }
        }
        public static FileDialogParameters DocumentFileParameters
        {
            get
            {
                FileDialogParameters fileParams;
                fileParams.Filter = "Rich Text Files (*.rtf)|*.rtf";
                fileParams.DefaultExtension = "rtf";
                return fileParams;
            }
        }
        public static FileDialogParameters WordDocumentFileParameters
        {
            get
            {
                FileDialogParameters fileParams;
                fileParams.Filter = "Word Documents (*.docx)|*.docx";
                fileParams.DefaultExtension = "docx";
                return fileParams;
            }
        }
        public static FileDialogParameters CSVFileParameters
        {
            get
            {
                FileDialogParameters fileParams;
                fileParams.Filter = "Comma Separated Values Files (*.csv)|*.csv";
                fileParams.DefaultExtension = "csv";
                return fileParams;
            }
        }
        public static FileDialogParameters ExcelFileParameters
        {
            get
            {
                FileDialogParameters fileParams;
                fileParams.Filter = "Excel files (*.xlsx)|*.xlsx";
                fileParams.DefaultExtension = "xlsx";
                return fileParams;
            }
        }
        #endregion
    }
}
