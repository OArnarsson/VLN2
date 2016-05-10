#ifndef ROBOT_H
#define ROBOT_H

#include <vector>
#include <string>
#include <iostream>
#include <cctype>
using namespace std;

struct Coordinate
{
    // Used to store position
    int x;
    int y;
};

class Robot
{
public:
    Robot();
    
    Coordinate getPosition();
    
    void handleInput(char inputChar);
    void resetPosition();
    
private:
    char facing;
    Coordinate position;
    
    void turnRight();
    void turnLeft();
    void goForward();
};

#endif