#include <iostream>
#include <string>
#include "Robot.h"

using namespace std;

const unsigned int MAXPOWER = 10000;

int main()
{
    Robot skundi;
    
    int times;
    cin >> times;
    cin.ignore();
    
    for(int x = 0; x < times; x++)
    {
        string input;
        getline(cin, input);
        
        for(unsigned int x = 0; x < input.size() && x < MAXPOWER; x++)
        {
            skundi.handleInput(input[x]);
        }
        
        cout << skundi.getPosition().x << " " << skundi.getPosition().y << endl;
        skundi.resetPosition();
        
    }
}
