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
			result[0] = new String[]
			{
				"49_38a.cpp", "t61_31.cpp"
			};

			result[1] = new String[]
			{
				"91_a1.cpp", "9l_3_2dEd.cpp"
			};

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
