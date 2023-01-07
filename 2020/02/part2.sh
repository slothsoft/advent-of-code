# Day 2: Password Philosophy (Part II)
# ====================================
# Puzzle: https://adventofcode.com/2020/day/2
# Compiler: https://www.jdoodle.com/test-bash-shell-script-online/
#
# Each policy actually describes two positions in the password, where 1 means the first character, 2 means the second
# character, and so on. Exactly one of these positions must contain the given letter.

validPasswords=0

while read line
do
  # echo "$line"
  lineSplit=(${line// / })

  # 14-17 (the two positions the character might appear)
  positions=(${lineSplit[0]//-/ })

  # s: (the character that should appear once)
  charToCompare=${lineSplit[1]:0:1}

  # ngxxvqwxzlhxwpxxxz (the password itself)
  password=${lineSplit[2]}

  # get the characters and compare them
  char1=`expr substr $password ${positions[0]} 1`
  char2=`expr substr $password ${positions[1]} 1`

  if [ "$char1" = "$charToCompare" ] && [ "$char2" != "$charToCompare" ]
  then
    validPasswords=$((validPasswords+1))
  fi
  if [ "$char2" = "$charToCompare" ] && [ "$char1" != "$charToCompare" ]
  then
    validPasswords=$((validPasswords+1))
  fi
done < "${1:-/dev/stdin}"

echo "Valid passwords: $validPasswords"