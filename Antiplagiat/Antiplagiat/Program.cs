using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antiplagiat
{
	class Program
	{
		static void Main(string[] args)
		{
			String[] filePathes = IO.ReadInput("input.txt");
			SourceFile[] files = new SourceFile[filePathes.Length];
			InitFiles(files, filePathes);





			String[][] result = new String[2][];
			IO.WriteOutput(result, "output.txt");
		}

		static void InitFiles(SourceFile[] files, String[] filePathes)
		{
			for (int i = 0; i < files.Length; i++)
			{
				String filePath = filePathes[i];
				files[i] = new SourceFile(DetectLanguage(filePath), IO.ReadFile(filePath));
			}
		}

		static Language DetectLanguage(String path)
		{
			String typeStr = path.Substring(path.LastIndexOf('.') + 1).Trim().ToLower();
			switch (typeStr)
			{
				case "cs":
					return Language.CSHARP;
				case "py":
					return Language.PYTHON;
				case "pas":
					return Language.PASCAL;
				case "java":
					return Language.JAVA;
				case "cpp":
					return Language.CPLUSPLUS;
				case "c":
					return Language.C;
				default:
					return Language.UNKNOWN;
			}
		}



	}


	class Comparer
	{
		private SourceFile[] sortedFiles;

		private const double Threshold = 0.1;


		public Comparer(SourceFile[] files)
		{
			sortedFiles = files.OrderBy(a => a.ToString().Length).ToArray();
		}

		public static int LevenshteinDistance(string string1, string string2)
		{
			if (string1 == null) throw new ArgumentNullException("string1");
			if (string2 == null) throw new ArgumentNullException("string2");
			int diff;
			int[,] m = new int[string1.Length + 1, string2.Length + 1];

			for (int i = 0; i <= string1.Length; i++) m[i, 0] = i;
			for (int j = 0; j <= string2.Length; j++) m[0, j] = j;

			for (int i = 1; i <= string1.Length; i++)
				for (int j = 1; j <= string2.Length; j++)
				{
					diff = (string1[i - 1] == string2[j - 1]) ? 0 : 1;

					m[i, j] = Math.Min(Math.Min(m[i - 1, j] + 1,
											 m[i, j - 1] + 1),
											 m[i - 1, j - 1] + diff);
				}

			return m[string1.Length, string2.Length];
		}
	}





	#region Imported
	#region Done
	static class IO
	{
		static public String[] ReadInput(String path)
		{
			StreamReader sr = new StreamReader(path);
			int count = int.Parse(sr.ReadLine());
			String[] res = new string[count];
			for (int i = 0; i < count; i++)
			{
				res[i] = sr.ReadLine();
			}
			sr.Close();
			return res;
		}

		static public String ReadFile(String path)
		{
			StreamReader sr = new StreamReader(path);
			String res = sr.ReadToEnd();
			sr.Close();
			return res;
		}

		static public void WriteOutput(String[][] res, String path)
		{
			StreamWriter sw = new StreamWriter(path);
			sw.WriteLine(res.Length);
			for (int i = 0; i < res.Length; i++)
			{
				sw.WriteLine(ConcatArr(res[i]));
			}
			sw.Close();
		}

		static private String ConcatArr(String[] arr)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < arr.Length - 1; i++)
			{
				sb.Append(arr[i] + " ");
			}
			sb.Append(arr[arr.Length - 1]);
			return sb.ToString();
		}

	}
	#endregion



	#endregion


}
