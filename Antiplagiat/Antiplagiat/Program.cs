using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
				Console.WriteLine(file.path);
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
				case "dpr":
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
		private const double Threshold = 0.4;

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
				var file = group.files[0];
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



		#region LCS


		public static int GetLCS(string str1, string str2)
		{
			int[,] table;
			return GetLCSInternal(str1, str2, out table);
		}

		private static int GetLCSInternal(string str1, string str2, out int[,] matrix)
		{
			matrix = null;

			if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
			{
				return 0;
			}

			int[,] table = new int[str1.Length + 1, str2.Length + 1];
			for (int i = 0; i < table.GetLength(0); i++)
			{
				table[i, 0] = 0;
			}
			for (int j = 0; j < table.GetLength(1); j++)
			{
				table[0, j] = 0;
			}

			for (int i = 1; i < table.GetLength(0); i++)
			{
				for (int j = 1; j < table.GetLength(1); j++)
				{
					if (str1[i - 1] == str2[j - 1])
						table[i, j] = table[i - 1, j - 1] + 1;
					else
					{
						if (table[i, j - 1] > table[i - 1, j])
							table[i, j] = table[i, j - 1];
						else
							table[i, j] = table[i - 1, j];
					}
				}
			}

			matrix = table;
			return table[str1.Length, str2.Length];
		}




		static int[,] c;

		static int max(int a, int b)
		{
			return (a > b) ? a : b;
		}

		static int LCS(string s1, string s2)
		{
			for (int i = 1; i <= s1.Length; i++)
				c[i, 0] = 0;
			for (int i = 1; i <= s2.Length; i++)
				c[0, i] = 0;

			for (int i = 1; i <= s1.Length; i++)
				for (int j = 1; j <= s2.Length; j++)
				{
					if (s1[i - 1] == s2[j - 1])
						c[i, j] = c[i - 1, j - 1] + 1;
					else
					{
						c[i, j] = max(c[i - 1, j], c[i, j - 1]);

					}

				}

			return c[s1.Length, s2.Length];

		}
		/*      Prints one LCS
			   static string BackTrack(string s1, string s2, int i, int j)
				{

					if (i == 0 || j == 0)
						return "";
					if (s1[i - 1] == s2[j - 1])
						return BackTrack(s1, s2, i - 1, j - 1) + s1[i - 1];
					else if (c[i - 1, j] > c[i, j - 1])
						return BackTrack(s1, s2, i - 1, j);
					else
						return BackTrack(s1, s2, i, j - 1);

				}*/

		static SortedSet<string> backtrack(string s1, string s2, int i, int j)
		{
			if (i == 0 || j == 0)
				return new SortedSet<string>() { "" };
			else if (s1[i - 1] == s2[j - 1])
			{
				SortedSet<string> temp = new SortedSet<string>();
				SortedSet<string> holder = backtrack(s1, s2, i - 1, j - 1);
				if (holder.Count == 0)
				{
					temp.Add(s1[i - 1].ToString());
				}
				foreach (string str in holder)

					temp.Add(str + s1[i - 1]);


				return temp;
			}
			else
			{
				SortedSet<string> Result = new SortedSet<string>();
				if (c[i - 1, j] >= c[i, j - 1])
				{
					SortedSet<string> holder = backtrack(s1, s2, i - 1, j);

					foreach (string s in holder)
						Result.Add(s);
				}

				if (c[i, j - 1] >= c[i - 1, j])
				{
					SortedSet<string> holder = backtrack(s1, s2, i, j - 1);

					foreach (string s in holder)
						Result.Add(s);
				}


				return Result;
			}

		}


		#endregion



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
				sb.Append(sourceFile.path + " ");
			}

			return sb.ToString().TrimEnd();
		}

	}
	#endregion


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
		public string path { private set; get; }
		public Language lagnguage { private set; get; }
		public string content { private set; get; }

		public SourceFile() { }

		public SourceFile(Language language, string content, string path)
		{
			this.lagnguage = language;
			this.content = content;
			this.path = path;
		}

		//cpp c java
		private void RemoveCommentJAVA()
		{
			char? prev = null;
			char current = '\0';
			char[] result = new char[content.Length];
			int resultLength = 0;
			//0 - init
			//1 - line comment
			//2 - string
			//3 - multiline comment
			int state = 0;
			Action<char> put = chr => { result[resultLength] = chr; ++resultLength; };
			Action<char?> updPrev = chr => { prev = chr; };
			Action<int> updState = st => { state = st; };
			Action step = () => { if (prev.HasValue) put(prev.Value); updPrev(current); };
			Action finite_state_machine_body = () =>
			{
				switch (state)
				{
					case 0:	//default
						#region state0 default
						switch (current)
						{
							case '"':
								//string begin
								updState(2);
								step();
								break;
							case '*':
								if (prev.HasValue && prev == '/')
								{
									//ml cmmnt
									updState(3);
									updPrev(null);
								}
								else
								{
									step();
								}
								break;
							case '/':
								if (prev.HasValue && prev == '/')
								{
									//l cmmnt
									updState(1);
								}
								else
								{
									step();
								}
								break;
							default:
								step();
								break;
						}
						#endregion
						break;
					case 1:	//line comment
						#region state1 line comment
						switch (current)
						{
							case '\n':
								updState(0);
								updPrev(current);
								break;
							default:
								updPrev(current);
								break;
						}
						#endregion
						break;
					case 2:	//string
						#region state3 string
						switch (current)
						{
							case '"':
								if (!prev.HasValue || prev != '\\')
								{
									updState(0);
									step();
								}
								break;
							default:
								step();
								break;
						}
						#endregion
						break;
					case 3:	//ml comment
						#region state3 multiline comment
						switch (current)
						{
							case '/':
								if (prev.HasValue && prev == '*')
								{
									updState(0);
									updPrev(null);
								}
								else
								{
									updPrev(current);
								}
								break;
							default:
								updPrev(current);
								break;
						}
						#endregion
						break;
					default:
						break;
				}
			};

			#region finite state machine
			foreach (var _current in content)
			{
				//init current
				current = _current;
				//state machine body
				finite_state_machine_body();
			}
			#endregion

			#region last step
			current = '\n';
			finite_state_machine_body();
			#endregion

			content = new string(result.Take(resultLength).ToArray());
		}

		//cs
		private void RemoveCommentCSHARP()
		{
			char? prev = null;
			char current = '\0';
			char[] result = new char[content.Length];
			int resultLength = 0;
			//0 - init
			//1 - line comment
			//2 - string
			//3 - multiline comment
			//4 - raw string
			int state = 0;
			Action<char> put = chr => { result[resultLength] = chr; ++resultLength; };
			Action<char?> updPrev = chr => { prev = chr; };
			Action<int> updState = st => { state = st; };
			Action step = () => { if (prev.HasValue) put(prev.Value); updPrev(current); };
			Action finite_state_machine_body = () =>
			{
				switch (state)
				{
					case 0:	//default
						#region state0 default
						switch (current)
						{
							case '"':
								//string begin
								if (!prev.HasValue || (prev != '@' && prev != '\''))
								{
									updState(2);
									step();
								}
								else if (prev.HasValue && prev == '@')
								{
									updState(4);
									step();
								}
								break;
							case '*':
								if (prev.HasValue && prev == '/')
								{
									//ml cmmnt
									updState(3);
									updPrev(null);
								}
								else
								{
									step();
								}
								break;
							case '/':
								if (prev.HasValue && prev == '/')
								{
									//l cmmnt
									updState(1);
								}
								else
								{
									step();
								}
								break;
							default:
								step();
								break;
						}
						#endregion
						break;
					case 1:	//line comment
						#region state1 line comment
						switch (current)
						{
							case '\n':
								updState(0);
								updPrev(current);
								break;
							default:
								updPrev(current);
								break;
						}
						#endregion
						break;
					case 2:	//string
						#region state3 string
						switch (current)
						{
							case '"':
								if (!prev.HasValue || prev != '\\')
								{
									updState(0);
									step();
								}
								break;
							default:
								step();
								break;
						}
						#endregion
						break;
					case 3:	//ml comment
						#region state3 multiline comment
						switch (current)
						{
							case '/':
								if (prev.HasValue && prev == '*')
								{
									updState(0);
									updPrev(null);
								}
								else
								{
									updPrev(current);
								}
								break;
							default:
								updPrev(current);
								break;
						}
						#endregion
						break;
					case 4:	//raw string
						#region raw string
						switch (current)
						{
							case '"':
								updState(0);
								step();
								break;
							default:
								step();
								break;
						}
						#endregion
						break;
					default:
						break;
				}
			};

			#region finite state machine
			foreach (var _current in content)
			{
				//init current
				current = _current;
				//state machine body
				finite_state_machine_body();
			}
			#endregion

			#region last step
			current = '\n';
			finite_state_machine_body();
			#endregion

			content = new string(result.Take(resultLength).ToArray());
		}

		//pas
		private void RemoveCommentPASCAL()
		{
			char? prev = null;
			char current = '\0';
			char[] result = new char[content.Length];
			int resultLength = 0;
			//0 - init
			//1 - line comment
			//2 - string
			//3 - multiline comment { L1
			//4 - multiline comment (* L1
			//5 - multiline comment { L2
			//6 - multiline comment (* L2
			int state = 0;
			Action<char> put = chr => { result[resultLength] = chr; ++resultLength; };
			Action<char?> updPrev = chr => { prev = chr; };
			Action<int> updState = st => { state = st; };
			Action step = () => { if (prev.HasValue) put(prev.Value); updPrev(current); };
			Action finite_state_machine_body = () =>
			{
				switch (state)
				{
					case 0:	//default
						#region state0 default
						switch (current)
						{
							case '\'':
								//string begin
								updState(2);
								step();
								break;
							case '*':
								if (prev.HasValue && prev == '(')
								{
									//ml cmmnt (* L1
									updState(4);
									updPrev(null);
								}
								else
								{
									step();
								}
								break;
							case '{':
								//ml cmmnt { L1
								updState(3);
								step();
								break;
							case '/':
								if (prev.HasValue && prev == '/')
								{
									//l cmmnt
									updState(1);
								}
								else
								{
									step();
								}
								break;
							default:
								step();
								break;
						}
						#endregion
						break;
					case 1:	//line comment
						#region state1 line comment
						switch (current)
						{
							case '\n':
								updState(0);
								updPrev(current);
								break;
							default:
								updPrev(current);
								break;
						}
						#endregion
						break;
					case 2:	//string
						#region state3 string
						switch (current)
						{
							case '\'':
								updState(0);
								step();
								break;
							default:
								step();
								break;
						}
						#endregion
						break;
					case 3:	//ml comment { L1
						#region state3 multiline comment { L1
						switch (current)
						{
							case '*':
								if (prev.HasValue && prev == '(')
								{
									//ml comment (* L2
									updState(6);
									updPrev(null);
								}
								else
								{
									updPrev(current);
								}
								break;
							case '}':
								updState(0);
								updPrev(null);
								break;
							default:
								updPrev(current);
								break;
						}
						#endregion
						break;
					case 4:	//ml comment (* L1
						#region state4 multiline comment (* L1
						switch (current)
						{
							case ')':
								if (prev.HasValue && prev == '*')
								{
									updState(0);
									updPrev(null);
								}
								else
								{
									updPrev(current);
								}
								break;
							case '{':
								//ml comment { L2
								updState(5);
								updPrev(current);
								break;
							default:
								updPrev(current);
								break;
						}
						#endregion
						break;
					case 5:	//ml comment { L2
						#region state3 multiline comment { L2
						switch (current)
						{
							case '}':
								updState(4);
								updPrev(current);
								break;
							default:
								updPrev(current);
								break;
						}
						#endregion
						break;
					case 6:	//ml comment (* L2
						#region state4 multiline comment (* L2
						switch (current)
						{
							case ')':
								if (prev.HasValue && prev == '*')
								{
									updState(3);
									updPrev(current);
								}
								else
								{
									updPrev(current);
								}
								break;
							default:
								updPrev(current);
								break;
						}
						#endregion
						break;
					default:
						break;
				}
			};

			#region finite state machine
			foreach (var _current in content)
			{
				//init current
				current = _current;
				//state machine body
				finite_state_machine_body();
			}
			#endregion

			#region last step
			current = '\n';
			finite_state_machine_body();
			#endregion

			content = new string(result.Take(resultLength).ToArray());
		}

		//py
		private void RemoveCommentPYTHON()
		{
			char? prev = null;
			char current = '\0';
			char[] result = new char[content.Length];
			char quote = '\0';
			int resultLength = 0;
			//0 - init
			//1 - line comment
			//2 - string
			//3 - multiline comment
			int state = 0;
			Action<char> put = chr => { result[resultLength] = chr; ++resultLength; };
			Action<char?> updPrev = chr => { prev = chr; };
			Action<int> updState = st => { state = st; };
			Action step = () => { if (prev.HasValue) put(prev.Value); updPrev(current); };
			Action finite_state_machine_body = () =>
			{
				switch (state)
				{
					case 0:	//default
						#region state0 default
						if (current == '#')
						{
							updState(1);
							step();
						}
						else if (current == '\"' || current == '\'')
						{
							updState(2);
							quote = current;
							step();
						}
						else
						{
							step();
						}
						#endregion
						break;
					case 1:	//line comment
						#region state1 line comment
						switch (current)
						{
							case '\n':
								updState(0);
								updPrev(current);
								break;
							default:
								updPrev(current);
								break;
						}
						#endregion
						break;
					case 2:	//string open q1
						#region state3 string
						if (current == quote)
						{
							updState(3);//open q2
							step();
						}
						else
						{
							updState(4);//continue q1
							step();
						}
						#endregion
						break;
					#region string states
					case 4:	//string continue q1
						if (current == quote)
						{
							if (prev.HasValue && prev == '\\')
							{
								step();
							}
							else
							{
								//close string
								updState(0);
								step();
							}
						}
						else
						{
							step();
						}
						break;
					case 3:
						//open q2
						if (current == quote)
						{
							//continue q3
							updState(5);
							step();
						}
						else
						{
							updState(0);
							step();
						}
						break;
					case 5:
						//continue q3
						if (current == quote)
						{
							if (!prev.HasValue || prev != '\\')
							{
								updState(6); //close q1
								step();
							}
							else { step(); }
						}
						else { step(); }
						break;
					case 6:
						//close q1
						if (current == quote)
						{
							updState(7); //close q2
							step();
						}
						else
						{
							updState(5); //continue q3
							step();
						}
						break;
					case 7:
						//close q2
						if (current == quote)
						{
							updState(0);
							step();
						}
						else
						{
							updState(5);
							step();
						}
						break;
					#endregion
					default:
						break;
				}
			};

			#region finite state machine
			foreach (var _current in content)
			{
				//init current
				current = _current;
				//state machine body
				finite_state_machine_body();
			}
			#endregion

			#region last step
			current = '\n';
			finite_state_machine_body();
			#endregion

			content = new string(result.Take(resultLength).ToArray());
		}

		private void RemoveShitCPLUSPLUS()
		{
			//remove comments
			RemoveCommentJAVA();  //not supported R "delimeter(*raw symbols)delimeter"
		}

		private void RemoveShitJAVA()
		{
			//remove comments
			RemoveCommentJAVA(); //similar to cplusplus
		}

		private void RemoveShitCSHARP()
		{
			//remove comments
			RemoveCommentCSHARP(); //similar to cplusplus
		}

		//rm cpp c cs java
		private void RemoveShitPASCAL()
		{
			//remove comments
			RemoveCommentPASCAL();
		}

		//rm cpp c cs java
		private void RemoveShitPYTHON()
		{
			//remove comments
			RemoveCommentPYTHON();

			RemoveEmptyStrings();

			//replace 4spaces on tabs
			_4spacesToTabPYTHON();

			//make blocks
			MakeBlocksPYTHON();
		}

		private void _4spacesToTabPYTHON()
		{
			Regex r = new Regex(@"\n(\t*)(    )");
			string old = null;
			do
			{
				old = content;
				content = r.Replace(content, new MatchEvaluator(m =>
				{
					return "\n" + m.Groups[1] + "\t";
				}));
			} while (old != content);
		}

		private void RemoveEmptyStrings()
		{
			Regex r = new Regex(@"\n\s*\n");
			content = r.Replace(content, "\n");
			r = new Regex(@"\n\n+");
			content = r.Replace(content, "\n");
		}

		private void MakeBlocksPYTHON()
		{
			Regex r = new Regex(@"((\n\t[^\n]+)+)");
			string old = null;
			do
			{
				old = content;
				content = r.Replace(content, new MatchEvaluator(m =>
				{
					return "\n{" + m.Groups[1].ToString().Replace("\n\t", "\n") + "\n}";
				}));
			} while (old != content);
		}

		public void removeShit()
		{
			//erase return carrier
			content = content.Replace("\r", "");
			switch (lagnguage)
			{
				case Language.CPLUSPLUS:
					RemoveShitCPLUSPLUS();
					break;
				case Language.C:
					RemoveShitCPLUSPLUS();
					break;
				case Language.JAVA:
					RemoveShitJAVA();
					break;
				case Language.CSHARP:
					RemoveShitCSHARP();
					break;
				case Language.PYTHON:
					RemoveShitPYTHON();
					break;
				case Language.PASCAL:
					RemoveShitPASCAL();
					break;
				case Language.UNKNOWN:
					break;
				default:
					break;
			}
		}

		public override string ToString()
		{
			return content;
		}
	}
	#endregion


}
