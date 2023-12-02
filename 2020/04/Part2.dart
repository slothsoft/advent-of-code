import 'dart:io';

/*
 * Day 4: Passport Processing (Part 2)
 * ===================================
 * Puzzle: https://adventofcode.com/2020/day/4
 * Compiler: https://www.jdoodle.com/execute-dart-online/
 *
 * Count the number of valid passports - those that have all required fields. 
 * Treat cid as optional. In your batch file, how many passports are valid?
 */
void main() {
  String? partOfPassport;
  String passport = "";
  int validPassports = 0;

  do {
    partOfPassport = stdin.readLineSync();

    if (partOfPassport == null || partOfPassport == "") {
      if (isValidPassport(passport + " ")) {
        validPassports++;
      }
      passport = "";
    } else {
      passport += " " + partOfPassport;
    }
  } while (partOfPassport != null);

  print("Valid passports: $validPassports");
}

var fields = {
  'byr': (String s) => validateYear(s, 1920, 2002),
  'iyr': (String s) => validateYear(s, 2010, 2020),
  'eyr': (String s) => validateYear(s, 2020, 2030),
  'hgt': (String s) => validateHeight(s),
  'hcl': (String s) => validateHairColor(s),
  'ecl': (String s) => validateEyeColor(s),
  'pid': (String s) => validatePassportId(s),
};

bool isValidPassport(passport) {
  for (var field in fields.keys) {
    if (!passport.contains(field + ':')) {
      return false;
    }

    var keyValueRegex = RegExp("$field:(([^\\s])*)\\s");
    var match = keyValueRegex.firstMatch(passport);
    var value = match!.group(1)!;

    if (!fields[field]!.call(value)) {
      // print(value);
      return false;
    }
  }
  return true;
}

bool validateYear(String string, int minYear, int maxYear) {
  var value = int.tryParse(string);
  if (value == null) {
    return false;
  }
  return value >= minYear && value <= maxYear;
}

bool validateEyeColor(String string) {
  var validColors = [
    'amb',
    'blu',
    'brn',
    'gry',
    'grn',
    'hzl',
    'oth',
  ];
  return validColors.contains(string);
}

var passportIdRegex = RegExp("^\\d{9}\$");

bool validatePassportId(String string) {
  return passportIdRegex.hasMatch(string);
}

var hairColorRegex = RegExp("^#[0-9a-f]{6}\$");

bool validateHairColor(String string) {
  return hairColorRegex.hasMatch(string);
}

bool validateHeight(String string) {
  if (string.endsWith("cm")) {
    return validateHeightAsCm(string.substring(0, string.length - 2));
  }
  if (string.endsWith("in")) {
    return validateHeightAsInch(string.substring(0, string.length - 2));
  }

  return false;
}

bool validateHeightAsCm(String string) {
  var value = int.tryParse(string);
  if (value == null) {
    return false;
  }
  return value >= 150 && value <= 193;
}

bool validateHeightAsInch(String string) {
  var value = int.tryParse(string);
  if (value == null) {
    return false;
  }
  return value >= 59 && value <= 76;
}