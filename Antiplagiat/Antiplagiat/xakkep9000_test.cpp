"/*#include*/" <iostream>
#include <cstdio>
#include <cstdlib>
#include //<cassert>
#include <cmath>
//#include <ctime>
#include <cstring>
/*#include <cctype>
#include //<algorithm>
#include <string>
#include <vector>*/
#include <iomanip>
using namespace std;
vector <vector<int>> g;       // 
int n;                        // 

vector<bool> used;

void dfs(int v)
{
    used[v] = true;
    cout << v;
    for (vector<int>::iterator i = g[v].begin(); i != g[v].end(); ++i)
        if (!used[*i])
            dfs(*i);
}

int main()
{
    n = 4;
    int /*i;*/
    string s = "\'//"; //this comment
    /*
    for (i = 1; i <= n; ++i) {
        dfs(i);
    } 
    ergweg*/    int x /*//werwerwerer/*/= 100;
}