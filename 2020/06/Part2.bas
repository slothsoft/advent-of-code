/' 
 ' Day 6: Custom Customs (Part 2)
 ' ==============================
 ' Puzzle: https://adventofcode.com/2020/day/6
 ' Compiler: https://www.jdoodle.com/execute-freebasic-online
 '
 ' For each group, count the number of questions to which everyone answered "yes". What is the sum of those counts?
 '/
FUNCTION CheckAnswers(currentGroup as string, newMember as string) as string

    IF currentGroup = "" THEN
        currentGroup = newMember
    ELSE
        Dim i As Integer = 1
        DO
            IF InStr(newMember, MID(currentGroup, i, 1)) = 0 THEN
                currentGroup = Left(currentGroup, i - 1) + RIGHT(currentGroup, Len(currentGroup) - i)
            ELSE    
                i += 1
            END IF
        LOOP WHILE i <= Len(currentGroup)
    END IF

    IF currentGroup = "" THEN
        currentGroup = "-"
    END IF
    
    return currentGroup
END function 

FUNCTION CountLetters(group as string) as integer
    Dim count As Integer = 0
    
    For i As Integer = 0 To Len(group) - 1
        IF group[i] <> ASC("-") THEN
            count += 1
        END IF
    Next
    
    return count
END function 


Open Cons For Input As #1
Open Cons For Output As #2

Dim inputLine As String
Dim endCounter As Integer = 0
Dim currentGroup As String
Dim count As Integer = 0
Dim entireCount As Integer = 0

DO 
    Line Input #1,inputLine
    
    IF inputLine = "" THEN
        endCounter += 1
        
        IF currentGroup <> "" THEN
            count = CountLetters(currentGroup)
            ' Print #2, "Group ";currentGroup;": ";count
            entireCount += count
            currentGroup = ""
        END IF
    ELSE
        endCounter = 0
        currentGroup = CheckAnswers(currentGroup, inputLine)
    END IF
LOOP UNTIL endCounter > 1 

Print #2, "Sum: ";entireCount // 3437

Close