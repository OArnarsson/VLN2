#ifndef STRINGSET_H
#define STRINGSET_H

#include <iostream>
#include <string>
#include <list>

using namespace std;

class StringSet
{
public:
    // Returns the number of elements in the set.
    // Worst-case time complexity: O(1)
    int size();

    // Inserts 'element' into the set. If 'element' is contained in the
    // set, this operation has no effect.
    // Worst-case time complexity: O(n)
    void insert(string element);

    // Removes 'element' from the set. If 'element' is not in the set, this
    // operation has no effect.
    // Worst-case time complexity: O(n)
    void remove(string element);

    // Returns true if and only if 'element' is a member of the set.
    // Worst-case time complexity: O(n)
    bool contains(string element);

    // Writes the contents of the set to outs
    // Worst-case time complexity: O(n)
    friend ostream& operator <<(ostream& outs, const StringSet& set);

private:
    list<string> set;
};

#endif // STRINGSET_H
