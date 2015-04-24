using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antiplagiat
{
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
}
