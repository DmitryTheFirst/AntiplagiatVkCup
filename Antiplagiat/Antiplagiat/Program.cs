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

			IOrderedEnumerable<SourceFile> orderedFiles = files.OrderBy(a => a.ToString().Length);

			Comparer cmp = new Comparer();

			foreach (var file in orderedFiles)
			{
				cmp.AddSrcFile(file);
			}


			IO.WriteOutput(cmp.GetCheatersGroups(), "output.txt");
		}

		static void InitFiles(SourceFile[] files, String[] filePathes)
		{
			for (int i = 0; i < files.Length; i++)
			{
				String filePath = filePathes[i];
				files[i] = new SourceFile(DetectLanguage(filePath), IO.ReadFile(filePath), filePath);
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
		private const double Threshold = 0.1;

		private List<Group> groups = new List<Group>();


		public Group[] GetCheatersGroups()
		{
			return groups.Where(a => a.files.Count > 1).ToArray();
		}
		public void AddSrcFile(SourceFile src)
		{
			bool added = false;
			foreach (var group in groups)
			{
				foreach (var file in group.files)
				{
					if (Normalizer(src.ToString().Length, file.ToString().Length) <= Threshold)
					{
						if (Equals(src.ToString(), file.ToString()))
						{
							//add to group
							group.AddFile(src);
							added = true;
							break;
						}
					}
				}
			}
			if (!added)
			{
				Group newGroup = new Group(src);
				newGroup.AddFile(src);
				groups.Add(newGroup);
			}
		}



		double Normalizer(int length1, int length2)
		{
			int maxLength = Math.Max(length1, length2);
			return Math.Abs(length1 - length2) / ((double)maxLength);
		}

		static int LevenshteinDistance(string string1, string string2)
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

		public static bool Equals(String str1, String str2)
		{
			int maxLength = Math.Max(str1.Length, str2.Length);
			return LevenshteinDistance(str1, str2) / ((double)maxLength) < Threshold;
		}



	}

	class Group
	{
		public double averageLength;
		public List<SourceFile> files;


		public Group(SourceFile initSourceFile)
		{
			int initLength = initSourceFile.ToString().Length;
			files = new List<SourceFile>();
			averageLength = initLength;
		}

		public void AddFile(SourceFile file)
		{
			int newLength = file.ToString().Length;
			averageLength = ((averageLength * files.Count) + newLength) / (files.Count + 1);
			files.Add(file);
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

		static public void WriteOutput(Group[] res, String path)
		{
			StreamWriter sw = new StreamWriter(path);
			sw.WriteLine(res.Length);
			for (int i = 0; i < res.Length; i++)
			{
				sw.WriteLine(ConcatArr(res[i].files));
			}
			sw.Close();
		}

		static private String ConcatArr(List<SourceFile> arr)
		{
			StringBuilder sb = new StringBuilder();
			foreach (var sourceFile in arr)
			{
				sb.Append(sourceFile + " ");
			}

			return sb.ToString().TrimEnd();
		}

	}
	#endregion



	#endregion


}
