import 'dart:io';

/*
 * Day 4: Passport Processing (Part 1)
 * ===================================
 * Puzzle: https://adventofcode.com/2020/day/4
 * Compiler: https://www.jdoodle.com/execute-dart-online/
 *
 * Count the number of valid passports - those that have all required fields. 
 * Treat cid as optional. In your batch file, how many passports are valid?
 */
void main() {
  String? partOfPassport = stdin.readLineSync();
  String passport = "";
  int validPassports = 0;

  while (partOfPassport != null) {
    if (partOfPassport == "") {
      if (isValidPassport(passport)) {
        validPassports++;
        // print(passport);
      }
      passport = "";
    } else {
      passport += partOfPassport;
    }
    partOfPassport = stdin.readLineSync();
  }

  print("Valid passports: $validPassports");
}

bool isValidPassport(passport) {
  var fields = [
    'byr',
    'iyr',
    'eyr',
    'hgt',
    'hcl',
    'ecl',
    'pid',
  ];
  for (var field in fields) {
    if (!passport.contains(field + ':')) {
      return false;
    }
  }
  return true;
}