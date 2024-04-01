using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace withDBSql
{
    internal class SertHelper
    {
        private FileInfo _fileInfo;

        public SertHelper(string fileName)
        {
            if (File.Exists(fileName))
            {
                _fileInfo = new FileInfo(fileName);
            }
            else
            {
                throw new ArgumentException("File not found");
            }
        }


        public string Process(Dictionary<string, string> items)
        {
            Word.Application app = null;
            app = new Word.Application();
            Object file = _fileInfo.FullName;

            Object missing = Type.Missing;

            app.Documents.Open(file);

            foreach (var item in items)
            {
                Word.Find find = app.Selection.Find;
                find.Text = item.Key;
                find.Replacement.Text = item.Value;

                Object warp = Word.WdFindWrap.wdFindContinue;
                Object replace = Word.WdReplace.wdReplaceAll;

                find.Execute(FindText: Type.Missing,
                    MatchCase: false,
                    MatchWholeWord: false,
                    MatchWildcards: false,
                    MatchSoundsLike: missing,
                    MatchAllWordForms: false,
                    Forward: true,
                    Wrap: warp,
                    Format: false,
                    ReplaceWith: missing, Replace: replace);
            }

            Object newFileName = Path.Combine(_fileInfo.DirectoryName, DateTime.Now.ToString("yyyyMMdd HHmmss ") + ".pdf");
            app.ActiveDocument.SaveAs2(newFileName);
            app.ActiveDocument.Close();
            if (app != null)
            {
                app.Quit();
            }
            return newFileName.ToString();
          
        }
    }
}

