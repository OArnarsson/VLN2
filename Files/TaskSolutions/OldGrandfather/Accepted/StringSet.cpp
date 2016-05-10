#include "StringSet.h"

int StringSet::size()
{
    return static_cast<int>(set.size());
}

void StringSet::insert(string element)
{
    if (!contains(element))
    {
        list<string>::iterator it = set.begin();
        while (it != set.end() && element > *it)
        {
            it++;
        }
        set.insert(it, element);
    }
}

void StringSet::remove(string element)
{
    for (list<string>::iterator it = set.begin(); it != set.end(); ++it)
    {
        if (*it == element)
        {
            set.erase(it);
            break; // There is never more than 1 instance of each element
        }
    }
}

bool StringSet::contains(string element)
{
    for (list<string>::const_iterator it = set.begin(); it != set.end(); ++it)
    {
        if (*it == element)
        {
            return true;
        }
    }
    return false;
}

ostream &operator <<(ostream &outs, const StringSet &set)
{
    for (list<string>::const_iterator it = set.set.begin(); it != set.set.end(); ++it)
    {
        outs << *it << endl;
    }
    return outs;
}
