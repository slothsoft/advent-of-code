# Day 2: Password Philosophy (Part I)
# ===================================
# Puzzle: https://adventofcode.com/2020/day/2
# Compiler: https://www.jdoodle.com/test-bash-shell-script-online/
#
# Each line gives the password policy and then the password. The password policy indicates the lowest and highest
# number of times a given letter must appear for the password to be valid.

validPasswords=0

while read line
do
  # echo "$line"
  lineSplit=(${line// / })

  # 14-17 (the range of occurrences of a character)
  range=(${lineSplit[0]//-/ })

  # s: (the character the range is for)
  charToCount=${lineSplit[1]:0:1}

  # ngxxvqwxzlhxwpxxxz (the password itself)
  password=${lineSplit[2]}

  # count the occurrences of the character
  replacement=`tr -dc "$charToCount" <<<"$password"`
  charCount=`echo -n $replacement | wc -m`

  if [ $((range[0])) -ge 1 ] && [ $((charCount)) -ge $((range[0])) ] && [ $((charCount)) -le $((range[1])) ]
  then
    validPasswords=$((validPasswords+1))
  fi
done < "${1:-/dev/stdin}"

echo "Valid passwords: $validPasswords"