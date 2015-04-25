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
            Action steps = () => { if (prev.HasValue) put('s'); updPrev(current); };
            Action finite_state_machine_body = () =>
            {
                switch (state)
                {
                    case 0: //default
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
                    case 1: //line comment
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
                    case 2: //string
                        #region state3 string
                        switch (current)
                        {
                            case '"':
                                if (!prev.HasValue || prev != '\\')
                                {
                                    updState(0);
                                    steps();
                                    updPrev(null);
                                }
                                break;
                            default:
                                steps();
                                break;
                        }
                        #endregion
                        break;
                    case 3: //ml comment
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
            Action steps = () => { if (prev.HasValue) put('s'); updPrev(current); };
            Action finite_state_machine_body = () =>
            {
                switch (state)
                {
                    case 0: //default
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
                                    steps();
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
                    case 1: //line comment
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
                    case 2: //string
                        #region state3 string
                        switch (current)
                        {
                            case '"':
                                if (!prev.HasValue || prev != '\\')
                                {
                                    updState(0);
                                    steps();
                                    updPrev(null);
                                }
                                break;
                            default:
                                steps();
                                break;
                        }
                        #endregion
                        break;
                    case 3: //ml comment
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
                    case 4: //raw string
                        #region raw string
                        switch (current)
                        {
                            case '"':
                                updState(0);
                                steps();
                                updPrev(null);
                                break;
                            default:
                                steps();
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
            Action steps = () => { if (prev.HasValue) put('s'); updPrev(current); };
            Action finite_state_machine_body = () =>
            {
                switch (state)
                {
                    case 0: //default
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
                    case 1: //line comment
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
                    case 2: //string
                        #region state3 string
                        switch (current)
                        {
                            case '\'':
                                updState(0);
                                steps();
                                updPrev(null);
                                break;
                            default:
                                steps();
                                break;
                        }
                        #endregion
                        break;
                    case 3: //ml comment { L1
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
                    case 4: //ml comment (* L1
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
                    case 5: //ml comment { L2
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
                    case 6: //ml comment (* L2
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
            Action steps = () => { if (prev.HasValue) put('s'); updPrev(current); };
            Action finite_state_machine_body = () =>
            {
                switch (state)
                {
                    case 0: //default
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
                    case 1: //line comment
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
                    case 2: //string open q1
                        #region state3 string
                        if (current == quote)
                        {
                            updState(3);//open q2
                            steps();
                        }
                        else
                        {
                            updState(4);//continue q1
                            steps();
                        }
                        #endregion
                        break;
                    #region string states
                    case 4: //string continue q1
                        if(current == quote)
                        {
                            if (prev.HasValue && prev == '\\')
                            {
                                steps();
                            }
                            else
                            {
                                //close string
                                updState(0);
                                steps();
                                updPrev(null);
                            }
                        }
                        else
                        {
                            steps();
                        }
                        break;
                    case 3:
                        //open q2
                        if (current == quote)
                        {
                            //continue q3
                            updState(5);
                            steps();
                        }
                        else
                        {
                            updState(0);
                            steps();
                            updPrev(null);
                        }
                        break;
                    case 5:
                        //continue q3
                        if (current == quote)
                        {
                            if (!prev.HasValue || prev != '\\')
                            {
                                updState(6); //close q1
                                steps();
                            }
                            else { steps(); }
                        }
                        else { steps(); }
                        break;
                    case 6:
                        //close q1
                        if (current == quote)
                        {
                            updState(7); //close q2
                            steps();
                        }
                        else
                        {
                            updState(5); //continue q3
                            steps();
                        }
                        break;
                    case 7:
                        //close q2
                        if (current == quote)
                        {
                            updState(0);
                            steps();
                            updPrev(null);
                        }
                        else
                        {
                            updState(5);
                            steps();
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

        //cpp c java
        private void RemoveCharJAVA()
        {
            Regex r = new Regex(@"'(\')?[^']*'");
            content = r.Replace(content, "c");
        }

		private void RemoveShitCPLUSPLUS()
		{
            //remove comments
            RemoveCommentJAVA();  //not supported R "delimeter(*raw symbols)delimeter"

            RemoveCharJAVA();

            List<string> sw = new List<string> { "array", "deque", "forward_list", "list", "map", "queue", "set", "stack", "unordered_map", "unordered_set", "vector"};

            RemoveTriangleBreakets(sw, "tt");

            sw = new List<string> { "template" };

            RemoveTriangleBreakets(sw, "");

            //
            //#include
            Regex r = new Regex(@"[^\n]*#include[^\n]*");
            content = r.Replace(content, "");
            //using
            r = new Regex(@"[^\n]*using[^\n]*");
            content = r.Replace(content, "");
            //

            RemoveEmptyStrings();
        }

        private void RemoveShitJAVA()
        {
            //remove comments
            RemoveCommentJAVA(); //similar to cplusplus

            RemoveCharJAVA();

            List<string> sw = new List<string> { "Queue", "Iterable", "Collection", "List", "ArrayList", "LinkedList", "ArrayDeque", "Deque", "Set", "HashSet", "LinkedHashSet", "TreeSet", "SortedSet", "Comparable", "Comparator", "Map", "HashMap", "LinkedHashMap", "TreeMap", "WeakHashMap", "SortedMap" };

            RemoveTriangleBreakets(sw, "tt");
            //#include
            Regex r = new Regex(@"[^\n]*import[^\n]*");
            content = r.Replace(content, "");
            //using
            r = new Regex(@"[^\n]*using[^\n]*");
            content = r.Replace(content, "");
            //
            RemoveEmptyStrings();

            
        }

        private void RemoveShitCSHARP()
        {
            //remove comments
            RemoveCommentCSHARP(); //similar to cplusplus

            RemoveCharJAVA();

            List<string> sw = new List<string> { "Dictionary", "List", "Queue", "SortedList", "Stack", "ArrayList", "Hashtable" };

            RemoveTriangleBreakets(sw, "tt");

            //#include
            Regex r = new Regex(@"[^\n]*import[^\n]*");
            content = r.Replace(content, "");
            //using
            r = new Regex(@"[^\n]*using[^\n]*");
            content = r.Replace(content, "");
            //

            RemoveEmptyStrings();
        }

        //rm cpp c cs java
        private void RemoveShitPASCAL()
        {
            //remove comments
            RemoveCommentPASCAL();

            //#include
            Regex r = new Regex(@"[^\n]*uses[^\n]*");
            content = r.Replace(content, "");
            //using
            r = new Regex(@"[^\n]*using[^\n]*");
            content = r.Replace(content, "");
            //
            r = new Regex(@"[^\n]*begin[^\n]*");
            content = r.Replace(content, "{");
            r = new Regex(@"[^\n]*end[^\n]*");
            content = r.Replace(content, "}");
            RemoveEmptyStrings();
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
            r = new Regex(@"[\s\n\t\r\n]+");
            content = r.Replace(content, " ");
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

        private void RemoveTriangleBreakets(List <string> stopwords, string replaced)
        {
            List<string> matches = new List<string>();
            foreach (var sw in stopwords)
            {
                int start = 0;
                Regex f = new Regex(@"[^_a-zA-Z](" + sw + @")\s*<");
                while(f.IsMatch(content, start))
                {
                    var match = f.Match(content, start);
                    start = content.IndexOf(match.ToString());
                    int balance = 1;
                    int pos = start + match.Length;
                    while (balance > 0 && pos < content.Length)
                    {
                        switch (content[pos])
                        {
                            case '<':
                                balance++;
                                break;
                            case '>':
                                balance--;
                                break;
		                    default:
                                break;
                        }
                        pos++;
                    }
                    content = content.Remove(start + 1, pos - start - 1).Insert(start + 1, replaced);
                }
            }
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
                    RemoveEmptyStrings();
					break;
				default:
					break;
			}
            if (lagnguage != Language.UNKNOWN)
            {
                Regex r = new Regex("(?:[^0-9a-zA-Z_])[0-9][_a-zA-Z0-9]*");
                content = r.Replace(content, "n");
            }
		}

        public override string ToString() 
        {
            return content;
        }
	}
}
