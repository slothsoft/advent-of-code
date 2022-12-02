
$inputFile=$args[0]
$outputFile=$args[1]

# ----------------------------------------
# Read all necessary JSON files
# ----------------------------------------

$calendarJson = (gc "$PSScriptRoot\calendar.json" -encoding utf8) | ConvertFrom-Json

$environmentJson = (gc "$PSScriptRoot\environment.json" -encoding utf8) | ConvertFrom-Json
if (!$environmentJson.Cookie) {
    throw "Environment file with cookie was not found!"
}

$webClient = new-object system.net.webclient
$webClient.Headers.add("Cookie", -join("session=", $environmentJson.Cookie))
$colorsJson = $webClient.DownloadString("https://raw.githubusercontent.com/ozh/github-colors/master/colors.json") | ConvertFrom-Json

# ----------------------------------------
# Create SVG tiles
# ----------------------------------------

$tilesFolder = "$PSScriptRoot\Tiles"
$calendarData = [ordered]@{ }

foreach ($codeSet in $calendarJson.CodeSets) {
    $year = $codeSet.Year
    $language = $codeSet.Language
    $directory = $codeSet.Directory
    $prefix = $codeSet.Prefix
    
    $tilesForYear = "$tilesFolder\$year"
    if (!(Test-Path -Path $tilesForYear)) {
        New-Item $tilesForYear -itemType Directory
    }
    
    if (!$calendarData.Contains($year)) {
        $calendarData[$year] = [ordered]@{ }
    }

    $color = $colorsJson.PSObject.Properties[$language].PSObject.Properties["Value"].Value.PSObject.Properties["color"].Value # ???
    if (!$color) {
        $color = "#FF0000"
    }
      
    # Read my Leaderboard

    $leaderboardContent = $webClient.DownloadString("https://adventofcode.com/$year/leaderboard/self")
    # $leaderboardContent = [string] (gc "$PSScriptRoot\Examples\leaderboard-$year.html" -encoding utf8)
    
    $regex = '\d{1,2}\s+[^\s]+\s+\d+\s+\d+\s+[^\s]+\s+\d+\s+\d+'
    $dailyScores = @{ }
    foreach ($score in ([regex]$regex).Matches($leaderboardContent)) {
        $scoreSplit = $score.Value.Trim() -split '\s+'
        
        $dailyScores[[int] $scoreSplit[0]] = @{
            Part1Time = $scoreSplit[1]
            Part1Rank = [int] $scoreSplit[2]
            Part1Score = [int] $scoreSplit[3]
            Part2Time = $scoreSplit[4]
            Part2Rank = [int] $scoreSplit[5]
            Part2Score = [int] $scoreSplit[6]
        }
    }
	
	# Find the correct folders with source code
	
    $dayFolders = Get-ChildItem "$PSScriptRoot\..\$directory" -Directory -Filter "$prefix*"
	
    foreach ($file in $dayFolders) {
        $folderName = $file.Name
		if ($prefix) {
			$baseFileName = $folderName -replace $prefix, ''
		} else {
			$baseFileName = $folderName
		}
		
        $day = ""
        if ([int]::TryParse($baseFileName, [ref]$day))
        {
            $dayWithZero = '{0:d2}'-f $day
            if ($dailyScores.Contains($day))
            {
                $dailyScore = $dailyScores[$day]
            } else {
                $dailyScore = @{
                    Part1Time = "--:--:--"
                    Part1Rank = "-"
                    Part1Score = "-"
                    Part2Time = "--:--:--"
                    Part2Rank = "-"
                    Part2Score = "-"
                }
            }
         
            $svg = Get-Content "$PSScriptRoot\calendar-tile.svg"

            $svg = $svg -replace 'XXX', $dayWithZero
            $svg = $svg -replace 'PART_1_TIME', $dailyScore.Part1Time
            $svg = $svg -replace 'PART_2_TIME', $dailyScore.Part2Time
            $svg = $svg -replace 'LANGUAGE', $language
            $svg = $svg -replace '#00FF00', $color

            $svg | Out-File -encoding utf8 ("$tilesForYear\$dayWithZero.svg")

            $calendarData[$year][$dayWithZero] = "<a href=`"./$directory/$folderName`"><img src=`"./Calendar/Tiles/$year/$dayWithZero.svg`" width=`"150px`"></a>"
        }
    }
}
# ----------------------------------------
# Create the Markdown for the Readme
# ----------------------------------------

$readmeReplacement = ""
foreach ($year in $calendarData.Keys)
{
    $readmeReplacement = $readmeReplacement + "`n`n# $year`n`n"

    foreach ($day in $calendarData[$year].Keys)
    {
        $readmeReplacement = $readmeReplacement + $calendarData[$year][$day] + "`n"
    }
}

# ----------------------------------------
# Read all necessary JSON files
# ----------------------------------------

$readme = Get-Content $inputFile

$readme = $readme -replace '<!-- CALENDAR-TILES -->', $readmeReplacement

$readme | Out-File -encoding utf8 $outputFile