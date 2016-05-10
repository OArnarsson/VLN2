#include "Robot.h"
using namespace std;

Robot::Robot()
{
    position.x = 0;
    position.y = 0;
    facing = 'N';
}

Coordinate Robot::getPosition()
{
    return position;
}

void Robot::handleInput(char inputChar)
{
    inputChar = toupper(inputChar);
    switch (inputChar)
    {
        case 'H':
            turnRight();
            break;
        case 'V':
            turnLeft();
            break;
        case 'F':
            goForward();
            break;
        default:
            break;
    }
}

void Robot::resetPosition()
{
    position.x = 0;
    position.y = 0;
    facing = 'N';
}

void Robot::turnRight()
{
    switch (facing)
    {
        case 'N':
            facing = 'E';
            break;
        case 'E':
            facing = 'S';
            break;
        case 'S':
            facing = 'W';
            break;
        case 'W':
            facing = 'N';
            break;
        default:
            break;
    }
}

void Robot::turnLeft()
{
    switch (facing)
    {
        case 'N':
            facing = 'W';
            break;
        case 'W':
            facing = 'S';
            break;
        case 'S':
            facing = 'E';
            break;
        case 'E':
            facing = 'N';
            break;
        default:
            break;
    }
}

void Robot::goForward()
{
    switch (facing)
    {
        case 'N':
            position.y++;
            break;
        case 'E':
            position.x++;
            break;
        case 'S':
            position.y--;
            break;
        case 'W':
            position.x--;
            break;
        default:
            break;
    }
}
