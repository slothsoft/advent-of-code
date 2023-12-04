/' 
 ' Day 6: Custom Customs (Part 1)
 ' ==============================
 ' Puzzle: https://adventofcode.com/2020/day/6
 ' Compiler: https://www.jdoodle.com/execute-freebasic-online
 '
 ' For each group, count the number of questions to which anyone 
 ' answered "yes". What is the sum of those counts?
 '/
FUNCTION CountLetters(group as string) as integer
    Dim letters(26) As String = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" }
    Dim count As Integer = 0

    For i As Integer = LBound(letters) To UBound(letters)
        IF InStr(group, letters(i)) > 0 THEN
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
        currentGroup += inputLine
    END IF
LOOP UNTIL endCounter > 1 

Print #2, "Sum: ";entireCount // 6630

Close