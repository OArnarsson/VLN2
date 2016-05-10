#include "StringSet.h"

int main()
{
    StringSet set;

    while (true)
    {
        string inp;
        cin >> inp;

        if (inp[0] == '#')
        {
            break;
        }
        else if (inp == "not")
        {
            cin >> inp;
            set.remove(inp);
        }
        else
        {
            set.insert(inp);
        }
    }

    cout << set.size() << endl;
    cout << set;
}
