using System;
using System.Collections.Generic;
using System.Linq;

// place in state for each character
const int FATHER = 0;
const int MOTHER = 1;
const int SON_1 = 2;
const int SON_2 = 3;
const int DAUGHTER_1 = 4;
const int DAUGHTER_2 = 5;
const int THIEF = 6;
const int POLICE = 7;
const int BOAT = 8;

// initial state
char[] START_STATE = { 'L', 'L', 'L', 'L', 'L', 'L', 'L', 'L', 'L' };

// define thief rule
bool ThiefRule(char[] state)
{
    return state[POLICE] == state[THIEF] || (
        state[THIEF] != state[FATHER]
        && state[THIEF] != state[MOTHER]
        && state[THIEF] != state[SON_1]
        && state[THIEF] != state[SON_2]
        && state[THIEF] != state[DAUGHTER_1]
        && state[THIEF] != state[DAUGHTER_2]
    );
}

// define daughter rule
bool DaughterRule(char[] state)
{
    return (
        state[DAUGHTER_1] == state[MOTHER] || state[DAUGHTER_1] != state[FATHER]
    ) && (state[DAUGHTER_2] == state[MOTHER] || state[DAUGHTER_2] != state[FATHER]);
}

// define son rule
bool SonRule(char[] state)
{
    return (state[SON_1] == state[FATHER] || state[SON_1] != state[MOTHER]) && (
        state[SON_2] == state[FATHER] || state[SON_2] != state[MOTHER]
    );
}

// We have defined each rule and in this function we check whether the given state is valid or not
bool IsValid(char[] state)
{
    return ThiefRule(state) && DaughterRule(state) && SonRule(state);
}

// checking for goal state
bool IsGoal(char[] state)
{
    return Enumerable.SequenceEqual(state, new char[] { 'R', 'R', 'R', 'R', 'R', 'R', 'R', 'R', 'R' });
}

// generates all possible moves
IEnumerable<char[]> GenerateChild(char[] state)
{
    foreach (int other in new int[] { FATHER, MOTHER, DAUGHTER_1, DAUGHTER_2, SON_1, SON_2, THIEF, POLICE })
    {
        if (state[FATHER] == state[other] && state[FATHER] == state[BOAT])
        {
            char[] move = (char[])state.Clone();
            move[FATHER] = move[FATHER] == 'R' ? 'L' : 'R';
            move[other] = move[other] == 'R' ? 'L' : 'R';
            move[BOAT] = move[BOAT] == 'R' ? 'L' : 'R';
            yield return move;
        }

        if (state[MOTHER] == state[other] && state[MOTHER] == state[BOAT])
        {
            char[] move = (char[])state.Clone();
            move[MOTHER] = move[MOTHER] == 'R' ? 'L' : 'R';
            move[other] = move[other] == 'R' ? 'L' : 'R';
            move[BOAT] = move[BOAT] == 'R' ? 'L' : 'R';
            yield return move;
        }

        if (state[POLICE] == state[other] && state[POLICE] == state[BOAT])
        {
            char[] move = (char[])state.Clone();
            move[POLICE] = move[POLICE] == 'R' ? 'L' : 'R';
            move[other] = move[other] == 'R' ? 'L' : 'R';
            move[BOAT] = move[BOAT] == 'R' ? 'L' : 'R';
            yield return move;
        }
    }
}

// filter out only the valid moves
List<char[]> ValidMoves(IEnumerable<char[]> states)
{
    List<char[]> validList = new List<char[]>();
    foreach (char[] s in states)
    {
        if (IsValid(s) && !validList.Any(x => Enumerable.SequenceEqual(x, s)))
        {
            validList.Add(s);
        }
    }
    return validList;
}

// we do a depth limited search, using a previous_states list to keep track of where we have been. This function only returns a winning answer.
List<char[]> DLS(char[] state, List<char[]> previous_states, int maxDepth)
{
    previous_states.Add(state);

    if (IsGoal(state))
    {
        return previous_states;
    }

    if (maxDepth <= 0)
    {
        return null;
    }

    var generatedchilren = GenerateChild(state);
    foreach (char[] move in ValidMoves(generatedchilren))
    {
        if (!previous_states.Any(x => Enumerable.SequenceEqual(x, move)))
        {
            List<char[]> result = DLS(move, previous_states, maxDepth - 1);

            if (result != null)
            {
                return result;
            }
            previous_states.RemoveAt(previous_states.Count - 1);
        }
    }

    return null;
}

// print one state in the terminal
void PrintState(char[] state)
{
    Console.WriteLine("Father: {0}, Mother: {1}, Son 1: {2}, Son 2: {3}, Daughter 1: {4}, Daughter 2: {5}, Thief: {6}, Police: {7}, Boat: {8}", state[FATHER], state[MOTHER], state[SON_1], state[SON_2], state[DAUGHTER_1], state[DAUGHTER_2], state[THIEF], state[POLICE], state[BOAT]);
}

// main function
void Main()
{
    List<char[]> state_list = Array.Empty<char[]>().ToList();
    state_list = DLS(START_STATE, new List<char[]>(), 30);

    foreach (char[] s in state_list)
    {
        PrintState(s);
        Console.WriteLine();
    }
}

Main();
