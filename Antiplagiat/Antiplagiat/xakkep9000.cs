using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Antiplagiat
{
	public enum Language
	{
		CPLUSPLUS,
		C,
		JAVA,
		CSHARP,
		PYTHON,
		PASCAL,
		UNKNOWN
	}

	public class SourceFile
	{
		private Language lagnguage { set; get; }
		private string content { set; get; }

		public SourceFile() { }

		public SourceFile(Language language, string content)
		{
			this.lagnguage = language;
			this.content = content;
		}

        //rm cpp c cs java
        private void RemoveShitCPLUSPLUS()
        {
		//rm cpp c cs java
		private void RemoveShitCPLUSPLUS()
		{
            //remove comments
            var regex = new Regex("/\\*[^*]*\\*+([^/*][^*]*\\*+)*/|(\"(\\\\.|[^\"\\\\])*\"|\'(\\\\.|[^\'\\\\])*\'|.[^/\"\'\\\\]*)");
            content = regex.Replace(content, "");
		}

		public void removeShit()
		{
			switch (lagnguage)
			{
				case Language.CPLUSPLUS:
					RemoveShitCPLUSPLUS();
					break;
				case Language.C:
					break;
				case Language.JAVA:
					break;
				case Language.CSHARP:
					break;
				case Language.PYTHON:
					break;
				case Language.PASCAL:
					break;
				case Language.UNKNOWN:
					break;
				default:
					break;
			}
		}
	}
}
