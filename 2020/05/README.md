# Day 5: Binary Boarding

- Puzzle: https://adventofcode.com/2020/day/5

"As a sanity check, look through your list of boarding passes. 
What is the highest seat ID on a boarding pass? What is the ID of your seat?"

_(Using Excel to solve today's puzzle seems like cheating, but I couldn't find
another programming language with E, which has an online compiler that excepts
input from stdin or file.)_


## Formulas

- **Input**
- **Row String** - `=LEFT({Input}; 7)`
- **Col String** - `=RIGHT({Input}; 3)`
- **Row Binary** - `=SUBSTITUTE(SUBSTITUTE({Row String}; "B"; "1"); "F"; "0")`
- **Col Binary** - `=SUBSTITUTE(SUBSTITUTE({Col String}; "R"; "1"); "L"; "0")`
- **Row Decimal** - `=BIN2DEC({Row Binary})`
- **Col Decimal** - `=BIN2DEC({Col Binary})`
- **Seat ID** - `={Row Decimal} * 8 + {Col Decimal}`
- **+1 Exists** - `=IF(COUNTIF({Seat IDs}; {Seat ID} + 1) > 0; true; false)`

| Input | Row String | Col String | Row Binary | Col Binary | Row Decimal | Col Decimal | Seat ID | +1 Exists |
| ----- | ---------- | ---------- | ---------- | ---------- | ----------- | ----------- | ------- | --------- |
| `BFFBBBBLRR` | `BFFBBBB` | `LRR` | `1001111` | `011` | `79`  | `3` | `635` | `TRUE`  |
| `BFFFFFBRRL` | `BFFFFFB` | `RRL` | `1000001` | `110` | `65`  | `6` | `526` | `FALSE` |
| `BBFBBBBRLL` | `BBFBBBB` | `RLL` | `1101111` | `100` | `111` | `4` | `892` | `TRUE`  |


### Part 1

**Max Seat ID** - `=MAX({Seat IDs})`

### Part 2

Seat ID 526 has no **+1**, so 527 is our seat. 



